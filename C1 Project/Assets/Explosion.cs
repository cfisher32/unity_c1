using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public GameObject spawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Mouse0))
		{
			Respawn();
		}
	}

	void Respawn()
	{
		GameObject explosion = (GameObject)Instantiate(spawn, transform.position, transform.rotation);
		Destroy(gameObject);	
	}
}
