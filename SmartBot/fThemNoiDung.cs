using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SmartBot
{
    public partial class fThemNoiDung : Form
    {
        public fThemNoiDung()
        {
            InitializeComponent();
            listTextBox.Add(txt_noidung);
            listDelLabel.Add(lb_NhapNoiDung);
            listButton.Add(btn_Anh);
            listCheckBox.Add(cb_Anh);
            pathList.Add(pathAnh);
            LoadData();
            
        }
        int countTextBox = 0;
        List<RichTextBox> listTextBox = new List<RichTextBox>();
        List<Label> listDelLabel = new List<Label>();
        List<Button> listButton = new List<Button>();
        List<CheckBox> listCheckBox = new List<CheckBox>();
        public string typeNoiDung = "";
        private string[] pathAnh = {};
        List<string[]> pathList = new List<string[]>();
        private void addRichText()
        {
            var sizeBoundText = txt_noidung.Bounds;
            var sizeForm = this.Bounds;
            RichTextBox richTxtBox = new RichTextBox();
            richTxtBox.Location = new Point(sizeBoundText.X, sizeBoundText.Y + countTextBox * (sizeBoundText.Height + 35));
            richTxtBox.Size = new Size(sizeBoundText.Width, sizeBoundText.Height);
            richTxtBox.Visible = true;
            //richTxtBox.TabIndex = 9;
            richTxtBox.Text = "";
            richTxtBox.Anchor = txt_noidung.Anchor;
            Label lb_NND = new Label();
            lb_NND.Size = new Size(lb_NhapNoiDung.Size.Width, lb_NhapNoiDung.Size.Height);
            lb_NND.Location = new Point(lb_NhapNoiDung.Location.X, lb_NhapNoiDung.Location.Y + countTextBox * (sizeBoundText.Height + 35));
            lb_NND.Text = lb_NhapNoiDung.Text;
            lb_NND.AutoSize = true;
            //lbDelRichText.Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lb_NND.TextAlign = ContentAlignment.MiddleCenter;
            Button btn_Them_Anh = new Button();
            btn_Them_Anh.Size = btn_Anh.Size;
            btn_Them_Anh.Text = btn_Anh.Text;
            btn_Them_Anh.Location = new Point(btn_Anh.Location.X, btn_Anh.Location.Y + countTextBox * (sizeBoundText.Height + 35));
            btn_Them_Anh.Click += (sender, EventArgs) => { btn_Anh_Click_1(sender, EventArgs, countTextBox); };
            CheckBox cb_Them_Anh = new CheckBox();
            cb_Them_Anh.Size = cb_Anh.Size;
            cb_Them_Anh.Text = cb_Anh.Text;
            cb_Them_Anh.Location = new Point(cb_Anh.Location.X, cb_Anh.Location.Y + countTextBox * (sizeBoundText.Height + 35));
            listButton.Add(btn_Them_Anh);
            listCheckBox.Add(cb_Them_Anh);
            listTextBox.Add(richTxtBox);
            listDelLabel.Add(lb_NND);
            pathList.Add(pathAnh);
            //return richTxtBox;
        }
        private void LoadData()
        {
            try
            {
                string pathPhanHoi = "Data/Drafts.json";
                //list_acc = File.ReadAllLines(filePath);
                var strPhanHoi = File.ReadAllText(pathPhanHoi);
                List<PhanHoi> listPhanHoi = JsonConvert.DeserializeObject<List<PhanHoi>>(strPhanHoi);
                if (listPhanHoi != null && listPhanHoi.Count > 0)
                {
                    foreach(var ph in listPhanHoi) {
                        countTextBox++;
                        var sizeBoundText = txt_noidung.Bounds;
                        var sizeForm = this.Bounds;
                        addRichText();
                        for (int i = countTextBox - 1; i < listTextBox.Count; i++)
                        {
                            
                            Controls.Add(listTextBox[i]);
                            if (i < listPhanHoi.Count)
                            {
                                listTextBox[i].Text = ph.Content;
                            }
                            Controls.Add(listDelLabel[i]);
                            Controls.Add(listButton[i]);
                            Controls.Add(listCheckBox[i]);
                        }
                        this.Size = new Size(this.Size.Width, this.Size.Height + sizeBoundText.Height + 30);
                        lb_Them.Location = new Point(lb_Them.Location.X, lb_Them.Location.Y + sizeBoundText.Height + 35);
                        lb_DelRichText.Location = new Point(lb_DelRichText.Location.X, lb_DelRichText.Location.Y + sizeBoundText.Height + 35);
                        btn_XacNhan.Location = new Point(btn_XacNhan.Location.X, btn_XacNhan.Location.Y + sizeBoundText.Height + 35);
                        btn_Huy.Location = new Point(btn_Huy.Location.X, btn_Huy.Location.Y + sizeBoundText.Height + 35);
                        btn_Save.Location = new Point(btn_Save.Location.X, btn_Save.Location.Y + sizeBoundText.Height + 35);
                    } 
                }
            }
            catch
            {

            }
        }
        private void xoaRichTxt()
        {
            var sizeBoundText = txt_noidung.Bounds;

            if (listTextBox.Count > 1)
            {
                countTextBox--;

                Controls.Remove(listTextBox[listTextBox.Count - 1]);
                Controls.Remove(listDelLabel[listDelLabel.Count - 1]);
                listTextBox.RemoveAt(listTextBox.Count - 1);
                listDelLabel.RemoveAt(listDelLabel.Count - 1);
                Controls.Remove(listButton[listButton.Count - 1]);
                Controls.Remove(listCheckBox[listCheckBox.Count - 1]);
                listButton.RemoveAt(listButton.Count - 1);
                listCheckBox.RemoveAt(listCheckBox.Count - 1);
                pathList.RemoveAt(pathList.Count - 1);
                this.Size = new Size(Size.Width, Size.Height - sizeBoundText.Height - 30);
                lb_Them.Location = new Point(lb_Them.Location.X, lb_Them.Location.Y - sizeBoundText.Height - 35);
                lb_DelRichText.Location = new Point(lb_DelRichText.Location.X, lb_DelRichText.Location.Y - sizeBoundText.Height - 35);
                btn_XacNhan.Location = new Point(btn_XacNhan.Location.X, btn_XacNhan.Location.Y - sizeBoundText.Height - 35);
                btn_Huy.Location = new Point(btn_Huy.Location.X, btn_Huy.Location.Y - sizeBoundText.Height - 35);
                btn_Save.Location = new Point(btn_Save.Location.X, btn_Save.Location.Y - sizeBoundText.Height - 35);
            }
        }

        private void lb_Them_Click(object sender, EventArgs e)
        {
            countTextBox++;
            var sizeBoundText = txt_noidung.Bounds;
            var sizeForm = this.Bounds;
            addRichText();
            for (int i = countTextBox - 1; i < listTextBox.Count; i++)
            {
                Controls.Add(listTextBox[i]);
                Controls.Add(listDelLabel[i]);
                Controls.Add(listButton[i]);
                Controls.Add(listCheckBox[i]);
            }
            this.Size = new Size(this.Size.Width, this.Size.Height + sizeBoundText.Height + 30);
            lb_Them.Location = new Point(lb_Them.Location.X, lb_Them.Location.Y + sizeBoundText.Height + 35);
            lb_DelRichText.Location = new Point(lb_DelRichText.Location.X, lb_DelRichText.Location.Y + sizeBoundText.Height + 35);
            btn_XacNhan.Location = new Point(btn_XacNhan.Location.X, btn_XacNhan.Location.Y + sizeBoundText.Height + 35);
            btn_Huy.Location = new Point(btn_Huy.Location.X, btn_Huy.Location.Y + sizeBoundText.Height + 35);
            btn_Save.Location = new Point(btn_Save.Location.X, btn_Save.Location.Y + sizeBoundText.Height + 35);
            //this.ControlAdded(richTxtBox);

            //MessageBox.Show(sizeBoundText);
        }

        private void lb_DelRichText_Click(object sender, EventArgs e)
        {

            var sizeForm = this.Bounds;
            xoaRichTxt();

        }

        private void btn_XacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                string pathPhanHoi = "Database/Data_PhanHoi.json";
                //list_acc = File.ReadAllLines(filePath);
                var strPhanHoi = File.ReadAllText(pathPhanHoi);
                List<PhanHoi> listPhanHoi = JsonConvert.DeserializeObject<List<PhanHoi>>(strPhanHoi);
                //listDraft.Clear();

                for (int i = 0; i < listTextBox.Count; i++)
                {
                    PhanHoi draft = new PhanHoi();
                    draft.STT = i + 1;
                    draft.ResponseID = $"Local000{i + 1}";
                    draft.ContentID = $"Local0{i + 1}";
                    draft.Type = typeNoiDung;
                    draft.Status = "0";
                    draft.UserID = "";
                    draft.Link = "";
                    draft.Create_time = System.DateTime.Now.ToString();
                    
                    draft.Content = listTextBox[i].Text;
                    foreach (var img in pathList[i])
                    {
                        draft.Image += " \"" + img + "\"";
                    }
                    if ((draft.Content.Length > 1) || (draft.Image != null))
                    {
                        listPhanHoi.Add(draft);
                    }
                }
                using (StreamWriter file = File.CreateText("Database/Data_PhanHoi.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, listPhanHoi);
                }
            }
            catch { }
            MessageBox.Show("Thêm nội dung thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
        }
        List<PhanHoi> listDraft = new List<PhanHoi>();

        private void btn_Save_Click(object sender, EventArgs e)
        {
            listDraft.Clear();

            for (int i = 0; i < listTextBox.Count; i++)
            {
                PhanHoi draft = new PhanHoi();
                draft.STT = i + 1;
                draft.ResponseID = $"Draft000{i+1}";
                draft.ContentID = $"Draft0{i+1}";
                draft.Type = typeNoiDung;
                draft.Status = "0";
                draft.UserID = "";
                draft.Link = "";
                draft.Create_time = System.DateTime.Now.ToString();
                draft.Content = listTextBox[i].Text;
                foreach (var img in pathList[i])
                {
                    draft.Image += " \"" + img + "\"";
                }
                if ((draft.Content.Length > 0) || (draft.Image != null))
                {
                    listDraft.Add(draft);
                }
            }
            using (StreamWriter file = File.CreateText("Data/Drafts.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, listDraft);
            }
            MessageBox.Show("Đã lưu lại bản nháp!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fThemNoiDung_Load(object sender, EventArgs e)
        {

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
                    pathList[0] = pathAnh;

                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
                
            }
        }
        private void btn_Anh_Click_1(object sender, EventArgs e, int index)
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
                    pathList[index] = pathAnh;

                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }

            }
        }
    }
}
