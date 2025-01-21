using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ikat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a folder";
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    label2.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.PASS_TITLE = "Encryption File";
            frm.PASS_PATH = label2.Text;
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.PASS_TITLE = "Decryption File";
            frm.PASS_PATH = label2.Text;
            frm.Show();
        }
    }
}
