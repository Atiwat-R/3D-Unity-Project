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
	public Transform hit_effect;

    public GameObject CanonPoint;

	public LayerMask ShotLayer;

	private State currentState;
	public float RotationSpeed;

    public int VisionRange;

	private ScoreManager scoreManager;
	private float maxHealth = 30f;
	private float HP;
	private float damageFromPlayer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag("Player");
		this.currentState = State.Following;

		this.scoreManager = FindObjectOfType<ScoreManager>();
		this.HP = maxHealth;

		this.InvokeRepeating(nameof(Behave), Random.Range(0f, 2f), 1f);
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

			Vector3 verticalAim = player.transform.position - this.CanonPoint.transform.position;
			verticalAim.Normalize();
			verticalAim = this.CanonPoint.transform.InverseTransformDirection(verticalAim);
			this.CanonPoint.transform.localRotation = Quaternion.RotateTowards(this.CanonPoint.transform.localRotation, Quaternion.LookRotation(verticalAim), Time.deltaTime * this.RotationSpeed);
		}
		//Behave();
    }

	protected void Behave()
	{
		if (this.currentState == State.Aiming && this.CanShootTarget())
		{
			// Fire if aim is valid.
			this.currentState = State.Shooting;
			Shoot();
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

	// private void OnEnable()
	// {
	// 	this.InvokeRepeating(nameof(Behave), Random.Range(0f, 2f), 3f);
	// }

    protected bool CanSeeTarget()
	{
		if (Vector3.Distance(this.transform.position, player.transform.position) > this.VisionRange)
		{
			return false;
		}

		RaycastHit hit;
		if (Physics.Linecast(this.CanonPoint.transform.position, player.transform.position, out hit, this.ShotLayer))
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
		Instantiate(this.ProjectilePrefab, this.CanonPoint.transform.position, this.CanonPoint.transform.rotation);
	}

	// When Tank is destroyed
	private void tankDeath() {
		Instantiate(boom_effect, transform.position, boom_effect.rotation); // Special effects
		if (this.agent.enabled)
		{
			this.agent.isStopped = true;
			this.agent.enabled = false;
			this.CancelInvoke(nameof(Behave));
		}
		Destroy(gameObject);
	}

	// When Tank collides with something
    protected void OnCollisionEnter(Collision collision)
	{
		// Collision with Player
		if (collision.collider.tag == "Player" || collision.collider.tag == "Fist") 
		{
			Instantiate(hit_effect, transform.position, hit_effect.rotation);
			this.HP -= damageFromPlayer; // Take damage

			// Check if Dead
            if (this.HP <= 0) { 
                Debug.Log("DEAD!!!!!!!!!!!!!!!!!!!!!!!!!");
				this.tankDeath();
            }
		}
		// Collision with Buildings (Instakill)
		else if (collision.collider.tag == "Building" && collision.rigidbody && !collision.rigidbody.isKinematic)
		{
            this.tankDeath();
			this.scoreManager.AddScore(this.maxHealth);
		}
	}


    private enum State
	{
		Following,
		Aiming,
		Shooting,
	}
}
