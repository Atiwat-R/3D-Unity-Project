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

	public LayerMask ShotLayer;

	private State currentState;
	public float RotationSpeed;

    public int VisionRange;
    // Start is called before the first frame update
    void Start()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag("Player");
		this.currentState = State.Following;
    }

    // Update is called once per frame
    void Update()
    {
		if (this.currentState == State.Aiming)
		{
			Vector3 horizontalAim = player.transform.position - this.transform.position;
			horizontalAim.y = 0f;
			horizontalAim.Normalize();
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(horizontalAim, Vector3.up), Time.deltaTime * this.RotationSpeed);
		}
		Behave();
    }

	protected void Behave()
	{
		
		if (this.currentState == State.Aiming && this.CanShootTarget())
		{
			// Fire if aim is valid.
			this.currentState = State.Shooting;
		}
		else if (this.CanSeeTarget())
		{
			// Aim if target is in sight.
			this.currentState = State.Aiming;
			this.agent.isStopped = true;
		}
		else
		{
			// Follow if line of sight is broken.
			this.currentState = State.Following;
			this.agent.isStopped = false;
			this.agent.SetDestination(player.transform.position);
		}

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

	private bool CanShootTarget()
	{
		RaycastHit hit;
		if (Physics.Raycast(this.CanonPoint.transform.position, this.CanonPoint.transform.forward, out hit, 30f, this.ShotLayer))
		{
			return hit.collider.tag == "Player";
		}
		else
		{
			return false;
		}
	}

    public void Shoot()
	{
		GameObject projectile = GameObject.Instantiate(this.ProjectilePrefab, this.CanonPoint.transform.position, this.CanonPoint.transform.rotation, this.transform.parent);
		GameObject.Destroy(projectile, 10f);
	}

    protected void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Player"
		 || collision.collider.tag == "Building" && collision.rigidbody && !collision.rigidbody.isKinematic)
		{
            Instantiate(boom_effect, transform.position, boom_effect.rotation);
			if (this.agent.enabled)
			{
				this.agent.isStopped = true;
				this.agent.enabled = false;
			}
			Destroy(gameObject);
		}
	}

    private enum State
	{
		Following,
		Aiming,
		Shooting,
	}
}
