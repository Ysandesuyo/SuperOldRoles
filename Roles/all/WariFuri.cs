
using BepInEx;
using System.Collections.Generic;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
using SuperOldRoles.Internal.Data;
using SuperOldRoles.Roles;
using SuperOldRoles;
using System;
using static SuperOldRoles.Roles.all.roleenum;
using BepInEx.Logging;
using SuperOldRoles.Rpc;
using System.Linq;
using System.Threading;
using AmongUs.GameOptions;
using SuperOldRoles.Roles.all;
using Hazel;

namespace SuperOldRoles.Roles.all
{
    public static class WariFuri
    {

        //役職設定画面ができるまでは手動で人数を設定しよう
        public static int JesterKazu = 1;
        public static int BaitKazu = 3;
        public static int EmperorKazu = 0;
        public static int SheriffKazu = 1;
        public static int PresidentKazu = 0;
        public static int ZenbuKazu = JesterKazu + BaitKazu + EmperorKazu + SheriffKazu + PresidentKazu;
        //clearしてない状態で呼ぶのを防ごう
        public static int clearsitakazu = 0;
        public static List<PlayerControl> dataaa = new List<PlayerControl>();
        public static List<PlayerRolePair> rolelist = new List<PlayerRolePair>();
        public static bool zeninclearkana = false;
        [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
        public static class UketoriRpc
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {









                if (callId == (byte)rpcenum.rpc.RoleClear)
                {
                    rolelist.Clear();
                    dataaa.Clear();
                    zeninclearkana = false;
                    clearsitakazu = 0;
                    if (rolelist == null || rolelist.Count == 0)
                    {
                        rolelist = new List<PlayerRolePair>();
                    }
                    MessageWriter writer7 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.RoleClearKakunin, SendOption.Reliable, AmongUsClient.Instance.HostId);
                    writer7.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer7);
                    return;
                }
                if(callId == (byte)rpcenum.rpc.SetRole)
                {

                    MyMyPlugin.Instance.Log.LogInfo("SetRole Rpc wo uketorimasita");
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
                    foreach (var p in PlayerControl.AllPlayerControls.ToArray().ToList())
                    {
                        if (p.PlayerId == dareid)
                        {
                            pl = p;
                            break;
                        }
                    }
                    //よし、これで全員のゲームの内部情報にだれがどの役職かセットされたぞ！
                    rolelist.Add(new PlayerRolePair(pl, naniroleenum));

                    MyMyPlugin.Instance.Log.LogInfo(rolelist.Count);

                    //以下、テスト用

                    //わかりやすく名前つけとく
                    pl.SetName("私の役職は"+naniroleenum+"です");




                    // RoleListは、クライアントそれぞれに保存したい

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

                    //以下、テスト用

                    //わかりやすく名前つけとく
                    pl.SetName("私の役職はインポスターです");
                    


                }
            }
        }
        public static class RoleWariFuri
        {

            public static void Clear()
            {

                //これをコピーしてrpcを使おう
                MessageWriter writer5 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.RoleClear, SendOption.Reliable);
                writer5.Write("rolecleardayo");
                AmongUsClient.Instance.FinishRpcImmediately(writer5);
                //ホストにも送る
                MessageWriter writer6 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.RoleClear, SendOption.Reliable,AmongUsClient.Instance.HostId);
                writer6.Write("rolecleardayo");
                AmongUsClient.Instance.FinishRpcImmediately(writer6);




            }
            public static void SetRole(PlayerControl playerdata, byte naniroleenum)
            {




                PlayerControl selectedPlayer = playerdata;
                    

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRole, SendOption.Reliable);
                    writer.Write(naniroleenum);
                    writer.Write(selectedPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    //ホストにも送る
                    MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRole, SendOption.Reliable, AmongUsClient.Instance.HostId);
                    writer2.Write(naniroleenum);
                    writer2.Write(selectedPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);



                    /*  呼び出し時に移転
                    dataaa.Remove(selectedPlayer); // 役職を与えたプレイヤーは、役職が二重になっても困るので、リストから消しとく
                    */

                
                

            }
        }
        public static bool shouldrun = false;

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class UketoriRpc2
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                
                if ((rpcenum.rpc) callId == rpcenum.rpc.RoleClearKakunin)
                {
                    if (!AmongUsClient.Instance.AmHost)
                    {
                        return;
                    }
                    
                    clearsitakazu++;
                    MyMyPlugin.Instance.Log.LogInfo(clearsitakazu);//途中
                    if(clearsitakazu >= PlayerControl.AllPlayerControls.Count)
                    {
                        clearsitakazu = 0;
                        shouldrun = false;
                        new Thread(() =>
                        {
                            Thread.Sleep(250); // 1秒待つ
                            shouldrun = true;
                        }).Start();
                        while (!shouldrun)
                        {

                        }

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.ZenInClear, SendOption.Reliable);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.ZenInClear, SendOption.Reliable,AmongUsClient.Instance.HostId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer2);

                        zeninclearkana = true;


                        dataaa = new List<PlayerControl>();
                        dataaa = PlayerControl.AllPlayerControls.ToArray().ToList();




                        if (dataaa.Count < 4)
                        {
                            return;
                        }
                        for (int i = dataaa.Count - 1; i >= 0; i--)
                        {
                            PlayerControl pla = dataaa[i];
                            if (pla.Data.RoleType == RoleTypes.Impostor)
                            {
                                PlayerControl selectedPlayer = pla;
                                List<PlayerControl> allpl = PlayerControl.AllPlayerControls.ToArray().ToList();


                                MessageWriter writer3 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable);
                                writer3.Write((byte)RoleEnum.Impostor);
                                writer3.Write(selectedPlayer.PlayerId);

                                AmongUsClient.Instance.FinishRpcImmediately(writer3);
                                MessageWriter writer4 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable, AmongUsClient.Instance.HostId);
                                writer4.Write((byte)RoleEnum.Impostor);
                                writer4.Write(selectedPlayer.PlayerId);

                                AmongUsClient.Instance.FinishRpcImmediately(writer4);




                                dataaa.Remove(pla); // 役職を与えたプレイヤーは、役職が二重になっても困るので、リストから消しとく
                            }
                        }

                        Random random = new Random(Guid.NewGuid().GetHashCode());
                        List<RoleEnum> fruits = new List<RoleEnum>();
                        for (int i = 0; i < BaitKazu; i++) fruits.Add(RoleEnum.Bait);
                        for (int i = 0; i < JesterKazu; i++) fruits.Add(RoleEnum.Jester);
                        for (int i = 0; i < EmperorKazu; i++) fruits.Add(RoleEnum.Emperor);
                        for (int i = 0; i < SheriffKazu; i++) fruits.Add(RoleEnum.Sheriff);
                        for (int i = 0; i < PresidentKazu; i++) fruits.Add(RoleEnum.president);
                        fruits = fruits.OrderBy(a => Guid.NewGuid()).ToList();
                        List<PlayerControl> availableBoxIndices = dataaa;
                        int fruitsToPlace = Math.Min(fruits.Count, dataaa.Count);
                        for (int i = 0; i < fruitsToPlace; i++)
                        {
                            RoleEnum fruit = fruits[i];

                            int randomBoxIndex = random.Next(availableBoxIndices.Count);
                            PlayerControl boxIndex = availableBoxIndices[randomBoxIndex];
                            RoleWariFuri.SetRole(boxIndex, (byte)fruit);
                            availableBoxIndices.RemoveAt(randomBoxIndex);
                        }





                        dataaa.Clear();


                    }
                }
                if ((rpcenum.rpc)callId == rpcenum.rpc.ZenInClear)
                {
                    zeninclearkana = true;
                }
            }
        }

        //いけるんかな
        [HarmonyPatch(typeof(ShipStatus),nameof(ShipStatus.Begin))]
        public static class hajimattatoki
        {
            
            public static void Postfix(ShipStatus __instance)
            {
                MyMyPlugin.Instance.Log.LogInfo("shipbegindayo");
                rolelist.Clear();
                dataaa.Clear();
                zeninclearkana = false;
                clearsitakazu = 0;
                if (!AmongUsClient.Instance.AmHost)
                {
                    return;
                }

                RoleWariFuri.Clear();

                /*

                dataaa = new List<PlayerControl>();
                    dataaa = PlayerControl.AllPlayerControls.ToArray().ToList();
                



                if (dataaa.Count < 4)
                {
                    return;
                }
                for (int i = dataaa.Count - 1; i >= 0; i--)
                {
                    PlayerControl pla = dataaa[i];
                    if (pla.Data.RoleType==RoleTypes.Impostor)
                    {
                        PlayerControl selectedPlayer = pla;
                        List<PlayerControl> allpl = PlayerControl.AllPlayerControls.ToArray().ToList();


                            MessageWriter writer3 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable);
                            writer3.Write((byte)RoleEnum.Impostor);
                            writer3.Write(selectedPlayer.PlayerId);

                            AmongUsClient.Instance.FinishRpcImmediately(writer3);
                        MessageWriter writer4 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpcenum.rpc.SetRoleImpo, SendOption.Reliable, AmongUsClient.Instance.HostId);
                        writer4.Write((byte)RoleEnum.Impostor);
                        writer4.Write(selectedPlayer.PlayerId);

                        AmongUsClient.Instance.FinishRpcImmediately(writer4);




                        dataaa.Remove(pla); // 役職を与えたプレイヤーは、役職が二重になっても困るので、リストから消しとく
                    }
                }

                RoleWariFuri.SetRole(dataaa, (byte) RoleEnum.Jester, JesterKazu);
                RoleWariFuri.SetRole(dataaa, (byte)RoleEnum.Bait, BaitKazu);
                RoleWariFuri.SetRole(dataaa, (byte)RoleEnum.Emperor, EmperorKazu);




                //クルーの処理


                */

                return;
            }
        }



    }

}

