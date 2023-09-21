using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace SmartBot
{
    public partial class fCaiDatTuongTac_Stories : Form
    {
        public fCaiDatTuongTac_Stories()
        {
            InitializeComponent();
        }
        public int minBaiViet;
        public int maxBaiViet;
        public int minDelay;
        public int maxDelay;
        public string noiDung;
        public bool bool_Anh = false;
        public string pathAnh;
        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            minBaiViet = Convert.ToInt16(num_MinBaiViet.Value);
            maxBaiViet = Convert.ToInt16(num_MaxBaiViet.Value);
            minDelay = Convert.ToInt16(num_MinDelay.Value);
            maxDelay = Convert.ToInt16(num_MaxDelay.Value);
            noiDung = txt_NoiDung.Text;
            if (cb_Anh.Checked)
            {
                if (pathAnh == null)
                {
                    MessageBox.Show("Chưa chọn thư mục chứa ảnh/video", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    bool_Anh = true;
                }
            }
            MessageBox.Show("Đã cấu hình thành công tương tác cho Stories", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btn_Anh_Click(object sender, EventArgs e)
        {
            if (openFile_Anh.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".mp4" };
                    pathAnh = openFile_Anh.FileName;
                    //pathAnh = Directory.GetFiles(path_IMGs)
                    //                    .Where(file => allowedExtensions
                    //                    .Any(file.ToLower().EndsWith))
                    //                    .ToArray();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_NoiDung_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
