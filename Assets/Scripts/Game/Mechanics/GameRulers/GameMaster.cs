using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject[] alienPrefab; // Несколько префабов для инстанцирования пришельцев
    [SerializeField] private GameObject heartPrefab; // Префаб для инстанцирования сердец
    [SerializeField] private int maxAliensNumber = 3;//Максимальное количество прищельцев на сцене / Заданное начальное значение потом вырастет в течении игры
    [SerializeField] private int health = 5; //Здоровье
    [SerializeField] private KeyCode pauseKey = KeyCode.P; // Настройка для кнопки паузы
    [SerializeField] private float scoreSpeedParam = 10000f; //параметр для изменения скорости игры в зависимости от очков 
    [SerializeField] private int scoreAlienNumberParam = 500; //параметр для изменения максимального количества пришельцев в зависимости от очков
    [SerializeField] private double alienSpawnChanseParam = 0.9; //Параметр Шанса появления пришельца
    [SerializeField] private double heartSpawnChanseParam = 0.9995; // Параметр Шанса появления сердца

    enum GameState // Энумерация для состояний игры
    {
        Playing,
        Pause,
        GameOver
    }
    enum SpeedAffector // Энумерация необходимая для регулирования скорости игры
    {
        Score,
        Pause,
        Gameover
    }
    private GameState gameState = GameState.Playing; //Текущее состояние игры (в процессе , пауза, проигрышь)
    private int score = 0; // Очки
    public int totalAlienNumber = 0; // Количество пришельцев в данный момент на сцене
    private float gameSpeed = 1; // Скорость игры
    void Start()
    {
        mainCam = Camera.main;
    }

    /*Функция клика. 
     * Вызывает рейкаст, в случае успешного попадения по объекту на который можно кликнуть вызывает на том GetShoted, далее вызывает функцию изменения очков, на то количество которое заддано в объекте  */
    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), mainCam.transform.forward);
        if (hit.collider != null)
        {
            GameObject hitGameObj = hit.collider.gameObject;
            if (hitGameObj.GetComponent<ShootableObject>() != null)
            {
                ChangeScore(hitGameObj.GetComponent<ShootableObject>().GetShoted());
                
            }



        }
    }

    /*Функция для появления прешельцев в заданной точке 
      При подаче вектора спавнит прешельца в данной точке иначе спавнит пришельца в случайной точке на экране
      Если подать номер пришельца то заспавнит его, иначе создаст случайного, в том числе если подать номер за пределами списка пришельцев*/
    private void SpawnAllien(Vector3? whereSpawn = null, int spawnNumber = -1)
    {
        if (spawnNumber < 0 || spawnNumber > alienPrefab.Length - 1)
        {
            spawnNumber = Mathf.RoundToInt(Random.Range(-0.49999f, alienPrefab.Length - 1 + 0.49999f));
        }
        if (whereSpawn != null)
        {
            Instantiate(alienPrefab[spawnNumber], whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * Random.Range(0.1f, 0.9f), mainCam.pixelHeight * Random.Range(0.1f, 0.9f), 0));
            Instantiate(alienPrefab[spawnNumber], new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
    }
    /*Функция для появления сердец в заданной точке 
      При подаче вектора спавнит сердце в данной точке иначе спавнит сердце в случайной точке на экране*/
    private void SpawnHeart(Vector3? whereSpawn = null)
    {
        if (whereSpawn != null)
        {
            Instantiate(heartPrefab, whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * Random.Range(0.1f, 0.9f), mainCam.pixelHeight * Random.Range(0.1f, 0.9f), 0));
            Instantiate(heartPrefab, new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
    }
    /*Функция для изменения здоровья
     Другие обьекты могут вызвать ее чтобы изменить количество здоровья
     Если здоровье становится <= 0 ты происходит проигрыш*/
    public void ChangeHealth(int Amount)
    {
        health += Amount;
        Debug.Log(message: "Health is " + health + " Score is " + score );
        if (health <= 0)
        {
            Debug.Log("Game over !!!");
            ChangeGameSpeed(SpeedAffector.Gameover);
        }
    }
    /*Функция для изменения очков
      При определенном значении очков увеличивает максимально возможное значение пришельцев, а так же скорость игры*/
    private void ChangeScore(int Amount)
    {
        score += Amount;
        ChangeGameSpeed(SpeedAffector.Score);
        maxAliensNumber = Mathf.Max(maxAliensNumber, Mathf.RoundToInt(score / scoreAlienNumberParam));
        Debug.Log(message: "Health is " + health + " Score is " + score);
    }
    
    /* Функция изменения скорости игры 
       Передаваемый параметр говорит функции о том как повлиять на скорость игры
       Очки - увеличить скорость в зависимости от очков
       Пауза - остановить игру или вернуть скорость обратно
       Проигрышь - остановить игру*/
    private void ChangeGameSpeed(SpeedAffector speedAffector)
    {
        if (speedAffector == SpeedAffector.Score)
        {
            gameSpeed = Mathf.Min(100, (1f + score / scoreSpeedParam));
            Time.timeScale = gameSpeed;
        }
        else if (speedAffector == SpeedAffector.Pause)
        {
            if (gameState == GameState.Playing)
            {
                gameState = GameState.Pause;
                Time.timeScale = 0;
            }
            else if (gameState == GameState.Pause)
            {
                gameState = GameState.Playing;
                Time.timeScale = gameSpeed;
            }
        }
        else if (speedAffector == SpeedAffector.Gameover)
        {
            gameState = GameState.GameOver;
            Time.timeScale = 0;
        }
    }


    void Update()
    {
        if (gameState != GameState.GameOver) //Если игра не проигранна
        {
            if (Input.GetKeyDown(pauseKey)) //Постановка и снятие с паузы
            {
                ChangeGameSpeed(SpeedAffector.Pause);
            }
            if (gameState == GameState.Playing) //Если игра в состоянии игры
            {
                if (Input.GetMouseButtonDown(0)) // Клик при ЛКМ
                {
                    Shoot();

                }

                if (Random.value > alienSpawnChanseParam && totalAlienNumber < maxAliensNumber) //Появление пришельца с некоторым шансом и если пришельцев еще не максимум
                {                   
                    SpawnAllien();

                }
                if (Random.value > heartSpawnChanseParam) // Появление сердца с некоторым шансом
                {
                    SpawnHeart();
                }
            }
        }
    }
}
