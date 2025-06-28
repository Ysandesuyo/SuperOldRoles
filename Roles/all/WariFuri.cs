
using BepInEx;
using System.Collections.Generic;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
using JetBrains.Annotations;
using SuperOldRoles.Internal.Data;
using SuperOldRoles.Roles;
using SuperOldRoles;
using System;
using static SuperOldRoles.Roles.all.roleenum;
using BepInEx.Logging;
using SuperOldRoles.Rpc;
using System.Linq;
using AmongUs.GameOptions;
using SuperOldRoles.Roles.all;
using Hazel;

namespace SuperOldRoles.Roles.all
{
    public static class WariFuri
    {
        public static bool jeswarisita = false;

        //役職設定画面ができるまでは手動で人数を設定しよう
        public static int JesterKazu = 0;
        
        
        public static List<PlayerControl> dataaa;
        public static List<PlayerRolePair> rolelist;

        [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
        public static class UketoriRpc
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                if (callId == (byte)rpcenum.rpc.RoleClear)
                {
                    rolelist = new List<PlayerRolePair>();
                    return;
                }
                if(callId == (byte)rpcenum.rpc.SetRole)
                {


                    if (__instance == null)
                    {
                        MyMyPlugin.Instance.Log.LogError("PlayerControl is null in SetRole RPC handler.");
                        return;
                    }

                    RoleEnum naniroleenum = (RoleEnum)reader.ReadByte();
                    byte dareid = reader.ReadByte();


                    // reader.ReadByte();は、1回目がnaniroleenum、2回目がPlayerId。


                    // 何の役職を選ぶことにしたっけ？

                    //誰を選ぶんだっけ、受け取ったIDからプレイヤー情報を抽出しよっと
                    PlayerControl pl = null;
                    foreach (var p in PlayerControl.AllPlayerControls)
                    {
                        if (p.PlayerId == dareid)
                        {
                            pl = p;
                            break;
                        }
                    }
                    //よし、これで全員のゲームの内部情報にだれがどの役職かセットされたぞ！
                    rolelist.Add(new PlayerRolePair(pl, naniroleenum));


                    //以下、テスト用

                    //わかりやすく名前つけとく
                    pl.RpcSetName("私の役職は"+naniroleenum+"です");




                    // RoleListは、クライアントそれぞれに保存したい
                    MyMyPlugin.Instance.Log.LogInfo("Jesterは、"+rolelist.Where(i=>i.Role==RoleEnum.Jester).First().Player.PlayerId);
                    
                }
                if (callId == (byte)rpcenum.rpc.SetRoleImpo)
                {
                    RoleEnum naniroleenum = (RoleEnum) reader.ReadByte();
                    byte dareid = reader.ReadByte();


                    // reader.ReadByte();は、1回目がnaniroleenum、2回目がPlayerId。


                    // 何の役職を選ぶことにしたっけ？

                    //誰を選ぶんだっけ、受け取ったIDからプレイヤー情報を抽出しよっと
                    PlayerControl pl = null;
                    foreach (var p in PlayerControl.AllPlayerControls)
                    {
                        if (p.PlayerId == dareid)
                        {
                            pl = p;
                            break;
                        }
                    }


                    //よし、これで全員のゲームの内部情報にだれがどの役職かセットされたぞ！
                    rolelist.Add(new PlayerRolePair(pl, naniroleenum));

                    //ここのdataaa.Removeで、nullが検出されたのでエラーになる
                    MyMyPlugin.Instance.Log.LogInfo(rolelist.Count+"rolelistcount");

                    //以下、テスト用

                    //わかりやすく名前つけとく
                    pl.RpcSetName("私の役職はインポスターです");
                    


                }
            }
        }
        public static class RoleWariFuri
        {
            public static void Clear()
            {
                MyMyPlugin.Instance.Log.LogInfo("Clear ni kimasita");
                List<PlayerControl> allpl = PlayerControl.AllPlayerControls.ToArray().ToList();
                


                //これをコピーしてrpcを使おう
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.RoleClear, SendOption.Reliable);
                writer.Write("rolecleardayo");
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                //ホストにも送る
                MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.RoleClear, SendOption.Reliable,AmongUsClient.Instance.HostId);
                writer2.Write("rolecleardayo");
                AmongUsClient.Instance.FinishRpcImmediately(writer2);




            }
            public static void SetRole(List<PlayerControl> players, byte naniroleenum, int count)
            {
                // 選ぶ予定の人数より無役のプレイヤーが少なかったらブッチ
                if(players.Count < count)
                {
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    //誰を選ぶかに使うランダムな数字をセット
                    var rand = new Random();
                    PlayerControl selectedPlayer = players[rand.Next(players.Count)];
                    

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRole, SendOption.Reliable);
                    writer.Write(naniroleenum);
                    writer.Write(selectedPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    //ホストにも送る
                    MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRole, SendOption.Reliable, AmongUsClient.Instance.HostId);
                    writer2.Write(naniroleenum);
                    writer2.Write(selectedPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);




                    dataaa.Remove(selectedPlayer); // 役職を与えたプレイヤーは、役職が二重になっても困るので、リストから消しとく


                }
                

            }
        }

        [HarmonyPatch(typeof(GameManager),nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            
            public static void Postfix(GameManager __instance)
            {
               
                    rolelist = new List<PlayerRolePair>();
                

                if (!AmongUsClient.Instance.AmHost)
                {
                    return;
                }



                RoleWariFuri.Clear();
                
                    dataaa = new List<PlayerControl>();
                    dataaa = PlayerControl.AllPlayerControls.ToArray().ToList();
                



                if (dataaa.Count < 4)
                {
                    return;
                }
                foreach (PlayerControl pla in dataaa)
                {
                    if (pla.Data.RoleType==RoleTypes.Impostor)
                    {
                        PlayerControl selectedPlayer = pla;
                        List<PlayerControl> allpl = PlayerControl.AllPlayerControls.ToArray().ToList();


                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable);
                            writer.Write((byte)RoleEnum.Impostor);
                            writer.Write(selectedPlayer.PlayerId);

                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                        MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable, AmongUsClient.Instance.HostId);
                        writer2.Write((byte)RoleEnum.Impostor);
                        writer2.Write(selectedPlayer.PlayerId);

                        AmongUsClient.Instance.FinishRpcImmediately(writer2);




                        dataaa.Remove(pla); // 役職を与えたプレイヤーは、役職が二重になっても困るので、リストから消しとく



                        break;
                    }
                }

                RoleWariFuri.SetRole(dataaa, (byte) RoleEnum.Jester, JesterKazu);

                //クルーの処理

                return;
            }
        }



    }

}

