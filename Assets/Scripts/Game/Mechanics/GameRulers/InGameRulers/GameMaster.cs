using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject[] alienPrefab; // ��������� �������� ��� ��������������� ����������
    [SerializeField] private GameObject heartPrefab; // ������ ��� ��������������� ������
    [SerializeField] private int DefaultmaxAliensNumber = 3;//������������ ���������� ���������� �� ����� / �������� ��������� �������� ����� �������� � ������� ����
    [SerializeField] private int Defaulthealth = 5; //��������
    [SerializeField] private KeyCode pauseKey = KeyCode.P; // ��������� ��� ������ �����
    [SerializeField] private float scoreSpeedParam = 10000f; //�������� ��� ��������� �������� ���� � ����������� �� ����� 
    [SerializeField] private int scoreAlienNumberParam = 500; //�������� ��� ��������� ������������� ���������� ���������� � ����������� �� �����
    [SerializeField] private double alienSpawnChanseParam = 0.9; //�������� ����� ��������� ���������
    [SerializeField] private double heartSpawnChanseParam = 0.9995; // �������� ����� ��������� ������
    [SerializeField] private GameObject alienSquadPrefab;
    [SerializeField] private GameObject postProcessingPrefab;

    //========================================================================




    enum GameState // ���������� ��� ��������� ����
    {
        Playing,
        Pause,
        GameOver,
        NotinGame
    }
    enum SpeedAffector // ���������� ����������� ��� ������������� �������� ����
    {
        Score,
        Pause,
        Gameover
    }

    public event Action<int,int> gameOvered;

    private int health;
    private int maxAliensNumber;

    private GameState gameState = GameState.NotinGame; //������� ��������� ���� (� �������� , �����, ���������)
    private int score = 0; // ����
    public int totalAlienNumber = 0; // ���������� ���������� � ������ ������ �� �����
    private float gameSpeed = 1; // �������� ����
    private InGameUIManager inGameUI;
    private float playTime = 0;
    private float difficultyParam = 1;

    private bool crowdingEnabled;

    private GameObject damagingPostProcessor;

    private PrefabFactory _prefabFactory;
    [Inject]
    private void Construct(PrefabFactory prefabFactory, InGameUIManager _inGameUIManager)
    {
        _prefabFactory = prefabFactory;
        inGameUI = _inGameUIManager;
    }
    void Start()
    {
        mainCam = Camera.main;
        Time.timeScale = 0;
        //inGameUI = gameObject.GetComponent<InGameUIManager>();
        damagingPostProcessor = Instantiate(postProcessingPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        damagingPostProcessor.gameObject.SetActive(false);
    }
    public void StartGame(bool crowdingAllowed,GameModel.Difficulty difficulty= GameModel.Difficulty.Medium)
    {
        if (difficulty == GameModel.Difficulty.Easy)
        {
            difficultyParam = 0.3f;
        }else if (difficulty == GameModel.Difficulty.Medium)
        {
            difficultyParam = 1f;
        }else if (difficulty == GameModel.Difficulty.Hard)
        {
            difficultyParam = 3f;
        }
        crowdingEnabled = crowdingAllowed;
        gameState = GameState.Playing;
        Time.timeScale = 1;
        health = Defaulthealth;
        maxAliensNumber = DefaultmaxAliensNumber;
        score = 0;
        playTime = 0;
        inGameUI.CountScore(score);
        inGameUI.CountHealth(health);
        inGameUI.HideTextLabel();
        StartCoroutine(Timer());
    }

    /*������� �����. 
     * �������� �������, � ������ ��������� ��������� �� ������� �� ������� ����� �������� �������� �� ��� GetShoted, ����� �������� ������� ��������� �����, �� �� ���������� ������� ������� � �������  */
    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), mainCam.transform.forward);
        if (hit.collider != null)
        {
            GameObject hitGameObj = hit.collider.gameObject;
            if (hitGameObj.GetComponent<ShootableObject>() != null)
            {
                ChangeScore(((int)(hitGameObj.GetComponent<ShootableObject>().GetShoted() * difficultyParam)));
                inGameUI.CountScore(score);
            }



        }
    }

    /*������� ��� ��������� ���������� � �������� ����� 
      ��� ������ ������� ������� ��������� � ������ ����� ����� ������� ��������� � ��������� ����� �� ������
      ���� ������ ����� ��������� �� ��������� ���, ����� ������� ����������, � ��� ����� ���� ������ ����� �� ��������� ������ ����������*/
    private void SpawnAllien(Vector3? whereSpawn = null, int spawnNumber = -1)
    {
        GameObject spawnedAlien;
        if (spawnNumber < 0 || spawnNumber > alienPrefab.Length - 1)
        {
            spawnNumber = Mathf.RoundToInt(UnityEngine.Random.Range(-0.49999f, alienPrefab.Length - 1 + 0.49999f));
        }
        if (whereSpawn != null)
        {
            spawnedAlien = _prefabFactory.Spawn(alienPrefab[spawnNumber], whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * UnityEngine.Random.Range(0.1f, 0.9f), mainCam.pixelHeight * UnityEngine.Random.Range(0.1f, 0.9f), 0));
            spawnedAlien = _prefabFactory.Spawn(alienPrefab[spawnNumber], new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);

        }
        totalAlienNumber++;
        if (!crowdingEnabled)
        {
            spawnedAlien.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        spawnedAlien.GetComponent<AlienScript>().ImBangedSelfEvent += GameMaster_ImBangedSelfEvent;
        spawnedAlien.GetComponent<AlienScript>().ImDestroidEvent += GameMaster_ImDestroidEvent;
    }

    private void SpawnAlienSquad(int aliensInSpawnedSquad = 3, Vector3? whereSpawn = null)
    {
        GameObject spawnedAlienSquad;
        alienSquadPrefab.GetComponent<AlienSquadScript>().aliensInSquad = aliensInSpawnedSquad;
        if (whereSpawn != null)
        {
            spawnedAlienSquad = _prefabFactory.Spawn(alienSquadPrefab, whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * UnityEngine.Random.Range(0.1f, 0.9f), mainCam.pixelHeight * UnityEngine.Random.Range(0.1f, 0.9f), 0));
            spawnedAlienSquad = _prefabFactory.Spawn(alienSquadPrefab, new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
        totalAlienNumber += aliensInSpawnedSquad;
        spawnedAlienSquad.GetComponent<AlienSquadScript>().SquadIsReady += GameMaster_SquadIsReady;
    }

    private void GameMaster_SquadIsReady(GameObject obj)
    {
        for (int alienInSquad = 0; alienInSquad < obj.transform.childCount; alienInSquad++)
        {
            AlienScript alienScriptOfAlienInSquad = obj.transform.GetChild(alienInSquad).GetComponent<AlienScript>();
            alienScriptOfAlienInSquad.ImBangedSelfEvent += GameMaster_ImBangedSelfEvent;
            alienScriptOfAlienInSquad.ImDestroidEvent += GameMaster_ImDestroidEvent;
        }
        obj.GetComponent<AlienSquadScript>().SquadIsReady -= GameMaster_SquadIsReady;
    }

    private void GameMaster_ImDestroidEvent(GameObject obj)
    {
        totalAlienNumber--;
        obj.GetComponent<AlienScript>().ImBangedSelfEvent -= GameMaster_ImBangedSelfEvent;
        obj.GetComponent<AlienScript>().ImDestroidEvent -= GameMaster_ImDestroidEvent;
    }

    private void GameMaster_ImBangedSelfEvent(GameObject obj)
    {
        ChangeHealth(obj.GetComponent<AlienScript>().Damage);
        obj.GetComponent<AlienScript>().ImBangedSelfEvent -= GameMaster_ImBangedSelfEvent;
    }

    /*������� ��� ��������� ������ � �������� ����� 
 ��� ������ ������� ������� ������ � ������ ����� ����� ������� ������ � ��������� ����� �� ������*/
    private void SpawnHeart(Vector3? whereSpawn = null)
    {
        if (whereSpawn != null)
        {
            _prefabFactory.Spawn(heartPrefab, whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * UnityEngine.Random.Range(0.1f, 0.9f), mainCam.pixelHeight * UnityEngine.Random.Range(0.1f, 0.9f), 0));
            _prefabFactory.Spawn(heartPrefab, new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
    }
    /*������� ��� ��������� ��������
     ������ ������� ����� ������� �� ����� �������� ���������� ��������
     ���� �������� ���������� <= 0 �� ���������� ��������*/
    public void ChangeHealth(int Amount)
    {
        health += Amount;
        Debug.Log(message: "Health is " + health + " Score is " + score );
        inGameUI.CountHealth(health);
        if (health <= 0)
        {
            Debug.Log("Game over !!!");
            inGameUI.ShowTextLabel("Game Over");
            ChangeGameSpeed(SpeedAffector.Gameover);
            foreach (ParticleSystem particle in FindObjectsOfType<ParticleSystem>())
            {
                particle.Stop();
            }
            FindObjectsOfType<ShootableObject>();
            foreach (ShootableObject shootable in FindObjectsOfType<ShootableObject>())
            {
                Destroy(shootable.gameObject);
            }
            foreach (ParticleSystem particle in FindObjectsOfType<ParticleSystem>())
            {
                Destroy(particle.gameObject);
            }
                gameOvered?.Invoke(score, (int)playTime);
        }
        if (Amount < 0)
        {
            StartCoroutine(damagingEffect());
        }
    }
    /*������� ��� ��������� �����
      ��� ������������ �������� ����� ����������� ����������� ��������� �������� ����������, � ��� �� �������� ����*/
    private void ChangeScore(int Amount)
    {
        score += Amount;
        ChangeGameSpeed(SpeedAffector.Score);
        maxAliensNumber = Mathf.Max(maxAliensNumber, Mathf.RoundToInt((score / scoreAlienNumberParam)*difficultyParam));
        Debug.Log(message: "Health is " + health + " Score is " + score);
    }
    
    /* ������� ��������� �������� ���� 
       ������������ �������� ������� ������� � ��� ��� �������� �� �������� ����
       ���� - ��������� �������� � ����������� �� �����
       ����� - ���������� ���� ��� ������� �������� �������
       ��������� - ���������� ����*/
    private void ChangeGameSpeed(SpeedAffector speedAffector)
    {
        if (speedAffector == SpeedAffector.Score)
        {
            gameSpeed = Mathf.Min(100, (1f + (score / scoreSpeedParam)*difficultyParam) );
            Time.timeScale = gameSpeed;
        }
        else if (speedAffector == SpeedAffector.Pause)
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
        else if (speedAffector == SpeedAffector.Gameover)
        {
            gameState = GameState.GameOver;
            Time.timeScale = 0;
            StopCoroutine(Timer());
        }
    }
    public void TelUImanagerToSwitchInGameMenu(bool switcher)
    {
        inGameUI.SwitchInGameMenu(switcher);
    }
    private IEnumerator damagingEffect()
    {
        while (true)
        {
            if (!damagingPostProcessor.gameObject.activeInHierarchy)
            {
                damagingPostProcessor.SetActive(true);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            else
            {
                damagingPostProcessor.SetActive(false);
                yield break;
            }
        }
    }
    public IEnumerator Timer()
    {
        while (gameState == GameState.Playing)
        {
            playTime = playTime + Time.unscaledDeltaTime;
            inGameUI.CountTime((((int)playTime).ToString()));
            yield return new WaitForEndOfFrame();
        }
    }


    void Update()
    {
        if (gameState != GameState.GameOver && gameState != GameState.NotinGame) //���� ���� �� ����������
        {
            if (Input.GetKeyDown(pauseKey)) //���������� � ������ � �����
            {
                ChangeGameSpeed(SpeedAffector.Pause);
            }
            if (gameState == GameState.Playing) //���� ���� � ��������� ����
            {
                if (Input.GetMouseButtonDown(0)) // ���� ��� ���
                {
                    Shoot();

                }
                //========================================================
                if (Input.GetMouseButtonDown(1)) // ���� ��� ���
                {
                    Instantiate(alienSquadPrefab, new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);

                }
                //========================================================


                if (UnityEngine.Random.value > alienSpawnChanseParam && totalAlienNumber < maxAliensNumber) //��������� ��������� � ��������� ������ � ���� ���������� ��� �� ��������
                {
                    int tryedAliensInSquad = Mathf.RoundToInt(UnityEngine.Random.Range(2, 5));
                    if (totalAlienNumber + tryedAliensInSquad <= maxAliensNumber && UnityEngine.Random.value < 0.5 * difficultyParam)
                    {
                        SpawnAlienSquad(tryedAliensInSquad);

                    }
                    else
                    {
                        SpawnAllien();
                    }
                }
                if (UnityEngine.Random.value > heartSpawnChanseParam) // ��������� ������ � ��������� ������
                {
                    SpawnHeart();
                }
            }
        }
    }
}
