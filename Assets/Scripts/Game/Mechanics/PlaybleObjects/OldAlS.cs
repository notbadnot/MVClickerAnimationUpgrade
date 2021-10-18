using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AllianScript : MonoBehaviour
{
    public bool dead = false;
    [SerializeField]private int score = 50;
    [SerializeField]private Vector3 GrowingSpeed = ((Vector3.up + Vector3.right)/1000);
    [SerializeField]private static float MaxGrowSize = 0.45f;
    [SerializeField] private int Damage = -1;
    private GameMaster gameMaster;
    
    private Rigidbody2D rigidBody2;


    public int Getshoted()
    {
        rigidBody2.velocity = Vector2.down * 4;
        rigidBody2.angularVelocity = 720;
        dead = true;
        return score;
    }
    private void Grow()
    {
        transform.localScale = transform.localScale + GrowingSpeed;
    }
    private void BangSelf()
    {
        gameMaster.ChangeHealth(Damage);
        Destroy(transform.gameObject);

    }
    void Start()
    {
        rigidBody2 = GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.totalAlienNumber += 1;
    }

    private void OnDestroy()
    {
        gameMaster.totalAlienNumber -= 1;
    }


    private void FixedUpdate()
    {
        
        if (transform.localScale.x < MaxGrowSize && transform.localScale.y < MaxGrowSize)
        {
            Grow();
        }
        else
        {
            BangSelf();
        }

        
    }

}
