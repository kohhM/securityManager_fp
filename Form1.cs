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

namespace securityManager_fp
{
    public partial class Form1 : Form
    {
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
                    richTextBox1.AppendText(timeStamp());
                    richTextBox1.SelectionColor = Color.FromArgb(0,153,0);
                    richTextBox1.AppendText(label2.Text);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "の監視を開始します\n");
                    BLDstate[comboBox1.SelectedItem.ToString()] = 1;
                    label2.ForeColor = Color.FromArgb(0, 153, 0);
                    button1.Text = "停止";
                    //ここでimを使ってスリープを送り続けるのを止める
                }
                else
                {
                    richTextBox1.AppendText(timeStamp());
                    richTextBox1.SelectionColor = Color.FromArgb(32, 32, 32);
                    richTextBox1.AppendText(label2.Text);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "を待機状態にします\n");
                    BLDstate[comboBox1.SelectedItem.ToString()] = 2;
                    label2.ForeColor = Color.FromArgb(32, 32, 32);
                    button1.Text = "起動";
                    //ここでimを使ってスリープを送らせ続ける
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
                //ここでimを使ってスリープを止める
            }
            else
            {

                richTextBox1.AppendText(timeStamp() + "● 全棟を待機状態にします\n");

                for (int i = 0; i < BLDs.Count; i++)
                {
                    BLDstate[BLDs[i]] = 2;
                }

                label2.ForeColor = Color.FromArgb(32, 32, 32);
                label3.ForeColor = Color.FromArgb(32, 32, 32);
                button1.Text = "起動";
                isSleepingAll = true;
                //ここでimを使って全体にスリープを送らせ続ける
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
    }
}
