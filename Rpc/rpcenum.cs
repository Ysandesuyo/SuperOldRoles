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
            RoleClearKakunin=103,
            ZenInClear = 104,
            EmperorVictory = 105,
            JesterVictory = 106,
            RoleSetRpc = 107,
        }
        public enum rolesetrpc : byte
        {
            SheriffCool = 1,
            PresidentKaisu = 2,
        }
       
    }
}
