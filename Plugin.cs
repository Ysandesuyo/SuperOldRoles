
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
//情報を読み込む


namespace SuperOldRoles;

//土台
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MyMyPlugin : BasePlugin
{
    
    public static MyMyPlugin Instance;
    //Harmonyとは、アモアスで発生した出来事を改造するやつ
    public Harmony harmony;
    
    //ロードされたときにやる
    public override void Load()
    {
        //ログにちゃんと正常起動したかのメモする
        Log.LogInfo("超古い役職たちが読み込まれました！");
        
        //前述のHarmonyを取り出す。これを、「インスタンスを作る」という。
        harmony = new Harmony("com.github.Ysandesuyo.SuperOldRoles");
        //ようわからんけどやっとこ
        harmony.PatchAll();

        Instance = this;



    }
    /*
    [HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.Start))]
    public static class ListenPortPatch
    {
        public static void Prefix(InnerNet.InnerNetClient __instance)
        {
            // 22000〜23000 の間でランダムポートに
            ushort randomPort = (ushort)UnityEngine.Random.Range(22000, 23000);
            __instance.networkPort = randomPort;

            MyMyPlugin.Instance.Log.LogInfo($"[Mod] ListenPort set to: {randomPort}");
        }
    }
    */


    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    static class SendChatPatch
    {
        static bool Prefix(ChatController __instance)
        {
            ModManager.Instance.ShowModStamp();
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
            //ここでtrueを返すと、もともと送られてたメッセージをそのままにする。falseだと、なかったことにする。
            //多分だけど、なかったことにするのはPrefixの時だけだと思う。Prefixは発生する前に実行されるからで、Postfixは発生した後に実行されるから。
            return okkana;

        }
    }
    /*
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcCompleteTask))]
    //[改造するよ（typeof（改造する大きな種類）,nameof(そのなかで何が起こったら改造する？）]
    public static class killkillpatch//これは適当な名前
    { 
        static void Postfix(PlayerControl __instance)
            //static void Postfix(プレイヤーの型　この文字で呼び出す)
        {
            PlayerControl.LocalPlayer.MurderPlayer(__instance,MurderResultFlags.Succeeded);
            // タスクを終わらせたプレイヤー　.　プレイヤーに対して　.　殺す　（）の中は殺し方のカスタム　(タスクを終わらせたプレイヤー, MurderResultFlags.Succeeded);
        }
    }*/

    

}





//インポを追放して勝ったらどうなる？