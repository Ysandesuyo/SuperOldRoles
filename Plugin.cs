
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using InnerNet;
using SuperOldRoles.Patch;
using TMPro;
using UnityEngine;
using static Il2CppMono.Security.X509.X520;
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

    [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Awake))]
    static class ModManagerAwakePatch
    {
        static void Postfix(MainMenuManager __instance)
        {
            //ModManagerのインスタンスを取り出して、ModStampを表示する。
            ModManager.Instance.ShowModStamp();
            GameObject basyo = __instance.mainMenuUI.transform.Find("AspectScaler").Find("RightPanel").gameObject;
            GameObject titletext = new GameObject("SORtext1");
            titletext.transform.SetParent(basyo.transform, false);
            titletext.layer = 5;
            titletext.transform.localPosition = new Vector3(-3f, 2f, -2f);
            titletext.transform.localScale = new Vector3(2.25f, 2f, 1f);

            TextMeshPro testText1 = titletext.AddComponent<TextMeshPro>();
            testText1.fontSize = 2; // フォントサイズ 48px
            testText1.color = new Color(1f, 0f, 0f);
            testText1.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText1.enableWordWrapping = false; // 自動改行無し
            testText1.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText1.text = "Super";

            GameObject titletext2 = new GameObject("SORtext2");
            titletext2.transform.SetParent(basyo.transform, false);
            titletext2.layer = 5;
            titletext2.transform.localPosition = new Vector3(-1f, 2f, -2f);
            titletext2.transform.localScale = new Vector3(2.25f, 2f, 1f);

            TextMeshPro testText2 = titletext2.AddComponent<TextMeshPro>();
            testText2.fontSize = 2; // フォントサイズ 48px
            testText2.color = new Color(0f, 0f, 1f);
            testText2.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText2.enableWordWrapping = false; // 自動改行無し
            testText2.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText2.text = "Old";

            GameObject titletext3 = new GameObject("SORtext3");
            titletext3.transform.SetParent(basyo.transform, false);
            titletext3.layer = 5;
            titletext3.transform.localPosition = new Vector3(1f, 2f, -2f);
            titletext3.transform.localScale = new Vector3(2.25f, 2f, 1f);

            TextMeshPro testText3 = titletext3.AddComponent<TextMeshPro>();
            testText3.fontSize = 2; // フォントサイズ 48px
            testText3.color = new Color(0f, 1f, 0f);
            testText3.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText3.enableWordWrapping = false; // 自動改行無し
            testText3.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText3.text = "Roles";

            GameObject back = GameOption.CreatePrimitive2D("SORtextback", new Vector2(6f, 0.75f), new Color(1f, 1f, 1f));
            back.transform.SetParent(basyo.transform, false);
            back.transform.localPosition = new Vector3(-1f, 2f, -1f);
        }
    }

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
                    __instance.AddChat(PlayerControl.LocalPlayer, "暴言吐かないでください");
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
     * これを使い、座標を得る


    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
    public static class aabcde
    {
        public static void Postfix(PlayerControl __instance)
        {
            Vector2 pos = __instance.GetTruePosition();
            __instance.SetName(pos.x + "aaa" + pos.y + "aaa");
        }
    }



    */


    
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