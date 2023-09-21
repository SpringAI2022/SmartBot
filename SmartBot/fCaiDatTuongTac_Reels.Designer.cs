namespace SmartBot
{
    partial class fCaiDatTuongTac_Reels
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
            btn_Anh = new System.Windows.Forms.Button();
            txt_NoiDung = new System.Windows.Forms.RichTextBox();
            label27 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btn_Huy = new System.Windows.Forms.Button();
            btn_Them = new System.Windows.Forms.Button();
            num_MaxDelay = new System.Windows.Forms.NumericUpDown();
            num_MaxBaiViet = new System.Windows.Forms.NumericUpDown();
            num_MinDelay = new System.Windows.Forms.NumericUpDown();
            num_MinBaiViet = new System.Windows.Forms.NumericUpDown();
            label20 = new System.Windows.Forms.Label();
            label21 = new System.Windows.Forms.Label();
            label22 = new System.Windows.Forms.Label();
            txt_Ten = new System.Windows.Forms.TextBox();
            label23 = new System.Windows.Forms.Label();
            label24 = new System.Windows.Forms.Label();
            label25 = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            openFile_Video = new System.Windows.Forms.OpenFileDialog();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_MaxDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_MaxBaiViet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_MinDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_MinBaiViet).BeginInit();
            SuspendLayout();
            // 
            // btn_Anh
            // 
            btn_Anh.Location = new System.Drawing.Point(21, 153);
            btn_Anh.Name = "btn_Anh";
            btn_Anh.Size = new System.Drawing.Size(164, 23);
            btn_Anh.TabIndex = 15;
            btn_Anh.Text = "Chọn video";
            btn_Anh.UseVisualStyleBackColor = true;
            btn_Anh.Click += btn_Anh_Click;
            // 
            // txt_NoiDung
            // 
            txt_NoiDung.Location = new System.Drawing.Point(21, 53);
            txt_NoiDung.Name = "txt_NoiDung";
            txt_NoiDung.Size = new System.Drawing.Size(298, 83);
            txt_NoiDung.TabIndex = 13;
            txt_NoiDung.Text = "";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label27.Location = new System.Drawing.Point(21, 27);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(49, 13);
            label27.TabIndex = 12;
            label27.Text = "Nội dung";
            label27.Click += label27_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txt_NoiDung);
            groupBox1.Controls.Add(label27);
            groupBox1.Controls.Add(btn_Anh);
            groupBox1.Location = new System.Drawing.Point(77, 185);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(351, 197);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Bài đăng";
            // 
            // btn_Huy
            // 
            btn_Huy.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            btn_Huy.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btn_Huy.Location = new System.Drawing.Point(243, 416);
            btn_Huy.Name = "btn_Huy";
            btn_Huy.Size = new System.Drawing.Size(91, 30);
            btn_Huy.TabIndex = 24;
            btn_Huy.Text = "Hủy";
            btn_Huy.UseVisualStyleBackColor = false;
            btn_Huy.Click += btn_Huy_Click;
            // 
            // btn_Them
            // 
            btn_Them.BackColor = System.Drawing.SystemColors.MenuHighlight;
            btn_Them.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btn_Them.Location = new System.Drawing.Point(118, 416);
            btn_Them.Name = "btn_Them";
            btn_Them.Size = new System.Drawing.Size(91, 30);
            btn_Them.TabIndex = 25;
            btn_Them.Text = "Thêm";
            btn_Them.UseVisualStyleBackColor = false;
            btn_Them.Click += btn_Them_Click;
            // 
            // num_MaxDelay
            // 
            num_MaxDelay.Location = new System.Drawing.Point(326, 135);
            num_MaxDelay.Name = "num_MaxDelay";
            num_MaxDelay.Size = new System.Drawing.Size(50, 23);
            num_MaxDelay.TabIndex = 41;
            num_MaxDelay.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // num_MaxBaiViet
            // 
            num_MaxBaiViet.Location = new System.Drawing.Point(326, 95);
            num_MaxBaiViet.Name = "num_MaxBaiViet";
            num_MaxBaiViet.Size = new System.Drawing.Size(50, 23);
            num_MaxBaiViet.TabIndex = 40;
            num_MaxBaiViet.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // num_MinDelay
            // 
            num_MinDelay.Location = new System.Drawing.Point(229, 135);
            num_MinDelay.Name = "num_MinDelay";
            num_MinDelay.Size = new System.Drawing.Size(50, 23);
            num_MinDelay.TabIndex = 39;
            num_MinDelay.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // num_MinBaiViet
            // 
            num_MinBaiViet.Location = new System.Drawing.Point(229, 95);
            num_MinBaiViet.Name = "num_MinBaiViet";
            num_MinBaiViet.Size = new System.Drawing.Size(50, 23);
            num_MinBaiViet.TabIndex = 38;
            num_MinBaiViet.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(391, 137);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(29, 15);
            label20.TabIndex = 37;
            label20.Text = "giây";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(391, 97);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(23, 15);
            label21.TabIndex = 36;
            label21.Text = "bài";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new System.Drawing.Point(290, 137);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(27, 15);
            label22.TabIndex = 35;
            label22.Text = "đến";
            // 
            // txt_Ten
            // 
            txt_Ten.Location = new System.Drawing.Point(229, 51);
            txt_Ten.Name = "txt_Ten";
            txt_Ten.Size = new System.Drawing.Size(187, 23);
            txt_Ten.TabIndex = 34;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new System.Drawing.Point(290, 97);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(27, 15);
            label23.TabIndex = 33;
            label23.Text = "đến";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new System.Drawing.Point(77, 136);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(120, 15);
            label24.TabIndex = 32;
            label24.Text = "Thời gian ngắt quãng";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new System.Drawing.Point(77, 97);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(95, 15);
            label25.TabIndex = 31;
            label25.Text = "Số lượng bài viết";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new System.Drawing.Point(77, 54);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(79, 15);
            label26.TabIndex = 30;
            label26.Text = "Tên tương tác";
            // 
            // openFile_Video
            // 
            openFile_Video.FileName = "openFileDialog1";
            // 
            // fCaiDatTuongTac_Reels
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(517, 480);
            Controls.Add(num_MaxDelay);
            Controls.Add(num_MaxBaiViet);
            Controls.Add(num_MinDelay);
            Controls.Add(num_MinBaiViet);
            Controls.Add(label20);
            Controls.Add(label21);
            Controls.Add(label22);
            Controls.Add(txt_Ten);
            Controls.Add(label23);
            Controls.Add(label24);
            Controls.Add(label25);
            Controls.Add(label26);
            Controls.Add(btn_Them);
            Controls.Add(btn_Huy);
            Controls.Add(groupBox1);
            Name = "fCaiDatTuongTac_Reels";
            Text = "Cấu hình đăng Reels";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_MaxDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_MaxBaiViet).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_MinDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_MinBaiViet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btn_Anh;
        private System.Windows.Forms.RichTextBox txt_NoiDung;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Huy;
        private System.Windows.Forms.Button btn_Them;
        private System.Windows.Forms.NumericUpDown num_MaxDelay;
        private System.Windows.Forms.NumericUpDown num_MaxBaiViet;
        private System.Windows.Forms.NumericUpDown num_MinDelay;
        private System.Windows.Forms.NumericUpDown num_MinBaiViet;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txt_Ten;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.OpenFileDialog openFile_Video;
    }
}