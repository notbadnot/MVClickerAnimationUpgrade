using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienScript : ShootableObject
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 GrowingSpeed = ((Vector3.up + Vector3.right) / 1000); //Скорость роста
    [SerializeField] private static float MaxGrowSize = 0.45f; //До какого размера может вырасти пришелец (одинаковый чтобы игрок не путался)
    [SerializeField] private int Damage = -1; //Урон наносимый пришелцем при взрыве
    public bool dead = false; // Мертв ли пришелец
    private Rigidbody2D rigidBody2;
    
    /*При клике на пришельца тот начинает падать и вращатся в случайную сторону, со случайной скоростью
      Пришелец становится мертвым
      Возвращает очки за пришельца*/
    public override int GetShoted()
    {
        if (!dead)
        {
            rigidBody2.velocity = Vector2.down * 4;
            rigidBody2.angularVelocity = 720 * Random.Range(-2f, 2f);
            dead = true;
            return base.GetShoted();
        }
        return 0;
    }
    //Функция роста пришельца на скорость роста
    private void Grow()
    {
        transform.localScale = transform.localScale + GrowingSpeed;
    }
    //Функция Взрыва с нанесением урона и самоуничтожением
    private void BangSelf()
    {
        gameMaster.ChangeHealth(Damage);
        Destroy(transform.gameObject);

    }
    /*Подключение управляющего скрипта
      и Rigidbody2D, увеличение количества пришельцев при появлении */
    void Start()
    {
        rigidBody2 = GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.totalAlienNumber += 1;
    }

    //При уничтожение уменьшается число пришельцев
    private void OnDestroy()
    {
        gameMaster.totalAlienNumber -= 1;
    }


    private void FixedUpdate()
    {

        if (transform.localScale.x < MaxGrowSize && transform.localScale.y < MaxGrowSize) //Растет пока не достиг максимального роста, иначе взрывается
        {
            if (!dead)
            {
                Grow();
            }
        }
        else
        {
            BangSelf();
        }


    }

}

