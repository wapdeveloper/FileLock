using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileLock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void opne_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt|exe文件|*.exe|所有文件|*.*";

            if (ofd.ShowDialog() == true)
            {
                txtFileName.Text = ofd.FileName;
            }
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string inFile = txtFileName.Text;
            string outFile = inFile + ".exe";
            string password = txtPassword.Password;
            DESFileClass.EncryptFile(inFile, outFile, password);//加密文件
            //删除加密前的文件
            //File.Delete(inFile);
            txtFileName.Text = string.Empty;
            MessageBox.Show("Decrypt Comleted!");
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            string inFile = txtFileName.Text;
            if (inFile != "")
            {
                string outFile = inFile.Substring(0, inFile.Length - 4);
                string password = txtPassword.Password;
                DESFileClass.DecryptFile(inFile, outFile, password);//解密文件
                //删除解密前的文件
                File.Delete(inFile);
                txtFileName.Text = string.Empty;
                MessageBox.Show("Encrypt Comleted!");
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chinese password is supported! But you need to type your password in notepad first!");
        }
    }
}
