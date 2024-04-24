using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class airWizard : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public Transform magicBallsStorage;
    public float shootingVelocity = 5f;
    public GameObject enemyMagicBall;
    public float shootingInterval = 2f;
    public RectTransform healthImage;
    public float health = 0f;
    bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MainPlayer>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        healthImage.offsetMax = new Vector2(health, healthImage.offsetMax.y);


        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(4, 4);
        }
        else if (player.transform.position.y < transform.position.x)
        {
            transform.localScale = new Vector2(-4, 4);
        }


        Vector3 lookDirection = player.position - firePoint.GetComponent<Rigidbody2D>().transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        firePoint.GetComponent<Rigidbody2D>().rotation = angle;

        if (!shooting)
        {
            StartCoroutine(shootPlayer());
        }

        if (health <= -1)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    IEnumerator shootPlayer()
    {
        shooting = true;
        GameObject blast = Instantiate(enemyMagicBall, firePoint);
        Rigidbody2D rb = blast.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * shootingVelocity, ForceMode2D.Impulse);
        blast.transform.parent = magicBallsStorage;
        yield return new WaitForSeconds(shootingInterval);
        shooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "magicBall")
        {
            health -= collision.GetComponent<magicBall>().damage;
            Destroy(collision.gameObject);
        }
    }
}
