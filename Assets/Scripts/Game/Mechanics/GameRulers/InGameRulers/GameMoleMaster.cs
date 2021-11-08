using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class GameMoleMaster : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject molePrefab; // Несколько префабов для инстанцирования пришельцев
    [SerializeField] private KeyCode pauseKey = KeyCode.P; // Настройка для кнопки паузы
    [SerializeField] private float TimeToPlay = 120f;
    [SerializeField] private float chanseMoleToAppear = 0.01f;

    private PrefabFactory _prefabFactory;
    [Inject]
    private void Construct(PrefabFactory prefabFactory)
    {
        _prefabFactory = prefabFactory;
    }

    enum GameState // Энумерация для состояний игры
    {
        Playing,
        Pause,
        GameOver,
        NotinGame
    }

    enum SpeedAffector // Энумерация необходимая для регулирования скорости игры
    {
        Score,
        Pause,
        Gameover
    }

    public event Action gameOvered;

    private GameState gameState = GameState.NotinGame; //Текущее состояние игры (в процессе , пауза, проигрышь)
    private int score = 0; // Очки
    private float gameSpeed = 1; // Скорость игры
    private InGameUIManager inGameUI;
    private float leftTime;
    private bool moleIsHere;
    void Start()
    {
        mainCam = Camera.main;
        inGameUI = gameObject.GetComponent<InGameUIManager>();
    }
    public void StartGame()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1;
        score = 0;
        leftTime = TimeToPlay;
        inGameUI.CountScore(score);
        inGameUI.HideTextLabel();
        inGameUI.CountHealth(0);
        StartCoroutine(Timer());
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), mainCam.transform.forward);
        Debug.Log("hit");
        if (hit.collider != null)
        {
            GameObject hitGameObj = hit.collider.gameObject;
            if (hitGameObj.GetComponent<ShootableObject>() != null)
            {
                ChangeScore(((int)(hitGameObj.GetComponent<ShootableObject>().GetShoted())));
                inGameUI.CountScore(score);
            }



        }
    }

    private void SpawnMole(Vector3? whereSpawn = null)
    {
        GameObject spawnedMole;
        if (whereSpawn != null)
        {
            spawnedMole = _prefabFactory.Spawn(molePrefab, whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3((mainCam.pixelWidth / 4) * Mathf.Round(UnityEngine.Random.Range(1f, 3f)), (mainCam.pixelHeight / 3) * Mathf.Round( UnityEngine.Random.Range(1f, 2f)), 0));
            spawnedMole = _prefabFactory.Spawn(molePrefab, new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);

        }
        spawnedMole.GetComponent<MoleScript>().LeavingEvent += GameMoleMaster_LeavingEvent; ;
        moleIsHere = true;
    }

    private void GameMoleMaster_LeavingEvent(MoleScript obj)
    {
        moleIsHere = false;
        obj.LeavingEvent -= GameMoleMaster_LeavingEvent;
    }


    public void TelUImanagerToSwitchInGameMenu(bool switcher)
    {
        inGameUI.SwitchInGameMenu(switcher);
    }


    private void ChangeScore(int Amount)
    {
        score += Amount;
    }


    private void ChangeGameSpeed()
    {


        if (gameState == GameState.Playing)
        {
            gameState = GameState.Pause;
            Time.timeScale = 0;
            inGameUI.ShowTextLabel("Paused");
            StopCoroutine(Timer());
        }
        else if (gameState == GameState.Pause)
        {
            gameState = GameState.Playing;
            Time.timeScale = gameSpeed;
            inGameUI.HideTextLabel();
            StartCoroutine(Timer());
        }

    }
    public void Gameover()
    {
        foreach (ShootableObject shootable in FindObjectsOfType<ShootableObject>())
        {
            Destroy(shootable.gameObject);
        }
        foreach (ParticleSystem particle in FindObjectsOfType<ParticleSystem>())
        {
            Destroy(particle.gameObject);
        }
        gameState = GameState.GameOver;
        gameOvered?.Invoke();
    }



    public IEnumerator Timer()
    {
        while (gameState == GameState.Playing)
        {
            leftTime = leftTime - Time.unscaledDeltaTime;
            inGameUI.CountTime((((int)leftTime).ToString()));
            if (leftTime > 0) 
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                Gameover();
                yield break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameState != GameState.GameOver && gameState != GameState.NotinGame) //Если игра не проигранна
        {
            if (Input.GetKeyDown(pauseKey)) //Постановка и снятие с паузы
            {
                ChangeGameSpeed();
            }
            if (gameState == GameState.Playing) //Если игра в состоянии игры
            {
                if (Input.GetMouseButtonDown(0)) // Клик при ЛКМ
                {
                    Shoot();
                    Debug.Log("afterShoot");
                }
                if (!moleIsHere)
                {
                    if (UnityEngine.Random.value< chanseMoleToAppear)
                    {
                        SpawnMole();
                        Time.timeScale = UnityEngine.Random.Range(0.5f, 3f);
                    }
                }


            }
        }
    }
}

