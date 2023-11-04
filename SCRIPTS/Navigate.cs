using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Navigate : MonoBehaviour
{
    [SerializeField] private Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameManager");
        GameManager gm = g.GetComponent<GameManager>();

        target = gm.Player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
      
        agent.SetDestination(target.position);
    }
}
