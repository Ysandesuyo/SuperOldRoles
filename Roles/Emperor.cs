using HarmonyLib;
using InnerNet;
using StableNameDotNet;
using System.Collections.Generic;
using static SuperOldRoles.Roles.all.roleenum;
using static UnityEngine.GraphicsBuffer;
using SuperOldRoles.Roles.all;
using UnityEngine;

namespace SuperOldRoles.Roles
{
    class Emperor
    {

        public static Color color = Color.red;
        public static string rolename = "天皇陛下";
        public static string roledescription = "天皇陛下バンザｧｧｧｧイ！！";
        public static float EmperorDistance = 0.5f; // 天皇の近くにいるときの距離
        private static bool isingame = false;

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendFreeChat))]
        static class SendChatPatch
        {
            static bool Prefix(ChatController __instance)
            {




                // rolelistを読み込んでも初期化された状態にしかならない






                List<PlayerRolePair> rolelistoo = WariFuri.rolelist;
                string text = __instance.freeChatField.textArea.text;
                PlayerControl pl = PlayerControl.LocalPlayer;
                bool tennnouirukana = false;
                foreach (PlayerRolePair dare in rolelistoo)
                {
                    if (dare.Role == RoleEnum.Emperor)
                    {
                        tennnouirukana = true;
                        break;
                    }
                }
                if (!tennnouirukana)
                {
                    return true;
                }

                
                if ((text.Contains("天皇")||text.Contains("てんのう"))&&(text.Contains("しね") || text.Contains("ころすぞ") || text.Contains("ばか") || text.Contains("バカ") || text.Contains("アホ") || text.Contains("あほ") || text.Contains("死ね") || text.Contains("4ね") || text.Contains("殺すぞ")))
                {
                        
                    pl.RpcSendChat(pl.name+" は、天皇陛下への敬意を欠きました。不敬罪により死刑!!");
                    pl.MurderPlayer(pl, MurderResultFlags.Succeeded);
                    
                }

                if ((text.Contains("天皇") || text.Contains("てんのう")) && (text.Contains("ばんざ") || text.Contains("万歳") || text.Contains("バンザ") || text.Contains("さいこう") || text.Contains("最高") || text.Contains("大好き")))
                {

                    pl.RpcSendChat(pl.name + " さんの言葉に、天皇陛下はうれしく思っています。");

                }

                // ロビーで実行されないのが正しい
                

                return true;

            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
        public static class EmperorExilepatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                int plid = __instance.PlayerId;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == plid && dare.Role == RoleEnum.Emperor)
                    {
                        foreach(PlayerControl player in PlayerControl.AllPlayerControls)
                        {
                            __instance.MurderPlayer(player, MurderResultFlags.Succeeded);
                        }

                    }
                }
            }
        }

        

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                isingame = true;
            }
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
        public static class owattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                isingame = false;
            }
        }
        

        [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.FixedUpdate))]
        public static class EmperorNearPatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (!isingame)
                {
                    return;
                }
                int plid = __instance.PlayerId;
                bool ikukana = false;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == plid && dare.Role == RoleEnum.Emperor)
                    {
                        ikukana = true;  // 天皇がいるかどうかを確認
                    }
                }

                if (!ikukana)
                {
                    return;   // 天皇がいない場合は何もしない
                }

                if (__instance.Data == null || __instance == null || __instance.Data.IsDead)
                {
                    return;
                }
                var myPos = __instance.GetTruePosition();

                foreach (PlayerControl pl in PlayerControl.AllPlayerControls)
                {
                    if (pl == __instance)
                    {
                        continue;   // 自分自身は無視
                    }
                    if (pl.Data.IsDead || pl.Data.Disconnected || pl.inVent)
                    {
                        continue;   // 死んでいるプレイヤーは無視
                    }
                    float distance = Vector2.Distance(myPos,pl.GetTruePosition());
                    if(distance <= EmperorDistance)
                    {
                        pl.MurderPlayer(pl, MurderResultFlags.Succeeded); // 天皇の近くにいるプレイヤーを殺す
                        
                    }
                }
            }
        }
        

        /*
         * 動かんからなし
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
        public static class EmperorMurderedPatch
        {
            public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
            {
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    
                    if (dare.Player.PlayerId == target.PlayerId && dare.Role == RoleEnum.Emperor)
                    {
                        __instance.RpcMurderPlayer(__instance,true);
                        return false;
                    }
                    return true;
                }
                return true;
            }
        }
        */
    }
}


