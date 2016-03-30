using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public int damage = 1;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			Debug.Log("collision on: " + col.name);
			col.gameObject.GetComponent<Enemy>().Damage(damage);
		}
	}
}
