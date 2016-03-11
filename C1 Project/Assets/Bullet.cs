using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int damage;
	public float speed = 10f;
	public float lifeTime = 1f;
	public float bornTime;

	// Use this for initialization
	void Start () {
		bornTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward * speed * Time.deltaTime);

		if(Time.time >= bornTime + lifeTime)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision col) 
		{
			if(col.gameObject.tag == "Enemy")
			{
				col.gameObject.GetComponent<Enemy>().Damage(damage);
				Destroy(gameObject);
			}
		}
}
