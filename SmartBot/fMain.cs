using Newtonsoft.Json;
using SmartBot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using File = System.IO.File;
using System.Net.NetworkInformation;
using ChromeAuto;
using RestSharp;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SmartBot
{
    public partial class fMain : Form
    {
        private bool _stop = true;
        private int timeLoad { get; set; }
        private FbAction FB { get; set; }
        private fCaiDatTuongTac_NewFeeds formNewfeeds { get; set; }
        private fCaiDatTuongTac_Nhom formNhom { get; set; }
        private fCaiDatTuongTac_Reels formReels { get; set; }
        private fCaiDatTuongTac_Stories formStories { get; set; }
        private fCaiDatTuongTac_BinhLuan formBinhLuan { get; set; }
        private GeneralConfig jConfig { get; set; }
        private string Grfile = null;
        private List<dynamic> link_List;
        private string linkPath = "";
        private string pathUD = Environment.CurrentDirectory + "/UserData";
        private string pathBrowser = Environment.CurrentDirectory + "/Browser";
        private string[] listFolder = { };
        private string accPath = null;
        private PauseTokenSource pts;
        private CancellationTokenSource cts;
        private List<dynamic> acc_List;
        List<string> ListActivateSession = new List<string>();
        List<SessionChrome> sessionChromes = new List<SessionChrome>();
        string[] arr_Anh = null;
        public fMain()
        {
            InitializeComponent();
            this.timeLoad = Convert.ToInt16(num_Delay.Value) * 1000;
            readToken();
            loadConfig();
            LoadTinh();
        }
        private async void readToken()
        {
            string configPath = Environment.CurrentDirectory +"/config.json";
            string strToken = "";
            if (File.Exists(configPath))
            {
                strToken = File.ReadAllText(configPath);
            }
            var jToken = JsonConvert.DeserializeObject<GeneralConfig>(strToken);

            if (jToken == null || strToken == "" || jToken.access_token == "")
            {
                Console.WriteLine("Không có token, chạy hàm login");
                fLogin login = new fLogin();
                login.ShowDialog();
                _stop = login._stop;
            }
            else
            {
                //var authenticator = new JwtAuthenticator(jToken.access_token);
                var options = new RestClientOptions("http://127.0.0.1:5000");
                var client = new RestClient(options);
                var request = new RestRequest("/protected");
                //{
                //    Authenticator = authenticator
                //};
                request.AddHeader("Authorization", $"Bearer {jToken.access_token}");
                string hwID = HardwareID.Value();
                request.AddParameter("hwID", hwID);
                //request.AddFile("file", path);
                //var response = client.Post(request);
                var response = client.Post(request);
                if (response.IsSuccessful)
                {
                    var content = response.Content; // Raw content as string
                    var jResponse = JsonConvert.DeserializeObject<dynamic>(content);
                    var status = jResponse.status;
                    string mess = jResponse.message;
                    if (status == "success")
                    {
                        _stop = false;
                    }
                    else
                    {
                        _stop = true;
                        if (mess.Contains("expired")){
                            /*
                             * Check xem token hết hạn không.
                             * Hết hạn thì xóa token trong file config
                             * Mở form login lên để người dùng đăng nhập lại
                             * Và tạo token mới
                             */
                            jToken.access_token = "";
                            using (StreamWriter file = File.CreateText("config.json"))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Serialize(file, jToken);
                            }
                            var resultBox = MessageBox.Show("Phiên đăng nhập đã hết hạn, hãy đăng nhập lại!", "Thông tin", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if(resultBox == DialogResult.OK)
                            {
                                fLogin login = new fLogin();
                                login.ShowDialog();
                                _stop = login._stop;
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        //MessageBox.Show(jResponse.message);
                        //Console.WriteLine(jResponse.message);
                    }
                }
                else
                {
                    _stop = true;
                    MessageBox.Show("Lỗi kết nối đến server!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }
        }
        private void loadConfig()
        {
            /*
             * tạo ra 1 hàm thực hiện tải vào cấu hình của ứng dụng từ file json
             * các cấu hình từ file bao gồm các biến trong chương trình
             */
            try
            {
                var configPath = "config.json";
                var strConfig = File.ReadAllText(configPath);
                jConfig = JsonConvert.DeserializeObject<GeneralConfig>(strConfig);
                if(jConfig.pathUD == null || jConfig.pathUD == "") { jConfig.pathUD = Environment.CurrentDirectory + "/UserData"; }
                if(jConfig.pathLog == null || jConfig.pathLog == "") { jConfig.pathLog = Environment.CurrentDirectory + "/log.txt"; }
                if(jConfig.delayBeforeAction == null || jConfig.delayBeforeAction == 0) { jConfig.delayBeforeAction = 2000; }
                if(jConfig.delayAfterAction == null || jConfig.delayAfterAction == 0) { jConfig.delayAfterAction = 2000; }
            }
            catch { }

        }
        private void LoadTinh()
        {
            /*
             * Load đơn vị hành chính các tỉnh thành phố
             */
            try
            {
                var donviHanhChinh = File.ReadAllText("Data/DonViHanhChinh.json");
                dynamic donvi = JsonConvert.DeserializeObject<dynamic>(donviHanhChinh);
                foreach (dynamic item in donvi)
                {
                    cb_Tinh.Items.Add(item.Name);
                }
                cb_Tinh.SelectedIndex = 0;
            }
            catch { }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);
        private static IntPtr FindHandle(IntPtr parentHandle, string className, string title)
        {
            IntPtr handle = IntPtr.Zero;

            for (var i = 0; i < 50; i++)
            {
                handle = FindWindowEx(parentHandle, IntPtr.Zero, className, title);

                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }
            return handle;
        }
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, StringBuilder lParam);
        private async void Run4Profile(string Profile, int pro5)
        {
            /*
             * Chạy chương trình
             */
            FB = new FbAction(Profile, txt_UA.Text, txt_Proxy.Text);
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[pro5].Cells[1].Value = pro5 + 1;
                dataGridView.Rows[pro5].Cells[2].Value = Profile.Split()[1];
                dataGridView.Rows[pro5].Cells[3].Value = txt_UA.Text;
                dataGridView.Rows[pro5].Cells[4].Value = txt_Proxy.Text;
                dataGridView.Rows[pro5].Cells[5].Value = "Đang khởi tạo!!!";
            });
            await Task.Delay(jConfig.delayBeforeAction);
            if (cb_LoginFanPage.Checked)
            {
                await Task.Delay(jConfig.delayBeforeAction);
                await FB.SwitchPage();
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = "Đang chuyển sang tương tác fanpage";
                });
                await Task.Delay(this.timeLoad);
            }
            await Task.Delay(jConfig.delayAfterAction);
            int contNewFeeds = formNewfeeds == null ? 0 : formNewfeeds.listBaiDang_Select.Count;
            int countGroups = formNhom == null ? 0 : formNhom.listBaiDang_Select.Count;
            int countBinhluan = formBinhLuan == null ? 0 : formBinhLuan.listPhanHoi_Load.Count;
            int maxContent = contNewFeeds;
            if (countGroups > maxContent)
            {
                maxContent = countGroups;
            }
            else if (countBinhluan > maxContent)
            {
                maxContent = countBinhluan;
            }
            for (int i = 0; i < maxContent; i++)
            {
                if (clb_Actions.GetItemChecked(0) && contNewFeeds > 0 && i < contNewFeeds)
                {
                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = "Đang đăng bài lên tường";
                    });
                    await FB.PostWallAsync(formNewfeeds.listBaiDang_Select[i]);
                    saveLog("Đăng lên Tường", formNewfeeds.listBaiDang_Select[i].UserID, System.DateTime.Now.ToString(), "");
                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = $"Đã đăng {i + 1} bài lên tường thành công!!";
                    });
                    await Task.Delay(jConfig.delayAfterAction);

                }
                if (clb_Actions.GetItemChecked(1) && countGroups > 0 && i < countGroups)
                {
                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = "Đang đăng bài lên nhóm";
                    });
                    await FB.PostGroupAsync(formNhom.listBaiDang_Select[i]);
                    saveLog("Đăng lên Nhóm", formNhom.listBaiDang_Select[i].UserID, DateTime.Now.ToString(), formNhom.listBaiDang_Select[i].Link);

                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = $"Đã đăng {i + 1} bài lên Nhóm thành công!!";
                    });
                    await Task.Delay(jConfig.delayAfterAction);
                }
                if (clb_Actions.GetItemChecked(2) && countBinhluan > 0 && i < countBinhluan)
                {
                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = "Đang bình luận";
                    });
                    await FB.PostToID(formBinhLuan.listPhanHoi_Select[i]);
                    saveLog("Bình luận", formBinhLuan.listPhanHoi_Select[i].UserID, DateTime.Now.ToString(), formBinhLuan.listPhanHoi_Select[i].Link);

                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = $"Đã bình luận {i + 1} bình luận thành công!!";
                    });
                    await Task.Delay(jConfig.delayAfterAction);

                }
                if (clb_Actions.GetItemChecked(3) || clb_Actions.GetItemChecked(4))
                {
                    dataGridView.Invoke((MethodInvoker)delegate
                    {
                        dataGridView.Rows[pro5].Cells[5].Value = "Đang thả cảm xúc và chia sẻ bài viết!!!";
                    });
                    await Task.Delay(jConfig.delayBeforeAction);
                    string noidungChiase = ""; // Lấy nội dung chia sẻ từ formChiase
                    await FB.LikeAndShare(clb_Actions.GetItemChecked(4), clb_Actions.GetItemChecked(3), noidungChiase, "");
                    saveLog("Bình luận", formBinhLuan.listPhanHoi_Select[i].UserID, DateTime.Now.ToString(), formBinhLuan.listPhanHoi_Select[i].Link);
                }
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = "Nghỉ 10s để thực hiện tiếp các hành động!!!";
                });
                await Task.Delay(10000);
            }
            if (cb_JoinGroup.Checked)
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = "Đang tìm kiếm nhóm và tham gia nhóm";
                });
                await Task.Delay(jConfig.delayAfterAction);
                string keySearch = txt_KeySearchGroup.Text;
                int soluongGroupThamGia = 20;
                await FB.JoinGroupAsync(keySearch, soluongGroupThamGia);
                //saveLog("Tham gia nhóm")
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = $"Đã tham gia {soluongGroupThamGia} Nhóm từ key word: {keySearch}!!";
                });
                //await JoinGroupAsync(chrome);
                await Task.Delay(jConfig.delayAfterAction);
            }
            //if ((cb_PostID.Checked) && (txt_CmtID.Text != ""))
            //{
            //    await Task.Delay(2000);
            //    //await PostToID(chrome, txtCt.Text, txt_CmtID.Text);
            //    await Task.Delay(5000);
            //}
            var closeBox = MessageBox.Show("Tạm dừng 20s trước khi đóng!", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
            if (closeBox == DialogResult.OK)
            {
                await Task.Delay(20000);
            }
            await Task.Delay(2000);
        }
        private async Task RunAsync(CancellationToken ct)
        {
            List<Task> TaskList = new List<Task>();
            if(jConfig.pathUD == null || jConfig.pathUD == "") { jConfig.pathUD = Environment.CurrentDirectory + "/UserData"; }
            listFolder = Directory.GetDirectories(jConfig.pathUD, "Profile *");
            int Delay_ = Convert.ToInt16(timeLoad);
            this.BeginInvoke(new Action(() =>
            {
                bool flag = true;
                string[] lnoidung = { }, lgroup = { };
                if (listFolder.Length == 0)
                {
                    MessageBox.Show("Chưa đăng nhập. Hãy đăng nhập trước khi chạy!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    flag = false;
                }

                if (flag)
                {
                    for (int pro5 = 0; pro5 < listFolder.Length; pro5 += 2)
                    {
                        TaskList.Clear();
                        for (int pro6 = 0; pro6 < 2; pro6++)
                        {
                            //dataGridView.Rows[pro5].Cells["STT"].Value = pro5+1;
                            //dataGridView.Rows[pro5].Cells[2].Value = pro5+1;
                            string[] Profile_ = listFolder[pro5 + pro6].Split('\\');
                            int idxFolderProfile = Profile_.Length - 1;
                            string Profile = Profile_[idxFolderProfile];
                            Task task = new Task(() => Run4Profile(Profile, pro5 + pro6));
                            task.Start();
                            TaskList.Add(task);
                        }
                        //await TaskList[0];
                        //await TaskList[1];
                        Task.WaitAll(TaskList.ToArray());

                    }
                }

            }));

            //if (ct.IsCancellationRequested)
            //{
            //    break;
            //}

            await Task.Delay(20000);
            await pts.WaitWhilePausedAsync();
        }
        private void saveLog(string tenHanhDong, string uid, string thoiGian, string hanhDongVoi)
        {
            var obj = new
            {
                Ten = tenHanhDong,
                UID = uid,
                Thoi_Gian = thoiGian,
                Den = hanhDongVoi
            };
            using (StreamWriter file = File.AppendText(jConfig.pathLog))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj);
            }
        }
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            //Run();
            //tab
            btn_Stop.Enabled = true;
            cts = new CancellationTokenSource();
            pts = new PauseTokenSource();
            var tokenSource = cts.Token;
            await Task.Run(() => RunAsync(tokenSource));
            //await Run(tokenSource);
        }
        private static Random random = new Random();
        private void btn_Gr_Click(object sender, EventArgs e)
        {
            if (open_LinkFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    linkPath = open_LinkFile.FileName;
                    //list_acc = File.ReadAllLines(filePath);
                    var link_String = File.ReadAllText(linkPath);
                    link_List = JsonConvert.DeserializeObject<List<dynamic>>(link_String);
                    //if (link_List.Count > 0)
                    //{
                    //    clb_ListGroup.Items.Add("Chọn tất cả");
                    //}
                    foreach (var link in link_List)
                    {

                        //link_List.re
                        //cb_Link.Items.Add(link["Name"].ToString());
                        clb_ListGroup.Items.Add(link["Name"].ToString());
                    }
                    //cb_Link.SelectedIndex = 0;
                    //txt_Username.Text = link_List[0]["Name"];
                    //txtID_Gr.Text = link_List[0]["Link"];
                }
                catch
                {
                    MessageBox.Show("Vui lòng chọn lại file!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void btn_Create_Click(object sender, EventArgs e)
        {
            string userName = txt_Username.Text;
            string passWord = txt_Pass.Text;
            if ((userName == "") && (passWord == ""))
            {
                MessageBox.Show("Tài khoản và mật khẩu không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string nameProfile;
                if (userName.Contains("@"))
                {
                    nameProfile = Regex.Replace(userName.Split("@")[0], "[^0-9A-Za-z _-]", "");
                }
                else
                {
                    nameProfile = Regex.Replace(userName, "[^0-9A-Za-z _-]", "");
                }
                FB = new FbAction("Profile " + nameProfile, txt_UA.Text, txt_Proxy.Text);
                await FB.LoginFB(userName, passWord);
                var selectionSave = MessageBox.Show("Lưu lại tài khoản?", "Lưu thông tin", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (selectionSave == DialogResult.OK)
                {
                    if (open_AccFile.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            accPath = open_AccFile.FileName;
                            var acc_String = File.ReadAllText(accPath);
                            acc_List = JsonConvert.DeserializeObject<List<dynamic>>(acc_String);
                            var myAcc = new
                            {
                                username = userName,
                                password = passWord,
                                NameOfProfile = nameProfile
                            };
                            if (acc_List != null)
                            {
                                acc_List.Add(myAcc);
                            }
                            else
                            {
                                acc_List = new List<dynamic> { myAcc };
                            }
                            using (StreamWriter file = File.CreateText(accPath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Serialize(file, acc_List);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Vui lòng chọn lại file!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            btn_Run.Enabled = true;
            btn_Stop.Enabled = false;
        }
        private void file_Acc_Click(object sender, EventArgs e)
        {
            if (open_AccFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    accPath = open_AccFile.FileName;
                    //list_acc = File.ReadAllLines(filePath);
                    var acc_String = File.ReadAllText(accPath);
                    acc_List = JsonConvert.DeserializeObject<List<dynamic>>(acc_String);
                    foreach (var acc in acc_List)
                    {
                        cb_Account.Items.Add(acc["username"].ToString());
                    }
                    cb_Account.SelectedIndex = 0;
                    txt_Username.Text = acc_List[0]["username"];
                    txt_Pass.Text = acc_List[0]["password"];
                }
                catch
                {
                    MessageBox.Show("Vui lòng chọn lại file!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        [DllImport("User32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        public static void LeftClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }
        private void btn_Texts_Click(object sender, EventArgs e)
        {
            if (open_NoiDung.ShowDialog() == DialogResult.OK)
            {
                string noidung_file = "";
                try
                {
                    var filePath = open_NoiDung.FileName;
                    noidung_file = File.ReadAllText(filePath);
                    txt_TaoNoiDung.Text = noidung_file;
                    if (noidung_file != "")
                    {
                        btn_Tao_Json.Enabled = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Chọn lại file", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void btn_IMG_Click(object sender, EventArgs e)
        {
            if (open_Folder_IMG.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".mp4" };
                    var path_IMGs = open_Folder_IMG.SelectedPath;
                    arr_Anh = Directory.GetFiles(path_IMGs)
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
        private void btn_Tao_Json_Click(object sender, EventArgs e)
        {
            string[] arr_NoiDung = txt_TaoNoiDung.Text.Split(";");
            var myData = new
            {
                Noi_Dung = arr_NoiDung,
                Link = arr_Anh
            };
            using (StreamWriter file = File.CreateText("json_Data.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, myData);
            }
        }
        private void cb_UA_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string listUA = File.ReadAllText("Brave\\ua.json");
                var json_Data = JsonConvert.DeserializeObject<dynamic>(listUA);
                var ua_Desktop = json_Data["Desktop"];
                var ua_Mobile = json_Data.Mobile;
                var rd = random.Next(ua_Desktop.Count);

                if (cb_UA.Checked)
                {
                    txt_UA.Enabled = false;
                    txt_UA.Text = ua_Desktop[rd]["ua"].ToString();
                }
                else { txt_UA.Enabled = true; }
            }
            catch
            {
                MessageBox.Show("Không tìm thấy File chưa User-Agent", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (txt_TaoNoiDung.Text != "")
            {
                btn_Tao_Json.Enabled = true;
            }
            if (txt_TaoNoiDung.Text == "")
            {
                btn_Tao_Json.Enabled = false;
            }
        }
        private void cb_Account_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Account.Items.Count > 0)
            {
                var test = cb_Account.SelectedIndex;
                txt_Username.Text = acc_List[test]["username"];
                txt_Pass.Text = acc_List[test]["password"];
            }
        }
        private void clb_ListGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bool SelectAll = false;
            //for(int i = 1; i < clb_ListGroup.Items.Count; i++) {
            //if(clb_ListGroup.GetItemChecked(0)) { clb_ListGroup.SetItemChecked(i, true); }
            //else if (!clb_ListGroup.GetItemChecked(0)) { clb_ListGroup.SetItemChecked(i, false); }
            //else { continue; }
            //}
            string linkCheck = "";
            for (int i = 0; i < clb_ListGroup.Items.Count; i++)
            {
                if ((clb_ListGroup.GetItemChecked(i)) && (link_List[i]["Link"] != null)) { linkCheck += link_List[i]["Link"] + ";"; }
                //txtCt.Text = linkCheck.Trim(';');
            }
            txtID_Gr.Text = linkCheck.Trim(';');
            //clb_ListGroup. += clb_ListGroup_SelectedIndexChanged;

        }
        private void btn_SetUpNewfeeds_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[0].Cells[5].Value = "Đang cài đặt bài đăng lên Tường!!";
            });
            formNewfeeds = new fCaiDatTuongTac_NewFeeds();
            formNewfeeds.ShowDialog();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[0].Cells[5].Value = "Đã cài đặt đăng lên Tường!!";
            });
        }
        private void btn_SetUpNhom_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[0].Cells[5].Value = "Đang cài đặt bài đăng lên Nhóm!!";
            });
            formNhom = new fCaiDatTuongTac_Nhom();
            formNhom.ShowDialog();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[0].Cells[5].Value = "Đã cài đặt đăng lên Nhóm!!";
            });
        }
        private void btn_SetUpStories_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[0].Cells[5].Value = "Đang cài đặt bài đăng lên Stories!!";
            });
            formStories = new fCaiDatTuongTac_Stories();
            formStories.ShowDialog();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[0].Cells[5].Value = "Đã cài đặt đăng lên Stories!!";
            });
        }
        private void btn_SetupTrang_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[0].Cells[5].Value = "Đang cài đặt Bình luận!!";
            });
            formBinhLuan = new fCaiDatTuongTac_BinhLuan();
            formBinhLuan.ShowDialog();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[0].Cells[5].Value = "Đã cài đặt bình luận!!";
            });
        }
        private void btn_SetUpReels_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.Add();
                dataGridView.Rows[0].Cells[5].Value = "Đang cài đặt bài đăng lên Reels!!";
            });
            formReels = new fCaiDatTuongTac_Reels();
            formReels.ShowDialog();
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[0].Cells[5].Value = "Đã cài đặt đăng lên Reels!!";
            });
        }
        private async void btn_GetPostFrGroup_Click(object sender, EventArgs e)
        {
            Chrome chrome = new Chrome("http://localhost:9222");
            var sessions = chrome.GetAvailableSessions();
            string sessionWSEndpoint = "";
            foreach (var ss in sessions)
            {
                if (ss.url.Contains("facebook.com"))
                {
                    sessionWSEndpoint = ss.webSocketDebuggerUrl;
                    break;
                }
            }
            chrome.SetActiveSession(sessionWSEndpoint);

            await Task.Delay(2000);

            List<string> listID = new List<string>();
            string readInfoPost = File.ReadAllText("Data/postList.json");
            List<dynamic> listInfo = new List<dynamic>();
            //listInfo = JsonConvert.DeserializeObject<List<dynamic>>(readInfoPost);
            //listBaiDang = 

            listID = await FB.GetID(txt_LinkGroupToGetPost.Text);
            foreach (string id in listID)
            {
                string textContent = await FB.GetPostFrID(id);
                var sss = await FB.GetLog();
                string camXuc = "";
                foreach (var ss in sss)
                {
                    if ((ss != "") && (ss != null))
                    {
                        camXuc += ss.Trim() + " | ";
                    }
                }
                camXuc = camXuc.Trim().Trim('|');
                var myInfo = new
                {
                    Content = textContent,
                    React = camXuc
                };
                listInfo.Add(myInfo);
            }
            await Task.Delay(jConfig.delayAfterAction);
            using (StreamWriter file = File.CreateText("Data/postList.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, listInfo);
            }
            await Task.Delay(jConfig.delayBeforeAction);
            MessageBox.Show("Đã ghi dữ liệu vào tệp tin Data/postList.json", "Sucess!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private async Task<bool> MultiSearch(string Profile, string keySearch, string location, int ngroup, int pro5)
        {
            FbAction fb = new FbAction(Profile, txt_UA.Text, txt_Proxy.Text, 5, sessionChromes);
            ListActivateSession.Add(fb.ActivateSession);
            sessionChromes.Add(fb.gSessionChrome);
            //FB = new FbAction(Profile, txt_UA.Text, txt_Proxy.Text);
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[pro5].Cells[1].Value = pro5 + 1;
                dataGridView.Rows[pro5].Cells[2].Value = Profile.Split()[1];
                dataGridView.Rows[pro5].Cells[3].Value = txt_UA.Text;
                dataGridView.Rows[pro5].Cells[4].Value = txt_Proxy.Text;
                dataGridView.Rows[pro5].Cells[5].Value = "Đang khởi tạo!!!";
            });
            await Task.Delay(jConfig.delayBeforeAction);
            if (cb_LoginFanPage.Checked)
            {
                await Task.Delay(jConfig.delayBeforeAction);
                await fb.SwitchPage();
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = "Đang chuyển sang tương tác fanpage";
                });
                await Task.Delay(this.timeLoad);
            }
            if (clb_OptionSearch.GetItemChecked(0))
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = $"Đang tìm kiếm TRANG với từ khóa {keySearch} tại {location}";
                });
                await Task.Delay(jConfig.delayBeforeAction);
                await fb.searchScrollSmooth(Profile.Split()[1], "pages", keySearch, location, ngroup);
                await Task.Delay(jConfig.delayAfterAction);
            }
            if (clb_OptionSearch.GetItemChecked(1))
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[pro5].Cells[5].Value = $"Đang tìm kiếm NHÓM với từ khóa {keySearch} tại {location}";
                });
                await Task.Delay(jConfig.delayBeforeAction);
                await fb.searchScrollSmooth(Profile.Split()[1], "groups", keySearch, location, ngroup);
                await Task.Delay(jConfig.delayAfterAction);
            }
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[pro5].Cells[5].Value = $"Đã xong và đang tạm dừng 10s!";
            });
            await Task.Delay(10000);

            //await Task.Run(()=> fb.CloseBrowser());
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows[pro5].Cells[5].Value = $"Done tại {location}";
            });
            await Task.Delay(jConfig.delayAfterAction);
            return true;
        }
        private async Task searchKey(CancellationToken ct)
        {
            var donviHanhChinh = await File.ReadAllTextAsync("Data/DonViHanhChinh.json");
            dynamic donvi = JsonConvert.DeserializeObject<dynamic>(donviHanhChinh);
            List<Task> TaskList = new List<Task>();
            listFolder = Directory.GetDirectories(jConfig.pathUD, "Profile *");
            int Delay_ = Convert.ToInt16(num_Delay.Value);
            int numTask = Convert.ToInt16(numThreads.Value);
            int lenProfile = listFolder.Length;
            int minPro5 = numTask < lenProfile ? numTask : lenProfile;
            string keySearch = txtSearch.Text;
            //string location = txtLocation.Text;
            int ngroup = Convert.ToInt16(numSoLuong.Value);
            //int maxMAXvsThread = maxQHvsProfile > numTask ? maxQHvsProfile : numTask;
            this.BeginInvoke(new Action(async () =>
            {
                var select_Tinh = cb_Tinh.SelectedItem;
                var QuanHuyen = donvi[select_Tinh];
                int countQH = QuanHuyen.Count;
                int maxQHvsProfile = countQH > lenProfile ? countQH : lenProfile;

                bool flag = true;
                string[] lnoidung = { }, lgroup = { };
                if (lenProfile == 0)
                {
                    MessageBox.Show("Chưa đăng nhập. Hãy đăng nhập trước khi chạy!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    flag = false;
                }
                if (flag)
                {
                    int counter = 0;
                    //int pro5 = 0;
                    int cout = 0;
                    while (counter < maxQHvsProfile)
                    //for (int pro6 = 0; pro6 < maxQHvsProfile; pro6 += minPro5)
                    {
                        int minTask = maxQHvsProfile - counter >= minPro5 ? minPro5 : maxQHvsProfile - counter;
                        //MessageBox.Show(minTask.ToString());
                        await Task.Delay(500);
                        TaskList.Clear();
                        for (int pro5 = 0; pro5 < minTask; pro5++)
                        {
                            //dataGridView.Rows[pro5].Cells["STT"].Value = pro5+1;
                            //dataGridView.Rows[pro5].Cells[2].Value = pro5+1;
                            int idxProfile = (counter) % lenProfile;
                            int idxQH = (counter) % countQH;
                            await Task.Delay(500);
                            string location = QuanHuyen[idxQH];
                            string[] Profile_ = listFolder[idxProfile].Split('\\');
                            int idxFolderProfile = Profile_.Length - 1;
                            await Task.Delay(jConfig.delayBeforeAction);
                            //Task task = new Task(() => { MultiSearch(Profile_[idxFolderProfile], keySearch, location, ngroup, pro5 + pro6); });
                            Task task = Task.Run(() => MultiSearch(Profile_[idxFolderProfile], keySearch, location, ngroup, pro5 + (minTask * cout)));
                            //task.Start();
                            counter++;
                            await Task.Delay(jConfig.delayAfterAction);
                            TaskList.Add(task);
                        }
                        await Task.WhenAll(TaskList.ToArray());
                        cout++;
                        await Task.Run(() => KillBrave());
                    }
                }
            }));
            await Task.Delay(20000);
            await pts.WaitWhilePausedAsync();
        }
        #region Kịch bản
        private async Task RunKichBan(CancellationToken ct)
        {
            var donviHanhChinh = await File.ReadAllTextAsync("Data/DonViHanhChinh.json");
            dynamic donvi = JsonConvert.DeserializeObject<dynamic>(donviHanhChinh);
            List<Task> TaskList = new List<Task>();
            listFolder = Directory.GetDirectories(jConfig.pathUD, "Profile *");
            int lenProfile = listFolder.Length;
            this.BeginInvoke(new Action(async () =>
            {

                bool flag = true;
                if (lenProfile == 0)
                {
                    MessageBox.Show("Chưa đăng nhập. Hãy đăng nhập trước khi chạy!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    flag = false;
                }
                if (flag)
                {
                    await LoadKichBan();
                }
            }));
            await Task.Delay(20000);
            await pts.WaitWhilePausedAsync();
        }
        public async Task LoadKichBan()
        {
            /*
             * Hàm này thực hiện load các kịch bản trong database
             * Sau đó đưa kịch bản vào chạy tại hàm ChayKichBan
             */
            try
            {
                string pathKichBan = "Database/Data_KichBan.json";
                var strKichBan = await File.ReadAllTextAsync(pathKichBan);
                List<KichBan> listKichBan = JsonConvert.DeserializeObject<List<KichBan>>(strKichBan);
                foreach (KichBan kb in listKichBan)
                {
                    if (kb != null)
                    {
                        if (kb.status)
                        {
                            await ChayKichBan(kb);
                            kb.status = false;
                        }
                    }
                }
                using (StreamWriter file = File.CreateText("Database/Data_KichBan.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, listKichBan);
                }
            }
            catch
            {
                MessageBox.Show("Không tìm thấy file chứa kịch bản. Hãy kiểm tra lại!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task ChayKichBan(KichBan kichban)
        {
            /*
             * Hàm này thực hiện chạy 1 kịch bản.
             * Load các hành động trong kịch bản.
             * Chạy lần lượt các hành động tại hàm ChayHanhDong
             */
            //List<HanhDong> listHanhDongOfKichBan = new List<HanhDong>();
            try
            {
                string pathHanhDong = "Database/Data_HanhDong.json";
                //list_acc = File.ReadAllLines(filePath);
                var strHanhDong = await File.ReadAllTextAsync(pathHanhDong);
                List<HanhDong> listHanhDong = JsonConvert.DeserializeObject<List<HanhDong>>(strHanhDong);
                foreach (string id_hanhdong in kichban.id_hanhdong)
                {
                    /*
                     * Load các hành động có trong kịch bản vào 1 danh sách
                     */
                    HanhDong hanhdong = listHanhDong.Find(
                        (HanhDong ob) => ob.id == id_hanhdong);
                    if (hanhdong != null)
                    {
                        //listHanhDongOfKichBan.Add(hanhdong);
                        if (hanhdong.status)
                        {
                            if(jConfig.allowUser != null)
                            {
                                if(jConfig.allowUser.Contains(hanhdong.user_profile))
                                {
                                    await ChayHanhDong(hanhdong);
                                    hanhdong.status = false;

                                }
                            }
                            else if(jConfig.denyUser != null)
                            {
                                if(!jConfig.denyUser.Contains(hanhdong.user_profile))
                                {
                                    await ChayHanhDong(hanhdong);
                                    hanhdong.status = false;

                                }
                            }
                            else if((jConfig.allowUser == null) && (jConfig.denyUser == null))
                            {
                                await ChayHanhDong(hanhdong);
                                hanhdong.status = false;

                            }
                            //await ChayHanhDong(hanhdong);
                            //hanhdong.status = false;
                        }
                    }
                }
                using (StreamWriter file = File.CreateText("Database/Data_HanhDong.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, listHanhDong);
                }
                //if (listHanhDongOfKichBan.Count > 0)
                //{
                //    foreach (HanhDong hanhDong in listHanhDongOfKichBan)
                //    {
                //        if (hanhDong.status)
                //        {
                //            /*
                //             * Chạy hành động trong danh sách đã được load trên kia
                //             * Nếu hành động chưa được chạy (true), thực hiện chạy
                //             */
                //            await ChayHanhDong(hanhDong);
                //            hanhDong.status = false;

                //            //MessageBox.Show(hanhDong.ToString());
                //        }
                //    }
                //}
            }
            catch
            {
                MessageBox.Show("Không tìm thấy file chứa hành động. Hãy kiểm tra lại!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task ChayHanhDong(HanhDong hanhdong)
        {
            DateTime nowTime = DateTime.Now;
            DateTime postTime = Convert.ToDateTime(hanhdong.post_time);
            double diffTime = (postTime - nowTime).TotalMilliseconds;
            string folder_Profile = $"Profile {hanhdong.user_profile}";
            SessionChrome ss = sessionChromes.Find(
                    ob => ob.Profile == folder_Profile);
            FbAction fb;
            int idxStatus = 0;
            if (diffTime > 0)
            {
                await Task.Delay(Convert.ToInt32(diffTime));
            }
            if (ss == null)
            {
                idxStatus = dataGridView.Rows.Count;
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows.Add();
                    dataGridView.Rows[idxStatus].Cells[1].Value = idxStatus + 1;
                    dataGridView.Rows[idxStatus].Cells[2].Value = hanhdong.user_profile;
                    dataGridView.Rows[idxStatus].Cells[3].Value = txt_UA.Text;
                    dataGridView.Rows[idxStatus].Cells[4].Value = txt_Proxy.Text;
                    dataGridView.Rows[idxStatus].Cells[5].Value = "Đang khởi tạo!!!";
                });
                fb = new FbAction(folder_Profile, txt_UA.Text, txt_Proxy.Text, 5, sessionChromes);
                sessionChromes.Add(fb.gSessionChrome);
            }
            else
            {
                //MessageBox.Show(ss.Profile);
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        if (dataGridView.Rows[i].Cells[2].Value == hanhdong.user_profile)
                        {
                            idxStatus = i;
                        }
                    }
                });
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows.Add();
                    dataGridView.Rows[idxStatus].Cells[1].Value = idxStatus + 1;
                    dataGridView.Rows[idxStatus].Cells[2].Value = hanhdong.user_profile;
                    dataGridView.Rows[idxStatus].Cells[3].Value = txt_UA.Text;
                    dataGridView.Rows[idxStatus].Cells[4].Value = txt_Proxy.Text;
                    dataGridView.Rows[idxStatus].Cells[5].Value = "Đang khởi tạo!!!";
                });
                fb = new FbAction();
                fb.SetAcivateSession(ss.session);
                /*
                 * Lấy session đang chạy để chạy tiếp hoặc mở thêm tab mới từ session đó, rồi đưa Session vào Danh sách.
                 * Cần đưa FBAction vào 1 danh sách để quản lý, Danh sách chứa đối tượng: FBAction, tên Profile, Session Profile
                 */
            }
            if (hanhdong.type == 0)
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = "Đang đăng bài lên tường";
                });
                await fb.PostWall_KichBan(hanhdong);
                saveLog("Đăng lên Tường", hanhdong.user_profile, System.DateTime.Now.ToString(), hanhdong.link);
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = $"{hanhdong.user_profile} Đã đăng bài lên tường thành công!!";
                });
                await Task.Delay(jConfig.delayAfterAction);
            }
            else if (hanhdong.type == 1)
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = "Đang đăng bài vào groups";
                });
                await fb.PostGroup_KichBan(hanhdong);
                saveLog("Đăng lên Nhóm", hanhdong.user_profile, System.DateTime.Now.ToString(), hanhdong.link);
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = $"{hanhdong.user_profile} Đã đăng bài lên Nhóm thành công!!";
                });
                await Task.Delay(jConfig.delayAfterAction);
            }
            else if (hanhdong.type == 2)
            {
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = "Đang bình luận!!!";
                });
                await fb.CommentToID_KichBan(hanhdong);
                saveLog("Đăng lên Nhóm", hanhdong.user_profile, System.DateTime.Now.ToString(), hanhdong.link);
                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.Rows[idxStatus].Cells[5].Value = $"{hanhdong.user_profile} Đã đăng Bình luận thành công!!";
                });
                await Task.Delay(jConfig.delayAfterAction);
            }
        }
        #endregion
        private void KillBrave()
        {
            /*
             * Đóng trình duyệt
             */
            Process[] AllProcesses = Process.GetProcesses();
            foreach (var process in AllProcesses)
            {
                string s = process.ProcessName.ToLower();
                if (s == "brave")
                {
                    process.Kill();
                }
                //process.CloseMainWindow();
            }
        }
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
            });
            cts = new CancellationTokenSource();
            pts = new PauseTokenSource();
            var tokenSource = cts.Token;
            await Task.Run(() => searchKey(tokenSource));
        }

        private async void btnKichBan_Click(object sender, EventArgs e)
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                dataGridView.Rows.Clear();
            });
            cts = new CancellationTokenSource();
            pts = new PauseTokenSource();
            var tokenSource = cts.Token;
            await Task.Run(() => RunKichBan(tokenSource));
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            if (this._stop == true)
            {
                this.Close();

            }
        }
    }
}
