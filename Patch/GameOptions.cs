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

namespace SuperOldRoles.Patch
{
    class GameOption
    {
        static MeetingHud motopanel = null;
        static PlayerVoteArea basepanel = null;
        static PlayerVoteArea header = null;

        static bool isshow = false;
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class startgame 
        {
            public static void Postfix()
            {
                GameObject.Destroy(basepanel);
                isshow = false;
            }
        }

        [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
        public static class Hwoositara
        {

            public static void Postfix(ControllerManager __instance)
            {


                


                if (Input.GetKeyDown(KeyCode.H) && AmongUsClient.Instance.AmHost){

                        
                        


                    if (isshow)
                    {
                        isshow = false;
                        GameObject.Destroy(motopanel.gameObject);
                        GameObject.Destroy(header.gameObject);
                        GameObject.Destroy(basepanel.gameObject);
                        HudManager.Instance.transform.FindChild("LobbyInfoPane").FindChild("AspectSize").gameObject.SetActive(true);
                        HudManager.Instance.transform.FindChild("GameStartManager").gameObject.SetActive(true);
                    }
                    else
                    {
                        
                        HudManager.Instance.transform.FindChild("LobbyInfoPane").FindChild("AspectSize").gameObject.SetActive(false);
                        HudManager.Instance.transform.FindChild("GameStartManager").gameObject.SetActive(false);

                        motopanel = GameObject.Instantiate(HudManager.Instance.MeetingPrefab);
                        motopanel.name = "motopanel";
                        motopanel.transform.SetParent(HudManager.Instance.transform, false);
                        motopanel.BlackBackground.gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("OverlayParent").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("baseGlass").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("Background").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("UI_Phone_Button").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("UI_Icon_Battery").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("UI_Icon_Wifi").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("ButtonStuff").FindChild("TimerText_TMP").gameObject.SetActive(false);
                        motopanel.meetingContents.FindChild("ButtonStuff").gameObject.SetActive(false);
                        motopanel.gameObject.layer = 5;
                        motopanel.transform.SetLocalZ(-200f);
                        motopanel.meetingContents.FindChild("PhoneUI").FindChild("baseColor").gameObject.transform.SetLocalZ(-200f);
                        
                        
                        VerticalLayoutGroup layout = motopanel.gameObject.AddComponent<VerticalLayoutGroup>();
                        RectOffset offset = new RectOffset();
                        offset.left = 0;
                        offset.top = 0;
                        offset.right = 0;
                        offset.bottom = 0;
                        layout.padding = offset;
                        layout.spacing = 0.25f;
                        layout.childAlignment = TextAnchor.UpperLeft;
                        layout.childForceExpandHeight = false;
                        layout.childForceExpandWidth = false;
                        layout.childControlHeight = false;
                        layout.childControlWidth = false;


                        RectTransform mrt = motopanel.gameObject.GetComponent<RectTransform>();
                        mrt.sizeDelta = new Vector2(8.5f, 4.75f);



                        header = GameObject.Instantiate(HudManager.Instance.MeetingPrefab.PlayerButtonPrefab);
                        header.name = "header";
                        Vector2 headersize = new Vector2(8.5f, 1f);
                        header.transform.SetParent(motopanel.transform, false);
                        header.MaskArea.transform.localScale = headersize;
                        header.transform.FindChild("PlayerLevel").gameObject.SetActive(false);
                        header.MaskArea.color = new Color(0.1f, 0.1f, 0.1f, 1f);
                        header.gameObject.AddComponent<LayoutElement>();
                        RectTransform hrt = header.gameObject.GetComponent<RectTransform>();
                        hrt.sizeDelta = headersize;
                        hrt.anchoredPosition = new Vector2(0f, 0f);
                        header.transform.SetLocalZ(-201f);
                        

                        basepanel = GameObject.Instantiate(HudManager.Instance.MeetingPrefab.PlayerButtonPrefab);
                        basepanel.name = "basepanel";
                        basepanel.transform.SetParent(motopanel.transform, false);
                        Vector2 basesize = new Vector2(8.5f, 3.5f);
                        basepanel.MaskArea.transform.localScale = basesize;
                        basepanel.MaskArea.color = new Color(0.05f, 0.05f, 0.05f, 1f);
                        basepanel.gameObject.AddComponent<LayoutElement>();
                        RectTransform brt = basepanel.gameObject.GetComponent<RectTransform>();
                        brt.sizeDelta = basesize;
                        brt.anchoredPosition = new Vector2(0f, 0f);
                        basepanel.transform.SetLocalZ(-201f);
                        
                        BoxCollider2D col = basepanel.gameObject.AddComponent<BoxCollider2D>();
                        col.size = basesize;
                        

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
                        if (clickedGameObject.name == "basepanel")
                        {

                            GameObject.Destroy(clickedGameObject);//ゲームオブジェクトを破壊
                        }
                        Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                    }

                }
            }
        }

    }
}
