using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperOldRoles.Roles.all
{
    public class roleenum
    {
        public class PlayerRolePair
        {
            public PlayerControl Player;
            public RoleEnum Role;

            public PlayerRolePair(PlayerControl player, RoleEnum role)
            {
                Player = player;
                Role = role;
            }
        }

        public enum RoleEnum:byte
        {
            Crewmate = 0,
            Bait = 1,
            

            Jester = 50,
            Emperor = 51,

            Impostor = 100,
        }
    }
}
