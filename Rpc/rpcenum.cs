using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazel;

namespace SuperOldRoles.Rpc
{
    public class rpcenum
    {
        public enum rpc:byte
        {
            RoleClear=100,
            SetRole=101,
            SetRoleImpo=102,
            StarNameYellow=110,
        }
       
    }
}
