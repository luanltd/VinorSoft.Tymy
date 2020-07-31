using System;
using System.Collections.Generic;
using System.Text;

namespace VinorSoft.Tymy.Service.Model
{
   public class KTAppDomainResult
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public int ResultCode { get; set; }
        //public IList<string> Messages { get; set; }
        public string ResultMessage { get; set; }
    }
}
