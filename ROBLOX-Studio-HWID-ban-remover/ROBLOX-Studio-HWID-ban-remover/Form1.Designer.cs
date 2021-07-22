namespace ROBLOX_Studio_HWID_ban_remover
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uninstall = new System.Windows.Forms.CheckBox();
            this.tele = new System.Windows.Forms.CheckBox();
            this.unban = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.log = new System.Windows.Forms.RichTextBox();
            this.musicChkBx = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uninstall);
            this.groupBox1.Controls.Add(this.tele);
            this.groupBox1.Controls.Add(this.unban);
            this.groupBox1.Location = new System.Drawing.Point(12, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 113);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main";
            // 
            // uninstall
            // 
            this.uninstall.AutoSize = true;
            this.uninstall.Location = new System.Drawing.Point(7, 84);
            this.uninstall.Name = "uninstall";
            this.uninstall.Size = new System.Drawing.Size(180, 17);
            this.uninstall.TabIndex = 2;
            this.uninstall.Text = "Uninstall Studio (Recommended)";
            this.uninstall.UseVisualStyleBackColor = true;
            // 
            // tele
            // 
            this.tele.AutoCheck = false;
            this.tele.AutoSize = true;
            this.tele.Enabled = false;
            this.tele.Location = new System.Drawing.Point(7, 60);
            this.tele.Name = "tele";
            this.tele.Size = new System.Drawing.Size(230, 17);
            this.tele.TabIndex = 1;
            this.tele.Text = "Block ROBLOX Telemetry (Recommended)";
            this.tele.UseVisualStyleBackColor = true;
            // 
            // unban
            // 
            this.unban.Location = new System.Drawing.Point(7, 20);
            this.unban.Name = "unban";
            this.unban.Size = new System.Drawing.Size(281, 33);
            this.unban.TabIndex = 0;
            this.unban.Text = "Unban System from Studio";
            this.unban.UseVisualStyleBackColor = true;
            this.unban.Click += new System.EventHandler(this.unban_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.log);
            this.groupBox2.Location = new System.Drawing.Point(12, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 144);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logs";
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(6, 19);
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.Size = new System.Drawing.Size(282, 119);
            this.log.TabIndex = 0;
            this.log.Text = "";
            // 
            // musicChkBx
            // 
            this.musicChkBx.AutoSize = true;
            this.musicChkBx.Checked = true;
            this.musicChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.musicChkBx.Location = new System.Drawing.Point(12, 439);
            this.musicChkBx.Name = "musicChkBx";
            this.musicChkBx.Size = new System.Drawing.Size(54, 17);
            this.musicChkBx.TabIndex = 3;
            this.musicChkBx.Text = "Music";
            this.musicChkBx.UseVisualStyleBackColor = true;
            this.musicChkBx.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.DarkOrchid;
            this.linkLabel1.Location = new System.Drawing.Point(219, 440);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(87, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Poggers//Github";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ROBLOX_Studio_HWID_ban_remover.Properties.Resources.Image;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(294, 151);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 466);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.musicChkBx);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ROBLOX Studio HWID Ban Remover";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox uninstall;
        private System.Windows.Forms.CheckBox tele;
        private System.Windows.Forms.Button unban;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox musicChkBx;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.RichTextBox log;
    }
}

