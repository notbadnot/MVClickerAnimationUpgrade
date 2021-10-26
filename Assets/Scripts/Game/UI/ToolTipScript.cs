using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScript : MonoBehaviour
{
    private Text tipText;
    private RectTransform backGroundTransform;
    private Component rectTransformCoponent;
    private Camera mainCam;
    public static ToolTipScript selfStatic;
    private void Awake()
    {
        selfStatic = this;
        backGroundTransform = transform.Find("ToolTipPanel").GetComponent<RectTransform>();
        tipText = transform.Find("ToolTipPanel").Find("ToolTipText").GetComponent<Text>();
        mainCam = Camera.main;
    }
    private void ShowToolTip(string tipString)
    {
        gameObject.SetActive(true);
        tipText.text = tipString;
        float padding = 4f;
        Vector2 backGround = new Vector2(tipText.preferredWidth + padding, tipText.preferredHeight + padding);
        backGroundTransform.sizeDelta = backGround;
    }
    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowToolTip("Hi i am a tooltip");
        selfStatic.HideToolTip();
    }
    private void Update()
    {
        //Vector3 worldPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //transform.localPosition = new Vector3(worldPoint.x, worldPoint.y, 0);
        //transform.Find("ToolTipPanel").position = new Vector2(worldPoint.x + 1, worldPoint.y);
        //Debug.Log("I am moving");
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), new Vector2(Input.mousePosition.x,Input.mousePosition.y), mainCam, out pos);
        transform.localPosition = pos;


    }
    public static void HideToolTipStatic()
    {
        selfStatic.HideToolTip();
    }
    public static void ShowToolTipStatic(string tipString)
    {
        selfStatic.ShowToolTip(tipString);
    }



}
