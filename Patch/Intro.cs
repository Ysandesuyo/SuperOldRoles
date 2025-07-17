
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
using static SuperOldRoles.Roles.all.roleenum;
using SuperOldRoles.Roles.all;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using SuperOldRoles.Roles;
using AsmResolver;
using Il2CppSystem.Collections.Generic;
using Hazel;
using SuperOldRoles.Rpc;
using System.Collections;
using BepInEx.Unity.IL2CPP.Utils;
using Epic.OnlineServices.Metrics;
using System.Linq;

namespace SuperOldRoles.Patch
{
    [HarmonyPatch]
    class Intro
    {




        private static RoleEnum role;


        private static bool beginsitakana = false;
        private static bool kaisikana = false;
        private static bool _hasReceivedRpc = false;


        public static bool HasReceivedRpc => _hasReceivedRpc;

        public static void OnRpcReceived()
        {
            _hasReceivedRpc = true;
        }

        public static void ResetRpcFlag()
        {
            _hasReceivedRpc = false;
        }


        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class startji
        {
            public static void Postfix(GameManager __instance)
            {
                kaisikana = true;
                beginsitakana = false;
            }
        }
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
        public static class endji
        {
            public static void Postfix(GameManager __instance)
            {
                kaisikana = false;
            }
        }



        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class UketoriRpc2
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {

                if ((rpcenum.rpc)callId == rpcenum.rpc.ZenInClear)
                {
                    OnRpcReceived();
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene),nameof(IntroCutscene.BeginImpostor))]
        public static class beginimpo
        {
            public static void Postfix()
            {

                role = RoleEnum.Impostor;
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch2
        {
            public static bool Prefix(IntroCutscene __instance, ref List<PlayerControl> teamToDisplay)
            {
                // 別のMonoBehaviourからコルーチンを開始
                if (HasReceivedRpc)
                {
                    MyMyPlugin.Instance.Log.LogInfo("prehaittayo");
                    foreach (PlayerRolePair dare in WariFuri.rolelist)
                    {
                        if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && ((byte)dare.Role) >= 50 && ((byte)dare.Role) < 100)
                        {
                            
                            teamToDisplay.Clear();
                            teamToDisplay.Add(PlayerControl.LocalPlayer);
                        }
                    }
                    return true;
                }
                __instance.BeginCrewmate(teamToDisplay);
                return false;
            }
            public static void Postfix(IntroCutscene __instance)
            {
                if (!HasReceivedRpc)
                {
                    MyMyPlugin.Instance.Log.LogInfo("postreturn");
                    return;
                }
                role = RoleEnum.Crewmate;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        role = dare.Role;
                    }
                }
                if((byte)role >= 50 && (byte) role < 100)
                {
                    Color col = Color.gray;
                    __instance.BackgroundBar.material.color = col;
                    __instance.TeamTitle.text = "第三陣営";
                    __instance.TeamTitle.color = col;
                }
                
            }
        }
        
       

        [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
        public static class shorolepatch
        {
            public static bool Prefix(IntroCutscene._ShowRole_d__41 __instance)
            {
                MyMyPlugin.Instance.Log.LogInfo("shorolenoprefixnihaittayoooooooo");
                if (HasReceivedRpc)
                {
                    return true;
                }

                __instance.__4__this.ShowRole();
                return false;
            }
            public static void Postfix(IntroCutscene._ShowRole_d__41 __instance)
            {
                IntroCutscene instance = __instance.__4__this;
                MyMyPlugin.Instance.Log.LogInfo(role);
                switch (role)
                {
                    case RoleEnum.Bait:
                        instance.RoleText.text = Bait.rolename;
                        instance.RoleText.color = Bait.color;
                        instance.RoleBlurbText.text = Bait.roledescription;
                        instance.RoleBlurbText.color = Bait.color;
                        break;
                    case RoleEnum.president:
                        instance.RoleText.text = PresidentPatch.rolename;
                        instance.RoleText.color = PresidentPatch.color;
                        instance.RoleBlurbText.text = PresidentPatch.roledescription;
                        instance.RoleBlurbText.color = PresidentPatch.color;
                        break;
                    case RoleEnum.Emperor:
                        instance.RoleText.text = Emperor.rolename;
                        instance.RoleText.color = Emperor.color;
                        instance.RoleBlurbText.text = Emperor.roledescription;
                        instance.RoleBlurbText.color = Emperor.color;
                        break;
                    case RoleEnum.Jester:
                        instance.RoleText.text = JesterPatch.rolename;
                        instance.RoleText.color = JesterPatch.color;
                        instance.RoleBlurbText.text = JesterPatch.roledescription;
                        instance.RoleBlurbText.color = JesterPatch.color;
                        break;
                    case RoleEnum.Sheriff:
                        instance.RoleText.text = Sheriff.rolename;
                        instance.RoleText.color = Sheriff.color;
                        instance.RoleBlurbText.text = Sheriff.roledescription;
                        instance.RoleBlurbText.color = Sheriff.color;
                        break;

                }
            }
        }


    }
}
