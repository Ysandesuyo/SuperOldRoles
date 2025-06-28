using HarmonyLib;
using SuperOldRoles.Roles.all;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    class JesterPatch
    {
        static bool jeskatikana = false;
        /*
         * 表示の仕方わからん
        [HarmonyPatch(typeof(IntroCutscene),nameof(IntroCutscene.CoBegin))]
        public static class jesterrolehyouji
        {
            public static bool Prefix(IntroCutscene __instance)
            {
                PlayerControl pl = PlayerControl.LocalPlayer;
                if (WariFuri.role.Count == 0)
                {
                    return true;
                }
                foreach (PlayerRolePair pair in WariFuri.role)
                {
                    if (pair.Role == RoleEnum.Jester && pl.PlayerId==pair.Player.plid)
                    {
                        __instance.TeamTitle.text = "第三陣営";
                        __instance.TeamTitle.color = Color.gray;
                        __instance.RoleText.text = "ジェスター";
                        __instance.RoleBlurbText.text = "ジェスター";
                        __instance.RoleBlurbText.color = Color.magenta;
                        __instance.RoleText.color = Color.magenta;
                        __instance.BackgroundBar.material.color = Color.gray;
                        break;
                    }
                }
                return true;
            }
        }
        */

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
        public static class JesterExilepatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                MyMyPlugin.Instance.Log.LogInfo(WariFuri.rolelist.Count + WariFuri.rolelist[1].Player.PlayerId);
                int plid = __instance.PlayerId;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == plid && dare.Role == RoleEnum.Jester)
                    {
                        jeskatikana = true;
                        GameManager.Instance.RpcEndGame(GameOverReason.ImpostorDisconnect, false);
                        
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
