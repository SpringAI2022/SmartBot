using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBot
{
    internal class ObjectSTT
    {
    }
    //public class ARRAY_NOIDUNG
    //{
    //public string[]
    //}
    public class JSON_Convert
    {
        public string[] Noi_dung { get; set; }
        public string[] Link { get; set; }
    }
    public class PhanHoi
    {
        public int STT { get; set; }
        public string ResponseID { get; set; }
        public string ContentID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string UserID { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Create_time { get; set; }
    }

    public class HanhDongOfKichBan
    {
        public int id { get; set; }
        public bool status { get; set; }
    }

    public class KichBan
    {
        public int id { get; set;}
        public List<int> id_hanhdong { get; set; }
        public string mota { get; set; }
        public bool status { get; set; }
        public string created_at { get; set; } // Năm-Tháng-Ngày Giờ:Phút
    }

    public class HanhDong
    {
        public int id { get; set; }
        public int type { get; set; }
        public string content { get; set; }
        public string link { get; set; }
        public string attach { get; set; }
        public string user_profile { get; set; }
        public bool status { get; set; }
        public string post_time { get; set; } // Năm-Tháng-Ngày Giờ:Phút
        public string created_at { get; set; } // Năm-Tháng-Ngày Giờ:Phút
    }

}
