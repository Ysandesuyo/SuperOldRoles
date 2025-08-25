using HarmonyLib;
using SuperOldRoles.Roles.all;
using TMPro;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    class Bait
    {

        public static Color color = Color.cyan;
        public static string rolename = "ベイト";
        public static string roledescription = "俺の最後の会議だぜ！\n受け取ってくれー！！";

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && dare.Role == RoleEnum.Bait)
                    {

                        GameObject titletext = new GameObject("jibunrolehyoujimoji" + PlayerControl.LocalPlayer.PlayerId);
                        titletext.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
                        titletext.layer = 5;
                        titletext.transform.SetLocalZ(-1f);
                        titletext.transform.SetLocalY(1.5f);
                        titletext.transform.localScale = new Vector3(2f, 3f, 1f);

                        TextMeshPro testText1 = titletext.AddComponent<TextMeshPro>();
                        testText1.fontSize = 1; // フォントサイズ 48px
                        testText1.color = Color.cyan;
                        testText1.alignment = TextAlignmentOptions.Center; // 中央揃え
                        testText1.enableWordWrapping = false; // 自動改行無し
                        testText1.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
                        testText1.text = "ベイト";
                    }
                }
            }
        }

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
