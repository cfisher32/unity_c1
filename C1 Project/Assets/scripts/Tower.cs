using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	public static void CanSeePlayer(Transform player, Transform target, float nearDistance, float fieldOfView)
	{
		Vector3 vecDiff = target.position - player.position;
		float dot = Vector3.Dot(player.forward.normalized, vecDiff.normalized);

		//if player is behind enemy
		if (dot < 0)
			return;

		//if player is not in viewing distance
		if (fieldOfView < (90f - dot * 90f))
			return;

		//enemy is facing player, check for line of sight
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			NavMeshAgent navMesh = transform.GetComponent<NavMeshAgent>();
			navMesh.SetDestination(hit.point);
		}
	}
	*/
}
