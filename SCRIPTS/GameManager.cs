using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform fpsCamera;
    private GameObject currentGun;
    [SerializeField] private GameObject currentGun1;
    [SerializeField] private GameObject currentGun2;
    [SerializeField] private GameObject currentGun3;
    [SerializeField] private float impactForce;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject IAP;
    [SerializeField] private GameObject SP;
    [SerializeField] private GameObject activePanel;
    [SerializeField] public Text healthText;
    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text enemiesText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text SprintText;
    [SerializeField] private Text infiniteAmmoText;
    [SerializeField] private RawImage currentGunIcon;
    [SerializeField] private Texture[] gunIcons;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject[] buffs;
    [SerializeField] private GameObject[] centerLevel2;
    [SerializeField] public GameObject Player;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    private bool[] levelSpawn;
    private int level;
    [SerializeField] private AudioSource audioController;
    public static int enemiesRemaining = 1;
    private float nextShotTime = 0;
    private bool reloading = false;
    private int currentAmmo = 32;
    public static float playerArmor=0;
    [SerializeField] Text armorText;
    private float Health;
    public static float playerHealth = 100;
    [SerializeField] private float currentWeapon = 0;
    public static bool isInfinite = false;
    public static bool isFast = false;
    private bool rn = false;
    private bool rn2 = false;
    private bool level1 = false;
    private bool level2 = false;
    private bool level3 = false;
    FirstPersonController fc;
    // Start is called before the first frame update
    void Start()
    {
       // Instantiate(enemies[0], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
        fc = GameObject.FindObjectOfType<FirstPersonController>();
        level = SceneManager.GetActiveScene().buildIndex;

        if (!fpsCamera)
        {
            fpsCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        Debug.Log(level);
       // StartCoroutine("enemySpawn");
       // StartCoroutine("buffsCall");
        currentGun1.SetActive(false);
        currentGun2.SetActive(false);
        currentGun3.SetActive(false);
    }

    public void changeHealthText( )
    {
       
        healthText.text = playerHealth + " / 100";
    }
    public void changeEnemyText()
    {

        enemiesText.text = "Enemies Remaining: " + enemiesRemaining;
    }
    public void changeAmmoText()
    {

        ammoText.text = currentAmmo + " / 32";
    }
    public void changeArmorText()
    {
        armorText.text = playerArmor + " / 100";
    }
    public void changeLevelText()
    {
        levelText.text = "Level: " + level;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (levelSpawn[level] == false)
        {
            enemiesRemaining = 10 * level;
            levelSpawn[level] = true;
            StartCoroutine("enemySpawn");
            


           
        }*/
        if(level != 0 && !level1 && level == 1)
        {
            level1 = true;
            Debug.Log("level1 in");
            StartCoroutine("enemySpawn");
            StartCoroutine("buffsCall");
        }
        Debug.Log(level +" "+ level2  );
        if (level > 1 && !level2  && level == 2)
        {
            level2 = true;
            Debug.Log("level2 inSSSSSSSSSSSSSSSSSS");
            StartCoroutine("buffsCall");
            StartCoroutine("enemySpawn");
            
        }
        if (level != 0 && !level3 &&  level == 3)
        {
            level3 = true;
            StartCoroutine("buffsCall");
            StartCoroutine("enemySpawn");
           
        }

        if (isInfinite && rn == false)
        {
            rn = true;
            //isInfinite = false;
            Debug.Log("infinite TRUEUEUEUE");
            StartCoroutine("infiniteAmmo");
        }
        if (isFast && rn2 == false)
        {
            rn2 = true;
            StartCoroutine("superSpeed");
        }
        if(currentAmmo <= 0 && !reloading)
        {
            reloading = true;
            StartCoroutine("iAmReloading");
        }
        if(Input.GetKey("r"))
        {
            reloading = true;
            StartCoroutine("iAmReloading");
        }
        if(enemiesRemaining <= 0)
        {
            StartCoroutine("nextLevel");
        }
        Debug.DrawRay(fpsCamera.position, fpsCamera.forward, Color.black);
       
            if (Input.GetButton("Fire1") && Time.time > nextShotTime && !reloading)
            {
                Shoot();
                nextShotTime = Time.time + 1 / currentGun.GetComponent<Gun>().rate;

            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (currentGun != null)
                    currentGun.SetActive(false);
                currentWeapon = (currentWeapon + 1) % 3;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (currentGun != null)
                    currentGun.SetActive(false);
                currentWeapon = (currentWeapon - 1) % 4;
            }


            if (currentWeapon == 0 || currentWeapon == -3)
            {
                currentGunIcon.texture = gunIcons[0];
                currentGun = currentGun1;
                currentGun.SetActive(true);


            }
            else if (currentWeapon == 1 || currentWeapon == -2)
            {
                currentGunIcon.texture = gunIcons[1];
                currentGun = currentGun2;
                currentGun.SetActive(true);

            }
            else
            {
                currentGunIcon.texture = gunIcons[2];
                currentGun = currentGun3;
                currentGun.SetActive(true);
            }

            if (Input.GetKey("p"))
            {
                //  Debug.Log("adsasdasd");
                PauseGame();
            }
            if (Input.GetKey("u"))
            {
                //  Debug.Log("adsasdasd");
                ResumeGame();
            }

        if (playerHealth <= 0)
        {
            StartCoroutine("lost");
        }
       // Instantiate(enemies[0], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
        changeHealthText();
        changeEnemyText();
        changeAmmoText();
        changeArmorText();
        changeLevelText();
    }

    

    IEnumerator buffsCall()
    {
        GameObject e = new GameObject();
        Debug.Log("IN BUFFS CALL");
        for (int i = 0; i<3000; i++){
            Buffs(e);
            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator lost()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene(0);
    }
    private void Quit()
    {
       

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        
    }
    private void Buffs(GameObject e)
    {
        Debug.Log("IN BUFFS");
       // e.transform.position = gameObject.transform.position;
        Vector3 nextBuffPos = new Vector3(Random.Range(-10, 10), -1, Random.Range((-10), 10)) + fpsCamera.position;
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                Debug.Log("INSTANT ARMOR");
                GameObject hp = Instantiate(buffs[0], nextBuffPos, Quaternion.identity);
              //  hp.transform.parent = e.transform;
                break;
            case 1:
                //nextBuffPos.y++;
                GameObject st = Instantiate(buffs[1], nextBuffPos, Quaternion.identity);
                //st.transform.parent = e.transform;

                break;
             case 2:
                 GameObject gs = Instantiate(buffs[2], nextBuffPos, Quaternion.Euler(270,0,0));
                // gs.transform.parent = e.transform;
                 break;
            case 3:
                //  Debug.Log("IN INV: " + rand);
                GameObject iStar = Instantiate(buffs[3], nextBuffPos, Quaternion.identity);
                //iStar.transform.parent = e.transform;

                break;
            default:
                break;


        }
        
    }

    public void PauseGame()
    {
        
        pausePanel.SetActive(true);
        Time.timeScale = 0;
     }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;    
    }
    IEnumerator nextLevel()
    {
        winPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
       
        level = SceneManager.GetActiveScene().buildIndex + 1;
        levelText.text = "Level: " + level;
        enemiesRemaining = level * 10;
        winPanel.SetActive(false);
        SceneManager.LoadScene((level) % 4 );

    }

    private void Shoot()
    {
        if (!isInfinite) { 
        currentAmmo -= 1;
        }
        Debug.Log("EEEEE");
        AudioSource s = currentGun.GetComponent<AudioSource>();
        //Vector3 rotateValue = new Vector3(camera.transform.position.x + 20, 0, 0);
        //camera.transform.eulerAngles = new Vector3(0, 0, 0) + rotateValue;
        LayerMask lm = LayerMask.GetMask("player");
        if (s != null)
            s.Play();
        if (Physics.Raycast(fpsCamera.position, fpsCamera.forward, out RaycastHit hitInfo, currentGun.GetComponent<Gun>().range, ~lm))
        {
            
            Debug.DrawRay(fpsCamera.position, fpsCamera.forward, Color.red, currentGun.GetComponent<Gun>().range);

            Debug.Log("hit sometyhing!!");

            // Rigidbody rb = hitInfo.rigidbody;
            if(hitInfo.transform.gameObject.tag == "Soldier")
            {
                hitInfo.transform.GetComponent<SoldierBehavior>().health -= 12;
            }
            if (hitInfo.transform.gameObject.tag == "Sniper")
            {
                hitInfo.transform.GetComponent<SoldierBehaviorSniper>().health -= 15;
            }
            if (hitInfo.transform.gameObject.tag == "melee")
            {
                hitInfo.transform.GetComponent<SoldierBehaviorMelee>().health -= 10;
            }

            // hitInfo.
            if (hitInfo.normal != null)
            {
            //    Debug.Log("before");
            }
            Debug.Log(hitInfo.collider.name + ": ,  time is: " + Time.time);
            
            
           
        }
    }

    public void Destroy(GameObject g)
    {
        Destroy(g);
    }


    IEnumerator enemySpawn()
    {
        Debug.Log("shouwld be spawning 111, level: " + level);
       // GameObject obs = Instantiate(enemies[2], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
      
        
            Debug.Log("shouwld be spawning 222, " + level);
           // GameObject s = Instantiate(enemies[1], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
            yield return new WaitForSeconds(1f);
            if (level == 1)
            {

                Debug.Log("shouwld be spawning 333, " + level);

                for (int i = 0; i < 6; i++)
                {
                    Instantiate(enemies[0], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
                yield return new WaitForSeconds(2f);
            }
                          
                Debug.Log("spawning 1");
                      
                    
                    
                        Debug.Log("spawning 2");
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(enemies[1], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
                yield return new WaitForSeconds(2f);
            }
                       
                   
               
                    
                        Debug.Log("spawning 3");
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(enemies[2], new Vector3(center.transform.position.x + Random.Range(-150, 0), 10, center.transform.position.z + Random.Range(-30, 300)), Quaternion.identity);
                yield return new WaitForSeconds(2f);
            }
                      
                    
                
            } else if (level == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    Debug.Log("spawning 1");
                Instantiate(enemies[0], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
                yield return new WaitForSeconds(2f);
                }
                for (int i = 0; i < 8; i++)
                {
                    Debug.Log("spawning 2");
                Instantiate(enemies[1], new Vector3(center.transform.position.x + Random.Range(-150, 150), 10, center.transform.position.z + Random.Range(-200, 300)), Quaternion.identity);
            }
                for (int i = 0; i < 6; i++)
                {
                    Debug.Log("spawning 3");
                Instantiate(enemies[2], new Vector3(center.transform.position.x + Random.Range(-150, 0), 10, center.transform.position.z + Random.Range(-30, 300)), Quaternion.identity);
                yield return new WaitForSeconds(2f);
                }
            }
            else if (level == 3)
            {
                for (int i = 0; i < 13; i++)
                {
                    Debug.Log("spawning 1");
                    GameObject obsM = Instantiate(enemies[0], centerLevel2[0].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(2f);
                }
                for (int i = 0; i < 13; i++)
                {
                    Debug.Log("spawning 2");
                    GameObject obsM = Instantiate(enemies[1], centerLevel2[1].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(2f);
                }
                for (int i = 0; i < 9; i++)
                {
                    Debug.Log("spawning 3");
                    GameObject obsM = Instantiate(enemies[2], centerLevel2[2].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(2f);
                }
            }
           


    }

    IEnumerator iAmReloading()
    {
        currentGun.GetComponent<Animator>().SetBool("isReloading", true);
        Debug.Log("reloading");
        audioController.clip = (Resources.Load<AudioClip>("HUFG5L6-machine-gun-remove-clip"));
        audioController.Play();
        yield return new WaitForSeconds(1.0f);
        currentGun.GetComponent<Animator>().SetBool("isReloading", false);
        audioController.clip = (Resources.Load<AudioClip>("NBUFTRV-gun-reloading-designed-16"));
        audioController.Play();
        yield return new WaitForSeconds(1.3f);

        audioController.clip = (Resources.Load<AudioClip>("JHPV25T-gun-reloading-designed-04"));
        audioController.Play();
        yield return new WaitForSeconds(2.2f);

        currentAmmo = 32;
        
        reloading = false;
    }

    IEnumerator infiniteAmmo()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        IAP.SetActive(true);
       // infiniteAmmoText.enabled = true;
        int timer = 20;
        while (timer > 0)
        {
            infiniteAmmoText.text = "Infinite Ammo Time Left: " + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }
        //infiniteAmmoText.enabled = false;
        rn = false;
        IAP.SetActive(false);
        isInfinite = false;

    }

    IEnumerator superSpeed()
    {
        SP.SetActive(true);
        // infiniteAmmoText.enabled = true;

        fc.SprintSpeed = 70;
        
            int timer = 10;
        while (timer > 0)
        {
            SprintText.text = "Super Sprint Time Left: " + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }
        //SuperSprint.enabled = true;
        rn2 = false;
        fc.SprintSpeed = 20;
        SP.SetActive(false);
        isFast = false;
    }

    public void playGame()
    {
        
      
        level = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("loading " + level);
        enemiesRemaining = level * 10;
        SceneManager.LoadScene((level)); //level%4 for 4 levels
    }
}
