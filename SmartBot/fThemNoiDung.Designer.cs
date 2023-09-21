using System;

namespace SmartBot
{
    partial class fThemNoiDung
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
            lb_NhapNoiDung = new System.Windows.Forms.Label();
            lb_DelRichText = new System.Windows.Forms.Label();
            lb_Them = new System.Windows.Forms.Label();
            btn_Save = new System.Windows.Forms.Button();
            txt_noidung = new System.Windows.Forms.RichTextBox();
            btn_Huy = new System.Windows.Forms.Button();
            btn_XacNhan = new System.Windows.Forms.Button();
            cb_Anh = new System.Windows.Forms.CheckBox();
            btn_Anh = new System.Windows.Forms.Button();
            openFolder_Anh = new System.Windows.Forms.FolderBrowserDialog();
            SuspendLayout();
            // 
            // lb_NhapNoiDung
            // 
            lb_NhapNoiDung.AutoSize = true;
            lb_NhapNoiDung.Location = new System.Drawing.Point(45, 18);
            lb_NhapNoiDung.Name = "lb_NhapNoiDung";
            lb_NhapNoiDung.Size = new System.Drawing.Size(87, 15);
            lb_NhapNoiDung.TabIndex = 14;
            lb_NhapNoiDung.Text = "Nhập nội dung";
            // 
            // lb_DelRichText
            // 
            lb_DelRichText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lb_DelRichText.AutoSize = true;
            lb_DelRichText.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lb_DelRichText.Location = new System.Drawing.Point(205, 186);
            lb_DelRichText.Name = "lb_DelRichText";
            lb_DelRichText.Size = new System.Drawing.Size(18, 23);
            lb_DelRichText.TabIndex = 13;
            lb_DelRichText.Text = "-";
            lb_DelRichText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_DelRichText.Click += lb_DelRichText_Click;
            // 
            // lb_Them
            // 
            lb_Them.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lb_Them.AutoSize = true;
            lb_Them.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lb_Them.Location = new System.Drawing.Point(173, 186);
            lb_Them.Name = "lb_Them";
            lb_Them.Size = new System.Drawing.Size(26, 23);
            lb_Them.TabIndex = 12;
            lb_Them.Text = "+";
            lb_Them.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_Them.Click += lb_Them_Click;
            // 
            // btn_Save
            // 
            btn_Save.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btn_Save.BackColor = System.Drawing.Color.PeachPuff;
            btn_Save.Location = new System.Drawing.Point(167, 224);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new System.Drawing.Size(75, 23);
            btn_Save.TabIndex = 11;
            btn_Save.Text = "Lưu nháp";
            btn_Save.UseVisualStyleBackColor = false;
            btn_Save.Click += btn_Save_Click;
            // 
            // txt_noidung
            // 
            txt_noidung.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txt_noidung.Location = new System.Drawing.Point(45, 41);
            txt_noidung.Name = "txt_noidung";
            txt_noidung.Size = new System.Drawing.Size(317, 101);
            txt_noidung.TabIndex = 10;
            txt_noidung.Text = "";
            // 
            // btn_Huy
            // 
            btn_Huy.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btn_Huy.BackColor = System.Drawing.Color.LightCoral;
            btn_Huy.Location = new System.Drawing.Point(274, 224);
            btn_Huy.Name = "btn_Huy";
            btn_Huy.Size = new System.Drawing.Size(75, 23);
            btn_Huy.TabIndex = 9;
            btn_Huy.Text = "Hủy";
            btn_Huy.UseVisualStyleBackColor = false;
            btn_Huy.Click += btn_Huy_Click;
            // 
            // btn_XacNhan
            // 
            btn_XacNhan.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btn_XacNhan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btn_XacNhan.Location = new System.Drawing.Point(53, 224);
            btn_XacNhan.Name = "btn_XacNhan";
            btn_XacNhan.Size = new System.Drawing.Size(75, 23);
            btn_XacNhan.TabIndex = 8;
            btn_XacNhan.Text = "Xác nhận";
            btn_XacNhan.UseVisualStyleBackColor = false;
            btn_XacNhan.Click += btn_XacNhan_Click;
            // 
            // cb_Anh
            // 
            cb_Anh.AutoSize = true;
            cb_Anh.Location = new System.Drawing.Point(45, 152);
            cb_Anh.Name = "cb_Anh";
            cb_Anh.Size = new System.Drawing.Size(111, 19);
            cb_Anh.TabIndex = 16;
            cb_Anh.Text = "Đăng ảnh/video";
            cb_Anh.UseVisualStyleBackColor = true;
            // 
            // btn_Anh
            // 
            btn_Anh.Location = new System.Drawing.Point(185, 148);
            btn_Anh.Name = "btn_Anh";
            btn_Anh.Size = new System.Drawing.Size(164, 23);
            btn_Anh.TabIndex = 17;
            btn_Anh.Text = "Chọn thư mục ảnh/video";
            btn_Anh.UseVisualStyleBackColor = true;
            btn_Anh.Click += btn_Anh_Click;
            //btn_Anh.Click += (sender, EventArgs) => { btn_Anh_Click_1(sender, EventArgs, 0); };
            // 
            // fThemNoiDung
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(402, 289);
            Controls.Add(cb_Anh);
            Controls.Add(btn_Anh);
            Controls.Add(lb_NhapNoiDung);
            Controls.Add(lb_DelRichText);
            Controls.Add(lb_Them);
            Controls.Add(btn_XacNhan);
            Controls.Add(btn_Save);
            Controls.Add(btn_Huy);
            Controls.Add(txt_noidung);
            Name = "fThemNoiDung";
            Text = "fThemNoiDung";
            Load += fThemNoiDung_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lb_NhapNoiDung;
        private System.Windows.Forms.Label lb_DelRichText;
        private System.Windows.Forms.Label lb_Them;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.RichTextBox txt_noidung;
        private System.Windows.Forms.Button btn_Huy;
        private System.Windows.Forms.Button btn_XacNhan;
        private System.Windows.Forms.CheckBox cb_Anh;
        private System.Windows.Forms.Button btn_Anh;
        private System.Windows.Forms.FolderBrowserDialog openFolder_Anh;
    }
}