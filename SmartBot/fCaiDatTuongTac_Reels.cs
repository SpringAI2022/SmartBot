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
    public partial class fCaiDatTuongTac_Reels : Form
    {
        public fCaiDatTuongTac_Reels()
        {
            InitializeComponent();
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }
        public string pathAnh;
        private void btn_Anh_Click(object sender, EventArgs e)
        {
            if (openFile_Video.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".mp4" };
                    pathAnh = openFile_Video.FileName;
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
        public int minBaiViet;
        public int maxBaiViet;
        public int minDelay;
        public int maxDelay;
        public string noiDung;

        public bool bool_Anh = false;
        private void btn_Them_Click(object sender, EventArgs e)
        {
            minBaiViet = Convert.ToInt16(num_MinBaiViet.Value);
            maxBaiViet = Convert.ToInt16(num_MaxBaiViet.Value);
            minDelay = Convert.ToInt16(num_MinDelay.Value);
            maxDelay = Convert.ToInt16(num_MaxDelay.Value);
            noiDung = txt_NoiDung.Text;
            if (pathAnh == null)
            {

                MessageBox.Show("Chưa chọn video", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                MessageBox.Show("Đã cấu hình thành công tương tác cho Reels", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
