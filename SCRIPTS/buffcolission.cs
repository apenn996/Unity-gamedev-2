using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffcolission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager g = new GameManager();
       if (other.gameObject.tag == "Armor")
       {
            
        GameManager.playerArmor = 100;
            Destroy(other.gameObject);
       }
       if (other.gameObject.tag == "HealthPack" )
          {
              Debug.Log("healthpack");
              GameManager.playerHealth += 50;
            if(GameManager.playerHealth > 100)
            {
                GameManager.playerHealth = 100;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "infiniteAmmo")
        {
            Debug.Log("INFINITE AMMO");
            GameManager.isInfinite = true;
            Destroy(other.gameObject);
        }
          if (other.gameObject.tag == "Speed" )
          {
            GameManager.isFast = true;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().SprintSpeed.Equals(40);
            Destroy(other.gameObject);
          }
          
    }
}
