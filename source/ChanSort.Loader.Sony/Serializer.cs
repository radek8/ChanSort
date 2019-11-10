﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using ChanSort.Api;

namespace ChanSort.Loader.Sony
{
  class Serializer : SerializerBase
  {
    /*
     * At the time of this writing, there seem to be 4 different versions of this format.
     * One defines an element with a typo: <FormateVer>1.1.0</FormateVer>, which has different XML elements and checksum calculation than all other versions.
     * This format is identified as "e1.1.0" here, with the leading "e".
     * The other formats define <FormatVer>...</FormatVer> with versions 1.0.0, 1.1.0 and 1.2.0, which are otherwise identical.
     *
     * NOTE: Even within the same version, there are some files using CRLF and some using LF for newlines.
     */

    private const string SupportedFormatVersions = " e1.1.0 1.0.0 1.1.0 1.2.0 ";

    private XmlDocument doc;
    private byte[] content;
    private string textContent;
    private string format;
    private bool isEFormat;
    private string newline;

    private readonly Dictionary<SignalSource, ChannelListNodes> channeListNodes = new Dictionary<SignalSource, ChannelListNodes>();


    #region ctor()
    public Serializer(string inputFile) : base(inputFile)
    {
      this.Features.ChannelNameEdit = ChannelNameEditMode.All;
      this.Features.DeleteMode = DeleteMode.FlagWithoutPrNr; // in Android/e-format, this will be changed to FlagWithPrNr

      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbT | SignalSource.Tv, "DVB-T TV"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbT | SignalSource.Radio, "DVB-T Radio"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbT | SignalSource.Data, "DVB-T Other"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbC | SignalSource.Tv, "DVB-C TV"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbC | SignalSource.Radio, "DVB-C Radio"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbC | SignalSource.Data, "DVB-C Other"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbS | SignalSource.Provider0, "DVB-S"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbS | SignalSource.Provider1, "DVB-S Preset"));
      this.DataRoot.AddChannelList(new ChannelList(SignalSource.DvbS | SignalSource.Provider2, "DVB-S Ci"));

