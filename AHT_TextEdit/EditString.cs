using AHT_TextEdit.Common;
using AHT_TextEdit.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHT_TextEdit
{
    public partial class EditString : Form
    {
        MainWindow mainWindow = null;
        TextItem item = null;

        public EditString(MainWindow mainWindow, TextItem item)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.item = item;

            Box_Text.Text = string.Copy(item.Str);

            //There are 44 character hashcodes
            for (int i = 0; i < 44;  i++)
            {
                HT_Character_Index c = (HT_Character_Index)i;
                CBox_Character.Items.Add(c.ToString().Replace("HT_Character_", ""));

                //Check if this item is the selected one
                if (c.ToString() == ((HT_Character)item.UserData1).ToString())
                {
                    CBox_Character.SelectedIndex = i;
                }
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (item.Str != Box_Text.Text)
            {
                item.Edited = true;
                item.Str = Box_Text.Text;
            }

            //This sucks but oh well
            //Grabs the string version of the selected character and converts it to a hashcode value to store in the text item.
            HT_Character_Index charSelected = (HT_Character_Index)CBox_Character.SelectedIndex;
            HT_Character charHash = (HT_Character)Enum.Parse(typeof(HT_Character), charSelected.ToString());

            if (item.UserData1 != (uint)charHash)
            {
                item.Edited = true;
                item.UserData1 = (uint)charHash;
            }

            mainWindow.UpdateTextItem(item);

            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
