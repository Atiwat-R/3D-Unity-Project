using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed;
    public Transform hit_effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.forward * Time.deltaTime * Speed;
    }

    protected void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player"
         || collision.collider.tag == "Wall"
         || collision.collider.tag == "Enemy"
		 || collision.collider.tag == "Building" && collision.rigidbody && !collision.rigidbody.isKinematic
         || collision.collider.tag == "Road"
         || collision.collider.tag == "Ground")
		{
            Instantiate(hit_effect, transform.position, hit_effect.rotation);
			Destroy(gameObject);
		}
	}
}
