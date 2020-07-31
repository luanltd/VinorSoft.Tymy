using System;
using System.Collections.Generic;
using System.Text;

namespace VinorSoft.Tymy.Service.Model
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string SubDomain { get; set; }
        public bool GrantPermissionDebug { get; set; }
        public bool EnableCaptCha { get; set; }
        public int Expired { get; set; }
    }
}
