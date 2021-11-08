using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] GameObject hearthImagePrefab;
    [SerializeField] GameObject heartPlacer;
    private Text scoreLabel;
    private Text timeLabel;
    private Text healthLabel;
    private InGameUIScript inGameUI;
    private GameObject textLabelPlace;
    private Text textLable;
    private RectTransform hearthPlace;



    // Start is called before the first frame update
    void Start()
    {
        inGameUI = FindObjectOfType<InGameUIScript>();
        /*textLabelPlace = inGameUI.gameObject.transform.Find("Canvas").Find("TextPanel").gameObject;
        textLable = textLabelPlace.transform.Find("TextForTextPanel").GetComponent<Text>();
        scoreLabel = inGameUI.gameObject.transform.Find("Canvas").Find("InfoBar").Find("ScorePanel").Find("ScoreText").GetComponent<Text>();
        healthLabel = inGameUI.gameObject.transform.Find("Canvas").Find("InfoBar").Find("HealthPanel").Find("HealthTextPlace").Find("HealthText").GetComponent<Text>();
        timeLabel = inGameUI.gameObject.transform.Find("Canvas").Find("InfoBar").Find("TimePanel").Find("TimeText").GetComponent<Text>();
        hearthPlace = inGameUI.gameObject.transform.Find("Canvas").Find("InfoBar").Find("HealthPanel").Find("HeartPlace").GetComponent<RectTransform>();*/
        textLabelPlace = inGameUI.textLabelPlace;
        textLable = inGameUI.textLable;
        scoreLabel = inGameUI.scoreLabel;
        healthLabel = inGameUI.healthLabel;
        timeLabel = inGameUI.timeLabel;
        hearthPlace = inGameUI.hearthPlace;
        textLabelPlace = inGameUI.textLabelPlace;
        textLabelPlace.gameObject.SetActive(false);

    }
    private void OnEnable()
    {

    }

    public void CountScore(int score = 0)
    {
        scoreLabel.text = "Score: " + score.ToString();
    }
    public void CountHealth(int health = 0)
    {
        if (hearthPlace.childCount > 0)
        {
            Destroy(hearthPlace.GetChild(hearthPlace.childCount - 1).gameObject);
        }
        var placer =Instantiate(heartPlacer, hearthPlace.transform);
        for (int i = 0; i < Mathf.Min(health,5); i++)
        {
            Instantiate(hearthImagePrefab, placer.transform);
        }
        if (health > 5)
        {
            healthLabel.text = " + " + (health - 5);

        }
        else
        {
            healthLabel.text = "";
        }
    }
    public void CountTime(string time)
    {
        timeLabel.text = "Time: " + time;
    }
    public void ShowTextLabel(string labelMessage)
    {
        textLabelPlace.SetActive(true);
        textLable.text = labelMessage;
    }
    public void HideTextLabel()
    {
        textLabelPlace.SetActive(false);
    }
    public void SwitchInGameMenu(bool switcher)
    {
        inGameUI.gameObject.SetActive(switcher);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
