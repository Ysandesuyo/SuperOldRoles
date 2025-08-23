using System;
using AmongUs.GameOptions;
using HarmonyLib;
using SuperOldRoles.Roles.all;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Roles
{
    public static class Sheriff
    {

        public static Color color = Color.yellow;
        public static string rolename = "シェリフ";
        public static string roledescription = "猿シェリフが動物園から脱走したぞ！！\n捕まえろ！！";
        private static bool iskillbtn = false;
        private static float KillTimer = 0f; // クールダウンタイマー
        public static int killCool = 27;
        private static bool isingame = false;

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class hajimattatoki
        {
            public static void Postfix(GameManager __instance)
            {
                isingame = true;
                iskillbtn = false;
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



        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class sheriffupdate
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (__instance != PlayerControl.LocalPlayer)
                {
                    return;
                }
                if (ShipStatus.Instance == null || !isingame)
                {
                    return;

                }
                if(WariFuri.rolelist.Count==0||WariFuri.rolelist == null)
                {
                    return; // 役職リストが空またはnullの場合は何もしない
                }
                
                if (!WariFuri.zeninclearkana)
                {
                    return;
                }


                /*

                [Error  :Il2CppInterop] During invoking native->managed trampoline
Exception: System.NullReferenceException: Object reference not set to an instance of an object.
   at SuperOldRoles.Roles.Sheriff.sheriffupdate.Postfix(PlayerControl __instance)
   at DMD<PlayerControl::FixedUpdate>(PlayerControl this)
   at (il2cpp -> managed) FixedUpdate(IntPtr , Il2CppMethodInfo* )





                */



                bool ikukana = false;

                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == __instance.PlayerId && dare.Role == RoleEnum.Sheriff)
                    {
                        ikukana = true;
                    }
                }
                if (!ikukana)
                {
                    return;
                }

                if (MeetingHud.Instance != null || __instance.Data.IsDead)
                {
                     HudManager.Instance.KillButton.gameObject.SetActive(false);

                }
                else
                {
                    if (HudManager.Instance.KillButton == null)
                    {
                        return;
                    }
                    HudManager.Instance.KillButton.gameObject.SetActive(true);
                    
                    KillTimer = Math.Max(0, KillTimer - Time.deltaTime); //reduce Cooldown
                    HudManager.Instance.KillButton.SetCoolDown((KillTimer > 0) ? KillTimer : 0, killCool);
                    PlayerControl closestPlayer = FindClosestTarget(PlayerControl.LocalPlayer,1f);
                    if (closestPlayer != null)
                    {
                        HudManager.Instance.KillButton.SetTarget(closestPlayer);
                    }
                    if (iskillbtn && KillTimer == 0 && closestPlayer != null)
                    {
                        if (IsSheriffKillOk(closestPlayer))
                        {
                            __instance.RpcMurderPlayer(closestPlayer, true); // クリックされたら殺す

                        }
                        else
                        {
                            __instance.RpcMurderPlayer(__instance, true); // クリックされたら自分を殺す
                           
                            


                        }

                        KillTimer = killCool;
                        
                        iskillbtn = false; // クリックされたらフラグをリセット
                    }else if (iskillbtn)
                    {
                        iskillbtn = false;
                    }
                    
                }

            }
        }


        [HarmonyPatch(typeof(KillButton),nameof(KillButton.DoClick))]
        public static class KillButtonClickPatch
        {
            public static void Postfix(KillButton __instance)
            {
                
                if(ShipStatus.Instance == null || !isingame)
                {
                    return;
                }
                
                
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && dare.Role != RoleEnum.Sheriff)
                    {
                        return;// 役職がシェリフでなければ何もしない
                    }
                }
                iskillbtn = true;
                return;
            }
        }

        public static bool IsSheriffKillOk(PlayerControl target)
        {
            iskillbtn = false;
            foreach (PlayerRolePair dare in WariFuri.rolelist)
            {
                if (dare.Player.PlayerId == target.PlayerId && ((byte)dare.Role >= 50 && (byte)dare.Role < 100))
                {
                    return true;
                }
            }

            if (target.Data.RoleType == RoleTypes.Shapeshifter || target.Data.RoleType == RoleTypes.Impostor || target.Data.RoleType == RoleTypes.Phantom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static PlayerControl FindClosestTarget(PlayerControl player, float nearvalue)
        {
            if(ShipStatus.Instance == null)
            {
                return null;
                
            }
            PlayerControl result = null;
            Vector2 pos = player.GetTruePosition();
            foreach ( PlayerControl pl in PlayerControl.AllPlayerControls)
            {
                if(!pl.Data.Disconnected && pl.PlayerId != player.PlayerId && !pl.Data.IsDead && !pl.inVent)
                {
                    Vector2 plpos = pl.GetTruePosition();
                    float distance = Vector2.Distance(pos, plpos);
                    if (distance <= nearvalue)
                    {
                        result = pl;
                    }
                }
            }
            return result;
        }
    }
}
