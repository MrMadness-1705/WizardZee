using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xboxMouse : MonoBehaviour
{
    public CapsuleCollider2D player;

    private void Start()
    {
        //player = FindObjectOfType<MainPlayer>();
    }
    private void Update()
    {
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), player);
        foreach (martialHero go in Resources.FindObjectsOfTypeAll(typeof(martialHero)) as martialHero[])
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), go.GetComponent<CapsuleCollider2D>());
        }

        foreach (spearFighter go in Resources.FindObjectsOfTypeAll(typeof(spearFighter)) as spearFighter[])
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), go.GetComponent<CapsuleCollider2D>());
        }
        foreach (warrior go in Resources.FindObjectsOfTypeAll(typeof(warrior)) as warrior[])
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), go.GetComponent<CapsuleCollider2D>());
        }
        foreach (airWizard go in Resources.FindObjectsOfTypeAll(typeof(airWizard)) as airWizard[])
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), go.GetComponent<BoxCollider2D>());
        }
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        touchingPlayer = true;
    //        touchingEnemy = false;
    //    }
    //    else if (collision.tag == "Enemy")
    //    {
    //        Debug.Log("touching enemy");
    //        touchingPlayer = false;
    //        touchingEnemy = true;
    //        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision);
    //    }
    //}
}
