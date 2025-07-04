using System;
using HarmonyLib;
using SuperOldRoles.Roles.all;
using UnityEngine;
using static SuperOldRoles.Roles.all.roleenum;

namespace SuperOldRoles.Patch
{
    class NamePlateUnderRole
    {/*
        
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class NamePlateUnderRolepatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                if (!WariFuri.zeninclearkana)
                {
                    return;
                }
                if (__instance != PlayerControl.LocalPlayer)
                {
                    return;
                }
                string settext = "none";
                Color setcolor = Color.black;
                foreach (PlayerRolePair dare in WariFuri.rolelist)
                {
                    if (dare.Player.PlayerId == __instance.PlayerId)
                    {
                        switch (dare.Role)
                        {
                            case RoleEnum.Sheriff:
                                settext = "Sheriff";
                                setcolor = Color.yellow;
                                break;
                            case RoleEnum.Bait:
                                settext = "Bait";
                                setcolor = Color.green;
                                break;
                            case RoleEnum.Jester:
                                settext = "Jester";
                                setcolor = Color.magenta;
                                break;
                            case RoleEnum.Emperor:
                                settext = "天皇陛下";
                                setcolor = Color.yellow;
                                break;
                        }
                    }
                }
                GameObject textObj = new GameObject("LocalNameTag");
                textObj.transform.SetParent(__instance.transform);
                textObj.transform.localPosition = new Vector2(0f, 0.25f); // 名前の下に表示

                TextMesh textMesh = textObj.AddComponent<TextMesh>();
                textMesh.text = settext;
                textMesh.fontSize = 100;
                textMesh.characterSize = 0.01f;
                textMesh.color = setcolor;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;

            }
        }*/
    }
}
