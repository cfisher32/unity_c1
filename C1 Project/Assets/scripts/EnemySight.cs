using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 playerLastSighting;

	private NavMesh nav; //not needed for tower
	private SphereCollider col;
	public Vector3 lastPlayerSighting; //not tower LastPlayerSighting
	private GameObject player;
	private Animator playerAnim;
	//private PlayerHearth playerHealth;
	//private HashIDs hash;
	private Vector3 previousSighting;


	// Use this for initialization
	void Awake () {
		//nav
		col = GetComponentInChildren<SphereCollider>();
		//lastPlayerSighting
		player = GameObject.FindGameObjectWithTag("Player");
		//playerAnim
		//playerHealth
		//hash
		//personalLastSighting
		//previousSighting
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)
			playerInSight = false;
	}
	*/

	//triggers
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collision detected with trigger object " + other.name);
	}

	void OnTriggerStay(Collider other)
	{
		//Debug.Log("Still colliding with trigger object " + other.name);

		if (other.gameObject == player)
		{
			playerInSight = false;

			//get player angle to tower
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			//check angle
			if (angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;

				//NOTES: transform.up used to center on enemy for casting out ray
				if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
				{
					if (hit.collider.gameObject == player) //raycast hit player
					{
						playerInSight = true;
						lastPlayerSighting = player.transform.position;
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log(gameObject.name + " and trigger object " + other.name + " are no longer colliding");

		if (other.gameObject == player)
			playerInSight = false;
	}

	// float CalculatePathLenght
}
