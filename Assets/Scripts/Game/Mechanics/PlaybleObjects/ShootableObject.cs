using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    protected GameMaster gameMaster; //Ссылка на скрипт управляющий игрой
    [SerializeField] private int score = 50; //Количество очков за попадение в данный объект

    public virtual int GetShoted() //Наследуемый метод что делать в слечае попадения по данному объекту
    {
        return score;
    }
    void Start()
    {

    }

    void Update()
    {
        
    }
}
