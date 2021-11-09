using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameCore : MonoBehaviour
{
    private GameMaster gameMaster;
    private OutGameUIManager outGameUI;
    private GameModel gameModel;
    private GameMoleMaster gameMoleMaster;
    // Start is called before the first frame update
    [Inject]
    private void Construct(GameMaster _gameMaster, OutGameUIManager _outGameUI, GameMoleMaster _gameMoleMaster)
    {
        gameMaster = _gameMaster;
        outGameUI = _outGameUI;
        gameMoleMaster = _gameMoleMaster;
    }
    void Start()
    {
        /*gameMaster = FindObjectOfType<GameMaster>();
        gameMoleMaster = FindObjectOfType<GameMoleMaster>();*/
        gameModel = new GameModel();
        //outGameUI = FindObjectOfType<OutGameUIManager>();
        outGameUI.gameModel = gameModel;
        gameMaster.gameOvered += GameMaster_gameOvered;
        outGameUI.GameStarted += OutGameUI_GameStarted;
        gameMoleMaster.gameOvered += GameMoleMaster_gameOvered;
        gameMaster.TelUImanagerToSwitchInGameMenu(false);
    }

    private void GameMoleMaster_gameOvered()
    {
        outGameUI.SwitchMenu(true);
        gameMaster.TelUImanagerToSwitchInGameMenu(false);
        outGameUI.ShowStartMenu();
    }

    private void OutGameUI_GameStarted()
    {
        if (gameModel.gameMode == GameModel.GameMode.AlienGame)
        {
            gameMaster.StartGame(gameModel.enableCrowding, gameModel.difficulty);
            gameMaster.TelUImanagerToSwitchInGameMenu(true);

        }else if (gameModel.gameMode == GameModel.GameMode.MoleMiniGame)
        {
            gameMoleMaster.StartGame();
            gameMoleMaster.TelUImanagerToSwitchInGameMenu(true);
        }
        outGameUI.SwitchMenu(false);
    }

    private void GameMaster_gameOvered(int score, int time)
    {
        outGameUI.SwitchMenu(true);
        gameMaster.TelUImanagerToSwitchInGameMenu(false);
        outGameUI.ShowGameOverMenu(score, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
