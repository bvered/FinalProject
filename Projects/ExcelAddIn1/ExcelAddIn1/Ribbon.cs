using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using Range = Microsoft.Office.Interop.Excel.Range;
using WordApplication = Microsoft.Office.Interop.Word.Application;
using System.Diagnostics;

namespace CreateSendCertificateAddin
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void BtnGenerateClick(object sender, RibbonControlEventArgs e)
        {
            ExportToWord();
        }

        private void ExportToWord()
        {
            var ap = new WordApplication();

            try
            {
                var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;

                var worksheet = (Worksheet) activeWorkbook.ActiveSheet;

                var fileDialog = new OpenFileDialog() {Filter = @"Template Files|*.dotx", CheckFileExists = true, Multiselect = false, Title = @"בחר קובץ תבנית"};

                DialogResult dialogResult = fileDialog.ShowDialog();

                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }

                string templateFile = fileDialog.FileName; //ConfigurationManager.AppSettings["TemplateFile"];
                string tableBookmarkName = ConfigurationManager.AppSettings["TableBookmarkName"];

                Document doc = ap.Documents.Add(templateFile);
                doc.Activate();

                Bookmark tableBookmark = null;

                foreach (Bookmark bookmark in doc.Bookmarks)
                {
                    if (tableBookmarkName == bookmark.Name)
                    {
                        tableBookmark = bookmark;
                        break;
                    }

                    Name name = activeWorkbook.Names.Item(bookmark.Name.TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', '9', '0'));

                    if (name != null && name.RefersToRange.Value != null)
                    {
                        bookmark.Range.Text = name.RefersToRange.Value.ToString();
                    }
                }

                Range bodyRange = GetBodyRange(worksheet);

                bodyRange.Copy();

                Microsoft.Office.Interop.Word.Range range;

                range = tableBookmark != null ? tableBookmark.Range : doc.Range(doc.Content.End, doc.Content.End);

                range.PasteExcelTable(false, false, false);

                Table table = doc.Tables[1];
                table.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitContent);
                table.Rows[1].HeadingFormat = -1;
                ap.Visible = true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", "Exception Caught: " + ex.Message);
                ap.Quit(false);
            }
        }


        private Range GetBodyRange(Worksheet worksheet)
        {
            return worksheet.UsedRange;
        }
    }
}
