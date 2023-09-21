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
    public partial class fCaiDatTuongTac_BinhLuan : Form
    {
        public fCaiDatTuongTac_BinhLuan()
        {
            InitializeComponent();
            LoadData();

        }
        public int minBaiViet;
        public int maxBaiViet;
        public int minDelay;
        public int maxDelay;
        public string noiDungBinhLuan;
        public bool bool_Anh = false;
        public string[] pathAnhBinhLuan;
        private List<PhanHoi> listPhanHoi;
        public List<PhanHoi> listPhanHoi_Load = new List<PhanHoi>();
        public List<PhanHoi> listPhanHoi_Select = new List<PhanHoi>();

        public void LoadData()
        {
            try
            {
                string pathPhanHoi = "Database/Data_PhanHoi.json";
                //list_acc = File.ReadAllLines(filePath);
                var strPhanHoi = File.ReadAllText(pathPhanHoi);
                listPhanHoi = JsonConvert.DeserializeObject<List<PhanHoi>>(strPhanHoi);
                listPhanHoi_Load.Clear();
                clb_BinhLuan.Items.Clear();
                foreach (var ph in listPhanHoi)
                {
                    if ((ph.UserID == "") && (ph.Type == "Comment"))
                    {
                        clb_BinhLuan.Items.Add(ph.ResponseID);
                        listPhanHoi_Load.Add(ph);
                    }
                    // Sau sắp xếp danh sách theo mức độ phù hợp với tính cách của UserID, và hiển thị ra maxBinhLuan
                }
                for (int i = 0; i < clb_BinhLuan.Items.Count; i++)
                {
                    clb_BinhLuan.SetItemChecked(i, true);
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn lại file!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_Them_Click(object sender, EventArgs e)
        {
            minBaiViet = Convert.ToInt16(num_MinBaiViet.Value);
            maxBaiViet = Convert.ToInt16(num_MaxBaiViet.Value);
            minDelay = Convert.ToInt16(num_MinDelay.Value);
            maxDelay = Convert.ToInt16(num_MaxDelay.Value);
            noiDungBinhLuan = txt_BinhLuan.Text;
            listPhanHoi_Select.Clear();
            foreach (var resID in clb_BinhLuan.CheckedItems)
            {
                foreach (var ph in listPhanHoi_Load)
                {
                    if (ph.ResponseID == resID.ToString() && !listPhanHoi_Select.Contains(ph))
                    {
                        listPhanHoi_Select.Add(ph);
                    }
                }
            }
            //MessageBox.Show(listPhanHoi_Select[0].ToString());
            //listPhanHoi_Select.Clear();

            //foreach(var ph in listPhanHoi)
            if (cb_AnhBinhLuan.Checked)
            {
                if (pathAnhBinhLuan == null)
                {
                    MessageBox.Show("Chưa chọn thư mục chứa ảnh/video", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    bool_Anh = true;
                }
            }
            MessageBox.Show("Đã cấu hình thành công tương tác cho Bình luận", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    int idxSelect = clb_BinhLuan.SelectedIndex;
            //    clb_BinhLuan.Items.Insert(idxSelect, txt_BinhLuan.Text);
            //    clb_BinhLuan.SelectedIndex = idxSelect;
            //    clb_BinhLuan.Items.RemoveAt(idxSelect + 1);
            //}
            //catch { }
            foreach (var ph in listPhanHoi_Load)
            {
                if (ph.ResponseID == clb_BinhLuan.SelectedItem.ToString())
                {
                    ph.Content = txt_BinhLuan.Text;
                }
            }
        }

        private void clb_BinhLuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clb_BinhLuan.SelectedItem != null)
            {
                foreach (var ph in listPhanHoi_Load)
                {
                    if (ph.ResponseID == clb_BinhLuan.SelectedItem.ToString())
                    {
                        txt_BinhLuan.Text = ph.Content;
                        btn_Edit.Enabled = true;
                        txt_BinhLuan.Enabled = true;
                    }
                }
            }
        }

        private void btn_AddContent_Click(object sender, EventArgs e)
        {
            fThemNoiDung themNoiDung = new fThemNoiDung();
            themNoiDung.typeNoiDung = "Comment";
            themNoiDung.Text = "Thêm Bình luận";
            themNoiDung.ShowDialog();
            LoadData();

        }
    }
}
