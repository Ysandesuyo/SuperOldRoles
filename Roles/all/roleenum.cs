using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperOldRoles.Patch;
using UnityEngine;
using static SuperOldRoles.Patch.EndGame;

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
        public class cPlayerRolePair
        {
            public PlayerSnapshot Player;
            public RoleEnum Role;

            public cPlayerRolePair(PlayerSnapshot player, RoleEnum role)
            {
                Player = player;
                Role = role;
            }
        }
        public enum RoleEnum:byte
        {
            Crewmate = 0,
            Bait = 1,
            Sheriff = 2,
            president = 3,

            Jester = 50,
            Emperor = 51,

            Impostor = 100,
        }
    }
}
