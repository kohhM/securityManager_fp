
namespace securityManager_fp
{
    partial class map
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.hensyuTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.画像を変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ボタンリセットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 262);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hensyuTSM,
            this.画像を変更ToolStripMenuItem,
            this.modeTSM});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(333, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // hensyuTSM
            // 
            this.hensyuTSM.Name = "hensyuTSM";
            this.hensyuTSM.Size = new System.Drawing.Size(43, 20);
            this.hensyuTSM.Text = "編集";
            this.hensyuTSM.Click += new System.EventHandler(this.hensyuTSM_Click);
            // 
            // 画像を変更ToolStripMenuItem
            // 
            this.画像を変更ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm1,
            this.tsm2,
            this.ボタンリセットToolStripMenuItem});
            this.画像を変更ToolStripMenuItem.Name = "画像を変更ToolStripMenuItem";
            this.画像を変更ToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.画像を変更ToolStripMenuItem.Text = "ボタン設定";
            // 
            // tsm1
            // 
            this.tsm1.Name = "tsm1";
            this.tsm1.Size = new System.Drawing.Size(180, 22);
            this.tsm1.Text = "拡大";
            this.tsm1.Click += new System.EventHandler(this.tsm1_Click);
            // 
            // tsm2
            // 
            this.tsm2.Name = "tsm2";
            this.tsm2.Size = new System.Drawing.Size(180, 22);
            this.tsm2.Text = "縮小";
            this.tsm2.Click += new System.EventHandler(this.tsm2_Click);
            // 
            // ボタンリセットToolStripMenuItem
            // 
            this.ボタンリセットToolStripMenuItem.Name = "ボタンリセットToolStripMenuItem";
            this.ボタンリセットToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ボタンリセットToolStripMenuItem.Text = "ボタンリセット";
            this.ボタンリセットToolStripMenuItem.Click += new System.EventHandler(this.ボタンリセットToolStripMenuItem_Click);
            // 
            // modeTSM
            // 
            this.modeTSM.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.modeTSM.Enabled = false;
            this.modeTSM.Name = "modeTSM";
            this.modeTSM.Size = new System.Drawing.Size(12, 20);
            // 
            // map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(333, 293);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "map";
            this.Text = "map";
            this.Load += new System.EventHandler(this.map_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hensyuTSM;
        private System.Windows.Forms.ToolStripMenuItem 画像を変更ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsm1;
        private System.Windows.Forms.ToolStripMenuItem tsm2;
        private System.Windows.Forms.ToolStripMenuItem modeTSM;
        private System.Windows.Forms.ToolStripMenuItem ボタンリセットToolStripMenuItem;
    }
}