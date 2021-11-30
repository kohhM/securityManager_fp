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
        List<string> bld = new List<string>();
        Dictionary<string, string> bld_num = new Dictionary<string, string>();

        int w,h;

        public Button[] buttons;
        Boolean hensyu = false;

        public map()
        {
            InitializeComponent();
            this.buttons = null;
        }

        private void map_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"data_folder\bg.jpg"))
            {
                panel1.BackgroundImageLayout = ImageLayout.Zoom;
                panel1.BackgroundImage = Image.FromFile(@"data_folder\bg.jpg");
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
                    //panel1.BackgroundImageLayout = ImageLayout.Zoom;
                    panel1.BackgroundImage = Image.FromFile(@"data_folder\bg.jpg");
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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            

            using (StreamReader sr = new StreamReader(@"data_folder\bld.csv", Encoding.GetEncoding("shift_jis")))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] readData = line.Split(',');

                    bld.Add(readData[1]);
                }
            }

            this.buttons = new Button[bld.Count];
            for(int i=0;i< bld.Count; i++)
            {
                this.buttons[i] = new Button();

                this.buttons[i].Name = "button" + i;
                this.buttons[i].Text = bld[i];
                this.buttons[i].Location = new Point(20,40 + i * 22);
                this.buttons[i].Size = new Size(20, 20);

                this.Controls.Add(this.buttons[i]);
                this.buttons[i].BringToFront();
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

        private void map_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }
}
