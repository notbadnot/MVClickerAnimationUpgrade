using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienScript : ShootableObject
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 GrowingSpeed = ((Vector3.up + Vector3.right) / 1000); //Скорость роста
    [SerializeField] private static float MaxGrowSize = 0.45f; //До какого размера может вырасти пришелец (одинаковый чтобы игрок не путался)
    [SerializeField] public int Damage = -1; //Урон наносимый пришелцем при взрыве
    [SerializeField] private GameObject teleportPS;
    [SerializeField] private GameObject backTeleportPS;
    [SerializeField] private GameObject smokePS;
    [SerializeField] private GameObject firePS;
    public bool dead = false; // Мертв ли пришелец
    private Rigidbody2D rigidBody2;
    private Collider2D collider2;
    private Animator animator;

    private SoundManager _soundManager;
    [SerializeField] AudioClip inTeleportClip;
    [SerializeField] AudioClip outTeleportClip;
    [SerializeField] AudioClip hittedClip;
    [SerializeField] AudioClip bangedSelfClip;

    public event Action<GameObject> ImDestroidEvent;
    public event Action<GameObject> ImBangedSelfEvent;
    public event Action<GameObject> ImGetShotedEvent;
    /*При клике на пришельца тот начинает падать и вращатся в случайную сторону, со случайной скоростью
      Пришелец становится мертвым
      Возвращает очки за пришельца*/
    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    public override int GetShoted()
    {
        if (!dead)
        {
            if (animator !=  null)
            {
                animator.SetBool("GetHitted", true);
            }
            _soundManager.SpawnSoundObject().Play(hittedClip, gameObject.transform.position);
            ImGetShotedEvent?.Invoke(gameObject);
            collider2.isTrigger = true;
            rigidBody2.drag = 0;
            rigidBody2.angularDrag = 0.0005f;
            rigidBody2.velocity = Vector2.down * 4;
            rigidBody2.angularVelocity = 720 * UnityEngine.Random.Range(-2f, 2f);
            dead = true;
            Instantiate(smokePS, transform);
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
        ImBangedSelfEvent.Invoke(gameObject);
        //gameMaster.ChangeHealth(Damage);
        _soundManager.SpawnSoundObject().Play(bangedSelfClip, gameObject.transform.position);
        Instantiate(firePS, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 4), Quaternion.identity);
        Destroy(transform.gameObject);

    }
    public void PlayTeleportBack()
    {
        Instantiate(backTeleportPS, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 4), Quaternion.identity);
        _soundManager.SpawnSoundObject().Play(outTeleportClip, gameObject.transform.position);
    }
    /*Подключение управляющего скрипта
      и Rigidbody2D, увеличение количества пришельцев при появлении */
    void Start()
    {
        //======================================================

        //=====================================================
        rigidBody2 = GetComponent<Rigidbody2D>();
        //gameMaster = FindObjectOfType<GameMaster>();
        //gameMaster.totalAlienNumber += 1;
        Instantiate(teleportPS, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 4), Quaternion.identity);
        _soundManager.SpawnSoundObject().Play(inTeleportClip, gameObject.transform.position);
        collider2 = GetComponent<Collider2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    //При уничтожение уменьшается число пришельцев
    private void OnDestroy()
    {
        ImDestroidEvent?.Invoke(gameObject);
        //gameMaster.totalAlienNumber -= 1;
    }
    public void BangingSelf()
    {
        BangSelf();
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
            if (animator == null)
            {


                BangSelf();
            }
            else
            {
                animator.SetBool("ReadyToExplode", true);
            }
        }


    }

}

