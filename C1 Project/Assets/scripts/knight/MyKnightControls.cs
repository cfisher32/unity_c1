using UnityEngine;
using System.Collections;

public class MyKnightControls : MonoBehaviour {

	public float speed = 10.0f;
	public float turnSpeed = 200.0f;
	public GameObject bulletPrefab;
	public int bulletDamage = 1;
	public float shotInterval = 0.5f;

	private float lastTimeShot;
	NavMeshAgent agent;

	public bool shouldMove = false;
	Vector2 velocity = Vector2.zero;

	public Vector3 curPos;
	public Vector3 newPos;

	//new stuff

	public float animSpeed = 1.5f;

	private Animator anim;
	public AnimatorStateInfo currentBaseState;         // a reference to the current state of the animator, used for base layer
	private CapsuleCollider col;                    // a reference to the capsule collider of the character

	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");          // these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");                // and are used to check state for various actions to occur
	static int attackState = Animator.StringToHash("Base Layer.Attack1");

	void Start () {
		anim = transform.GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		//agent = GetComponent<NavMeshAgent>();
	}


	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");                // setup h variable as our horizontal input axis
		float v = Input.GetAxis("Vertical");

		anim.SetFloat("speed", v);
		anim.SetFloat("direction", h);

		anim.speed = animSpeed;

		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

		// STANDARD JUMPING

		// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
		if (currentBaseState.nameHash == locoState)
		{
			if (Input.GetButtonDown("Jump"))
			{
				anim.SetBool("jump", true);
			}
		}
		// if we are in the jumping state... 
		else if (currentBaseState.nameHash == jumpState)
		{
			//  ..and not still in transition..
			if (!anim.IsInTransition(0))
			{
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("jump", false);
			}
		}

		if (currentBaseState.nameHash == idleState || currentBaseState.nameHash == locoState )
		{
			if (Input.GetButtonDown("Fire1"))
			{
				anim.SetBool("attack", true);
			}
		}
		else if (currentBaseState.nameHash == attackState)
		{
			//  ..and not still in transition..
			if (!anim.IsInTransition(0))
			{
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("attack", false);
			}
		}
	}

	void Fire()
	{
		if (Time.time >= lastTimeShot + shotInterval)
		{
			//myAnimator.SetTrigger("attack");

			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
			bullet.GetComponent<Bullet>().damage = bulletDamage;
			bullet.GetComponent<Bullet>().owner = gameObject;

			lastTimeShot = Time.time;
		}
	}
}
