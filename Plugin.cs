
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
using UnityEngine;
using System.Linq;
using AmongUs.GameOptions;

namespace SuperOldRoles;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public Harmony harmony;
    public override void Load()
    {
        Log.LogInfo("超古い役職たちが読み込まれました！");
        harmony = new Harmony("com.github.Ysandesuyo.SuperOldRoles");
        harmony.PatchAll();

    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    static class SendChatPatch
    {
        static bool Prefix(ChatController __instance)
        {
            bool okkana = true;
            string text = __instance.freeChatField.textArea.text;
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Joined)
            {
                if (text.Contains("しね") || text.Contains("ころすぞ") || text.Contains("ばか") || text.Contains("バカ") || text.Contains("アホ") || text.Contains("あほ") || text.Contains("死ね")||text.Contains("4ね") || text.Contains("殺すぞ"))
                    {
                    __instance.AddChat(PlayerControl.LocalPlayer, "暴言吐くなやカスがよ");
                    __instance.freeChatField.textArea.text = "[規制済み]";
                    __instance.SendFreeChat();
                    __instance.freeChatField.Clear();
                    okkana = false;
                    
                }
            }
            return okkana;

        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcCompleteTask))]
    public static class killkillpatch
    { 
        static void Postfix(PlayerControl __instance)
        {
            PlayerControl.LocalPlayer.MurderPlayer(__instance,MurderResultFlags.Succeeded);
        }
    }

    

}