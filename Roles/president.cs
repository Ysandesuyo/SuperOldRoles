using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using SuperOldRoles.Roles.all;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    //  skip, count
    class PresidentPatch
    {

        public static Color color = Color.yellow;
        public static string rolename = "トナルト・ドランブ大統領";
        public static string roledescription = "大統領ｫｫﾉｫｫﾄﾞｸｻｲｾｲｼﾞｷﾓﾁｨｨｨ\n（実際の人物・団体とは関係ありません）";
        public static int kaisu = 1;
        public static int count = 0;
        public static bool enable = false;

        [HarmonyPatch(typeof(GameManager),nameof(GameManager.StartGame))]
        public static class aaaaa
        {
            public static void Postfix(GameManager __instance)
            {
                count = 0;
            }
        }


        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CastVote))]
        public static class PresidentVotepatch
        {
            public static void Prefix(MeetingHud __instance, [HarmonyArgument(0)] byte srcplid, [HarmonyArgument(1)] byte susplid)
            {
                bool ikukana = false;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == srcplid && dare.Role == RoleEnum.president)
                    {

                        ikukana = true;
                    }
                }
                if (kaisu <= count)
                {
                    return;
                }
                if (!ikukana)
                {

                    return;
                }
                PlayerControl srcpl = null;
                PlayerControl suspl = null;
                foreach (PlayerControl pl in PlayerControl.AllPlayerControls)
                {
                    if (srcplid == pl.PlayerId)
                    {
                        srcpl = pl;
                        continue;
                    }
                    else if (susplid == pl.PlayerId)
                    {
                        suspl = pl;
                        continue;
                    }
                }
                if (srcpl == null || suspl == null)
                {

                    return;
                }

                __instance.CancelInvoke();

                Il2CppStructArray<MeetingHud.VoterState> aaaa = new Il2CppStructArray<MeetingHud.VoterState>(1);

                MeetingHud.VoterState bbbb = new MeetingHud.VoterState();
                bbbb.VoterId = srcplid;
                bbbb.VotedForId = susplid;

                aaaa.AddItem(bbbb);

                MyMyPlugin.Instance.Log.LogInfo(bbbb);
                if (aaaa == null)
                {

                    MyMyPlugin.Instance.Log.LogInfo("aaaanull");
                }
                enable = true;
                foreach (PlayerControl pl in PlayerControl.AllPlayerControls)
                {
                    __instance.RpcClearVote(pl.PlayerId);
                }
                __instance.RpcVotingComplete(aaaa, suspl.Data, false);
                count++;

            }
        }

    }
}
