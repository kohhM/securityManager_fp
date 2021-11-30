/*
<memo>

文字コードはエクセルのせいでshift_jis

0003は新2号館

todo
データレシーブのマルチスレッドのとこ 別に直さなくても問題はないかな
地図 別フォームか，同じフォームか， ボタン(画像の仕様など) まぁ，おまけだからやんなくてもいいかな感
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
using System.Threading;
using System.Media;
using System.Text.RegularExpressions;

namespace securityManager_fp
{
    public partial class Form1 : Form
    {
        Dictionary<string,string> bld_num = new Dictionary<string,string>();
        List<string> BLDs = new List<string>();
        Dictionary<string, int> BLDstate = new Dictionary<string, int>();
        //2がスリープ，0でセンサー異常，1で反応なし監視中
        //ラズパイ側は1でスリープ，0で動作

        List<string> xbee_id = new List<string>();
        Dictionary<string, string> sensor = new Dictionary<string, string>();

        Boolean isSleepingAll = true;
        static string command = "";
        int error_cnt = 0;
        string SoudFile = "source\\p01.wav";
        Boolean soundMute = false;
        string receive_data_chk = @"^[0-9]{2},00[0-9]{2},[0-9A-Z]{2}:[A-Z]{1}[0-9]{3}[a-z]{3}\r\n$";

        Boolean chkRS = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"data_folder\bld.csv") & File.Exists(@"data_folder\log.csv")& File.Exists(@"data_folder\sensor.csv"))
            {
                using (StreamReader sr = new StreamReader(@"data_folder\bld.csv",Encoding.GetEncoding("shift_jis")))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] readData = line.Split(',');

                        bld_num.Add(readData[1],readData[0]);
                        BLDs.Add(readData[1]);
                        BLDstate.Add(readData[1], int.Parse(readData[2]));
                    }
                }

                using (StreamReader sr = new StreamReader(@"data_folder\sensor.csv", Encoding.GetEncoding("shift_jis")))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] readData = line.Split(',');

                        xbee_id.Add(readData[0]);
                        sensor.Add(readData[0], readData[1]);
                    }
                }

                    comboBox1.Items.AddRange(BLDs.ToArray());
                comboSetting();
                richTextBox1.AppendText(timeStamp() + "security manager_fp\n");
                richTextBox1.AppendText(timeStamp() + "各建物のセンサー状態を管理します\n");
            }
            else
            {
                richTextBox1.AppendText("ファイルが存在しません\nアプリを終了してファイルを確認してください");
                comboBox2.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
            }



        }

        private void rtb(string x)
        {
            richTextBox1.Focus();
            richTextBox1.AppendText(timeStamp() + x);
        }

        private void spw(string x)
        {
            chkRS = true;
            command = x;
            serialPort1.Write(x);
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
                        spw("TXDU "+bld_num[comboBox1.SelectedItem.ToString()]+",0\r\n");

                    }
                    catch
                    {
                        richTextBox1.AppendText("仮想シリアル通信に失敗\n");
                        return;
                    }
                }
                else if(BLDstate[comboBox1.SelectedItem.ToString()] == 0)
                {
                    richTextBox1.AppendText("センサー異常発生中です\n状態を変更できません\n");
                }
                else
                {
                    //ここでimを使ってスリープを送らせ続ける
                    try
                    {
                        spw("TXDU " + bld_num[comboBox1.SelectedItem.ToString()] + ",1\r\n");

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
                rtb("建物が選択されていません\n");
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
                    spw("TXDA 0\r\n");

                }
                catch
                {
                    richTextBox1.AppendText("仮想シリアル通信に失敗\nCOMポートを確認してください");
                    return;
                }
            }
            else
            {
                try
                {
                    spw("TXDA 1\r\n");

                }
                catch
                {
                    richTextBox1.AppendText("仮想シリアル通信に失敗\nCOMポートを確認してください");
                    return;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Focus();
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
                File.AppendAllText(@"data_folder\log.csv","memo,"+timeStamp()+","+textBox1.Text+Environment.NewLine,System.Text.Encoding.GetEncoding("shift_jis"));
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
            using (StreamWriter sw = new StreamWriter(@"data_folder\bld.csv",false, Encoding.GetEncoding("shift_jis")))
            {
                for(int i = 0;i<bld_num.Count; i++)
                {
                    sw.WriteLine(bld_num[BLDs[i]] + "," + BLDs[i] + "," + BLDstate[BLDs[i]]);
                }
            }
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
                    button7.Enabled = true;
                    button8.Enabled = true;
                    richTextBox1.AppendText(timeStamp()+comboBox2.Text +"に接続しました\n");
                    File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ",COMポート接続" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
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

        delegate void SetTextCallBack(string text);
        private void res(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(res);
                BeginInvoke(d, new object[] { text });
                //Invokeで同期式．

            }
            else
            {
                if(text.Contains("\r\n"))
                {
                    if(chkRS == true)
                    {
                        if(text == "OK\r\n")
                        {
                            error_cnt = 0;
                            if(command.Length > 9)//個別の送信の結果
                            {
                                if(command.Substring(10,1) == "0")
                                {
                                    richTextBox1.Focus();
                                    richTextBox1.AppendText(timeStamp());
                                    richTextBox1.SelectionColor = Color.FromArgb(0, 153, 0);
                                    richTextBox1.AppendText(label2.Text);
                                    richTextBox1.SelectionColor = Color.Black;
                                    richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "の監視を開始します\n");
                                    BLDstate[comboBox1.SelectedItem.ToString()] = 1;
                                    label2.ForeColor = Color.FromArgb(0, 153, 0);
                                    button1.Text = "停止";
                                    chkRS = false;
                                    File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ","+ comboBox1.SelectedItem.ToString()+ ",監視開始" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
                                }
                                else
                                {
                                    richTextBox1.Focus();
                                    richTextBox1.AppendText(timeStamp());
                                    richTextBox1.SelectionColor = Color.FromArgb(32, 32, 32);
                                    richTextBox1.AppendText(label2.Text);
                                    richTextBox1.SelectionColor = Color.Black;
                                    richTextBox1.AppendText(" " + comboBox1.SelectedItem.ToString() + "を待機状態にします\n");
                                    BLDstate[comboBox1.SelectedItem.ToString()] = 2;
                                    label2.ForeColor = Color.FromArgb(32, 32, 32);
                                    button1.Text = "起動";
                                    chkRS = false;
                                    File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + "," + comboBox1.SelectedItem.ToString() + ",監視停止" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
                                }
                            }
                            else
                            {
                                if(command.Substring(5,1) == "0")
                                {
                                    richTextBox1.Focus();
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
                                    chkRS = false;
                                    File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ",全棟,監視開始" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
                                }
                                else
                                {
                                    rtb("● 全棟を待機状態にします\n");

                                    for (int i = 0; i < BLDs.Count; i++)
                                    {
                                        BLDstate[BLDs[i]] = 2;
                                    }

                                    label2.ForeColor = Color.FromArgb(32, 32, 32);
                                    label3.ForeColor = Color.FromArgb(32, 32, 32);
                                    button1.Text = "起動";
                                    isSleepingAll = true;
                                    chkRS = false;
                                    File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ",全棟,監視停止" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
                                }
                            }
                        }
                        else
                        {
                            //マルチスレッドができてない．まぁ動いてるしいっかなくらいに思ってしまってる．誰か修正して
                            richTextBox1.Focus();
                            richTextBox1.AppendText("送信に失敗\n5秒後に再送を試みます\n");
                            for(int i = 0; i < 5; i++)
                            {
                                Thread.Sleep(1000);
                                richTextBox1.AppendText(i+"秒経過\n");
                            }
                            richTextBox1.AppendText("再送します\n");
                            error_cnt++;
                            if(error_cnt > 3)
                            {
                                error_cnt = 0;
                                com_close();
                                rtb("エラー回数が規定の回数を上回りました\n");
                                return;
                            }
                            serialPort1.Write(command);
                        }
                    }
                    else if(Regex.IsMatch(text,receive_data_chk))
                    {
                        string bld_name = text.Substring(3, 4);
                        string sensor_name = text.Substring(11, 4);
                        string sensor_state = text.Substring(15);
                        string bn;
                        try
                        {
                            bn = bld_num.First(x => x.Value.Equals(bld_name)).Key;
                        }
                        catch
                        {
                            rtb("異常が発生した建物が登録されていません\n");
                            rtb(sensor_name + "で異常が発生しています\n");
                            return;
                        }

                        if (sensor_state == "mdt\r\n" & 1 == BLDstate[bld_num.First(x => x.Value.Equals(bld_name)).Key])
                        {
                            richTextBox1.Focus();
                            richTextBox1.AppendText(timeStamp());
                            richTextBox1.SelectionColor = Color.FromArgb(232, 0, 43);
                            richTextBox1.AppendText("● ");
                            richTextBox1.SelectionColor = Color.Black;
                            richTextBox1.AppendText(bn+"で異常が発生しました\n");

                            BLDstate[bld_num.First(x => x.Value.Equals(bld_name)).Key] = 0;

                            
                            for(int i = 0; i < xbee_id.Count; i++)
                            {
                                if(sensor_name == xbee_id[i])
                                {
                                    sensor_name = sensor[xbee_id[i]];
                                }
                            }

                            richTextBox1.AppendText(sensor_name + "が反応しました\n");
                            try
                            {
                                PlaySound();
                            }
                            catch { }
                            File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ","+ bld_num.First(x => x.Value.Equals(bld_name)).Key + "," + sensor_name + "で異常発生" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
                        }
                    }
                    else
                    {
                        //ここのアペンドは最後は消す.デバよう
                        //richTextBox1.AppendText(timeStamp() + "データ欠落>>"+text);
                    }

                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string received_data = sp.ReadExisting();

            res(received_data);

        }

        private void com_reset_click(object sender, EventArgs e)
        {
            com_close();
            comboBox2.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;

            groupBox1.Enabled = false;
            button4.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            File.AppendAllText(@"data_folder\log.csv", "info," + timeStamp() + ",COMポート切断" + Environment.NewLine, System.Text.Encoding.GetEncoding("shift_jis"));
        }

        private SoundPlayer player = null;
        
        private void StopSound()
        {
            if(player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

        private void PlaySound()
        {
            if (SoudFile != "mute")
            {
                player = new SoundPlayer(SoudFile);
                player.PlayLooping();
            }
        }

        private void mute_Click(object sender, EventArgs e)
        {
            StopSound();
            if (soundMute == false)
            {
                SoudFile = "mute";
                soundMute = true;
                button6.Text = "アラーム設定\nミュート解除";
                rtb("アラームがミュートになりました\n");
            }
            else
            {
                SoudFile = "source\\p01.wav";
                soundMute = false;
                button6.Text = "アラーム設定\nミュートにする";
                rtb("アラームのミュートが解除されました");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Focus();
            richTextBox1.AppendText(timeStamp()+"正常化中です\n操作しないでください\n");
            //bldの状態を走査して異常をだしてるところを全て正常化に
            StopSound();
            for(int i = 0;i<bld_num.Count; i++)
            {
                if(BLDstate[BLDs[i]] == 0)
                {
                    BLDstate[BLDs[i]] = 2;
                    try
                    {
                        spw("TXDU " + bld_num[BLDs[i]] + ",1\r\n");
                    }
                    catch
                    {
                        richTextBox1.AppendText(BLDs[i] + "の正常化中にエラー\n正常化失敗\n");
                        return;
                    }
                }
            }
            richTextBox1.AppendText(timeStamp() + "正常化の処理が終了\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

            }
        }
    }
}
