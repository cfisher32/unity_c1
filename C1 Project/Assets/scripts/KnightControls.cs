using UnityEngine;
using System.Collections;

public class KnightControls : MonoBehaviour {

	Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			myAnimator.SetTrigger("attack");
		}
	}
}
