using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    public float HP;

    public Transform boom_effect;

    public Transform hit_effect;

    private ScoreManager scoreManager;

    private SoundEffectManager soundEffectManager;

    private float buildingScore;

    // Start is called before the first frame update
    void Start()
    {
        buildingScore = HP;
        scoreManager = FindObjectOfType<ScoreManager>();
        soundEffectManager = FindObjectOfType<SoundEffectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If Dead
        if (HP <= 0)
        {
            soundEffectManager.PlayBoomSF();
            Instantiate(boom_effect, transform.position, boom_effect.rotation);
            Destroy(gameObject);
            scoreManager.AddScore(buildingScore);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
     {

         // Debug.Log(collision.collider.gameObject.tag);
        if (collision.collider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Building"))
         {
            soundEffectManager.PlayHitSF();
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
            HP -= 5f; // Initially 10f;
         }
        else if (collision.collider.gameObject.CompareTag("Fist"))
         {
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
            HP -= 15f;
         }
     }
}
