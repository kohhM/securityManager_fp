﻿/*
<memo>
https://dobon.net/vb/dotnet/control/tbscrolltolast.html

csvはsjis

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;

namespace securityManager_fp
{
    public partial class Form1 : Form
    {
        FormConfig FormConfig = new FormConfig();

        List<string> BLDs = new List<string>() { "1号館","新2号館","8号館","10号館"};
        //要素は例．あとでpythonのプログラムのやり方に合わせて取得
        Dictionary<string, int> BLDstate = new Dictionary<string, int>();
        //0でセンサー反応,1で異常なし,2がスリープ状態

        Boolean isSleepingAll = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            for(int i = 0;i < BLDs.Count; i++)
            {
                BLDstate.Add(BLDs[i], 2);
            }
            //あとで変更されるだろう部分2

            comboBox1.Items.AddRange(BLDs.ToArray());
            comboSetting();
            richTextBox1.AppendText(timeStamp() + "security manager_fp\n");
            richTextBox1.AppendText(timeStamp()+"各建物のセンサー状態を管理します\n");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null) {
                if (BLDstate[comboBox1.SelectedItem.ToString()] == 2)
                {
                    //ここでimを使ってスリープを送り続けるのを止める
                    try
                    {
                        serialPort1.Write("TXDU 0003,0\r\n");
                        richTextBox1.AppendText(timeStamp());
                        richTextBox1.SelectionColor = Color.FromArgb(0, 153, 0);
                        richTextBox1.AppendText(label2.Text);
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "の監視を開始します\n");
                        BLDstate[comboBox1.SelectedItem.ToString()] = 1;
                        label2.ForeColor = Color.FromArgb(0, 153, 0);
                        button1.Text = "停止";
                    }
                    catch
                    {
                        richTextBox1.AppendText("仮想シリアル通信に失敗\n");
                        return;
                    }
                }
                else
                {
                    //ここでimを使ってスリープを送らせ続ける
                    try
                    {
                        serialPort1.Write("TXDU 0003,1\r\n");
                        richTextBox1.AppendText(timeStamp());
                        richTextBox1.SelectionColor = Color.FromArgb(32, 32, 32);
                        richTextBox1.AppendText(label2.Text);
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "を待機状態にします\n");
                        BLDstate[comboBox1.SelectedItem.ToString()] = 2;
                        label2.ForeColor = Color.FromArgb(32, 32, 32);
                        button1.Text = "起動";
                        
                    }
                    catch
                    {
                        richTextBox1.AppendText("仮想シリアル通信に失敗\n");
                        return;
                    }
                }
            }
            else
            {
                richTextBox1.AppendText(timeStamp()+"建物が選択されていません\n");
            }
        }

        static string timeStamp()
        {
            DateTime dt = DateTime.Now;
            string stamp = dt.ToString("[yyyy/MM/dd HH:mm:ss] ");

            return stamp;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (BLDstate[comboBox1.SelectedItem.ToString()])
            {
                case 0:
                    label2.ForeColor = Color.FromArgb(232,0,43);
                    button1.Text = "停止";
                    break;
                case 1:
                    label2.ForeColor = Color.FromArgb(0, 153, 0);
                    button1.Text = "停止";
                    break;
                default:
                    label2.ForeColor = Color.FromArgb(32, 32, 32);
                    button1.Text = "起動";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isSleepingAll == true)
            {
                try
                {
                    serialPort1.Write("TXDA 0\r\n");
                    richTextBox1.AppendText(timeStamp());
                    richTextBox1.SelectionColor = Color.FromArgb(0, 153, 0);
                    richTextBox1.AppendText(label2.Text);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.AppendText(" 全棟の監視を開始します\n");

                    for (int i = 0; i < BLDs.Count; i++)
                    {
                        BLDstate[BLDs[i]] = 1;
                    }

                    label2.ForeColor = Color.FromArgb(0, 153, 0);
                    label3.ForeColor = Color.FromArgb(0, 153, 0);
                    button1.Text = "停止";
                    isSleepingAll = false;
                }
                catch
                {
                    richTextBox1.AppendText("仮想シリアル通信に失敗\n");
                    return;
                }
            }
            else
            {
                try
                {
                    serialPort1.Write("TXDA 1\r\n");
                    richTextBox1.AppendText(timeStamp() + "● 全棟を待機状態にします\n");

                    for (int i = 0; i < BLDs.Count; i++)
                    {
                        BLDstate[BLDs[i]] = 2;
                    }

                    label2.ForeColor = Color.FromArgb(32, 32, 32);
                    label3.ForeColor = Color.FromArgb(32, 32, 32);
                    button1.Text = "起動";
                    isSleepingAll = true;
                }
                catch
                {
                    richTextBox1.AppendText("仮想シリアル通信に失敗\n");
                    return;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(timeStamp()+ " 各棟のステータスを表示\n");

            for(int i = 0; i < BLDs.Count; i++)
            {
                switch (BLDstate[BLDs[i]])
                {
                    case 0:
                        richTextBox1.SelectionColor = Color.FromArgb(232, 0, 43);
                        richTextBox1.AppendText("● ");
                        richTextBox1.SelectionColor = Color.Black;
                        break;
                    case 1:
                        richTextBox1.SelectionColor = Color.FromArgb(0, 153, 0);
                        richTextBox1.AppendText("● ");
                        richTextBox1.SelectionColor = Color.Black;
                        break;
                    default:
                        richTextBox1.AppendText("● ");
                        break;
                }
                richTextBox1.AppendText(BLDs[i]+"\n");
            }

            richTextBox1.AppendText("-------書き出し終了-------\n");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                richTextBox1.AppendText(timeStamp() + "[memo]" + textBox1.Text + "\n");
                textBox1.ResetText();
            }
        }

        private void comboSetting()
        {
            comboBox2.Items.Clear();
            string[] port = SerialPort.GetPortNames();
            for(int i = 0; i < port.Length; i++)
            {
                comboBox2.Items.Add(port[i]);
            }
        }

        private void com_close()
        {
            comboBox2.Enabled = true;
            button10.Enabled = true;
            if (button10.Enabled == false)
            {
                richTextBox1.AppendText(timeStamp() + comboBox2.Text + "から切断しました\n");
            }

            comboSetting();
            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                serialPort1.Close();
            }
            catch { };
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            com_close();
        }

        private void reload_Click(object sender, EventArgs e)
        {
            com_close();
            comboSetting();
        }

        private void COMopen(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                serialPort1.PortName = comboBox2.Text;
                try
                {
                    serialPort1.Open();
                    comboBox2.Enabled = false;
                    button9.Enabled = false;
                    button10.Enabled = false;

                    groupBox1.Enabled = true;
                    button4.Enabled = true;
                    button6.Enabled = true;
                    button8.Enabled = true;
                    richTextBox1.AppendText(timeStamp()+comboBox2.Text +"に接続しました\n");
                }
                catch
                {
                    richTextBox1.AppendText(timeStamp() + "接続に問題が発生しました\n接続を終了します\n");
                    com_close();
                }
            }
            else
            {
                richTextBox1.AppendText(timeStamp() + "COMポートを選択してください");
            }
        }

        private void com_reset(object sender, EventArgs e)
        {
            com_close();
            comboBox2.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;

            groupBox1.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            FormConfig.Visible = true;

            FormConfig.Show();
        }


        delegate void SetTextCallBack(string text);
        private void res(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(res);
                Invoke(d, new object[] { text });
                //beginInvokeで同期式

            }
            else
            {
                if(text.Contains("\r\n"))
                {
                    richTextBox1.AppendText(text);
                }
                //あとで受信するときはレングスとってそれ以上なら処理にする
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string received_data = sp.ReadExisting();

            res(received_data);

        }
    }
}
