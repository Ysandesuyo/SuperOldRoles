
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SuperOldRoles.Internal.Data;
using SuperOldRoles.Roles;
using SuperOldRoles.Roles.all;
using SuperOldRoles.Rpc;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Patch
{
    public class EndGame
    {
        // できない、warifuri.rolelistがnullになる
        static GameOverReason areason = 0;/*
        static List<cPlayerRolePair> currentList = new List<cPlayerRolePair>();*/
        static List<PlayerSnapshot> currentList = new List<PlayerSnapshot>();
        static List<cPlayerRolePair> thisuseList = new List<cPlayerRolePair>();

        //gameoverreasonはすでに8まである
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public static class EndGamePatch
        {
            public static void Prefix([HarmonyArgument(0)] ref EndGameResult reason)
            {
                areason = reason.GameOverReason;
                currentList.Clear(); // 既存のリストをクリア
                thisuseList.Clear(); // 既存のリストをクリア
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    foreach (PlayerRolePair item in WariFuri.rolelist)
                    {
                        if (item.Player.PlayerId == player.PlayerId)
                        {

                            thisuseList.Add(new cPlayerRolePair(new PlayerSnapshot(player),item.Role));
                            break;
                        }
                    }
                }
            }
        }
        public class PlayerSnapshot
        {
            public byte PlayerId;
            public bool IsDead;
            public NetworkedPlayerInfo PlayerData;

            public PlayerSnapshot(PlayerControl player)
            {
                PlayerId = player.PlayerId;
                IsDead = player.Data.IsDead;
                PlayerData = player.Data;
            }
        }

        /*
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class endgameeeee
        {
            public static void Postfix()
            {
                MyMyPlugin.Instance.Log.LogInfo("warifuri.rolelist count  "+WariFuri.rolelist.Count);
                currentList.Clear(); // 既存のリストをクリア

                CachedPlayerData pll = null;
                foreach (PlayerRolePair item in WariFuri.rolelist)
                {
                    
                    foreach(PlayerControl pllll in PlayerControl.AllPlayerControls)
                    {
                        if (item.Player.PlayerId == pllll.PlayerId)
                        {

                            pll = new CachedPlayerData(pllll.Data);

                            var copy = new cPlayerRolePair(pll, item.Role);
                            currentList.Add(copy);
                        }
                        else
                        {
                            MyMyPlugin.Instance.Log.LogInfo(currentList.Count + "foreach pl pl "+item.Player.PlayerId+pllll.PlayerId);
                        }
                    }


                }

                pcurrentList.Clear(); // 既存のリストをクリア

                foreach (PlayerRolePair item in WariFuri.rolelist)
                {
                    var matchingPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().First(p => p.PlayerId == item.Player.PlayerId);

                    if (matchingPlayer != null)
                    {
                        var copy = new PlayerRolePair(matchingPlayer, item.Role);
                        pcurrentList.Add(copy);
                    }
                    else
                    {
                        MyMyPlugin.Instance.Log.LogWarning($"[WARN] PlayerId {item.Player.PlayerId} に一致する PlayerControl が見つかりませんでした");
                    }

                }
                MyMyPlugin.Instance.Log.LogInfo(currentList.Count + "aaaacurrentcount");
                
                
            }
        }

        */


        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public static class DidImpostorsWinPatch
        {
            public static void Prefix(EndGameManager __instance)
            {
                MyMyPlugin.Instance.Log.LogInfo((byte) areason+"reason syuuryouji");
                    foreach (PoolablePlayer pb in __instance.transform.GetComponentsInChildren<PoolablePlayer>())
                    {
                        Object.Destroy(pb.gameObject);
                    }
                if((byte) areason <= 8)
                {
                    //  乗っ取り勝利役職が増えた場合
                    //  (1),(2),(3) を既存のものと同じように書く


                    // (1)
                    cPlayerRolePair emperor = null;


                    foreach (cPlayerRolePair dare in thisuseList)
                    {
                        if (dare.Player.IsDead)
                        {
                            continue;
                        }


                        // (2)
                        if (dare.Role == RoleEnum.Emperor && !dare.Player.IsDead)
                        {
                            emperor = dare;
                        }

                    }


                    // (3)
                    if (emperor!=null)
                    {
                        EndGameResult.CachedWinners.Clear();
                        EndGameResult.CachedWinners.Add(new CachedPlayerData(emperor.Player.PlayerData));

                        __instance.WinText.text = "天皇陛下 万歳";
                        __instance.WinText.color = Emperor.color;
                        __instance.WinText.gameObject.SetActive(true);

                        return;
                    }
                    
                }

                if ((EndGameReason)areason == EndGameReason.Jester)
                {
                    foreach (cPlayerRolePair dare in thisuseList)
                    {
                        if (dare.Role == RoleEnum.Jester)
                        {
                            MyMyPlugin.Instance.Log.LogInfo("jestersyouri");
                            EndGameResult.CachedWinners.Clear();
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(dare.Player.PlayerData));
                            

                            __instance.WinText.text = "吊られた 男";
                            __instance.WinText.color = JesterPatch.color;
                            __instance.WinText.gameObject.SetActive(true);
                            return;
                            /*
                            GameObject plobj = Object.Instantiate(dare.Player.Data.gameObject);
                            plobj.transform.position = new Vector3(0, 0, 0);
                            plobj.transform.localScale = new Vector3(1, 1, 1);
                            plobj.SetActive(true);*/
                        }
                    }
                }
                return;
            }

            public static void Postfix(EndGameManager __instance)
            {
                MyMyPlugin.Instance.Log.LogInfo((byte)areason + "reason syuuryouji");
                if ((byte)areason <= 8)
                {
                    cPlayerRolePair emperor = null;

                    foreach (cPlayerRolePair dare in thisuseList)
                    {
                        if (dare.Player.IsDead)
                        {
                            continue;
                        }
                        if (dare.Role == RoleEnum.Emperor && !dare.Player.IsDead)
                        {
                            emperor = dare;
                        }
                        /*
                        GameObject plobj = Object.Instantiate(dare.Player.Data.gameObject);
                        plobj.transform.position = new Vector3(0, 0, 0);
                        plobj.transform.localScale = new Vector3(1, 1, 1);
                        plobj.SetActive(true);*/
                    }

                    if (emperor != null)
                    {
                        EndGameResult.CachedWinners.Clear();
                        EndGameResult.CachedWinners.Add(new CachedPlayerData(emperor.Player.PlayerData));

                        __instance.WinText.text = "天皇陛下 万歳";
                        __instance.WinText.color = Emperor.color;
                        __instance.WinText.gameObject.SetActive(true);

                        return;
                    }
                }
                //インポが普通に勝ったらどうなる？

                if ((EndGameReason)areason == EndGameReason.Jester)
                {
                    foreach (cPlayerRolePair dare in thisuseList)
                    {
                        if (dare.Role == RoleEnum.Jester)
                        {

                            __instance.WinText.text = "ジェスター 勝利";
                            __instance.WinText.color = JesterPatch.color;
                            __instance.WinText.gameObject.SetActive(true);
                            return;
                            /*
                            GameObject plobj = Object.Instantiate(dare.Player.Data.gameObject);
                            plobj.transform.position = new Vector3(0, 0, 0);
                            plobj.transform.localScale = new Vector3(1, 1, 1);
                            plobj.SetActive(true);*/
                        }
                    }
                }
                return;
            }
        }
    }
}
