using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator anim;
   public float health = 100f;
    private Vector3 rost;
    private Vector3 post;
    private bool isDead = false;
    private bool isShooting = false;
     

    // Start is called before the first frame update
    void Start()
    {
        //GameObject g = GameObject.FindGameObjectWithTag("GameManager");
        //GameManager gm = g.GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.rotation.eulerAngles
        if (health <= 0 && !isDead)
        {
        //    Debug.Log("I died");
            StartCoroutine("iHaveDied");
        }
        Vector3 dir = player.transform.position - transform.position;
       dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 6 * Time.deltaTime);
        if (isDead == false)
        {
            if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 20) && (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) < 20))
            {
                 
                //gameObject.GetComponent<AudioSource>().Play();
                anim.SetBool("firing", true);
                anim.SetFloat("speed", -1f);

                if(!isShooting)
                StartCoroutine("iAmFiring");

                gameObject.GetComponent<NavMeshAgent>().speed = 0;
            }
            if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) > 30) || (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) > 30))
            {
                //  gameObject.GetComponent<AudioSource>().Stop();
                if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) > 40) || (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) > 40))
                {
                    anim.SetBool("firing", false);
                    //  Debug.Log("running");
                    anim.SetFloat("speed", 4.1f);
                    gameObject.GetComponent<NavMeshAgent>().speed = 4;


                }
                else
                {
                    //  Debug.Log("running");
                    anim.SetBool("firing", false);
                    anim.SetFloat("speed", 3f);
                    gameObject.GetComponent<NavMeshAgent>().speed = 3;
                }


            }
        }


        if (isDead)
        {
            gameObject.transform.position = post;
            gameObject.transform.eulerAngles = rost;
        }

    }

    IEnumerator iHaveDied()
    {
        //anim.SetBool("dead", true);
       // Debug.Log("health should be 0");
        anim.SetFloat("health", -1);
        rost = gameObject.transform.eulerAngles;
        post = gameObject.transform.position;
        isDead = true;
        // anim = null;
        GameManager.enemiesRemaining-=1;
        yield return new WaitForSeconds(40);
        
        Destroy(gameObject);
    }

    IEnumerator iAmFiring()
    {
          
        isShooting = true;
        while ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 20) && (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) < 20) && !isDead)
        {

            yield return new WaitForSeconds(2);
            int rand = Random.Range(1, 10);
            Debug.Log(rand + " rand");
            if (rand < 7)
            {
                
                Debug.Log("enemy walking  " + gameObject.transform.position.x + ",  player walking " + player.transform.position.x);
                gameObject.GetComponent<AudioSource>().Play();
                if(GameManager.playerArmor > 0)
                {
                    GameManager.playerArmor -= 10;
                    if (GameManager.playerArmor < 0)
                        GameManager.playerArmor = 0;
                }else
                GameManager.playerHealth -= 10;
                
               
            }
            
        }
        isShooting = false;
    }
}
