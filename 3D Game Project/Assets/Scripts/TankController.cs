using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{

    public GameObject player;

    protected NavMeshAgent agent;

    public GameObject ProjectilePrefab;
    public Transform boom_effect;

    public GameObject CanonPoint;

    public int VisionRange;
    // Start is called before the first frame update
    void Start()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool CanSeeTarget()
	{
		if (Vector3.Distance(this.transform.position, player.transform.position) > this.VisionRange)
		{
			return false;
		}

		RaycastHit hit;
		if (Physics.Linecast(this.CanonPoint.transform.position, player.transform.position, out hit))
		{
			return hit.collider.tag == "Player";
		}
		else
		{
			return false;
		}
	}

    public virtual void Shoot()
	{
		GameObject projectile = GameObject.Instantiate(this.ProjectilePrefab, this.CanonPoint.transform.position, this.CanonPoint.transform.rotation, this.transform.parent);
		GameObject.Destroy(projectile, 10f);
	}

    protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player"
		 || collision.collider.tag == "Building" && collision.rigidbody && !collision.rigidbody.isKinematic)
		{
            Instantiate(boom_effect, transform.position, boom_effect.rotation);
			GameObject.Destroy(this);
		}
	}

    private enum State
	{
		Following,
		Aiming,
		Shooting,
	}
}
