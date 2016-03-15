using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

	public float maxDamage = 120f;
	public float minDamage = 45f;

	public AudioClip shotClip;
	public float flashIntensity = 3f;
	public float fadeSpeed = 10f;

	private Animator anim;
	//hashid
	private LineRenderer laserShotLine;
	private Light laserShotLight;
	private SphereCollider col;
	private Transform player;
	//player health
	private bool shooting;
	private float scaledDamage;

	void Awake()
	{
		//anim
		laserShotLine = GetComponentInChildren<LineRenderer>();
		laserShotLight = laserShotLine.GetComponent<Light>();
		col = GetComponent<SphereCollider>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		//playerhealth
		//hash

		laserShotLine.enabled = false;
		laserShotLight.intensity = 0f;

		//scaledDamage
	}

	void Update()
	{
		//check for firing solution
		
	}

	public void Shoot()
	{
		if (!shooting)
		{ 
			shooting = true;

			float fractionalDistance = (col.radius - Vector3.Distance(transform.position, player.position)) / col.radius;
			// get damage
			//player.takedamage
			FiringEffects();
			shooting = false;
		}
	}

	void FiringEffects()
	{
		laserShotLine.SetPosition(0, laserShotLine.transform.position); //start pos
		laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f); //end pos
		laserShotLine.enabled = true;
		//light intensity
		//audio clip
	}
}
