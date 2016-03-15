using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int damage;
	public float speed = 10f;
	public float lifeTime = 1f;
	public float bornTime;
	public GameObject owner;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		bornTime = Time.time;
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		//rb.AddForce(transform.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * (speed * Time.deltaTime));


		if(Time.time >= bornTime + lifeTime)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy" && col.gameObject != owner)
		{
			Debug.Log("collision on: " + col.name);
			col.gameObject.GetComponent<Enemy>().Damage(damage);
			Destroy(gameObject);
		}
		else if (col.gameObject.tag == "Player" && col.gameObject != owner)
		{
			Debug.Log("hitting player");
			Destroy(gameObject);
		}
	}
}
