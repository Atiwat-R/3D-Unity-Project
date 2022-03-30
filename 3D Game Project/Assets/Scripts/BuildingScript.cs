using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public float HP;

    public Transform boom_effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Instantiate(boom_effect, transform.position, boom_effect.rotation);
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
     {
         if (collision.collider.gameObject.CompareTag("Player"))
         {
            HP -= 10f;
         }
     }
}
