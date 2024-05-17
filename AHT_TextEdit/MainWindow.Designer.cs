namespace AHT_TextEdit
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private System.Windows.Forms.Button BtnOpenFile;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SplitContainer panelsText;
        private System.Windows.Forms.ListView listSections;
        private System.Windows.Forms.ListView listText;
        private System.Windows.Forms.ColumnHeader SectionIndex;
        private System.Windows.Forms.ColumnHeader SectionHash;
        private System.Windows.Forms.ColumnHeader SectionNumItems;
        private System.Windows.Forms.ColumnHeader TextIndex;
        private System.Windows.Forms.ColumnHeader TextHash;
        private System.Windows.Forms.ColumnHeader TextStr;
        private System.Windows.Forms.Label Lbl_SectionListHeader;
        private System.Windows.Forms.Label Lbl_TestListHeader;
        private System.Windows.Forms.Label Lbl_GameVersion;
        private System.Windows.Forms.Button Btn_Edit;
        private System.Windows.Forms.ColumnHeader CharHash;
        private System.Windows.Forms.ColumnHeader SectionType;
        private System.Windows.Forms.ColumnHeader SectionLanguage;
    }
}

