using AHT_TextEdit.Common;
using AHT_TextEdit.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Windows.Forms;

namespace AHT_TextEdit
{
    public partial class MainWindow : Form
    {
        public static GamePlatform Platform;
        public static TextEDB ViewingFile;
        public static int selectedSection;

        public MainWindow()
        {
            InitializeComponent();

            //FileHandler.WriteTestFile();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.BtnOpenFile = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panelsText = new System.Windows.Forms.SplitContainer();
            this.Lbl_SectionListHeader = new System.Windows.Forms.Label();
            this.listSections = new System.Windows.Forms.ListView();
            this.SectionIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionNumItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Btn_Edit = new System.Windows.Forms.Button();
            this.Lbl_TestListHeader = new System.Windows.Forms.Label();
            this.listText = new System.Windows.Forms.ListView();
            this.TextIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TextHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TextStr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Lbl_GameVersion = new System.Windows.Forms.Label();
            this.CharHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.panelsText)).BeginInit();
            this.panelsText.Panel1.SuspendLayout();
            this.panelsText.Panel2.SuspendLayout();
            this.panelsText.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOpenFile
            // 
            this.BtnOpenFile.Location = new System.Drawing.Point(12, 12);
            this.BtnOpenFile.Name = "BtnOpenFile";
            this.BtnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.BtnOpenFile.TabIndex = 1;
            this.BtnOpenFile.Text = "Open File";
            this.BtnOpenFile.UseVisualStyleBackColor = true;
            this.BtnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(93, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panelsText
            // 
            this.panelsText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelsText.BackColor = System.Drawing.Color.Silver;
            this.panelsText.Location = new System.Drawing.Point(12, 41);
            this.panelsText.Name = "panelsText";
            // 
            // panelsText.Panel1
            // 
            this.panelsText.Panel1.Controls.Add(this.Lbl_SectionListHeader);
            this.panelsText.Panel1.Controls.Add(this.listSections);
            // 
            // panelsText.Panel2
            // 
            this.panelsText.Panel2.Controls.Add(this.Btn_Edit);
            this.panelsText.Panel2.Controls.Add(this.Lbl_TestListHeader);
            this.panelsText.Panel2.Controls.Add(this.listText);
            this.panelsText.Size = new System.Drawing.Size(1549, 810);
            this.panelsText.SplitterDistance = 348;
            this.panelsText.TabIndex = 3;
            // 
            // Lbl_SectionListHeader
            // 
            this.Lbl_SectionListHeader.AutoSize = true;
            this.Lbl_SectionListHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_SectionListHeader.Location = new System.Drawing.Point(3, 8);
            this.Lbl_SectionListHeader.Name = "Lbl_SectionListHeader";
            this.Lbl_SectionListHeader.Size = new System.Drawing.Size(74, 18);
            this.Lbl_SectionListHeader.TabIndex = 1;
            this.Lbl_SectionListHeader.Text = "Sections";
            // 
            // listSections
            // 
            this.listSections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSections.BackColor = System.Drawing.SystemColors.Menu;
            this.listSections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SectionIndex,
            this.SectionHash,
            this.SectionType,
            this.SectionLanguage,
            this.SectionNumItems});
            this.listSections.FullRowSelect = true;
            this.listSections.HideSelection = false;
            this.listSections.Location = new System.Drawing.Point(3, 29);
            this.listSections.MultiSelect = false;
            this.listSections.Name = "listSections";
            this.listSections.Size = new System.Drawing.Size(342, 778);
            this.listSections.TabIndex = 0;
            this.listSections.UseCompatibleStateImageBehavior = false;
            this.listSections.View = System.Windows.Forms.View.Details;
            this.listSections.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listSections_MouseDoubleClick);
            // 
            // SectionIndex
            // 
            this.SectionIndex.Text = "Index";
            this.SectionIndex.Width = 44;
            // 
            // SectionHash
            // 
            this.SectionHash.Text = "HashCode";
            this.SectionHash.Width = 76;
            // 
            // SectionNumItems
            // 
            this.SectionNumItems.Text = "Count";
            this.SectionNumItems.Width = 90;
            // 
            // Btn_Edit
            // 
            this.Btn_Edit.Location = new System.Drawing.Point(5, 5);
            this.Btn_Edit.Name = "Btn_Edit";
            this.Btn_Edit.Size = new System.Drawing.Size(88, 23);
            this.Btn_Edit.TabIndex = 4;
            this.Btn_Edit.Text = "Edit Selected";
            this.Btn_Edit.UseVisualStyleBackColor = true;
            this.Btn_Edit.Click += new System.EventHandler(this.Btn_Edit_Click);
            // 
            // Lbl_TestListHeader
            // 
            this.Lbl_TestListHeader.AutoSize = true;
            this.Lbl_TestListHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_TestListHeader.Location = new System.Drawing.Point(99, 8);
            this.Lbl_TestListHeader.Name = "Lbl_TestListHeader";
            this.Lbl_TestListHeader.Size = new System.Drawing.Size(152, 18);
            this.Lbl_TestListHeader.TabIndex = 2;
            this.Lbl_TestListHeader.Text = "No Section Loaded";
            // 
            // listText
            // 
            this.listText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listText.BackColor = System.Drawing.SystemColors.Menu;
            this.listText.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TextIndex,
            this.TextHash,
            this.CharHash,
            this.TextStr});
            this.listText.FullRowSelect = true;
            this.listText.HideSelection = false;
            this.listText.Location = new System.Drawing.Point(3, 29);
            this.listText.MultiSelect = false;
            this.listText.Name = "listText";
            this.listText.Size = new System.Drawing.Size(1191, 778);
            this.listText.TabIndex = 0;
            this.listText.UseCompatibleStateImageBehavior = false;
            this.listText.View = System.Windows.Forms.View.Details;
            this.listText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listText_MouseDoubleClick);
            // 
            // TextIndex
            // 
            this.TextIndex.Text = "Index";
            this.TextIndex.Width = 44;
            // 
            // TextHash
            // 
            this.TextHash.Text = "HashCode";
            this.TextHash.Width = 116;
            // 
            // TextStr
            // 
            this.TextStr.Text = "Contents";
            this.TextStr.Width = 853;
            // 
            // Lbl_GameVersion
            // 
            this.Lbl_GameVersion.AutoSize = true;
            this.Lbl_GameVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_GameVersion.Location = new System.Drawing.Point(174, 13);
            this.Lbl_GameVersion.Name = "Lbl_GameVersion";
            this.Lbl_GameVersion.Size = new System.Drawing.Size(122, 18);
            this.Lbl_GameVersion.TabIndex = 3;
            this.Lbl_GameVersion.Text = "No File Loaded";
            // 
            // CharHash
            // 
            this.CharHash.Text = "Character";
            this.CharHash.Width = 165;
            // 
            // SectionType
            // 
            this.SectionType.Text = "Contents";
            this.SectionType.Width = 65;
            // 
            // SectionLanguage
            // 
            this.SectionLanguage.Text = "Language";
            // 
            // MainWindow
            // 
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1573, 863);
            this.Controls.Add(this.Lbl_GameVersion);
            this.Controls.Add(this.BtnOpenFile);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.panelsText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "AHT Text Editor";
            this.panelsText.Panel1.ResumeLayout(false);
            this.panelsText.Panel1.PerformLayout();
            this.panelsText.Panel2.ResumeLayout(false);
            this.panelsText.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelsText)).EndInit();
            this.panelsText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "GeoFiles (*.edb)|*.edb";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        Platform = FileHandler.CheckPlatform(filePath);
                        ViewingFile = FileHandler.ReadFile(filePath, Platform);

                        Lbl_GameVersion.Text = string.Format("Loaded text file for {0}", Platform.ToString());
                        PopulateSectionList();
                    } catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Error opening file",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PopulateSectionList()
        {
            ResetLists();

            //Check for instantiation
            if (ViewingFile == null) { return; }

            SpreadSheetSectionList list = ViewingFile.SpreadSheetSectionList;

            for (int i = 0; i < list.Items.Count; i++)
            {
                ListViewItem item = new ListViewItem(list.Items[i].SectionIndex.ToString());
                item.SubItems.Add(String.Format("{0:X}", list.Items[i].SectionHashCode));

                //Grab 1st byte to get type
                SectionContents type = (SectionContents)(list.Items[i].SectionHashCode & 0xFF);
                item.SubItems.Add(type.ToString());

                //Grab 3rd byte to get language
                SectionLanguage lang = (SectionLanguage)(list.Items[i].SectionHashCode & 0xFF0000);
                item.SubItems.Add(lang.ToString());

                item.SubItems.Add(list.Items[i].Items.Count.ToString());

                listSections.Items.Add(item);
            }
        }

        public void PopulateTextList(ListViewItem selectedSection)
        {
            listText.Items.Clear();

            //No error handling because it could never possibly go wrong, right :)
            int sectIndex = int.Parse(selectedSection.Text) - 1;

            List<TextItem> list = ViewingFile.SpreadSheetSectionList.Items[sectIndex].Items;

            foreach (TextItem txt in list)
            {
                string strChar = ((HT_Character)txt.UserData1).ToString();
                strChar = strChar.Replace("HT_Character_", "");

                ListViewItem item = new ListViewItem(txt.TextIndex.ToString());
                //Remove "HT_Text" and "HT_MemCardText"
                item.SubItems.Add(txt.TextHash.ToString().Replace("HT_Text_", "").Replace("HT_MemCardText_", ""));
                item.SubItems.Add(strChar);
                item.SubItems.Add(txt.Str);

                if (txt.Edited)
                {
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }

                listText.Items.Add(item);
            }
        }

        public void UpdateTextItem(TextItem item)
        {
            foreach (ListViewItem lvi in listText.Items)
            {
                if (item.TextIndex == int.Parse(lvi.Text))
                {
                    //I should probably put this in a service method
                    lvi.SubItems[2].Text = ((HT_Character)item.UserData1).ToString().Replace("HT_Character_", "");
                    lvi.SubItems[3].Text = item.Str;

                    if (item.Edited)
                    {
                        lvi.Font = new Font(lvi.Font, FontStyle.Bold);
                    }
                    
                    return;
                }
            }
        }

        private void listSections_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UpdateTextList();
        }

        public void UpdateTextList()
        {
            try
            {
                ListViewItem item = listSections.SelectedItems[0];
                selectedSection = int.Parse(item.Text);
                PopulateTextList(item);

                Lbl_TestListHeader.Text = string.Format("Section Loaded: {0} ({1}) - {2}, {3}",
                    item.Text, item.SubItems[1].Text, item.SubItems[3].Text, item.SubItems[2].Text);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("No item was selected!");
            }
        }

        private void ResetLists()
        {
            listSections.Items.Clear();
            listText.Items.Clear();
            Lbl_TestListHeader.Text = "No Section Loaded";
        }

        private void Btn_Edit_Click(object sender, EventArgs e)
        {
            OpenTextEditWnd();
        }

        private void listText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenTextEditWnd();
        }

        private void OpenTextEditWnd()
        {
            if (listText.SelectedItems.Count == 0)
            {
                return;
            }

            //Get selected TextItem
            ListViewItem item = listText.SelectedItems[0];
            int selectedItem = int.Parse(item.Text);
            TextItem selected = ViewingFile.SpreadSheetSectionList.Items[selectedSection - 1].Items[selectedItem];

            EditString editString = new EditString(this, selected);
            editString.StartPosition = FormStartPosition.CenterParent;
            editString.ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ViewingFile == null)
            {
                MessageBox.Show("Open a text file first before exporting a new one.", "Nothing to export",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "GeoFiles (*.edb)|*.edb";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = saveFileDialog.FileName;
                    try
                    {
                        FileHandler.WriteFile(filePath, Platform, ViewingFile);
                        MessageBox.Show("Saved .edb file to "+filePath, "Export success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Error saving file",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
