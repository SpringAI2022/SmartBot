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
    public partial class fCaiDatTuongTac_NewFeeds : Form
    {
        public fCaiDatTuongTac_NewFeeds()
        {
            InitializeComponent();
            LoadData();
        }
        public int minBaiViet;
        public int maxBaiViet;
        public int minDelay;
        public int maxDelay;
        public string noiDung;
        public string noiDungBinhLuan;

        public bool bool_Anh = false;
        public string[] pathAnh;
        public string[] pathAnhBinhLuan;
        public List<dynamic> dataNewfeed = new List<dynamic>();
        private List<PhanHoi> listBaiDang;
        private List<PhanHoi> listBaiDang_Load = new List<PhanHoi>();
        public List<PhanHoi> listBaiDang_Select = new List<PhanHoi>();

        public void LoadData()
        {
            try
            {
                string pathPhanHoi = "Database/Data_PhanHoi.json";
                //list_acc = File.ReadAllLines(filePath);
                var strPhanHoi = File.ReadAllText(pathPhanHoi);
                listBaiDang = JsonConvert.DeserializeObject<List<PhanHoi>>(strPhanHoi);
                listBaiDang_Load.Clear();
                clb_BaiDang.Items.Clear();
                listBaiDang_Select.Clear();
                foreach (var ph in listBaiDang)
                {
                    if ((ph.UserID == "") && (ph.Type == "Wall"))
                    {
                        clb_BaiDang.Items.Add(ph.ResponseID);
                        listBaiDang_Load.Add(ph);
                    }
                    // Sau sắp xếp danh sách theo mức độ phù hợp với tính cách của UserID, và hiển thị ra maxBinhLuan

                }
                for (int i = 0; i < clb_BaiDang.Items.Count; i++)
                {
                    clb_BaiDang.SetItemChecked(i, true);
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn lại file!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            minBaiViet = Convert.ToInt16(num_MinBaiViet.Value);
            maxBaiViet = Convert.ToInt16(num_MaxBaiViet.Value);
            minDelay = Convert.ToInt16(num_MinDelay.Value);
            maxDelay = Convert.ToInt16(num_MaxDelay.Value);
            noiDung = txt_BaiDang.Text;
            listBaiDang_Select.Clear();
            //noiDungBinhLuan = txt_BinhLuan.Text;
            foreach (var resID in clb_BaiDang.CheckedItems)
            {
                foreach (var ph in listBaiDang_Load)
                {
                    if (ph.ResponseID == resID.ToString() && !listBaiDang_Select.Contains(ph))
                    {
                        listBaiDang_Select.Add(ph);
                    }
                }
            }
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
            MessageBox.Show("Đã cấu hình thành công tương tác cho Newfeeds", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();

        }

        private void btn_Anh_Click(object sender, EventArgs e)
        {
            if (openFolder_Anh.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".mp4" };
                    var path_IMGs = openFolder_Anh.SelectedPath;
                    pathAnh = Directory.GetFiles(path_IMGs)
                                        .Where(file => allowedExtensions
                                        .Any(file.ToLower().EndsWith))
                                        .ToArray();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void btn_AnhBinhLuan_Click(object sender, EventArgs e)
        {
            if (openFolder_AnhBinhLuan.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".mp4" };
                    var path_IMGs = openFolder_AnhBinhLuan.SelectedPath;
                    pathAnhBinhLuan = Directory.GetFiles(path_IMGs)
                                        .Where(file => allowedExtensions
                                        .Any(file.ToLower().EndsWith))
                                        .ToArray();
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

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    int idxSelect = clb_BaiDang.SelectedIndex;
            //    clb_BaiDang.Items.Insert(idxSelect, txt_BaiDang.Text);
            //    clb_BaiDang.SelectedIndex = idxSelect;
            //    clb_BaiDang.Items.RemoveAt(idxSelect + 1);
            //}
            //catch { }
            foreach (var ph in listBaiDang_Load)
            {
                if (ph.ResponseID == clb_BaiDang.SelectedItem.ToString())
                {
                    ph.Content = txt_BaiDang.Text;

                }
            }
        }

        private void clb_BaiDang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clb_BaiDang.SelectedItem != null)
            {
                foreach (var ph in listBaiDang_Load)
                {
                    if (ph.ResponseID == clb_BaiDang.SelectedItem.ToString())
                    {
                        txt_BaiDang.Text = ph.Content;
                        btn_Edit.Enabled = true;
                        txt_BaiDang.Enabled = true;
                    }
                }
            }
        }

        private void btn_AddContent_Click(object sender, EventArgs e)
        {
            fThemNoiDung themNoiDung = new fThemNoiDung();
            themNoiDung.typeNoiDung = "Wall";
            themNoiDung.Text = "Thêm Bài đăng lên Tường";
            themNoiDung.ShowDialog();
            LoadData();
        }

        private void txt_BaiDang_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
