using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Zenject;

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

    [Inject]
    private void Construct(InGameUIScript _inGameUIScript)
    {
        inGameUI = _inGameUIScript;
    }

    // Start is called before the first frame update
    void Start()
    {
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
