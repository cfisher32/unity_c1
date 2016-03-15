using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float patrolSpeed = 2f;
	public float chaseSpeed = 5f;
	public float chaseWaitTime = 5f;
	public float patrolWaitTime = 1f;

	public Transform[] patrolWaypoints;

	private EnemySight enemySight;
	private NavMesh nav;
	private Transform player;
	//playerhealth
	//private LastPlayerSighting lastPlayerSighting;
	private float chaseTimer;
	private float patrolTimer;
	private int waypointIndex;

	public float speed = 10.0f;
	public GameObject bulletPrefab;
	public int bulletDamage = 1;
	public float shotInterval = 0.5f;

	public float strength = 0.5f;

	private float lastTimeShot;

	private EnemyShooting enemyShooting;

	void Awake () {
		enemySight = GetComponentInChildren<EnemySight>();
		//nav
		player = GameObject.FindGameObjectWithTag("Player").transform;
		//playerhealth
		//lastplayersighting
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (enemySight.playerInSight)
		{
			Shooting();
		}
		//else if(enemySight.lastPlayerSighting != lastPlayerSighting) //chasing
		else
			Patrolling();
	}
	/*
	void LateUpdate()
	{
		if (enemySight.playerInSight)
		{
			Vector3 targetDir = player.position - transform.position;
			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
			Debug.DrawRay(transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation(newDir);

			Shooting();
		}
	}
	*/

	void Shooting()
	{
		//stop moving/rotating
		//enemyShooting.Shoot();

		/*
		Vector3 targetDir = player.position - transform.position;
		float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
		*/


		Fire();
	}

	void Chasing()	{ }

	void Patrolling()
	{
		//nav.speed

		//spin tower
	}

	void Fire()
	{
		if (Time.time >= lastTimeShot + shotInterval)
		{
			Vector3 targetPosition = player.position;
			targetPosition.y = transform.position.y;
			transform.LookAt(targetPosition);

			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

			bullet.GetComponent<Bullet>().damage = bulletDamage;
			bullet.GetComponent<Bullet>().owner = gameObject;
			//bullet.transform.LookAt(targetPosition);

			lastTimeShot = Time.time;
		}
	}
}
