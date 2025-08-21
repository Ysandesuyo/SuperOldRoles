using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using SuperOldRoles.Roles.all;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SuperOldRoles.Patch
{
    class GameOption
    {
        static GameObject motopanel = null;
        static GameObject header = null;
        static GameObject modpanel = null;
        static GameObject crewpanel = null;
        static GameObject impopanel = null;
        static GameObject neutpanel = null;


        static bool isshow = false;
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class startgame 
        {
            public static void Postfix()
            {
                Hclose();
            }
        }

        private static void Hclose()
        {
            isshow = false;
            GameObject.Destroy(motopanel.gameObject);
            //GameObject.Destroy(header.gameObject);
            //GameObject.Destroy(basepanel.gameObject);
            HudManager.Instance.transform.FindChild("LobbyInfoPane").FindChild("AspectSize").gameObject.SetActive(true);
            HudManager.Instance.transform.FindChild("GameStartManager").gameObject.SetActive(true);
        }

        private static GameObject CreatePrimitive2D(string name, Vector2 size, Color color)
        {
            // 空のオブジェクト作成
            GameObject obj = new GameObject(name);
            obj.layer = 5;

            // SpriteRenderer追加
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

            // 白いテクスチャを作って色を付ける
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();

            sr.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

            // サイズ設定
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);

            // 必要ならコライダー追加
            
            obj.AddComponent<BoxCollider2D>();
            

            return obj;
        }

        private static GameObject CreateNumberSetting(string name,string text, GameObject parent)
        {
            GameObject oomoto = new GameObject(name);
            oomoto.layer = 5;
            oomoto.transform.SetLocalZ(-1f);
            oomoto.transform.localScale=new Vector3(0.1f, 0.075f, 1f);
            oomoto.AddComponent<LayoutElement>();
            oomoto.transform.SetParent(parent.transform, false);
            
            GameObject title = CreatePrimitive2D(name + "title", new Vector2(1f, 0.9f), Color.white);
            title.transform.SetParent(oomoto.transform, false);
            title.transform.SetLocalX(-2f);
            title.transform.SetLocalZ(-1f);

            GameObject titletext = new GameObject(name+"titletext");
            titletext.transform.SetParent(title.transform, false);
            titletext.layer = 5;
            titletext.transform.SetLocalZ(-1f);
            titletext.transform.localScale = new Vector3(1.25f, 5f, 1f);

            TextMeshPro testText1 = titletext.AddComponent<TextMeshPro>();
            testText1.fontSize = 1; // フォントサイズ 48px
            testText1.color = new Color(0f, 0f, 0f);
            testText1.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText1.enableWordWrapping = false; // 自動改行無し
            testText1.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText1.text = text;

            GameObject current = CreatePrimitive2D(name + "current", new Vector2(1f, 0.9f), Color.white);
            current.transform.SetParent(oomoto.transform, false);
            current.transform.SetLocalX(0.1f);
            current.transform.SetLocalZ(-1f);
            GameObject currenttext = new GameObject(name + "currenttext");
            currenttext.transform.SetParent(current.transform, false);
            currenttext.layer = 5;
            currenttext.transform.SetLocalZ(-1f);
            currenttext.transform.localScale = new Vector3(1.25f, 5f, 1f);

            TextMeshPro testText2 = currenttext.AddComponent<TextMeshPro>();
            testText2.fontSize = 1; // フォントサイズ 48px
            testText2.color = new Color(0f, 0f, 0f);
            testText2.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText2.enableWordWrapping = false; // 自動改行無し
            testText2.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText2.text = "NaNDayo";

            GameObject minus = CreatePrimitive2D(name + "minus", new Vector2(1f, 0.9f), Color.white);
            minus.transform.SetParent(oomoto.transform, false);
            minus.transform.SetLocalX(1.1f);
            minus.transform.SetLocalZ(-1f);
            GameObject minustext = new GameObject(name + "minustext");
            minustext.transform.SetParent(minus.transform, false);
            minustext.layer = 5;
            minustext.transform.SetLocalZ(-1f);
            minustext.transform.localScale = new Vector3(1.25f, 5f, 1f);

            TextMeshPro testText3 = minustext.AddComponent<TextMeshPro>();
            testText3.fontSize = 1; // フォントサイズ 48px
            testText3.color = new Color(0f, 0f, 0f);
            testText3.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText3.enableWordWrapping = false; // 自動改行無し
            testText3.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText3.text = "-";

            GameObject plus = CreatePrimitive2D(name + "plus", new Vector2(1f, 0.9f), Color.white);
            plus.transform.SetParent(oomoto.transform, false);
            plus.transform.SetLocalX(-0.9f);
            plus.transform.SetLocalZ(-1f);
            GameObject plustext = new GameObject(name + "plustext");
            plustext.transform.SetParent(plus.transform, false);
            plustext.layer = 5;
            plustext.transform.SetLocalZ(-1f);
            plustext.transform.localScale = new Vector3(1.25f, 5f, 1f);

            TextMeshPro testText4 = plustext.AddComponent<TextMeshPro>();
            testText4.fontSize = 1; // フォントサイズ 48px
            testText4.color = new Color(0f, 0f, 0f);
            testText4.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText4.enableWordWrapping = false; // 自動改行無し
            testText4.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText4.text = "+";
            return oomoto;
        }
        private static GameObject CreateHeaderEle(string name,string text)
        {
            Vector2 headermodsize = new Vector2(0.25f, 0.75f);
            GameObject headermod = CreatePrimitive2D(name, headermodsize, new Color(0.8f, 0.8f, 0.8f));
            headermod.layer = 5;
            headermod.transform.SetParent(header.transform, false);
            BoxCollider2D col = headermod.AddComponent<BoxCollider2D>();
            headermod.transform.SetLocalZ(-1f);
            headermod.transform.localScale = headermodsize;
            headermod.AddComponent<LayoutElement>();

            GameObject headermodtext = new GameObject("headermodtext");
            headermodtext.transform.SetParent(headermod.transform, false);
            headermodtext.layer = 5;
            headermodtext.transform.SetLocalZ(-1f);
            headermodtext.transform.localScale = new Vector3(1.25f, 5f, 1f);
            TextMeshPro testText = headermodtext.AddComponent<TextMeshPro>();
            testText.fontSize = 1; // フォントサイズ 48px
            testText.color = new Color(0.2f, 0.2f, 1f);
            testText.alignment = TextAlignmentOptions.Center; // 中央揃え
            testText.enableWordWrapping = false; // 自動改行無し
            testText.sortingOrder = 0; // 画像 (SpriteRenderer) と同様、描画順序指定が必要
            testText.text = text;
            return headermod;
        }

        private static void paneloff()
        {
            modpanel.SetActive(false);
            modpanel.transform.SetLocalZ(0f);
            crewpanel.SetActive(false);
            crewpanel.transform.SetLocalZ(0f);
            impopanel.SetActive(false);
            impopanel.transform.SetLocalZ(0f);
            neutpanel.SetActive(false);
            neutpanel.transform.SetLocalZ(0f);
        }

        private static void modon()
        {
            modpanel.SetActive(true);
            modpanel.transform.SetLocalZ(-1f);
        }

        private static void crewon()
        {
            crewpanel.SetActive(true);
            crewpanel.transform.SetLocalZ(-1f);
            Action<GameObject> action = (GameObject c) =>
            {
                string name = c.name;
                switch (name)
                {
                    case "bait":
                        c.transform.Find(name + "current").Find(name + "currenttext").GetComponent<TextMeshPro>().text = WariFuri.BaitKazu.ToString();
                        break;
                    case "sheriff":
                        c.transform.Find(name + "current").Find(name + "currenttext").GetComponent<TextMeshPro>().text = WariFuri.SheriffKazu.ToString();
                        break;
                    case "president":
                        c.transform.Find(name + "current").Find(name + "currenttext").GetComponent<TextMeshPro>().text = WariFuri.PresidentKazu.ToString();
                        break;
                    default:
                        break;
                }
            };
            crewpanel.ForEachChild(action);
        }
        private static void impoon()
        {
            impopanel.SetActive(true);
            impopanel.transform.SetLocalZ(-1f);
        }
        private static void neuton()
        {
            neutpanel.SetActive(true);
            neutpanel.transform.SetLocalZ(-1f);
            Action<GameObject> action = (GameObject c) =>
            {
                string name = c.name;
                switch (name)
                {
                    case "jester":
                        c.transform.Find(name + "current").Find(name + "currenttext").GetComponent<TextMeshPro>().text = WariFuri.JesterKazu.ToString();
                        break;
                    case "emperor":
                        c.transform.Find(name + "current").Find(name + "currenttext").GetComponent<TextMeshPro>().text = WariFuri.EmperorKazu.ToString();
                        break;
                    default:
                        break;
                }
            };
            neutpanel.ForEachChild(action);
        }


        [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
        public static class Hwoositara
        {

            public static void Postfix(ControllerManager __instance)
            {


                


                if (Input.GetKeyDown(KeyCode.H) && AmongUsClient.Instance.AmHost){

                        
                        


                    if (isshow)
                    {
                        Hclose();
                    }
                    else
                    {
                        HudManager.Instance.transform.FindChild("LobbyInfoPane").FindChild("AspectSize").gameObject.SetActive(false);
                        HudManager.Instance.transform.FindChild("GameStartManager").gameObject.SetActive(false);


                        
                        Vector2 motosize = new Vector2(9f, 5f);


                        motopanel = CreatePrimitive2D("motopanel", motosize, new Color(0.1f, 0.1f, 0.1f));


                        motopanel.transform.SetParent(HudManager.Instance.transform, false);
                        motopanel.gameObject.layer = 5;
                        motopanel.transform.SetLocalZ(-500f);
                        

                        


                        Vector2 headersize = new Vector2(0.95f, 0.15f);
                        header = CreatePrimitive2D("header", headersize, new Color(0.9f, 0.9f, 0.9f));
                        header.transform.SetParent(motopanel.transform, false);
                        header.gameObject.layer = 5;
                        header.transform.SetLocalZ(-1f);
                        header.transform.SetLocalY(0.4f);
                        header.AddComponent<HorizontalLayoutGroup>();

                        CreateHeaderEle("headermod", "MODの設定");
                        CreateHeaderEle("headercrew", "クルー");
                        CreateHeaderEle("headerimpo", "インポスター");
                        CreateHeaderEle("headerneut", "第三陣営");

                        modpanel = CreatePrimitive2D("modpanel", new Vector2(0.95f, 0.75f), new Color(0.8f, 0.8f, 1f));
                        modpanel.transform.SetParent(motopanel.transform, false);
                        modpanel.transform.SetLocalY(-0.1f);

                        crewpanel = CreatePrimitive2D("crewpanel", new Vector2(0.95f, 0.75f), new Color(0.8f, 1f, 1f));
                        crewpanel.transform.SetParent(motopanel.transform, false);
                        crewpanel.transform.SetLocalY(-0.1f);
                        crewpanel.AddComponent<VerticalLayoutGroup>();
                        CreateNumberSetting("bait", "ベイト", crewpanel);
                        CreateNumberSetting("sheriff", "シェリフ", crewpanel);
                        CreateNumberSetting("president", "大統領", crewpanel);


                        impopanel = CreatePrimitive2D("impopanel", new Vector2(0.95f, 0.75f), new Color(1f, 0.8f, 0.8f));
                        impopanel.transform.SetParent(motopanel.transform, false);
                        impopanel.transform.SetLocalY(-0.1f);

                        neutpanel = CreatePrimitive2D("neutpanel", new Vector2(0.95f, 0.75f), new Color(0.8f, 1f, 0.8f));
                        neutpanel.transform.SetParent(motopanel.transform, false);
                        neutpanel.transform.SetLocalY(-0.1f);
                        neutpanel.AddComponent<VerticalLayoutGroup>();
                        CreateNumberSetting("jester","ハングドマン",neutpanel);
                        CreateNumberSetting("emperor","天皇陛下",neutpanel);

                        paneloff();

                        //クリックが後ろのやつに反応しちゃう

                        isshow = true;
                    }
                        
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (hit2d)
                    {
                        GameObject clickedGameObject = hit2d.transform.gameObject;
                        switch (clickedGameObject.name)
                        {
                            case "headermod":
                                paneloff();
                                modon();
                                break;
                            case "headercrew":
                                paneloff();
                                crewon();
                                break;
                            case "headerimpo":
                                paneloff();
                                impoon();
                                break;
                            case "headerneut":
                                paneloff();
                                neuton();
                                break;
                            case "jesterplus":
                                WariFuri.JesterKazu++;
                                neuton();
                                break;
                            case "jesterminus":
                                if (WariFuri.JesterKazu <= 0)
                                {
                                    break;
                                }
                                WariFuri.JesterKazu--;
                                neuton();
                                break;
                            case "emperorplus":
                                WariFuri.EmperorKazu++;
                                neuton();
                                break;
                            case "emperorminus":
                                if (WariFuri.EmperorKazu <= 0)
                                {
                                    break;
                                }
                                WariFuri.EmperorKazu--;
                                neuton();
                                break;
                            case "baitplus":
                                WariFuri.BaitKazu++;
                                crewon();
                                break;
                            case "baitminus":
                                if (WariFuri.BaitKazu <= 0)
                                {
                                    break;
                                }
                                WariFuri.BaitKazu--;
                                crewon();
                                break;
                            case "sheriffplus":
                                WariFuri.SheriffKazu++;
                                crewon();
                                break;
                            case "sheriffminus":
                                if (WariFuri.SheriffKazu <= 0)
                                {
                                    break;
                                }
                                WariFuri.SheriffKazu--;
                                crewon();
                                break;
                            case "presidentplus":
                                WariFuri.PresidentKazu++;
                                crewon();
                                break;
                            case "presidentminus":
                                if (WariFuri.PresidentKazu <= 0)
                                {
                                    break;
                                }
                                WariFuri.PresidentKazu--;
                                crewon();
                                break;
                        }
                        Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                    }

                }
            }
        }

    }
}
