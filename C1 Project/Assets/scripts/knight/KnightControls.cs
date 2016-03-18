using UnityEngine;
using System.Collections;

public class KnightControls : MonoBehaviour {

	public float speed = 10.0f;
	public float turnSpeed = 200.0f;
	public GameObject bulletPrefab;
	public int bulletDamage = 1;
	public float shotInterval = 0.5f;

	private float lastTimeShot;
	Animator myAnimator;
	NavMeshAgent agent;

	public bool shouldMove = false;
	Vector2 velocity = Vector2.zero;

	public Vector3 curPos;
	public Vector3 newPos;

	void Start () {
		myAnimator = transform.GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//ControlsFPSNoAnim();  //remove MoveSimple script
		ControlsFPSAnim();
	}

	void Fire()
	{
		if (Time.time >= lastTimeShot + shotInterval)
		{
			myAnimator.SetTrigger("attack");

			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
			bullet.GetComponent<Bullet>().damage = bulletDamage;
			bullet.GetComponent<Bullet>().owner = gameObject;

			lastTimeShot = Time.time;
		}
	}

	void ControlsFPSNoAnim()
	{
		speed = 10;
		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(-Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			//transform.Translate(Vector3.right * speed * Time.deltaTime);
			transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			//transform.Translate(-Vector3.right * speed * Time.deltaTime);
			transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fire();
		}
	}


	void ControlsFPSAnim()
	{
		shouldMove = false;
		curPos = transform.position;

		if (Input.GetKey(KeyCode.W))
		{
			//transform.Translate(Vector3.forward * speed * Time.deltaTime);
			newPos = (Vector3.forward * speed * Time.deltaTime);
			agent.destination = transform.position;

			//velocity = Vector3.forward * speed * Time.deltaTime;
			shouldMove = true;


			myAnimator.SetBool("move", true);
			myAnimator.SetFloat("velx", newPos.z);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(-Vector3.forward * speed * Time.deltaTime);
			agent.destination = transform.position;

			//velocity = -Vector3.forward * speed * Time.deltaTime;
			//shouldMove = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			//transform.Translate(Vector3.right * speed * Time.deltaTime);
			transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
			agent.destination = transform.position;

			//velocity = Vector3.up * turnSpeed * Time.deltaTime;
			//shouldMove = true;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			//transform.Translate(-Vector3.right * speed * Time.deltaTime);
			transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
			agent.destination = transform.position;

			//velocity = -Vector3.up * turnSpeed * Time.deltaTime;
			//shouldMove = true;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fire();
		}

		//myAnimator.SetBool("move", shouldMove);
		//myAnimator.SetFloat("velx", newPos.x);
		//myAnimator.SetFloat("vely", newPos.y);
	}
}
