using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // controlls what happens in the game

    public int wave = 1;

    public Transform spawner;

    [Header("Necessary")]
    public MainPlayer player;
    public TMP_Text mainText;
    public TMP_Text playerThoughts;
    public TypeWriterEffect typeWriter;
    public Transform spawnerDeployedEnemies;
    public GameObject playerCanvas;
    public GameObject startScreenCanvas;
    public TMP_Dropdown dropD;
    public Toggle keyboardToggle;
    public Toggle xboxToggle;
    public Button playButton;
    public TMP_Text xboxPlay;
    public Toggle easyToggle;
    public Toggle normalToggle;
    public Toggle hardToggle;
    public GameObject xboxDifficultyLevel;
    public GameObject pcControls;
    public GameObject xboxControls;
    public float score = 0f;
    public TMP_Text scoreText;
    public GameObject adjustXboxTargetterSpeedGameObject;
    public TMP_Text xboxTargetterSpeed;
    public float xboxTargetterSpeedIncreaseAndDecreaseValue = 0.05f;
    public AudioSource victoryMusic;
    public AudioSource backGroundMusic;
    public AudioSource rechargeMusic;
    
    

    [Header("Corners(for development process)")]
    public Transform corner1;
    public Transform corner2;
    public Transform corner3;
    public Transform corner4;
    public Transform groundCorner;
    public Transform groundCorner2;

    [Header("Enemies")]
    public GameObject airWizard;
    public GameObject martialHerof;
    public GameObject spearFighter;
    public GameObject warrior;
    public GameObject powerUp;

    [Header("not to be altered in inspector")]
    public bool proceedWithGame;
    public bool authenticationToUseShield = false;

    // private variables
    bool wave1running;
    bool chatting = false;
    Coroutine lastRoutine = null;
    
    int gameMode = 0;

    void Start()
    {
        score = 0f;
        playerCanvas.SetActive(false);
        //saySomething("WAssup?", 1f, 20f);
        mainText.text = string.Empty;
        playButton.onClick.AddListener(OnPlayClicked);
        authenticationToUseShield = false;
        backGroundMusic.Play();
    }

    void Update()
    {
        if (!proceedWithGame)
        {
            xboxTargetterSpeed.text = Mathf.Round(player.xboxMouseMoveSpeed).ToString();
            Debug.Log("Increasing xboxTargetterSpeed");
        }

        if (Input.GetButton("FireX"))
        {
            player.xboxMouseMoveSpeed -= xboxTargetterSpeedIncreaseAndDecreaseValue;
        }
        else if (Input.GetButton("RightBumper"))
        {
            player.xboxMouseMoveSpeed += xboxTargetterSpeedIncreaseAndDecreaseValue;
        }

        if (!proceedWithGame)
        {
            if (!chatting)
            {
                lastRoutine = StartCoroutine(chat());
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            keyboardToggle.isOn = true;
            xboxToggle.isOn = false;
        }
        else if (Input.GetButtonDown("RightBumper"))
        {
            xboxToggle.isOn = true;
            keyboardToggle.isOn = false;
        }

        if (xboxToggle.isOn)
        {
            adjustXboxTargetterSpeedGameObject.SetActive(true);
            pcControls.SetActive(false);
            xboxControls.SetActive(true);
            xboxDifficultyLevel.SetActive(true);
            dropD.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            xboxPlay.gameObject.SetActive(true);
        }
        else if (keyboardToggle.isOn)
        {
            adjustXboxTargetterSpeedGameObject.SetActive(false);
            pcControls.SetActive(true);
            xboxControls.SetActive(false);
            xboxDifficultyLevel.SetActive(false);
            dropD.gameObject.SetActive(true);
            playButton.gameObject.SetActive(true);
            xboxPlay.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Y"))
        {
            easyToggle.isOn = true;
            normalToggle.isOn = false;
            hardToggle.isOn = false;
        }
        else if (Input.GetButtonDown("X"))
        {
            normalToggle.isOn = true;
            easyToggle.isOn = false;
            hardToggle.isOn = false;
        }
        else if (Input.GetButtonDown("B"))
        {
            hardToggle.isOn = true;
            easyToggle.isOn = false;
            normalToggle.isOn = false;
        }

        if (xboxToggle.isOn)
        {
            if (Input.GetButtonDown("LeftBumper"))
            {
                OnPlayClicked();
            }
        }


        if (proceedWithGame)
        {
            StopCoroutine(lastRoutine);
            scoreText.text = $"Score: {score}";
            switch (gameMode)
            {
                case 0:
                    foreach (martialHero go in Resources.FindObjectsOfTypeAll(typeof(martialHero)) as martialHero[])
                    {
                        go.damage = 15f;
                    }

                    foreach (enemyMagicBall go in Resources.FindObjectsOfTypeAll(typeof(enemyMagicBall)) as enemyMagicBall[])
                    {
                        go.damage = 10f;
                    }

                    foreach (spearFighter go in Resources.FindObjectsOfTypeAll(typeof(spearFighter)) as spearFighter[])
                    {
                        go.damage = 20f;
                    }
                    foreach (warrior go in Resources.FindObjectsOfTypeAll(typeof(warrior)) as warrior[])
                    {
                        go.damage = 30f;
                    }
                    break;

                case 1:

                    foreach (martialHero go in Resources.FindObjectsOfTypeAll(typeof(martialHero)) as martialHero[])
                    {
                        go.damage = 25f;
                    }

                    foreach (enemyMagicBall go in Resources.FindObjectsOfTypeAll(typeof(enemyMagicBall)) as enemyMagicBall[])
                    {
                        go.damage = 15f;
                    }

                    foreach (spearFighter go in Resources.FindObjectsOfTypeAll(typeof(spearFighter)) as spearFighter[])
                    {
                        go.damage = 30f;
                    }
                    foreach (warrior go in Resources.FindObjectsOfTypeAll(typeof(warrior)) as warrior[])
                    {
                        go.damage = 40f;
                    }
                    break;
                case 2:

                    foreach (martialHero go in Resources.FindObjectsOfTypeAll(typeof(martialHero)) as martialHero[])
                    {
                        go.damage = 30f;
                    }

                    foreach (enemyMagicBall go in Resources.FindObjectsOfTypeAll(typeof(enemyMagicBall)) as enemyMagicBall[])
                    {
                        go.damage = 20f;
                    }

                    foreach (spearFighter go in Resources.FindObjectsOfTypeAll(typeof(spearFighter)) as spearFighter[])
                    {
                        go.damage = 35f;
                    }
                    foreach (warrior go in Resources.FindObjectsOfTypeAll(typeof(warrior)) as warrior[])
                    {
                        go.damage = 50f;
                    }
                    break;

            }

            if (player.dying)
            {
                StopAllCoroutines();
            }

            if (wave == 1)
            {
                if (!wave1running)
                {
                    StartCoroutine(wave1());
                }
            }
        }

        //if (proceedWithGame)
        //{
        //    Debug.Log("'no more chat!' - said code");
        //    StopCoroutine(chat());
        //}
    }

    IEnumerator wave1()
    {
        wave1running = true;
        declare("Wave 1", 1f, 30f);
        replaceSpawner();
        //GameObject l = Instantiate(airWizard, spawner);
        //l.transform.parent = spawnerDeployedEnemies;
        //saySomething("Your magic is too weak!", 1f, 30f);
        //StartCoroutine(shutUpPlayer(3.5f));
        //yield return new WaitForSeconds(3.5f);

        summon(airWizard, "", false);
        yield return new WaitForSeconds(2f);

        summon(martialHerof, "I'll blast you to shreds!");
        yield return new WaitForSeconds(3.5f);

        summon(martialHerof, "You puny mortals!");
        yield return new WaitForSeconds(5f);

        summon(martialHerof, "Die!"); // wizard lookin kinda evil...
        yield return new WaitForSeconds(5f);

        summon(martialHerof, "I'll blast you to pieces!");
        summon(martialHerof, "I'll blast you to pieces!");
        yield return new WaitForSeconds(5f);

        summon(martialHerof, "I am the greatest wizard ever!"); // an egomaniac wizard...
        yield return new WaitForSeconds(5f);

        if (gameMode == 1)
        {
            summon(martialHerof, "I'll blast you to atoms!");
            summon(martialHerof);
            summon(airWizard, "", false);
        }

        if (gameMode == 2)
        {
            summon(martialHerof, "I'll blast you to atoms!");
            summon(martialHerof);
            summon(martialHerof);
            summon(airWizard, "", false);
        }

        while (player.enemiesInRange) yield return null; Debug.Log(player.enemiesInRange);

        StartCoroutine(wave2());
    }

    IEnumerator wave2()
    {
        declare("Wave 2");
        summon(airWizard, "Weak Wizards! Haha!", false);
        yield return new WaitForSeconds(5f);

        summon(martialHerof, "I'll blast you all to atoms!");
        
        yield return new WaitForSeconds(5f);

        summon(airWizard, "Your magic is no match against me!", false);
        yield return new WaitForSeconds(5f);

        summon(spearFighter, "Die!");
        yield return new WaitForSeconds(5f);

        summon(martialHerof, ""); // damn that martial hero must have a deadly look on his face
        yield return new WaitForSeconds(5f);

        summon(martialHerof, "I'll blast you to shreds!");
        yield return new WaitForSeconds(5f);

        if (gameMode == 0)
        {
            summon(airWizard, "", false);
        }

        if (gameMode == 1)
        {
            summon(martialHerof, "I'll blast you to shreds!");
            summon(martialHerof, "I'll blast you to shreds!");
            summon(martialHerof, "I'll blast you to shreds!");
        }

        if (gameMode == 2)
        {
            summon(martialHerof, "I'll blast you to shreds!");
            summon(martialHerof, "I'll blast you to shreds!");
            summon(airWizard, "", false);
        }


        while (player.enemiesInRange) yield return null; Debug.Log(player.enemiesInRange);

        if (player.health != 0)
        {
            rechargeMusic.Play();
            player.health += 40;
            player.shield += 30;
        }

        StartCoroutine(wave3());

    }

    IEnumerator wave3()
    {
        // bruh this is some extreme level of confidence xD

        declare("Wave 3");
        summon(spearFighter, "Get lost you muffin!");
        yield return new WaitForSeconds(5f);

        summon(airWizard, "No one can outmatch my wizardry skills!", false);
        yield return new WaitForSeconds(5f);

        summon(spearFighter, "Die!!!!");
        yield return new WaitForSeconds(5f);

        summon(spearFighter, "");
        yield return new WaitForSeconds(5f);

        summon(airWizard, "I am Immortal!", false);
        summon(airWizard, "I am Immortal!", false);
        yield return new WaitForSeconds(5f);

        summon(spearFighter, "I was the best student at Hogwarts!");
        yield return new WaitForSeconds(5f);

        summon(airWizard, "I'll blast you muffins to pieces!", false);
        yield return new WaitForSeconds(10f);

        if (gameMode == 1)
        {
            summon(spearFighter, "I was the best student at Hogwarts!");
            

        }

        if (gameMode == 2)
        {
            summon(spearFighter, "I was the best student at Hogwarts!");
            summon(spearFighter, "I was the best student at Hogwarts!");
            summon(airWizard, "", false);
        }

            while (player.enemiesInRange) yield return null; Debug.Log(player.enemiesInRange);

        if (player.health != 0)
        {
            rechargeMusic.Play();
            player.health += 50;
            player.shield += 40;
        }

        StartCoroutine(wave4());
        }

        IEnumerator wave4()
        {
            declare("Wave 4");
            summon(martialHerof, "I'll destroy you all!!!");
            summon(martialHerof);
            yield return new WaitForSeconds(2f);
        summon(airWizard, "", false);
        if (gameMode == 1)
        {
            summon(airWizard, "Your magic is too weak for me!", false);
        }

        if (gameMode == 2)
        {
            summon(airWizard, "I defeated Voldemort in the battle of Hogwarts!", false);
            summon(spearFighter);
        }

            yield return new WaitForSeconds(10f);
            summon(martialHerof);
            summon(martialHerof);
            summon(martialHerof, "Muffin heads!");
            yield return new WaitForSeconds(10f);

            summon(warrior, "Die!!!!!!!!");
        summon(warrior, "Die!!!!!!!!");
        if (gameMode == 1)
        {
            summon(martialHerof);
            summon(martialHerof);
        }

        if (gameMode == 2)
        {
            summon(warrior, "Come on!");
        }

            yield return new WaitForSeconds(5f);

            while (player.enemiesInRange) yield return null; Debug.Log(player.enemiesInRange);
        backGroundMusic.Stop();
        victoryMusic.Play();
            declare("Victory");
            saySomething("It was a piece of cake! I said I was the greatest wizard of all time HAHAHAHAHA!"); 
        yield return new WaitForSeconds(6f);
        SceneManager.LoadSceneAsync("SampleScene");
        }

        void replaceSpawner(bool ground = false)
        {
            Vector2 randomPos = new Vector3(0, 0, 0);
            if (!ground)
            {
                randomPos = new Vector2(Random.Range(corner3.transform.position.x, corner4.transform.position.x), Random.Range(corner1.transform.position.y, corner3.transform.position.y));
                spawner.position = randomPos;
            }
            else if (ground)
            {
                int randomNum = Random.Range(1, 3);
                if (randomNum == 1)
                {
                    randomPos = groundCorner.position;
                }
                else if (randomNum == 2)
                {
                    randomPos = groundCorner2.position;
                }
                else
                {
                    randomPos = groundCorner2.position;
                }
                //randomPos = new Vector2(Random.Range(corner3.transform.position.x, corner4.transform.position.x), Random.Range(corner3.transform.position.y, corner4.transform.position.y));
                spawner.position = randomPos;
            }
            //Debug.Log($"YOOOOOO wassup? spawner got replaced! New Position of spawner: {randomPos}");
        }

        public void saySomething(string whatToSay, float waitingTime = 1f, float sayingSpeed = 30f)
        {
            typeWriter.Run(whatToSay, playerThoughts, waitingTime, sayingSpeed);
        }

        public void declare(string whatToSay, float waitingTime = 1f, float sayingSpeed = 30f)
        {
            typeWriter.Run(whatToSay, mainText, waitingTime, sayingSpeed);
        }

        IEnumerator shutUpPlayer(float time)
        {
            yield return new WaitForSeconds(time);
            playerThoughts.text = string.Empty;
        }

        void summon(GameObject i, string comment = "", bool ground = true, bool dropPowerUp = false)
        {
            replaceSpawner(ground);
            GameObject n = Instantiate(i, spawner);
            if (dropPowerUp)
            {

            }
            n.transform.parent = spawnerDeployedEnemies;
            saySomething(comment);
            StartCoroutine(shutUpPlayer(3.5f));
        }

        void OnPlayClicked()
        {
            gameMode = dropD.value;
            proceedWithGame = true;
            startScreenCanvas.gameObject.SetActive(false);
            playerCanvas.gameObject.SetActive(true);
        StartCoroutine(authenticationApplication());
        }

    IEnumerator authenticationApplication()
    {
        yield return new WaitForSeconds(0.01f);
        authenticationToUseShield = true;
    }

    IEnumerator chat()
    {
        if (!proceedWithGame)
        {
            chatting = true;
            saySomething("Yooo there buddy wassup?");
            yield return new WaitForSeconds(3f);
            saySomething("You can choose the difficulty from the Drop Down above or from xbox buttons by switching to one mode");
            yield return new WaitForSeconds(5f);
            saySomething("I'm the greatest wizard so I'll win all of them haha!");
            yield return new WaitForSeconds(5f);
            saySomething("I was the best student at Hogwarts school of Wizardry!");
            yield return new WaitForSeconds(6f);
            saySomething("i am known for my awesome shielding powers.");
            yield return new WaitForSeconds(5f);
            saySomething("When I shield myself, no one can touch me.");
            yield return new WaitForSeconds(5f);
            saySomething("I can't rotate my hand in 360 degrees so i bewitched my wand to rotate according to my will.");
            yield return new WaitForSeconds(5f);
            saySomething("Can any wizard even do that? HAHA!");
            yield return new WaitForSeconds(5f);
            saySomething("Bahhhh! No time to listen to my talk!");
            yield return new WaitForSeconds(5f);
            saySomething("I want to make some monster noodles right now!");
            yield return new WaitForSeconds(5f);
            saySomething("Still waiting? Go Start the game dummy!");
            yield return new WaitForSeconds(5f);
            chatting = false;
        }
        //yield return new WaitForSeconds(1f);
    }
}

