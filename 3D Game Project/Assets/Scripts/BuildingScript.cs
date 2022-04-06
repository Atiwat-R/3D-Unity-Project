using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    public float HP;

    public Transform boom_effect;

    public Transform hit_effect;

    private ScoreManager scoreManager;

    private float buildingScore;

    // Start is called before the first frame update
    void Start()
    {
        buildingScore = HP;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Instantiate(boom_effect, transform.position, boom_effect.rotation);
            Destroy(gameObject);
            scoreManager.AddScore(buildingScore);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
     {
        if (collision.collider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Building"))
         {
            Debug.Log(collision.collider.gameObject.tag);
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
            HP -= 5f; // Initially 10f;
         }
        else if (collision.collider.gameObject.CompareTag("Fist"))
         {
            Debug.Log(collision.collider.gameObject.tag);
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
            HP -= 15f;
         }
     }
}
