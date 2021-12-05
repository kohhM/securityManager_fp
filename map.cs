/*
<memo>

ボタンサイズ用のcsvがあってもいい　もうめんどいからやらない

クリックしたときの処理
フォーム1で変えたときとマップで変えた時両方の変数を変えないといけない

ボタンリセットってボタンだけつくって中は作ってない
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

namespace securityManager_fp
{
    public partial class map : Form
    {

        public List<string> bld = new List<string>();
        //public Dictionary<string, string> bld_num = new Dictionary<string, string>();
        public Dictionary<string, int> bld_state = new Dictionary<string, int>();

        public List<int> bw = new List<int>();
        public List<int> bh = new List<int>();

        int w,h;

        public Button[] buttons;
        ToolTip toolTip1;
        public Boolean hensyu = false;
        Boolean isDrag = false;
        Point? diffPoint = null;
        Point point = new Point(0, 0);

        public map()
        {
            InitializeComponent();
            this.buttons = null;
        }

        public Button[] ButtonOnMap
        {
            get { return this.buttons; }
            //このxじゃだめっぽい
        }

        private void map_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"data_folder\bg.jpg"))
            {
                panel1.BackgroundImageLayout = ImageLayout.Zoom;

                using (Image image = Image.FromFile(@"data_folder\bg.jpg"))
                {
                    w = image.Width;
                    h = image.Height;
                }
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = "";
                ofd.Filter = "JPEGファイル(*.jpeg;*.jpg)|*.jpeg;*.jpg";
                //ofd.Filter = "画像ファイル(*.jpeg;*.jpg;*.gif;*png)|*.jpeg;*.jpg;*.gif;*png";
                ofd.FilterIndex = 2;
                ofd.Title = "地図画像を選択";
                ofd.RestoreDirectory = true;
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(ofd.FileName, "data_folder\\bg.jpg");
                    
                    using (Image image = Image.FromFile(@"data_folder\bg.jpg"))
                    {
                        w = image.Width;
                        h = image.Height;
                    }
                }
            }
            this.Width = w;
            this.Height = h;
            panel1.Width = w;
            panel1.Height = h;
            panel1.BackgroundImage = Image.FromFile(@"data_folder\bg.jpg");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            

            using (StreamReader sr = new StreamReader(@"data_folder\bld.csv", Encoding.GetEncoding("shift_jis")))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] readData = line.Split(',');

                    //bld_num.Add(readData[1], readData[0]);
                    bld.Add(readData[1]);

                    try
                    {
                        bld_state.Add(readData[1], int.Parse(readData[2]));
                        bw.Add(int.Parse(readData[3]));
                        bh.Add(int.Parse(readData[4]));
                    }
                    catch { 
                        bw.Add(0);
                        bh.Add(0);
                    }
                }
            }

            this.buttons = new Button[bld.Count];
            toolTip1 = new ToolTip();
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.ShowAlways = true;
            for(int i=0;i< bld.Count; i++)
            {
                this.buttons[i] = new Button();

                this.buttons[i].Name = i.ToString();
                this.buttons[i].Text = "";

                switch (bld_state[bld[i]])
                {
                    case 0:
                        buttons[i].BackColor = Color.FromArgb(232, 0, 43);
                        break;
                    case 1:
                        buttons[i].BackColor = Color.FromArgb(0, 153, 0);
                        break;
                    default:
                        buttons[i].BackColor = Color.FromArgb(32, 32, 32);
                        break;
                }

                if (bw[i] == 0 & bh[i] == 0)
                {
                    this.buttons[i].Location = new Point(20, 40 + i * 22);
                }
                else
                {
                    this.buttons[i].Location = new Point(bw[i], bh[i]);
                }
                this.buttons[i].Size = new Size(20, 20);

                this.Controls.Add(this.buttons[i]);
                buttons[i].MouseDown += Map_MouseDown;
                buttons[i].MouseUp += Map_MouseUp;
                buttons[i].MouseMove += Map_MouseMove;
                this.buttons[i].BringToFront();
                toolTip1.SetToolTip(buttons[i], bld[i]);
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            var bt = (Button)sender;
            if(hensyu == true)
            {
                if(isDrag == false)
                {
                    return;
                }
                int x = bt.Location.X + e.X - diffPoint.Value.X;
                int y = bt.Location.Y + e.Y - diffPoint.Value.Y;

                if(x <= 0)
                {
                    x = 0;
                }
                if(y <= 0)
                {
                    y = 0;
                }
                point =new Point(x, y);
            }
        }

        private void Map_MouseUp(object sender, MouseEventArgs e)
        {
            var bt = (Button)sender;
            if(hensyu == true)
            {
                Cursor.Current = Cursors.Default;
                isDrag = false;
                if(e.Button != MouseButtons.Left)
                {
                    return;
                }

                bt.Location = point;
            }
        }

        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            if(hensyu == true)
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }
                Cursor.Current = Cursors.Hand;
                isDrag = true;
                diffPoint = e.Location;
            }
        }

        private void tsm1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bld.Count; i++)
            {
                this.buttons[i].Size = new Size(buttons[i].Width+5,buttons[i].Height+ 5);
            }
        }

        private void tsm2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bld.Count; i++)
            {
                if (buttons[i].Width > 5)
                {
                    this.buttons[i].Size = new Size(buttons[i].Width - 5, buttons[i].Height - 5);
                }
            }
        }

        private void ボタンリセットToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0;i<bld.Count; i++)
            {
                this.buttons[i].Location = new Point(20, 40 + i * 22);
            }
        }

        private void hensyuTSM_Click(object sender, EventArgs e)
        {
            if(hensyu == false)
            {
                hensyu = true;
                modeTSM.Text = "編集モード";
            }
            else
            {
                hensyu = false;
                modeTSM.Text = "";
            }
        }

    }
}
