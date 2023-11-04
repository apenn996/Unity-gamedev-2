using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SoldierBehaviorMelee : MonoBehaviour
{
    // Start is called before the first frame update
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
            if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 1) && (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) < 1))
        {
          //  Debug.Log("walking melee" + player.transform.position.x);
            //gameObject.GetComponent<AudioSource>().Play();
            anim.SetBool("Punching", true);
            anim.SetFloat("speed", -1f);

                if (!isShooting)
                    StartCoroutine("iAmFiring");
              //  g.healthText.text = GameManager.playerHealth + " / 100";
                //  gameObject.GetComponent<AudioSource>().Play();

                
                gameObject.GetComponent<NavMeshAgent>().speed = 0;
        }
        if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) > 4) || (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) > 4))
        {
            gameObject.GetComponent<AudioSource>().Stop();
            if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) > 8) || (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) > 8))
            {
                anim.SetBool("Punching", false);
              //  Debug.Log("running");
                anim.SetFloat("speed", 4.1f);
                gameObject.GetComponent<NavMeshAgent>().speed = 4;


            }
            else
            {
            //    Debug.Log("running");
                anim.SetBool("Punching", false);
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
        GameManager.enemiesRemaining--;
        yield return new WaitForSeconds(40);
       
        Destroy(gameObject);
    }

    IEnumerator iAmFiring()
    {
        isShooting = true;
        while ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 1) && (Mathf.Abs(gameObject.transform.position.z - player.transform.position.z) < 1) && !isDead)
        {

            yield return new WaitForSeconds(1);
            

                Debug.Log("pubnched");
              //  gameObject.GetComponent<AudioSource>().Play();
                GameManager.playerHealth -= 5;

            

        }
        isShooting = false;
    }
}
