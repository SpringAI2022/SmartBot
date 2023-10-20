using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace SmartBot
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        public bool _stop = true;
        protected string message { get; set; }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            var options = new RestClientOptions("http://127.0.0.1:5000");
            //{
            //    Authenticator = new HttpBasicAuthenticator("username", "password")
            //};
            var client = new RestClient(options);
            var request = new RestRequest("/login");
            string hwID = HardwareID.Value();
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("hwID", hwID);
            //request.AddFile("file", path);
            //var response = client.Post(request);
            var response = client.Post(request);
            if (response.IsSuccessful)
            {
                var content = response.Content; // Raw content as string
                var jResponse = JsonConvert.DeserializeObject<dynamic>(content);
                var status = jResponse.status;
                if (status == "success")
                {
                    _stop = false;
                    string token = jResponse.token;
                    string configPath = Environment.CurrentDirectory + "/config.json";
                    string strToken = "";
                    if (File.Exists(configPath))
                    {
                        strToken = File.ReadAllText(configPath);
                    }
                    var jToken = JsonConvert.DeserializeObject<GeneralConfig>(strToken);
                    if (jToken == null || strToken == "")
                    {
                        jToken = new GeneralConfig();
                    }
                    jToken.access_token = token;
                    using (StreamWriter file = File.CreateText("config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, jToken);
                    }
                    message = "Đăng nhập thành công";
                    this.Close();
                }
                else
                {
                    _stop = true;
                    message = jResponse.message;
                    MessageBox.Show(message, "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _stop = true;
                MessageBox.Show("Lỗi kết nối đến server!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._stop = true;
            this.Close();
        }
    }
}
