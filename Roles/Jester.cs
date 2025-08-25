using HarmonyLib;
using SuperOldRoles.Internal.Data;
using SuperOldRoles.Roles.all;
using TMPro;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    class JesterPatch
    {
        public static Color color = Color.magenta;
        public static string rolename = "ハングドマン";
        public static string roledescription = "名残惜しいです が追放された。";
        static bool jeskatikana = false;


        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && dare.Role == RoleEnum.Jester)
                    {

                        GameObject titletext = new GameObject("jibunrolehyoujimoji" + PlayerControl.LocalPlayer.PlayerId);
                        titletext.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
                        titletext.layer = 5;
                        titletext.transform.SetLocalZ(-1f);
                        titletext.transform.SetLocalY(1.5f);
                        titletext.transform.localScale = new Vector3(2f, 3f, 1f);

                        TextMeshPro testText1 = titletext.AddComponent<TextMeshPro>();
                        testText1.fontSize = 1; // フォントサイズ 48px
                        testText1.color = Color.magenta;
                        testText1.alignment = TextAlignmentOptions.Center; // 中央揃え
                        testText1.enableWordWrapping = false; // 自動改行無し
                        testText1.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
                        testText1.text = "ハングドマン";
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
        public static class JesterExilepatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                int plid = __instance.PlayerId;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == plid && dare.Role == RoleEnum.Jester)
                    {
                        jeskatikana = true;
                        GameManager.Instance.RpcEndGame((GameOverReason) EndGameReason.Jester, false);
                        
                    }
                }
            }
        }
        
        /*
         * 表示の仕方わからん
        [HarmonyPatch(typeof(EndGameManager),nameof(EndGameManager.SetEverythingUp))]
        public static class owariwari
        {
            public static bool Prefix(EndGameManager __instance)
            {
                
                if (!jeskatikana)
                {
                    return true;
                }
                PoolablePlayer plobj = UnityEngine.Object.Instantiate(__instance.PlayerPrefab, __instance.transform);
                plobj.transform.localPosition = new Vector3(0, 0, 0);
                plobj.transform.localScale = new Vector3(10, 5, 1);
                __instance.PlayerPrefab = plobj;
                __instance.WinText.text = "ジェスターの勝ち！";
                __instance.WinText.color = Color.magenta;
                return true;
            }
        }
        */

        //[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameStart))]


        /*

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcCompleteTask))]
        //[改造するよ（typeof（改造する大きな種類）,nameof(そのなかで何が起こったら改造する？）]
        public static class killkillpatch//これは適当な名前
        {
            static void Postfix(PlayerControl __instance)
            //static void Postfix(プレイヤーの型　この文字で呼び出す)
            {
                PlayerControl.LocalPlayer.MurderPlayer(__instance, MurderResultFlags.Succeeded);
                // タスクを終わらせたプレイヤー　.　プレイヤーに対して　.　殺す　（）の中は殺し方のカスタム　(タスクを終わらせたプレイヤー, MurderResultFlags.Succeeded);
            }
        }*/
    }
}
