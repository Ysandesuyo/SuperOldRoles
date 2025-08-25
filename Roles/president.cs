using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using SuperOldRoles.Roles.all;
using TMPro;
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

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && dare.Role == RoleEnum.president)
                    {

                        GameObject titletext = new GameObject("jibunrolehyoujimoji" + PlayerControl.LocalPlayer.PlayerId);
                        titletext.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
                        titletext.layer = 5;
                        titletext.transform.SetLocalZ(-1f);
                        titletext.transform.SetLocalY(1.5f);
                        titletext.transform.localScale = new Vector3(2f, 3f, 1f);

                        TextMeshPro testText1 = titletext.AddComponent<TextMeshPro>();
                        testText1.fontSize = 1; // フォントサイズ 48px
                        testText1.color = new Color(1f, 1f, 0f);
                        testText1.alignment = TextAlignmentOptions.Center; // 中央揃え
                        testText1.enableWordWrapping = false; // 自動改行無し
                        testText1.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
                        testText1.text = "トナルト・ドランブ大統領";
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipStatus),nameof(ShipStatus.Begin))]
        public static class aaaaa
        {
            public static void Postfix()
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
