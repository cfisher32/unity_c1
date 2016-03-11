using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health = 3;
	public bool isDead = false;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Damage(int damage)
	{
		if(!isDead)
		{
			health -= damage;

			if(health <= 0)
			{
				//die
				Die();
			}
			Debug.Log("health is now: " + health.ToString());
		}
	}


	void Die()
	{
		Debug.Log("tower dead.");
		isDead = true;
		GameObject bullet = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	void Respawn()
	{
		Debug.Log("tower respawn.");
		//create new one or repair this one?
	}
}
