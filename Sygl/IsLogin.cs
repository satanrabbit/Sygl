using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sygl
{
    public class IsLogin
    {
        public IsLogin(string t)
        {
            Login = false;
            Title = t;
        }
        public bool Login { get; set; }
        public string Title { get; set; }
    }
}
