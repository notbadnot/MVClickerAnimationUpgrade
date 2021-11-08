using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZScript : MonoBehaviour
{
    // Start is called before the first frame update
    Collider2D myCollider2D;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Создается с коллайдером
    private void OnValidate()
    {
        myCollider2D = GetComponent<Collider2D>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        var triggerAlienInvider = collision.gameObject.GetComponentInChildren<AlienScript>();
        if (triggerAlienInvider != null)
        {



            if (collision.gameObject.GetComponentInChildren<AlienScript>().dead) //Если в триггер попадает мертвый пришелец то он уничтожается
            {
                collision.gameObject.GetComponentInChildren<AlienScript>().PlayTeleportBack();
                Destroy(collision.gameObject);
            }
        }
    }


}
