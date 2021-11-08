using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class MoleScript : ShootableObject
{

    [SerializeField] private GameObject teleportPS;
    [SerializeField] private GameObject backTeleportPS;
    [SerializeField] private float goingUpLength = 0.1f;
    public bool dead = false;
    private Rigidbody2D rigidBody2;
    private Collider2D collider2;
    private Animator animator;
    private float legthCounter = 0;
    private bool leaving = false;
    private Coroutine growingCoroutine;
    private SoundObject laughingSound;


    public event Action<MoleScript> LeavingEvent;

    private SoundManager _soundManager;
    [SerializeField] AudioClip inTeleportClip;
    [SerializeField] AudioClip outTeleportClip;
    [SerializeField] AudioClip hittedClip;
    [SerializeField] AudioClip laughingClip;


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
            if (animator != null)
            {
                animator.SetBool("GetHitted", true);
            }
            laughingSound.Stop();
             _soundManager.SpawnSoundObject().Play(hittedClip, transform.position);
            dead = true;
            leaving = true;
            StopCoroutine(growingCoroutine);
            StopCoroutine(Waiting());
            StartCoroutine(GoingDown());
            Debug.Log("OhNoImGetShoted");
            return base.GetShoted();
        }
        return 0;
    }


    public void PlayTeleportBack()
    {
        Instantiate(backTeleportPS, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 4), Quaternion.identity);
        _soundManager.SpawnSoundObject().Play(outTeleportClip, transform.position);
    }
    void Start()
    {

        rigidBody2 = GetComponent<Rigidbody2D>();
        Instantiate(teleportPS, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 4), Quaternion.identity);
        _soundManager.SpawnSoundObject().Play(inTeleportClip, transform.position);
        collider2 = GetComponent<Collider2D>();
        animator = gameObject.GetComponent<Animator>();
        legthCounter = 0;
        growingCoroutine = StartCoroutine(GoingUP());
        
    }

    public void Leaving()
    {
        PlayTeleportBack();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        LeavingEvent?.Invoke(gameObject.GetComponent<MoleScript>());
    }
    private IEnumerator GoingUP()
    {
        while (legthCounter < goingUpLength)
        {
            legthCounter += 0.01f;
            rigidBody2.MovePosition(rigidBody2.position + Vector2.up * 0.1f);
            Debug.Log("Going UP");
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Waiting());
        if (animator != null)
        {
            animator.SetBool("StartLaughing", true);
        }

        laughingSound = _soundManager.SpawnSoundObject();
        laughingSound.Play(laughingClip, transform.position);
        yield break;
    }
    private IEnumerator Waiting()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(2f);
        }
        StartCoroutine(GoingDown());
        yield break;
    }

    private IEnumerator GoingDown()
    {
        while (legthCounter > 0)
        {
            legthCounter -= 0.01f;
            Debug.Log("Going Down");
            rigidBody2.MovePosition(rigidBody2.position - Vector2.up * 0.1f);
            yield return new WaitForFixedUpdate();
        }
        Leaving();
        yield break;
    }


    private void FixedUpdate()
    {

    }

}

