using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainPlayer : MonoBehaviour
{
    // Script contains all essential functionalities for player - attacking, movement etc.
    [Header("Player Settings")]
    public float speed = 5f;
    public float shootingSpeed;
    public float shootingIntervalTime = 0.5f;
    public float health;
    public float shootLimit;
    public float decreaseShootLimitByEachShootValue;
    public float shootLimitIncreaseValueByPowerUpValue;
    public float shield = 0f;
    public GameObject shieldLight;
    public bool shielded = false;

    [Header("For dev")]
    public Transform xboxMouse;
    public float xboxMouseMoveSpeed;
    public bool developerMode;
    public bool enemiesInRange = false;
    


    [Header("Important values to Attach/Set or just important variables that must be made public for other scripts accessing it")]
    public Camera camera;
    public float angleOffset = 270f;
    public Transform wand;
    public GameObject normalMagicBall;
    //public GameObject repellerMagicBall;
    public RectTransform healthImage;
    public RectTransform shootingLimitImage;
    public TMP_Text playerThoughts;
    public RectTransform shieldImage;
    public LayerMask enemyLayerMask;
    public GameObject fader;
    public Transform magicBallsStorage;
    public bool dying = false;
    public AudioSource gameOver;
    public AudioSource magicBallShoot;
    public TMP_Text shieldLowWarning;
    public GameObject healthAlert;
    public Transform mainCanvas;


    // private variables
    Transform firePoint;
    GameController gc;
    bool alive = true;
    Vector2 wandStartPos;
    Vector2 mousePos;
    Vector2 lookDirection;
    float lookAngle;
    float horizontalInput;
    Rigidbody2D rb2D;
    Animator anim;
    bool shootingRunning = false;
    bool repellerRunning = false;
    bool regenerating = false;
    bool chatting = false;
    bool alerted = false;

    // xbox 
    float xboxControllerXInput;
    float xboxControllerYInput;
    Rigidbody2D xboxMouseRb;
    
     //public bool enemiesInRange = false;

    private void Start()
    {
        health = 0f;
        dying = false;
        gc = FindObjectOfType<GameController>();
        rb2D = GetComponent<Rigidbody2D>(); // gets the rigidbody2d component in the player
        anim = GetComponent<Animator>(); // gets the animator component in the player
        firePoint = wand.GetChild(0);
        wandStartPos = wand.transform.position;
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        shieldLight.SetActive(false);
        xboxMouseRb = xboxMouse.GetComponent<Rigidbody2D>();
        //magicBallsStorage = wand.GetChild(1);
    }

    void Update()
    {
        if (alive)
        {
           
            healthImage.offsetMax = new Vector2(health, healthImage.offsetMax.y);
            shieldImage.offsetMax = new Vector2(shield, shieldImage.offsetMax.y);
            shootingLimitImage.offsetMax = new Vector2(shootLimit, shootingLimitImage.offsetMax.y);

            enemiesInRange = Physics2D.OverlapCircle(transform.position, 50f, enemyLayerMask);
            // calculating the lookingAngle for shooting
            //lookDirection = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //lookAngle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;

            ////firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);

            //wand.rotation = Quaternion.Euler(0, 0, lookAngle + angleOffset);

            //Vector3 lookDirection = mousePos - wand.GetComponent<Rigidbody2D>().position;
            //float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            //wand.GetComponent<Rigidbody2D>().rotation = angle;

            if (!shielded)
            {
                if (!regenerating && health == 0)
                    StartCoroutine(regenerate());
            }
            else if (shielded)
            {
                if (!developerMode)
                {
                    StopCoroutine(regenerate());
                }
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetButton("FireX"))
            {
                if (gc.proceedWithGame)
                {
                    if (!shootingRunning && shootLimit > -190)
                        StartCoroutine(shoot());
                }
            }

            xboxControllerXInput = Input.GetAxisRaw("wandLookX");
            xboxControllerYInput = Input.GetAxisRaw("wandLookY");
            if (xboxControllerXInput != 0)
            {
                //Vector2 xmousePos = xboxMouse.transform.position;
                //xmousePos.x += xboxMouseMoveSpeed * xboxControllerXInput;
                //xboxMouse.transform.position = xmousePos;

                xboxMouseRb.velocity = new Vector2(xboxMouseMoveSpeed * xboxControllerXInput, xboxMouseRb.velocity.y);
            }

            Debug.Log($"Input x: {Input.GetButton("wandLookX")} and Input y: {Input.GetButton("wandLookY")}");

            if (xboxControllerYInput != 0)
            {
                //Vector2 ymousePos = xboxMouse.transform.position;
                //ymousePos.y += xboxMouseMoveSpeed * xboxControllerYInput;
                //xboxMouse.transform.position = ymousePos;

                xboxMouseRb.velocity = new Vector2(xboxMouseRb.velocity.x, xboxControllerYInput * xboxMouseMoveSpeed);
            }

            if (xboxControllerXInput == 0)
            {
                xboxMouseRb.velocity = new Vector2(0, xboxMouseRb.velocity.y);
            }
            
            if (xboxControllerYInput == 0)
            {
                xboxMouseRb.velocity = new Vector2(xboxMouseRb.velocity.x, 0);
            }

            //if (Input.GetKey(KeyCode.E)) {

            //    if (!repellerRunning)
            //    {

            //        StartCoroutine(shootRepeller());
            //    }
            //}

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("LeftBumper"))
            {
                if (gc.proceedWithGame && gc.authenticationToUseShield)
                {
                    if (!shielded)
                    {
                        if (shield > -140f)
                        {
                            StartCoroutine(Shield());
                        }
                    }
                }
            }

            if (shield < -140f)
            {
                shieldLowWarning.gameObject.SetActive(true);
            }
            else if (shield > -140f)
            {
                shieldLowWarning.gameObject.SetActive(false);
            }

            if (health < -100f && !alerted)
            {
                StartCoroutine(alertPlayer());
            }

            // moving the player
            horizontalInput = Input.GetAxisRaw("Horizontal"); // gets the horizontal Input between -1 and 1 or returns 0 when no movement.
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(3, transform.localScale.y, transform.localScale.z);
                playerThoughts.GetComponent<RectTransform>().localScale = new Vector3(1, 1, transform.localScale.z);
                anim.SetFloat("Speed", horizontalInput);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-3, transform.localScale.y, transform.localScale.z);
                playerThoughts.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, transform.localScale.z);
                anim.SetFloat("Speed", -horizontalInput);
            }
            else if (horizontalInput == 0)
            {
                anim.SetFloat("Speed", horizontalInput);
            }
            rb2D.velocity = new Vector2(horizontalInput * speed, rb2D.velocity.y); // giving a velocity to player to move.
        }

        if (health <= -160)
        {
            if (!dying)
            {
                StopAllCoroutines();
                alive = false;
                StartCoroutine(die());
            }
        }

        
    }

    IEnumerator alertPlayer()
    {
        alerted = true;
        Debug.Log("Alerting the player");
        GameObject i = Instantiate(healthAlert, mainCanvas);
        yield return new WaitForSeconds(1f);
        Destroy(i);
        yield return new WaitForSeconds(3f);
        alerted = false;
    }

    IEnumerator shoot()
    {
        shootingRunning = true;
        yield return new WaitForSeconds(shootingIntervalTime);
        magicBallShoot.Play();
        GameObject bullet = Instantiate(normalMagicBall, firePoint);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * shootingSpeed, ForceMode2D.Impulse);
        bullet.transform.parent = magicBallsStorage;
        if (gc.proceedWithGame)
        {
            shootLimit -= decreaseShootLimitByEachShootValue;
        }
        shootingRunning = false;
    }
    //IEnumerator shootRepeller()
    //{
    //    repellerRunning = true;
    //    yield return new WaitForSeconds(shootingIntervalTime);
    //    GameObject bullet = Instantiate(repellerMagicBall, firePoint);
    //    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    //    rb.AddForce(firePoint.up * shootingSpeed, ForceMode2D.Impulse);
    //    bullet.transform.parent = magicBallsStorage;
    //    repellerRunning = false;
    //}  

    IEnumerator Shield() { 
    
        shielded = true;
        shieldLight.SetActive(true);
        if (gc.proceedWithGame)
        {
            shield -= 40f;
        }
        yield return new WaitForSeconds(4f);
        shieldLight.SetActive(false);
        shielded = false;
    }

    IEnumerator regenerate()
    {
        regenerating = true;
            Debug.Log("added 2 to health");
            health += 0.5f;
        shield += 0.5f;
            yield return new WaitForSeconds(1f);
        regenerating = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive)
        {
            if (collision.tag == "hurtPlayer")
            {
                if (!shielded)
                {
                    if (!developerMode)
                        health -= collision.gameObject.GetComponent<enemyMagicBall>().damage;
                }
            }

            if (collision.tag == "powerup")
            {
                if (shootLimit != 0)
                {
                    shootLimit += shootLimitIncreaseValueByPowerUpValue;
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    IEnumerator die()
    {
        dying = true;
        gc.backGroundMusic.Stop();
        gameOver.Play();
        anim.SetFloat("Speed", 0);
        anim.SetTrigger("die");
        gc.declare("Defeat.");
        gc.saySomething("No... I can't die... I am the greatest wizard... of...");
        yield return new WaitForSeconds(5f);
        fader.SetActive(true);
        // RIP
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("SampleScene");
        
    }

   
}
