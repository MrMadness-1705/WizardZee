using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spearFighter : MonoBehaviour
{

    // Script contains functionality for a meelee fighter. I could've applied martial hero everywhere though, its no difference. But at any point
    // if i needed to access a different object like warrior or spear fighter, i could with this. Just to keep them differentiated.
    // This script can be useful for other hand-to-hand combat characters as well.

    public Transform player;
    public float moveSpeed = 3f;
    public float runTime = 2f;
    public float stopAndAttackRadius = 5f;
    public float attackingInterval;
    public LayerMask playerLayerMask;
    public Transform attackingPoint;
    public float attackingRadius;
    public float damage;
    public float health;
    public GameObject shootingPowerUpObject;
    public bool dropShootPowerUp;
    public Transform powerUpsStorage;
    public RectTransform healthImage;
    GameController gameC;

    Animator anim;
    private Rigidbody2D rb;
    [SerializeField] float horizontalSpeed;
    bool running;
    bool attacking;
    bool playerFound;
    float facing;
    // Start is called before the first frame update
    void Start()
    {
        gameC = FindObjectOfType<GameController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<MainPlayer>().transform;
        powerUpsStorage = FindObjectOfType<powerUpsDeployed>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.LogError(player.transform.position.x > transform.position.x);
        healthImage.offsetMax = new Vector2(health, healthImage.offsetMax.y);
        anim.SetInteger("walk", Mathf.RoundToInt(horizontalSpeed));
        //Debug.LogError(Mathf.RoundToInt(horizontalSpeed));

        if (!running)
        {
            StartCoroutine(runAtPlayer());
        }

        playerFound = Physics2D.OverlapCircle(transform.position, stopAndAttackRadius, playerLayerMask);

        if (playerFound)
        {
            //Debug.Log("attacking player now");
            StopCoroutine(runAtPlayer());
            horizontalSpeed = 0f;
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (!attacking)
            {
                StartCoroutine(attackPlayer());
            }
        }

        if (health <= -1)
        {
            GameObject kira = Instantiate(shootingPowerUpObject, transform);
            kira.transform.parent = powerUpsStorage;
            gameC.score += 7f;
            Destroy(this.gameObject);
        }
    }

    IEnumerator runAtPlayer()
    {
        running = true;
        if (player.transform.position.x > transform.position.x)
        {

            transform.localScale = new Vector3(4, 4, transform.localScale.z);
            horizontalSpeed = 1f;
            facing = 1f;
        }
        else
        {

            transform.localScale = new Vector3(-4, 4, transform.localScale.z);
            horizontalSpeed = -1f;
            facing = -1f;
        }
        rb.velocity = new Vector2(moveSpeed * horizontalSpeed, rb.velocity.y);
        yield return new WaitForSeconds(runTime);
        rb.velocity = new Vector2(0, rb.velocity.y);
        horizontalSpeed = 0f;
        running = false;
    }

    IEnumerator attackPlayer()
    {
        attacking = true;
        running = false;
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.30f);
        bool playerHurt = Physics2D.OverlapCircle(attackingPoint.position, attackingRadius, playerLayerMask);

        if (playerHurt)
        {
            Debug.Log("player is hurt!");
            if (!player.GetComponent<MainPlayer>().shielded)
            {
                player.GetComponent<MainPlayer>().health -= damage;
            }
        }
        else
        {
            Debug.Log("player is not hurt. yipeeeeeeeee");
        }

        yield return new WaitForSeconds(attackingInterval);
        attacking = false;
        running = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "magicBall")
        {
            health -= collision.GetComponent<magicBall>().damage;
            Destroy(collision.gameObject);
        }

        //if (collision.tag == "repeller")
        //{
        //    StopCoroutine(runAtPlayer());
        //    running = true;
        //    rb.velocity = new Vector2(-facing * collision.GetComponent<magicBall>().damage, rb.velocity.y);
        //    StartCoroutine(removeForce());
        //}
    }

    //IEnumerator removeForce(float time = 1f)
    //{
    //    yield return new WaitForSeconds(time);
    //    running = false;
    //}
}