      foreach (var list in this.DataRoot.ChannelLists)
      {
        list.VisibleColumnFieldNames.Remove("PcrPid");
        list.VisibleColumnFieldNames.Remove("VideoPid");
        list.VisibleColumnFieldNames.Remove("AudioPid");
        list.VisibleColumnFieldNames.Remove("Lock");
        list.VisibleColumnFieldNames.Remove("Skip");
        list.VisibleColumnFieldNames.Remove("ShortName");
        list.VisibleColumnFieldNames.Remove("Provider");
      }
    }
    #endregion

    #region Load()

    public override void Load()
    {
      bool fail = false;
      try
      {
        this.doc = new XmlDocument();
        this.content = File.ReadAllBytes(this.FileName);
        this.textContent = Encoding.UTF8.GetString(this.content);
        this.newline = this.textContent.Contains("\r\n") ? "\r\n" : "\n";

        var settings = new XmlReaderSettings
        {
          CheckCharacters = false,
          IgnoreProcessingInstructions = true,
          ValidationFlags = XmlSchemaValidationFlags.None,
          DtdProcessing = DtdProcessing.Ignore
        };
        using (var reader = XmlReader.Create(new StringReader(textContent), settings))
        {
          doc.Load(reader);
        }
      }
      catch
      {
        fail = true;
      }

      var root = doc.FirstChild;
      if (root is XmlDeclaration)
        root = root.NextSibling;
      if (fail || root == null || root.LocalName != "SdbRoot")
        throw new FileLoadException("\"" + this.FileName + "\" is not a supported Sony XML file");

      foreach (XmlNode child in root.ChildNodes)
      {
        switch (child.LocalName)
        {
          case "SdbXml":
            this.ReadSdbXml(child);
            break;
          case "CheckSum":
            this.ReadChecksum(child);
            break;
        }
      }

      if (!this.isEFormat)
      {
        foreach (var list in this.DataRoot.ChannelLists)
        {
          if ((list.SignalSource & SignalSource.Sat) != 0)
          {
            list.VisibleColumnFieldNames.Remove("Hidden");
            list.VisibleColumnFieldNames.Remove("Satellite");
          }
        }
      }
    }
    #endregion

    #region ReadSdbXml()
    private void ReadSdbXml(XmlNode node)
    {
      this.format = "";
      this.isEFormat = false;
      var formatNode = node["FormatVer"];
      if (formatNode != null)
        this.format = formatNode.InnerText;
      else if ((formatNode = node["FormateVer"]) != null)
      {
        this.format = "e" + formatNode.InnerText;
        this.isEFormat = true;
        this.Features.DeleteMode = DeleteMode.FlagWithPrNr;
      }

      if (SupportedFormatVersions.IndexOf(" " + this.format + " ", StringComparison.Ordinal) < 0)
        throw new FileLoadException("Unsupported file format version: " + this.format);

      foreach(XmlNode child in node.ChildNodes)
      {
        var name = child.LocalName.ToLowerInvariant();
        if (name == "sdbt")
          ReadSdb(child, SignalSource.DvbT, 0, "DvbT");
        else if (name == "sdbc")
          ReadSdb(child, SignalSource.DvbC, 0x10000, "DvbC");
        else if (name == "sdbgs")
          ReadSdb(child, SignalSource.DvbS | SignalSource.Provider0, 0x20000, "DvbS");
        else if (name == "sdbps")
          ReadSdb(child, SignalSource.DvbS | SignalSource.Provider1, 0x30000, "DvbS");
        else if (name == "sdbcis")
          ReadSdb(child, SignalSource.DvbS | SignalSource.Provider2, 0x40000, "DvbS");
      }
    }
    #endregion

    #region ReadSdb()
    private void ReadSdb(XmlNode node, SignalSource signalSource, int idAdjustment, string dvbSystem)
    {
      if (node["Editable"]?.InnerText == "F")
      {
        foreach (var list in this.DataRoot.ChannelLists)
        {
          if ((list.SignalSource & (SignalSource.MaskAdInput | SignalSource.MaskProvider)) == signalSource)
            list.ReadOnly = true;
        }
      }

      this.ReadSatellites(node, idAdjustment);
      this.ReadTransponder(node, idAdjustment, dvbSystem);

      if (this.isEFormat)
        this.ReadServicesE110(node, signalSource, idAdjustment);
      else
        this.ReadServices(node, signalSource, idAdjustment);
    }
    #endregion

    #region ReadSatellites()
    private void ReadSatellites(XmlNode node, int satIdAdjustment)
    {
      var satlRec = node["SATL_REC"];
      if (satlRec == null)
        return;
      var data = this.SplitLines(satlRec);
      var ids = data["ui2_satl_rec_id"];
      for (int i = 0, c = ids.Length; i < c; i++)
      {
        var sat = new Satellite(int.Parse(ids[i]) + satIdAdjustment);
        sat.Name = data["ac_sat_name"][i];
        var pos = int.Parse(data["i2_orb_pos"][i]);
        sat.OrbitalPosition = Math.Abs((decimal) pos / 10) + (pos < 0 ? "W" : "E");
        this.DataRoot.AddSatellite(sat);
      }
    }
    #endregion

    #region ReadTransponder()
    private void ReadTransponder(XmlNode node, int idAdjustment, string dvbSystem)
    {
      var mux = node["Multiplex"] ?? throw new FileLoadException("Missing Multiplex XML element");

      var transpList = new List<Transponder>();

      var muxData = SplitLines(mux);
      var muxIds = isEFormat ? muxData["MuxID"] : muxData["MuxRowId"];
      var rfParmData = isEFormat ? null : SplitLines(mux["RfParam"]);
      var dvbsData = isEFormat ? null : SplitLines(mux["RfParam"]?[dvbSystem]);
      var polarity = dvbsData?.ContainsKey("Pola") ?? false ? dvbsData["Pola"] : null;
      for (int i = 0, c = muxIds.Length; i < c; i++)
      {
        Satellite sat = null;
        var transp = new Transponder(int.Parse(muxIds[i]) + idAdjustment);
        if (isEFormat)
        {
          var freq = muxData.ContainsKey("ui4_freq") ? muxData["ui4_freq"] : muxData["SysFreq"];
          transp.FrequencyInMhz = int.Parse(freq[i]);
          if (muxData.ContainsKey("ui4_sym_rate"))
            transp.SymbolRate = int.Parse(muxData["ui4_sym_rate"][i]);
          if (Char.ToLowerInvariant(dvbSystem[dvbSystem.Length - 1]) == 's') // "DvbGs", "DvbPs", "DvbCis"
          {
            transp.Polarity = muxData["e_pol"][i] == "1" ? 'H' : 'V';
            var satId = int.Parse(muxData["ui2_satl_rec_id"][i]) + idAdjustment;
            sat = DataRoot.Satellites[satId];
          }
          else
          {
            transp.FrequencyInMhz /= 1000000;
            transp.SymbolRate /= 1000;
          }
        }
        else
        {
          transp.OriginalNetworkId = this.ParseInt(muxData["Onid"][i]);
          transp.TransportStreamId = this.ParseInt(muxData["Tsid"][i]);
          transp.FrequencyInMhz = int.Parse(rfParmData["Freq"][i]) / 1000;
          transp.Polarity = polarity == null ? ' ' : polarity[i] == "H_L" ? 'H' : 'V';
          if (dvbsData.ContainsKey("SymbolRate"))
            transp.SymbolRate = int.Parse(dvbsData["SymbolRate"][i]) / 1000;
        }

        this.DataRoot.AddTransponder(sat, transp);
        transpList.Add(transp);
      }

      // in the "E"-Format, there is a TS_Descr element that holds ONID and TSID, but lacks any sort of key (like "ui4_tsl_rec_id" or similar)
      // However, it seems like the entries correlate with the entries in the Multiplex element (same number and order)
      if (this.isEFormat)
      {
        var tsDescr = node["TS_Descr"];
        if (tsDescr == null)
          return;
        var tsData = SplitLines(tsDescr);
        var onids = tsData["Onid"];
        var tsids = tsData["Tsid"];

        if (onids.Length != muxIds.Length)
          return;

        for (int i = 0, c = onids.Length; i < c; i++)
        {
          var transp = transpList[i];
          transp.OriginalNetworkId = this.ParseInt(onids[i]);
          transp.TransportStreamId = this.ParseInt(tsids[i]);
        }
      }
    }
    #endregion

    #region ReadServicesE110()
    private void ReadServicesE110(XmlNode node, SignalSource signalSource, int idAdjustment)
    {
      var serviceNode = node["Service"] ?? throw new FileLoadException("Missing Service XML element");
      var svcData = SplitLines(serviceNode);
      var dvbData = SplitLines(serviceNode["dvb_info"]);

      // remember the nodes that need to be updated when saving
      var nodes = new ChannelListNodes();
      nodes.Service = serviceNode;
      this.channeListNodes[signalSource] = nodes;

      for (int i = 0, c = svcData["ui2_svl_rec_id"].Length; i < c; i++)
      {
        var recId = int.Parse(svcData["ui2_svl_rec_id"][i]);
        var chan = new Channel(signalSource, i, recId);
        chan.OldProgramNr = (ParseInt(svcData["No"][i]) >> 18) & 0x3FFFF;
        chan.IsDeleted = svcData["b_deleted_by_user"][i] != "1";
        var nwMask = uint.Parse(svcData["ui4_nw_mask"][i]);
        chan.AddDebug("NW=");
        chan.AddDebug(nwMask);
        chan.AddDebug("OPT=");
        chan.AddDebug(uint.Parse(svcData["ui4_nw_option_mask"][i]));
        chan.Hidden = (nwMask & 8) == 0;
        chan.Encrypted = (nwMask & 2048) != 0;
        //chan.Encrypted = dvbData["t_free_ca_mode"][i] == "1";
        chan.Favorites = (Favorites) ((nwMask & 0xF0) >> 4);
        chan.ServiceId = int.Parse(svcData["ui2_prog_id"][i]);
        chan.Name = svcData["Name"][i];
        var muxId = int.Parse(svcData["MuxID"][i]) + idAdjustment;
        var transp = this.DataRoot.Transponder[muxId];
        chan.Transponder = transp;
        if (transp != null)
        {
          chan.FreqInMhz = transp.FrequencyInMhz;
          chan.SymbolRate = transp.SymbolRate;
          chan.OriginalNetworkId = transp.OriginalNetworkId;
          chan.TransportStreamId = transp.TransportStreamId;
          chan.Polarity = transp.Polarity;
          chan.Satellite = transp.Satellite?.Name;
          chan.SatPosition = transp.Satellite?.OrbitalPosition;

          if ((signalSource & SignalSource.Cable) != 0)
            chan.ChannelOrTransponder = LookupData.Instance.GetDvbcChannelName(chan.FreqInMhz);
          if ((signalSource & SignalSource.Antenna) != 0)
            chan.ChannelOrTransponder = LookupData.Instance.GetDvbtTransponder(chan.FreqInMhz).ToString();
        }
        else
        {
          // this block should never be entered
          // only DVB-C and -T (in the E-format) contain non-0 values in these fields
          chan.OriginalNetworkId = this.ParseInt(dvbData["ui2_on_id"][i]);
          chan.TransportStreamId = this.ParseInt(dvbData["ui2_ts_id"][i]);
        }

        chan.ServiceType = int.Parse(dvbData["ui1_sdt_service_type"][i]);
        chan.SignalSource |= LookupData.Instance.IsRadioOrTv(chan.ServiceType); // could also use <ServiceFilter> information with 1=TV, 2=Radio, 3=Other

        CopyDataValues(serviceNode, svcData, i, chan.ServiceData);

        var list = this.DataRoot.GetChannelList(chan.SignalSource);
        this.DataRoot.AddChannel(list, chan);
      }
    }
    #endregion

    #region ReadServices()
    private void ReadServices(XmlNode node, SignalSource signalSource, int idAdjustment)
    {
      var serviceNode = node["Service"] ?? throw new FileLoadException("Missing Service XML element");
      var svcData = SplitLines(serviceNode);

      var progNode = node["Programme"] ?? throw new FileLoadException("Missing Programme XML element");
      var progData = SplitLines(progNode);

      // remember the nodes that need to be updated when saving
      var nodes = new ChannelListNodes();
      nodes.Service = serviceNode;
      nodes.Programme = progNode;
      this.channeListNodes[signalSource] = nodes;

      var map = new Dictionary<int, Channel>();
      for (int i = 0, c = svcData["ServiceRowId"].Length; i < c; i++)
      {
        var rowId = int.Parse(svcData["ServiceRowId"][i]);
        var chan = new Channel(signalSource, i, rowId);
        map[rowId] = chan;
        chan.OldProgramNr = -1;
        chan.IsDeleted = true;
        chan.ServiceType = int.Parse(svcData["Type"][i]);
        chan.OriginalNetworkId = this.ParseInt(svcData["Onid"][i]);
        chan.TransportStreamId = this.ParseInt(svcData["Tsid"][i]);
        chan.ServiceId = this.ParseInt(svcData["Sid"][i]);
        chan.Name = svcData["Name"][i];
        var muxId = int.Parse(svcData["MuxRowId"][i]) + idAdjustment;
        var transp = this.DataRoot.Transponder[muxId];
        chan.Transponder = transp;
        if (transp != null)
        {
          chan.FreqInMhz = transp.FrequencyInMhz;
          chan.SymbolRate = transp.SymbolRate;
          chan.Polarity = transp.Polarity;
          if ((signalSource & SignalSource.Cable) != 0)
            chan.ChannelOrTransponder = LookupData.Instance.GetDvbcChannelName(chan.FreqInMhz);
          if ((signalSource & SignalSource.Cable) != 0)
            chan.ChannelOrTransponder = LookupData.Instance.GetDvbtTransponder(chan.FreqInMhz).ToString();
        }

        chan.SignalSource |= LookupData.Instance.IsRadioOrTv(chan.ServiceType);
        var att = this.ParseInt(svcData["Attribute"][i]);
        chan.Encrypted = (att & 8) != 0;

        CopyDataValues(serviceNode, svcData, i, chan.ServiceData);

        var list = this.DataRoot.GetChannelList(chan.SignalSource);
        this.DataRoot.AddChannel(list, chan);
      }

      for (int i = 0, c = progData["ServiceRowId"].Length; i < c; i++)
      {
        var rowId = int.Parse(progData["ServiceRowId"][i]);
        var chan = map.TryGet(rowId);
        if (chan == null)
          continue;
        chan.IsDeleted = false;
        chan.OldProgramNr = int.Parse(progData["No"][i]);
        var flag = int.Parse(progData["Flag"][i]);
        chan.Favorites = (Favorites)(flag & 0x0F);

        CopyDataValues(progNode, progData, i, chan.ProgrammeData);
      }
    }
    #endregion

    #region SplitLines()
    private Dictionary<string, string[]> SplitLines(XmlNode parent)
    {
      var dict = new Dictionary<string, string[]>();
      foreach (XmlNode node in parent.ChildNodes)
      {
        if (node.Attributes?["loop"] == null)
          continue;
        var lines = node.InnerText.Trim('\n').Split('\n');
        dict[node.LocalName] = lines.Length == 1 && lines[0] == "" ? new string[0] : lines;
      }

      return dict;
    }
    #endregion

    #region CopyDataValues()
    private void CopyDataValues(XmlNode parentNode, Dictionary<string, string[]> svcData, int i, Dictionary<string, string> target)
    {
      // copy of data values from all child nodes into the channel. 
      // this inverts the [field,channel] data presentation from the file to [channel,field] and is later used for saving channels
      foreach (XmlNode child in parentNode.ChildNodes)
      {
        var field = child.LocalName;
        if (svcData.ContainsKey(field))
          target[field] = svcData[field][i];
      }
    }
    #endregion

    #region ReadChecksum()

    private void ReadChecksum(XmlNode node)
    {
      // skip "0x" prefix ("e"-format doesn't have it)
      uint expectedCrc = uint.Parse(this.isEFormat ? node.InnerText : node.InnerText.Substring(2), NumberStyles.HexNumber);

      uint crc = CalcChecksum(this.content, this.textContent);

      if (crc != expectedCrc)
        throw new FileLoadException($"Invalid checksum: expected 0x{expectedCrc:x8}, calculated 0x{crc:x8}");
    }
    #endregion

    #region CalcChecksum()
    private uint CalcChecksum(byte[] data, string dataAsText)
    {
      int start;
      int end;

      if (this.isEFormat)
      {
        // files with the typo-element "<FormateVer>1.1.0</FormateVer>" differ from the other formats:
        // - "\n" after the closing <SdbXml> Tag is included in the checksum,
        // - the file's bytes are used as-is for the calculation, without CRLF conversion
        start = FindMarker(data, "<SdbXml>");
        end = FindMarker(data, "</SdbXml>") + 10; // including the \n at the end
      }
      else
      {
        start = dataAsText.IndexOf("<SdbXml>", StringComparison.Ordinal);
        end = dataAsText.IndexOf("</SdbXml>", StringComparison.Ordinal) + 9;
        // the TV calculates the checksum with just LF as newline character, so we need to replace CRLF first
        var text = dataAsText.Substring(start, end - start);
        if (this.newline == "\r\n")
          text = text.Replace("\r\n", "\n");

        data = Encoding.UTF8.GetBytes(text);
        start = 0;
        end = data.Length;
      }

      return ~Crc32.Normal.CalcCrc32(data, start, end - start);
    }
    #endregion

    #region FindMarker()
    private int FindMarker(byte[] data, string marker)
    {
      var bytes = Encoding.ASCII.GetBytes(marker);
      var len = bytes.Length;
      int i = -1;
      for (;;)
      {
        i = Array.IndexOf(data, bytes[0], i + 1);
        if (i < 0)
          return -1;

        int j;
        for (j = 1; j < len; j++)
        {
          if (data[i + j] != bytes[j])
            break;
        }

        if (j == len)
          return i;

        i += j - 1;
      }
    }
    #endregion

    #region ParseInt()
    private int ParseInt(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return 0;
      if (input.Length > 2 && input[0] == '0' && char.ToLower(input[1]) == 'x')
        return int.Parse(input.Substring(2), NumberStyles.HexNumber);
      if (int.TryParse(input, out var value))
        return value;
      return 0;
    }
    #endregion



    #region Save()
    public override void Save(string tvOutputFile)
    {
      if (this.channeListNodes.TryGetValue(SignalSource.DvbT, out var nodes))
      {
        var dvbt = this.DataRoot.GetChannelList(SignalSource.DvbT | SignalSource.Tv).Channels
          .Concat(this.DataRoot.GetChannelList(SignalSource.DvbT | SignalSource.Radio).Channels)
          .Concat(this.DataRoot.GetChannelList(SignalSource.DvbT | SignalSource.Data).Channels)
          .ToList();
        this.UpdateChannelList(dvbt, nodes);
      }

      if (this.channeListNodes.TryGetValue(SignalSource.DvbC, out nodes))
      {
        var dvbc = this.DataRoot.GetChannelList(SignalSource.DvbC | SignalSource.Tv).Channels
          .Concat(this.DataRoot.GetChannelList(SignalSource.DvbC | SignalSource.Radio).Channels)
          .Concat(this.DataRoot.GetChannelList(SignalSource.DvbC | SignalSource.Data).Channels)
          .ToList();
        this.UpdateChannelList(dvbc, nodes);
      }

      foreach (var list in this.DataRoot.ChannelLists)
      {
        if ((list.SignalSource & SignalSource.DvbS) == SignalSource.DvbS && this.channeListNodes.TryGetValue(list.SignalSource & ~SignalSource.MaskTvRadioData, out nodes))
          this.UpdateChannelList(list.Channels, nodes);
      }

      // by default .NET reformats the whole XML. These settings produce almost same format as the TV xml files use
      var xmlSettings = new XmlWriterSettings();
      xmlSettings.Encoding = this.DefaultEncoding;
      xmlSettings.CheckCharacters = false;
      xmlSettings.Indent = true;
      xmlSettings.IndentChars = "";
      xmlSettings.NewLineHandling = NewLineHandling.None;
      xmlSettings.NewLineChars = this.newline;
      xmlSettings.OmitXmlDeclaration = false;

      string xml;
      using (var sw = new StringWriter())
      using (var w = new CustomXmlWriter(sw, xmlSettings, isEFormat))
      {
        this.doc.WriteTo(w);
        w.Flush();
        xml = sw.ToString();
      }

      // elements with a 'loop="0"' attribute must contain a newline instead of <...></...>
      var emptyTagsWithNewline = new[] { "loop=\"0\">", "loop=\"0\" notation=\"DEC\">", "loop=\"0\" notation=\"HEX\">" };
      foreach (var tag in emptyTagsWithNewline)
        xml = xml.Replace(tag + "</", tag + this.newline + "</");

      if (isEFormat)
        xml = xml.Replace(" />", "/>");

      xml += this.newline;

      // put new checksum in place
      var newContent = Encoding.UTF8.GetBytes(xml);
      var crc = this.CalcChecksum(newContent, xml);
      var i1 = xml.LastIndexOf("</CheckSum>", StringComparison.Ordinal);
      var i0 = xml.LastIndexOf(">", i1, StringComparison.Ordinal);
      var hexCrc = this.isEFormat ? crc.ToString("x") : "0x" + crc.ToString("X");
      xml = xml.Substring(0, i0 + 1) + hexCrc + xml.Substring(i1);

      var enc = new UTF8Encoding(false, false);
      File.WriteAllText(tvOutputFile, xml, enc);
    }
    #endregion

    #region UpdateChannelList()
    private void UpdateChannelList(IList<ChannelInfo> channels, ChannelListNodes nodes)
    {
      if (this.isEFormat || channels.Any(ch => ch.IsNameModified))
        this.UpdateDataInChildNodes(nodes.Service, channels.OrderBy(c => c.RecordOrder), ch => true, ch => ch.ServiceData, this.GetNewValueForServiceNode);

      if (!this.isEFormat)
        this.UpdateDataInChildNodes(nodes.Programme, channels.OrderBy(c => c.NewProgramNr), ch => !(ch.IsDeleted || ch.NewProgramNr < 0), ch => ch.ProgrammeData, this.GetNewValueForProgrammeNode);
    }
    #endregion

    #region UpdateDataInChildNodes()
    void UpdateDataInChildNodes(
      XmlNode parentNode, 
      IEnumerable<ChannelInfo> channels, 
      Predicate<ChannelInfo> accept, 
      Func<Channel,Dictionary<string,string>> getChannelData, 
      Func<Channel, string, string, string> getNewValue)
    {
      var count = 0;
      var sbDict = new Dictionary<string, StringBuilder>();
      foreach (XmlNode node in parentNode.ChildNodes)
      {
        if (node.Attributes["loop"] != null)
          sbDict[node.LocalName] = new StringBuilder(this.newline);
      }

      foreach (var channel in channels)
      {
        var ch = channel as Channel;
        if (ch == null)
          continue; // ignore proxy channels from reference lists

        if (!accept(ch))
          continue;

        foreach (var field in getChannelData(ch))
        {
          var sb = sbDict[field.Key];
          var value = getNewValue(ch, field.Key, field.Value);
          sb.Append(value).Append(this.newline);
        }
        ++count;
      }
      foreach (XmlNode node in parentNode.ChildNodes)
      {
        if (sbDict.TryGetValue(node.LocalName, out var sb))
        {
          node.InnerText = sb.ToString();
          node.Attributes["loop"].InnerText = count.ToString();
        }
      }
    }
    #endregion

    #region GetNewValueForServiceNode()
    private string GetNewValueForServiceNode(Channel ch, string field, string value)
    {
      if (field == "Name")
        return ch.IsNameModified ? ch.Name : value;

      if (this.isEFormat)
      {
        if (field == "b_deleted_by_user")
          return ch.IsDeleted ? "0" : "1"; // file seems to contain reverse logic (1 = not deleted)
        if (field == "No")
          return ((ch.NewProgramNr << 18) | (int.Parse(value) & 0x3FFFF)).ToString();
        if (field == "ui4_nw_mask")
          return (((uint)ch.Favorites << 4) | (ch.Hidden ? 0u : 8u) | (uint.Parse(value) & ~0xF8)).ToString();
      }
      return value;
    }
    #endregion

    #region GetNewValueForProgrammeNode()
    private string GetNewValueForProgrammeNode(Channel ch, string field, string value)
    {
      if (field == "No")
        return ch.NewProgramNr.ToString();
      if (field == "Flag")
        return ((int)ch.Favorites & 0x0F).ToString();
      return value;
    }
    #endregion
  }

  class ChannelListNodes
  {
    public XmlNode Service;
    public XmlNode Programme;
  }
}
