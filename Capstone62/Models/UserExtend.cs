using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone62.Models
{
    public class UserExtend
    {
        
        public int UserExtendId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }

        public UserExtend(string userName)
        {
            UserName = userName;
        }
    }
}
