﻿using UnityEngine;
using System.Collections;
using System.IO;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UICreator : MonoBehaviour
{

    private const int LayerUI = 5;

    public string item_name; 

    public bool UIExist = false;
    public float alpha = 0.5f;
    public Color UIColor = Color.gray;


    GameObject canvasObject;
    GameObject panel;

    bool CheckDone;

    void Start()
    {
        item_name = " ";
    }

    void Update()
    {
        if(UIExist == true)
        {
            CreateUI();
            UIExist = false;
        }
    }

    public void CreateUI()
    {
        /*
        canvasObject = new GameObject("Canvas");
        GameObject canvas = CreateCanvas(this.transform);
        CreateEventSystem(canvas.transform);
        */
        panel = CreatePanel(this.transform);
        CreateText(panel.transform, 0, 0, 160, 50, "Do you want to get the Item - " + item_name + " - ?", 24);

        CreateButton(panel.transform, 0, -100, 200, 50, "Press X to get it", delegate{OnUse();});
//        CreateButton(panel.transform, 100, -100, 160, 50, "No", delegate{OnCancel();});
    }


    private GameObject CreateCanvas(Transform parent)
    {
        // create the canvas

        canvasObject.layer = LayerUI;

        RectTransform canvasTrans = canvasObject.AddComponent<RectTransform>();

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;

        CanvasScaler canvasScal = canvasObject.AddComponent<CanvasScaler>();
        canvasScal.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScal.referenceResolution = new Vector2(800, 600);

        GraphicRaycaster canvasRayc = canvasObject.AddComponent<GraphicRaycaster>();

        canvasObject.transform.SetParent(parent);

        return canvasObject;
    }


    private GameObject CreateEventSystem(Transform parent)
    {
        GameObject esObject = new GameObject("EventSystem");

        EventSystem esClass = esObject.AddComponent<EventSystem>();
        esClass.sendNavigationEvents = true;
        esClass.pixelDragThreshold = 5;

        StandaloneInputModule stdInput = esObject.AddComponent<StandaloneInputModule>();
        stdInput.horizontalAxis = "Horizontal";
        stdInput.verticalAxis = "Vertical";

        TouchInputModule touchInput = esObject.AddComponent<TouchInputModule>();

        esObject.transform.SetParent(parent);

        return esObject;
    }

    private GameObject CreatePanel(Transform parent)
    {
        GameObject panelObject = new GameObject("Panel");
        panelObject.transform.SetParent(parent);

        panelObject.layer = LayerUI;

        RectTransform trans = panelObject.AddComponent<RectTransform>();
        trans.anchorMin = new Vector2(0, 0);
        trans.anchorMax = new Vector2(1, 1);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(0, 0);
        trans.offsetMin = new Vector2(0, 0);
        trans.offsetMax = new Vector2(0, 0);
        trans.localPosition = new Vector3(0, 0, 0);
        trans.sizeDelta = new Vector2(0, 0);
        trans.localScale = new Vector3(0.8f, 0.8f, 1.0f);
        trans.localRotation = Quaternion.Euler(0, 0, 0);

        CanvasRenderer renderer = panelObject.AddComponent<CanvasRenderer>();

        Image image = panelObject.AddComponent<Image>();

        //UI color
        Color c = Color.gray;
        c.a = alpha;
        image.color = c;

        Texture2D tex = Resources.Load<Texture2D>("panel_bkg");
        //        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        return panelObject;
    }

    private GameObject CreateText(Transform parent, float x, float y, float w, float h, string message, int fontSize)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent);

        textObject.layer = LayerUI;

        RectTransform trans = textObject.AddComponent<RectTransform>();
        trans.sizeDelta.Set(w, h);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);
        trans.localRotation = Quaternion.Euler(0, 0, 0);

        CanvasRenderer renderer = textObject.AddComponent<CanvasRenderer>();

        Text text = textObject.AddComponent<Text>();
        text.supportRichText = true;
        text.text = message;
        text.fontSize = fontSize;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(1, 1, 1);

        return textObject;
    }

    private GameObject CreateButton(Transform parent, float x, float y, float w, float h, string message, UnityAction eventListner)
    {
        GameObject buttonObject = new GameObject("Button");
        buttonObject.transform.SetParent(parent);

        buttonObject.layer = LayerUI;

        RectTransform trans = buttonObject.AddComponent<RectTransform>();
        SetSize(trans, new Vector2(w, h));
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);
        trans.localRotation = Quaternion.Euler(0, 0, 0);

        CanvasRenderer renderer = buttonObject.AddComponent<CanvasRenderer>();

        Image image = buttonObject.AddComponent<Image>();
        Color c = Color.gray;
        c.a = alpha + 0.1f;
        image.color = c;

        Texture2D tex = Resources.Load<Texture2D>("button_bkg");
        //        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),new Vector2(0.5f, 0.5f));

        Button button = buttonObject.AddComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(eventListner);

        GameObject textObject = CreateText(buttonObject.transform, 0, 0, 0, 0, message, 24);

        return buttonObject;
    }

    private static void SetSize(RectTransform trans, Vector2 size)
    {
        Vector2 currSize = trans.rect.size;
        Vector2 sizeDiff = size - currSize;
        trans.offsetMin = trans.offsetMin - new Vector2(sizeDiff.x * trans.pivot.x, sizeDiff.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(sizeDiff.x * (1.0f - trans.pivot.x), sizeDiff.y * (1.0f - trans.pivot.y));
    }

    public void OnUse()
    {
        //Do something 
        Application.Quit();
    }

    public void OnCancel()
    {
        UIExist = false;
        Destroy(panel);
    }

}
