using HarmonyLib;
using SuperOldRoles.Roles.all;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    class Bait
    {
        [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.MurderPlayer))]
        public static class BeitReportPatch
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
            {
                int plid = target.PlayerId;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == plid && dare.Role == RoleEnum.Bait)
                    {
                        if(target.Data.IsDead)
                        {
                            __instance.ReportDeadBody(target.Data);
                        }
                    }
                }
            }
        }
    }
}
