using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Security.Cryptography;


namespace Ikat
{
    public partial class Form2 : Form
    {
        public String PASS_PATH = null;
        public String PASS_TITLE = null;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = PASS_TITLE;
            button2.Text = "Process " + PASS_TITLE;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) { 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    label2.Text = openFileDialog.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PASS_TITLE == "Encryption File")
            {
                try
                {
                    string filePath = label2.Text;
                    string password = textBox1.Text;

                    if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Please select a file and enter a password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    byte[] encryptedData = EncryptFile(File.ReadAllBytes(filePath), password);
                    string encryptedFileName = Path.GetFileName(filePath);
                    //string encryptedFilePath = Path.ChangeExtension(PASS_PATH + "/" + encryptedFileName, ".aes");
                    string encryptedFilePath = PASS_PATH + "/" + encryptedFileName;
                    File.WriteAllBytes(encryptedFilePath, encryptedData);

                    MessageBox.Show("File encrypted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (PASS_TITLE == "Decryption File")
            {
                try
                {
                    string filePath = label2.Text;
                    string password = textBox1.Text;

                    if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Please select a file and enter a password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    /*
                    if (Path.GetExtension(filePath) != ".aes")
                    {
                        MessageBox.Show("The selected file is not a valid encrypted file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                     * */

                    byte[] decryptedData = DecryptFile(File.ReadAllBytes(filePath), password);
                    string encryptedFileName = Path.GetFileName(filePath);
                    //string originalFilePath = Path.ChangeExtension(PASS_PATH + "/" + encryptedFileName, null);
                    string originalFilePath = PASS_PATH + "/" + encryptedFileName;
                    File.WriteAllBytes(originalFilePath, decryptedData);

                    MessageBox.Show("File decrypted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /*######################*/
        private byte[] EncryptFile(byte[] fileData, string password)
        {
            Aes aes = Aes.Create();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = null;

            try
            {
                byte[] key = new byte[32];
                byte[] iv = new byte[16];

                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("AES_SALT"), 10000);
                key = keyDerivation.GetBytes(32);
                iv = keyDerivation.GetBytes(16);

                aes.Key = key;
                aes.IV = iv;

                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(fileData, 0, fileData.Length);
                cryptoStream.FlushFinalBlock();

                return memoryStream.ToArray();
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Dispose();
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                if (aes != null)
                {
                    aes.Dispose();
                }
            }
        }

        private byte[] DecryptFile(byte[] encryptedData, string password)
        {
            Aes aes = Aes.Create();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = null;

            try
            {
                byte[] key = new byte[32];
                byte[] iv = new byte[16];

                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("AES_SALT"), 10000);
                key = keyDerivation.GetBytes(32);
                iv = keyDerivation.GetBytes(16);

                aes.Key = key;
                aes.IV = iv;

                cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                cryptoStream.FlushFinalBlock();

                return memoryStream.ToArray();
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Dispose();
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                if (aes != null)
                {
                    aes.Dispose();
                }
            }
        }
    }
}
