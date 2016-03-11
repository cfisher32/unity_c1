using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	public float speed = 10.0f;
	public GameObject bulletPrefab;
	public int bulletDamage = 1;
	public float shotInterval = 0.5f;

	private float lastTimeShot;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.W))
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		else if(Input.GetKey(KeyCode.S))
		{
			transform.Translate(-Vector3.forward * speed * Time.deltaTime);
		}	
		if(Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}	
		else if(Input.GetKey(KeyCode.A))
		{
			transform.Translate(-Vector3.right * speed * Time.deltaTime);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Fire();
		}

	/*
	//click to moves
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				NavMeshAgent navMesh = transform.GetComponent<NavMeshAgent>();
				navMesh.SetDestination(hit.point);
			}
		}
	*/

		
	}

	void Fire()
	{
		if(Time.time > lastTimeShot + shotInterval)
		{
			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
			bullet.GetComponent<Bullet>().damage = bulletDamage;
		}
	}
}
