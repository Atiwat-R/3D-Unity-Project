using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public float HP;

    public Transform boom_effect;

    public Transform hit_effect;
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
         if (collision.collider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Building"))
         {
            Debug.Log(collision.collider.gameObject.tag);
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
            HP -= 10f;
         }
     }
}
