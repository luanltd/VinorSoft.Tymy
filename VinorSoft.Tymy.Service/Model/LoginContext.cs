using System;
using System.Collections.Generic;
using System.Text;

namespace VinorSoft.Tymy.Service.Model
{
   public sealed class LoginContext
    {
        public UserModel CurrentUser { set; get; }

        private LoginContext()
        {
            CurrentUser = new UserModel();
        }
        private static LoginContext instance = null;
        public static LoginContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginContext();
                }
                return instance;
            }

        }
        public void SetCurrentUser(UserModel user)
        {
            CurrentUser = user;
        }
    }
}
