using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class TestAIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }
}
