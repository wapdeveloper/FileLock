using System;
using System.Collections.Generic;
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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Text.RegularExpressions;


namespace GeneratePassword
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

        //加密
        string EncryptionInfo(string strInput, byte key)
        {
            byte[] buffer_o = Encoding.Default.GetBytes(strInput);
            List<byte> buffer = new List<byte>();
            for (int i = 0; i < buffer_o.Length; i++)
            {
                buffer.Add(buffer_o[i]);
            }
            buffer.Add(key);

            //保存字符串的矩阵
            Matrix<double> mb = Matrix<double>.Build.Dense(2, 2);

            int k = 0;
            for (int i = 0; i < 4; i++, k++)
            {
                mb[k / 2, i % 2] = buffer[i];
            }

            //转置
            Matrix<double> mbT = mb.Transpose();

            //赋值
            for (int i = 0; i < mbT.RowCount; i++)
            {
                for (int j = 0; j < mbT.ColumnCount; j++)
                {
                    buffer[j + i * 2] = (byte)mbT[i, j]; //行
                }
            }
           
            buffer[0] +=(byte)(key+1);
            buffer[1] +=(byte)(key-2);
            buffer[2] +=(byte)(key+3);
            buffer[3] +=(byte)(key-4);

            string strOutput = "";

            for (int i = 0; i < 4; i++)
            {
                int value = Convert.ToInt32(buffer[i]);
                //十进制转为十六进制
                string hexOutput = String.Format("{0:X}", value);
                strOutput += hexOutput;
            }

            if(buffer[4]<10)
                strOutput +="0"+String.Format("{0:X}", buffer[4]);
            else
                strOutput += String.Format("{0:X}", buffer[4]);

            return strOutput;
        }

        //解密
        void DecryptInfo(ref string strOutput,out byte key)
        {
            List<byte> buffer = new List<byte>();
            List<Match> each = Regex.Matches(strOutput, @"..").Cast<Match>().ToList();
            foreach (Match item in each)
            {
                //十六进制转为十进制
                byte b = (byte)Int32.Parse(item.ToString(), System.Globalization.NumberStyles.HexNumber);
                buffer.Add(b);
            }
            key = buffer[4];

            buffer[0] -= (byte)(key + 1);
            buffer[1] -= (byte)(key - 2);
            buffer[2] -= (byte)(key + 3);
            buffer[3] -= (byte)(key - 4);

            Matrix<double> nmb = Matrix<double>.Build.Dense(2, 2);
            int k = 0;
            //赋值
            for (int i = 0; i < 4; i++, k++)
            {
                nmb[k / 2, i % 2] = buffer[i];
            }

            Matrix<double> nmbT = nmb.Transpose();

            for (int i = 0; i < nmbT.RowCount; i++)
            {
                for (int j = 0; j < nmbT.ColumnCount; j++)
                {
                    buffer[j + i * 2] = (byte)nmbT[i, j];
                }
            }
            byte[] byteArray = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byteArray[i] = buffer[i];
            }
            strOutput = Encoding.Default.GetString(byteArray);

        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            string strInput, strOutput = "";

            strInput = input.Text;
            if (t1.Text != "")
            {
                byte key = byte.Parse(t1.Text);
                strOutput = EncryptionInfo(strInput, key);
            }
            output.Text = strOutput;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            string strOutput = "";
            strOutput = output.Text;
            byte key = 0;
            DecryptInfo(ref strOutput, out key);
            t1.Text = key.ToString();
            input.Text = strOutput;
        }

    }
}
