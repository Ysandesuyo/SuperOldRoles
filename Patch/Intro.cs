
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

namespace SuperOldRoles.Patch
{
    [HarmonyPatch]
    class Intro
    {
        

        //なんかまだ動かん
        public static void setupIntroTeam(ref IntroCutscene __instance, ref List<PlayerControl> yourTeam)
        {
            foreach (PlayerRolePair dare in WariFuri.rolelist)
            {
                if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && ((byte)dare.Role) >= 50 && ((byte)dare.Role) < 100)
                {
                    Color col = new Color();
                    col.r = 0.5f;
                    col.g = 0.5f;
                    col.b = 0.5f;
                    col.a = 1f;
                    __instance.BackgroundBar.material.color = col;
                    __instance.TeamTitle.text = "第三陣営";
                    __instance.TeamTitle.color = col;
                    __instance.TeamTitle.gameObject.SetActive(true);
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene),nameof(IntroCutscene.ShowRole))]
        public static class showrolepatch
        {
            static void setRoleTexts(ref IntroCutscene __instance)
            {
                RoleEnum role = RoleEnum.Crewmate;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        role = dare.Role;
                    }
                }
                switch (role)
                {
                    case RoleEnum.Bait:
                        __instance.RoleText.text = Bait.rolename;
                        __instance.RoleText.color = Bait.color;
                        __instance.RoleBlurbText.text = Bait.roledescription;
                        __instance.RoleBlurbText.color = Bait.color;
                        break;
                    case RoleEnum.president:
                        __instance.RoleText.text = PresidentPatch.rolename;
                        __instance.RoleText.color = PresidentPatch.color;
                        __instance.RoleBlurbText.text = PresidentPatch.roledescription;
                        __instance.RoleBlurbText.color = PresidentPatch.color;
                        break;
                    case RoleEnum.Emperor:
                        __instance.RoleText.text = Emperor.rolename;
                        __instance.RoleText.color = Emperor.color;
                        __instance.RoleBlurbText.text = Emperor.roledescription;
                        __instance.RoleBlurbText.color = Emperor.color;
                        break;
                    case RoleEnum.Jester:
                        __instance.RoleText.text = JesterPatch.rolename;
                        __instance.RoleText.color = JesterPatch.color;
                        __instance.RoleBlurbText.text = JesterPatch.roledescription;
                        __instance.RoleBlurbText.color = JesterPatch.color;
                        break;
                    case RoleEnum.Sheriff:
                        __instance.RoleText.text = Sheriff.rolename;
                        __instance.RoleText.color = Sheriff.color;
                        __instance.RoleBlurbText.text = Sheriff.roledescription;
                        __instance.RoleBlurbText.color = Sheriff.color;
                        break;

                }
                __instance.RoleText.gameObject.SetActive(true);
                __instance.RoleBlurbText.gameObject.SetActive(true);
            }

            public static void Prefix(IntroCutscene __instance)
            {
                setRoleTexts(ref __instance);
            }
        }
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            public static void Prefix(IntroCutscene __instance, [HarmonyArgument(0)] ref List<PlayerControl> teamToDisplay)
            {
                /*こうしたいけどむり
                while (!WariFuri.shouldrun)
                {

                }*/
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId && ((byte)dare.Role) >= 50 && ((byte)dare.Role) < 100)
                    {
                        List<PlayerControl> soloTeam = new List<PlayerControl>();
                        soloTeam.Add(PlayerControl.LocalPlayer);
                        teamToDisplay = soloTeam;
                    }
                }
            }

            public static void Postfix(IntroCutscene __instance, [HarmonyArgument(0)] ref List<PlayerControl> teamToDisplay)
            {
                setupIntroTeam(ref __instance, ref teamToDisplay);
            }
        }
    }
}
