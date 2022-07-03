﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChanSort.Ui.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ChanSort.Ui.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Donate {
            get {
                object obj = ResourceManager.GetObject("Donate", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Erase all channel data.
        /// </summary>
        internal static string MainForm_btnResetChannelData_Click_Caption {
            get {
                return ResourceManager.GetString("MainForm_btnResetChannelData_Click_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WARNING: All analog, DVB-C/T and DVB-S channel and transponder data will be deleted.
        ///You will have to run a full channel scan after loading this file into your TV.
        ///Proceed?.
        /// </summary>
        internal static string MainForm_btnResetChannelData_Click_Message {
            get {
                return ResourceManager.GetString("MainForm_btnResetChannelData_Click_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string MainForm_Cancel {
            get {
                return ResourceManager.GetString("MainForm_Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The channel list has been copied to the clipboard
        ///and can be pasted into Excel or any text editor..
        /// </summary>
        internal static string MainForm_ExportExcelList_Message {
            get {
                return ResourceManager.GetString("MainForm_ExportExcelList_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Supported Files|{0}|All Files (*.*)|*.
        /// </summary>
        internal static string MainForm_FileDialog_OpenFileFilter {
            get {
                return ResourceManager.GetString("MainForm_FileDialog_OpenFileFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}-Files|*{0}|All Files (*.*)|*.
        /// </summary>
        internal static string MainForm_FileDialog_SaveFileFilter {
            get {
                return ResourceManager.GetString("MainForm_FileDialog_SaveFileFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your TV might not be able to work correctly when there are gaps in the channel numbers.
        ///Do you want the channel numbers to be rearranged consecutively?.
        /// </summary>
        internal static string MainForm_HandleChannelNumberGaps {
            get {
                return ResourceManager.GetString("MainForm_HandleChannelNumberGaps", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modify current list: Start with current list and modify it as needed.
        /// </summary>
        internal static string MainForm_InitInitialChannelOrder_CurrentList {
            get {
                return ResourceManager.GetString("MainForm_InitInitialChannelOrder_CurrentList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create new list: Start with an empty list and add channels in desired order.
        /// </summary>
        internal static string MainForm_InitInitialChannelOrder_EmptyList {
            get {
                return ResourceManager.GetString("MainForm_InitInitialChannelOrder_EmptyList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How would you like to edit your channel list?.
        /// </summary>
        internal static string MainForm_InitInitialChannelOrder_Question {
            get {
                return ResourceManager.GetString("MainForm_InitInitialChannelOrder_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copy channel list: Order the channels the same way as in a reference list.
        /// </summary>
        internal static string MainForm_InitInitialChannelOrder_ReferenceList {
            get {
                return ResourceManager.GetString("MainForm_InitInitialChannelOrder_ReferenceList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The content of the file is invalid. It has either size 0 or all its bytes have the same value.
        ///Typical causes are USB sticks with an NTFS file system (try using FAT32 instead)
        ///or firmware upgrades without running a new channel scan. 
        ///(The new software in the TV might be unable to process the old channel data during the export.).
        /// </summary>
        internal static string MainForm_LoadFiles_AllZero {
            get {
                return ResourceManager.GetString("MainForm_LoadFiles_AllZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The input file contains multiple records that use the same 
        ///program number. It is possible that the TV will not accept
        ///the changes made by ChanSort.
        ///This is typically caused by running a manual transponder scan.
        ///It is recommended to use a clean input file for any modifications.
        ///To do that, turn Hotel Mode OFF, reset the TV to 
        ///factory defaults, run a new blind channel scan and turn
        ///Hotel Mode back ON, then export a new clean TLL file..
        /// </summary>
        internal static string MainForm_LoadFiles_DupeWarningMsg {
            get {
                return ResourceManager.GetString("MainForm_LoadFiles_DupeWarningMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading file.
        /// </summary>
        internal static string MainForm_LoadFiles_IOException {
            get {
                return ResourceManager.GetString("MainForm_LoadFiles_IOException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data validation.
        /// </summary>
        internal static string MainForm_LoadFiles_ValidationWarningCap {
            get {
                return ResourceManager.GetString("MainForm_LoadFiles_ValidationWarningCap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file content shows some anomalies and is possibly corrupt..
        /// </summary>
        internal static string MainForm_LoadFiles_ValidationWarningMsg {
            get {
                return ResourceManager.GetString("MainForm_LoadFiles_ValidationWarningMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured while loading the TV&apos;s data file:
        ///{0}.
        /// </summary>
        internal static string MainForm_LoadTll_Exception {
            get {
                return ResourceManager.GetString("MainForm_LoadTll_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file is not a valid .zip archive.
        ///TVs often export corrupted files to USB sticks formatted with the NTFS file system.
        ///Please try exporting to a stick formatted with FAT32.
        /// </summary>
        internal static string MainForm_LoadTll_InvalidZip {
            get {
                return ResourceManager.GetString("MainForm_LoadTll_InvalidZip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No plugin found to read/write {0} files..
        /// </summary>
        internal static string MainForm_LoadTll_SerializerNotFound {
            get {
                return ResourceManager.GetString("MainForm_LoadTll_SerializerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quell-TLL konnte nicht gefunden werden:
        ///&apos;{0}&apos;.
        /// </summary>
        internal static string MainForm_LoadTll_SourceTllNotFound {
            get {
                return ResourceManager.GetString("MainForm_LoadTll_SourceTllNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File not found.
        /// </summary>
        internal static string MainForm_LoadTvDataFile_FileNotFound_Caption {
            get {
                return ResourceManager.GetString("MainForm_LoadTvDataFile_FileNotFound_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {0} does not exist..
        /// </summary>
        internal static string MainForm_LoadTvDataFile_FileNotFound_Message {
            get {
                return ResourceManager.GetString("MainForm_LoadTvDataFile_FileNotFound_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to restore the backup file. All changes will be lost!
        ///Do you want to continue?.
        /// </summary>
        internal static string MainForm_miRestoreOriginal_ItemClick_Confirm {
            get {
                return ResourceManager.GetString("MainForm_miRestoreOriginal_ItemClick_Confirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No backup file found: {0}.
        /// </summary>
        internal static string MainForm_miRestoreOriginal_ItemClick_NoBackup {
            get {
                return ResourceManager.GetString("MainForm_miRestoreOriginal_ItemClick_NoBackup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to copy .bak file to {0}.
        /// </summary>
        internal static string MainForm_miRestoreOriginal_Message {
            get {
                return ResourceManager.GetString("MainForm_miRestoreOriginal_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Append unsorted channels the end of the list.
        /// </summary>
        internal static string MainForm_PromptHandlingOfUnsortedChannels_Append {
            get {
                return ResourceManager.GetString("MainForm_PromptHandlingOfUnsortedChannels_Append", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete unsorted channels from the list.
        /// </summary>
        internal static string MainForm_PromptHandlingOfUnsortedChannels_Delete {
            get {
                return ResourceManager.GetString("MainForm_PromptHandlingOfUnsortedChannels_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What should happen to unsorted channels?.
        /// </summary>
        internal static string MainForm_PromptHandlingOfUnsortedChannels_Question {
            get {
                return ResourceManager.GetString("MainForm_PromptHandlingOfUnsortedChannels_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Discard changes.
        /// </summary>
        internal static string MainForm_PromptSaveAndContinue_Discard {
            get {
                return ResourceManager.GetString("MainForm_PromptSaveAndContinue_Discard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want so save your changes?.
        /// </summary>
        internal static string MainForm_PromptSaveAndContinue_Question {
            get {
                return ResourceManager.GetString("MainForm_PromptSaveAndContinue_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save changes.
        /// </summary>
        internal static string MainForm_PromptSaveAndContinue_Save {
            get {
                return ResourceManager.GetString("MainForm_PromptSaveAndContinue_Save", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restore order from channel scan.
        /// </summary>
        internal static string MainForm_RestoreScanOrder_Caption {
            get {
                return ResourceManager.GetString("MainForm_RestoreScanOrder_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All custom storing will be lost.
        ///Are you sure you want to restore the order from the channel scan?.
        /// </summary>
        internal static string MainForm_RestoreScanOrder_Message {
            get {
                return ResourceManager.GetString("MainForm_RestoreScanOrder_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error saving the file. Please make sure that
        ///- you have write permission on the file
        ///- the file is not open in another program
        ///
        ///The error message is:.
        /// </summary>
        internal static string MainForm_SaveFiles_ErrorMsg {
            get {
                return ResourceManager.GetString("MainForm_SaveFiles_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File write error.
        /// </summary>
        internal static string MainForm_SaveFiles_ErrorTitle {
            get {
                return ResourceManager.GetString("MainForm_SaveFiles_ErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured while writing the TV data file:
        ///{0}.
        /// </summary>
        internal static string MainForm_SaveTllFile_Exception {
            get {
                return ResourceManager.GetString("MainForm_SaveTllFile_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorted TV data file was created successfully..
        /// </summary>
        internal static string MainForm_SaveTllFile_Success {
            get {
                return ResourceManager.GetString("MainForm_SaveTllFile_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ChanSort Reference List|*.csv|SamToolBox Reference List|*.chl|All Reference Lists|*.csv;*.chl.
        /// </summary>
        internal static string MainForm_ShowOpenReferenceFileDialog_Filter {
            get {
                return ResourceManager.GetString("MainForm_ShowOpenReferenceFileDialog_Filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Open Reference List.
        /// </summary>
        internal static string MainForm_ShowOpenReferenceFileDialog_Title {
            get {
                return ResourceManager.GetString("MainForm_ShowOpenReferenceFileDialog_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To swap channels an equal number of rows must be selected in the left and right table..
        /// </summary>
        internal static string MainForm_SwapChannels_RowCountMsg {
            get {
                return ResourceManager.GetString("MainForm_SwapChannels_RowCountMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Swap Channels.
        /// </summary>
        internal static string MainForm_SwapChannels_RowCountTitle {
            get {
                return ResourceManager.GetString("MainForm_SwapChannels_RowCountTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occured:
        ///{0}.
        /// </summary>
        internal static string MainForm_TryExecute_Exception {
            get {
                return ResourceManager.GetString("MainForm_TryExecute_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;body&gt;
        ///&lt;p style=&quot;font-family:Arial;font-size:12pt&quot;&gt;PayPal donation page is being opened...&lt;/p&gt;
        ///&lt;p&gt;&lt;/p&gt;
        ///&lt;p style=&quot;font-family:Arial;font-size:12pt&quot;&gt;PayPal Spendenseite wird ge&amp;ouml;ffnet...&lt;/p&gt;
        ///&lt;form action=&quot;https://www.paypal.com/cgi-bin/webscr&quot; method=&quot;post&quot;&gt;
        ///&lt;input type=&quot;hidden&quot; name=&quot;cmd&quot; value=&quot;_s-xclick&quot;&gt;
        ///&lt;input type=&quot;hidden&quot; name=&quot;encrypted&quot; value=&quot;-----BEGIN PKCS7-----MIIHVwYJKoZIhvcNAQcEoIIHSDCCB0QCAQExggEwMIIBLAIBADCBlDCBjjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRYwFAYDVQQHEw1Nb3VudGFpbiBW [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string paypal_button {
            get {
                return ResourceManager.GetString("paypal_button", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Antenna,Cable,Sat,IP,Analog,Digital,TV,Radio,Data.
        /// </summary>
        internal static string ReferenceListForm_AntennaCableSatIPAnalogDigitalTVRadio {
            get {
                return ResourceManager.GetString("ReferenceListForm_AntennaCableSatIPAnalogDigitalTVRadio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clear target list before applying reference list.
        /// </summary>
        internal static string ReferenceListForm_btnApply_Click_Clear {
            get {
                return ResourceManager.GetString("ReferenceListForm_btnApply_Click_Clear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keep current channel at the Pr#.
        /// </summary>
        internal static string ReferenceListForm_btnApply_Click_Keep {
            get {
                return ResourceManager.GetString("ReferenceListForm_btnApply_Click_Keep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Overwrite Pr# with the channel from the reference list.
        /// </summary>
        internal static string ReferenceListForm_btnApply_Click_Overwrite {
            get {
                return ResourceManager.GetString("ReferenceListForm_btnApply_Click_Overwrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How do you want to handle it when a Pr# is already taken in the target list?.
        /// </summary>
        internal static string ReferenceListForm_btnApply_ConflictHandling {
            get {
                return ResourceManager.GetString("ReferenceListForm_btnApply_ConflictHandling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a reference list to import.
        /// </summary>
        internal static string ReferenceListForm_ShowOpenFileDialog_Title {
            get {
                return ResourceManager.GetString("ReferenceListForm_ShowOpenFileDialog_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Version.
        /// </summary>
        internal static string UpdateCheck_NotifyAboutNewVersion_Caption {
            get {
                return ResourceManager.GetString("UpdateCheck_NotifyAboutNewVersion_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A newer version is available: v{0}.
        ///Do you want to open the download website?.
        /// </summary>
        internal static string UpdateCheck_NotifyAboutNewVersion_Message {
            get {
                return ResourceManager.GetString("UpdateCheck_NotifyAboutNewVersion_Message", resourceCulture);
            }
        }
    }
}
