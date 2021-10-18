using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : ShootableObject
{
    private int healing = 1; //На сколько лечит при попадении в сердце
    private float timer = 0f; // Таймер до уничтожения сердца
    [SerializeField]private float maxTime = 2f; // Значение которое таймер достигает после чего сердце уничтожается

    /*При попадении в сердце 
      Оно уничтожаетсся и увеличивается здоровье на размер лечения также увеличиваются очки*/
    public override int GetShoted()
    {
        gameMaster.ChangeHealth(healing);
        Destroy(transform.gameObject);
        return base.GetShoted();
    }
    //При появлении находит основной управляющий скрипт
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    //Выжидает таймер до самоуничтожения
    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer > maxTime)
        {
            Destroy(transform.gameObject);
        }
    }
}
