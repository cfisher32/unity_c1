using UnityEngine;
using System.Collections;

public class WarriorAnimationDemo : MonoBehaviour {
	Animator animator;
	public GameObject target;
	public GameObject weaponModel;
	public GameObject secondaryWeaponModel;
	float rotationSpeed = 15f;
	Vector3 inputVec;
	Vector3 targetDirection;
	Vector3 targetDashDirection;
	bool dead = false;
	bool isMoving;
	bool isStrafing;
	bool isBlocking = false;
	bool isStunned = false;
	bool inBlock;
	bool blockGui;
	bool weaponSheathed;
	bool weaponSheathed2;
	bool isInAir;
	bool isStealth;
	bool isWall;
	bool ledgeGui;
	bool ledge;
	bool canChain;
	bool chain1;  //used to select which attack to chain to
	bool chain2;  //used to select which attack to chain to
	bool specialAttack2Bool;
	public enum Warrior{Karate, Ninja, Brute, Sorceress, Knight, Mage, Archer, TwoHanded, Swordsman, Spearman, Hammer, Crossbow};
	public Warrior warrior;

	void Start(){
		animator = this.GetComponent<Animator>();
		if(warrior == Warrior.Archer)
			secondaryWeaponModel.gameObject.SetActive(false);
		if(warrior == Warrior.Archer || warrior == Warrior.Crossbow){ //sets the weight on any additional layers to 0
			if(animator.layerCount >= 1){
				animator.SetLayerWeight(1, 0);
			}
		}
		if(warrior == Warrior.TwoHanded) //sets the weight on any additional layers to 0
			secondaryWeaponModel.GetComponent<Renderer>().enabled = false;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.P)){
			Debug.Break();
		}
		if(!dead && !blockGui){  //ifcharacter isn't dead, blocking, or stunned (or in a move)
			//Get input from controls
			float z = Input.GetAxisRaw("Horizontal");
			float x = -(Input.GetAxisRaw("Vertical"));
			inputVec = new Vector3(x, 0, z);
			//Apply inputs to animator
			animator.SetFloat("Input X", z);
			animator.SetFloat("Input Z", -(x));
			if(x > .1 || x < -.1 || z > .1 || z < -.1){  //ifthere is some input (account for controller deadzone)
				//set that character is moving
				animator.SetBool("Moving", true);
				isMoving = true;
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("TargetBlock") > .1){  //ifstrafing
					isStrafing = true;
					animator.SetBool("Running", false);
				}
				else{
					isStrafing = false;
					animator.SetBool("Running", true);
				}
			}
			else{
				//character is not moving
				animator.SetBool("Moving", false);
				animator.SetBool("Running", false);
				isMoving = false;
			}
			if(!weaponSheathed){
				if(!blockGui){
					if(Input.GetAxis("TargetBlock") < -.1){
						if(!inBlock){
							animator.SetBool("Block", true);
							isBlocking = true;
						}
					}
					if(Input.GetAxis("TargetBlock") == 0){
						inBlock = false;
						animator.SetBool("Block", false);
						isBlocking = false;
					}
				}
				if(!isBlocking){  //ifnot blocking
					if(Input.GetButtonDown("Jump")){
						if(!ledge){
							if(isStrafing){
								animator.SetTrigger("JumpTrigger");
								StartCoroutine(COSetInAir(.3f, .4f));
							}
							if(isMoving){
								animator.SetTrigger("JumpForwardTrigger");
								StartCoroutine(COSetInAir(.3f, .4f));
							}else{
						    	animator.SetTrigger("JumpTrigger");
								StartCoroutine(COSetInAir(.3f, .4f));
							}
						}
					}
					if(Input.GetButtonDown("Fire0")){
						animator.SetTrigger("RangeAttack1Trigger");
						if(warrior == Warrior.Brute)  //ifcharacter is Brute
							StartCoroutine (COStunPause(2.4f));
						else if(warrior == Warrior.Mage)  //ifcharacter is Mage
							StartCoroutine (COStunPause(1.5f));
						else if(warrior == Warrior.Ninja)  //ifcharacter is Ninja
							StartCoroutine (COStunPause(.9f));
						else if(warrior == Warrior.Archer){  //ifcharacter is Archer
							StartCoroutine (COSetLayerWeight(.6f));
							StartCoroutine (COArcherArrow(.2f));
						}
						else if(warrior == Warrior.Crossbow){  //ifcharacter is Archer
							StartCoroutine (COSetLayerWeight(.6f));
							StartCoroutine (COArcherArrow(.2f));
						}
						else if(warrior == Warrior.TwoHanded){  //ifcharacter is 2Handed
							StartCoroutine (COStunPause(2.1f));
							StartCoroutine(COSecondaryWeaponVisibility(.66f, true));
							StartCoroutine(COWeaponVisibility(.66f, false));
							StartCoroutine(COSecondaryWeaponVisibility(2f, false));
							StartCoroutine(COWeaponVisibility(2f, true));
						}
						else if(warrior == Warrior.Hammer)
							StartCoroutine (COStunPause(1.7f));
						else
							StartCoroutine (COStunPause(1.2f));
					}
					if(Input.GetButtonDown("Fire1")){
						if(!canChain){ //used for characters who can chain attacks to chain to 2nd Attack
							if(isInAir){  //for Knigh Air Smash Attack
								if(warrior == Warrior.Knight){
									animator.SetTrigger("JumpAttack1Trigger");
									StartCoroutine (COStunPause(2f));
								}
							} 
							else{  //ifcharater is not in air, do regular attack
								animator.SetTrigger("Attack1Trigger");
								if(warrior == Warrior.Knight){
									StartCoroutine(COChainWindow(.1f, .7f));
									StartCoroutine (COStunPause(.7f));
								}
								else if(warrior == Warrior.TwoHanded){
									StartCoroutine(COChainWindow(.6f, 1f));
									StartCoroutine (COStunPause(1f));
								}
								else if(warrior == Warrior.Brute || warrior == Warrior.Sorceress){
									StartCoroutine(COChainWindow(.6f, 1f));
									StartCoroutine (COStunPause(1.2f));
								}
								else if(warrior == Warrior.Swordsman){
									StartCoroutine(COChainWindow(.6f, 1.1f));
									StartCoroutine (COStunPause(1f));
								}
								else if(warrior == Warrior.Spearman){
									StartCoroutine(COChainWindow(.2f, .8f));
									StartCoroutine (COStunPause(1.1f));
								}
								else if(warrior == Warrior.Hammer){
									StartCoroutine(COChainWindow(.6f, 1.2f));
									StartCoroutine (COStunPause(1.4f));
								}
								else if(warrior == Warrior.Crossbow){
									StartCoroutine (COStunPause(.7f));
								}
								else if(warrior == Warrior.Mage){
									StartCoroutine(COChainWindow(.4f, 1.2f));
									StartCoroutine (COStunPause(1.1f));
								}
								else{
									StartCoroutine(COChainWindow(.2f, .4f));
									StartCoroutine (COStunPause(.6f));
								}
								chain1 = true;
								chain2 = false;
							}
						}
						else{
							if(chain1){  //ifwithin chain time do ATTACK2
								animator.SetTrigger("Attack2Trigger");
								StopAllCoroutines();
								if(warrior == Warrior.Knight){
									StartCoroutine(COChainWindow(.1f, 1.1f));
									StartCoroutine (COStunPause(.6f));
								}
								else if(warrior == Warrior.TwoHanded){
									StartCoroutine(COChainWindow(.9f, 1.5f));
									StartCoroutine (COStunPause(.9f));
								}
								else if(warrior == Warrior.Brute || warrior == Warrior.Sorceress){
									StartCoroutine(COChainWindow(.5f, 1.2f));
									StartCoroutine (COStunPause(1.2f));
								}
								else if(warrior == Warrior.Karate){
									StartCoroutine(COChainWindow(.3f, .6f));
									StartCoroutine (COStunPause(.9f));
								}
								else if(warrior == Warrior.Swordsman){
									StartCoroutine(COChainWindow(.6f, 1.1f));
									StartCoroutine (COStunPause(1.1f));
								}else if(warrior == Warrior.Spearman){
									StartCoroutine(COChainWindow(.6f, 1.1f));
									StartCoroutine (COStunPause(1.1f));
								}
								else if(warrior == Warrior.Hammer){
									StartCoroutine(COChainWindow(.6f, 1.2f));
									StartCoroutine (COStunPause(1.4f));
								}
								else if(warrior == Warrior.Mage){
									StartCoroutine(COChainWindow(.4f, 1.2f));
									StartCoroutine (COStunPause(1.3f));
								}
								else
									StartCoroutine(COChainWindow(.1f, 2f));
								chain1 = false;
								chain2 = true;
							}
							else if(chain2){
								StopAllCoroutines();
								animator.SetTrigger("Attack3Trigger");
								chain1 = false;
								chain2 = false;
								if(warrior == Warrior.Knight)
									StartCoroutine (COStunPause(.9f));
								if(warrior == Warrior.Swordsman)
									StartCoroutine (COStunPause(1.4f));
								else if(warrior == Warrior.Hammer)
									StartCoroutine (COStunPause(1.5f));
								else if(warrior == Warrior.Karate)
									StartCoroutine (COStunPause(.8f));
								else if(warrior == Warrior.Brute)
									StartCoroutine (COStunPause(1.4f));
								else if(warrior == Warrior.TwoHanded)
									StartCoroutine (COStunPause(1f));
								else if(warrior == Warrior.Mage)
									StartCoroutine (COStunPause(1f));
								else{
									StartCoroutine (COStunPause(1.2f));
								}
								canChain = false;
							}
						}
					}
					if(Input.GetButtonDown("Fire2")){
						animator.SetTrigger("MoveAttack1Trigger");
						if(warrior == Warrior.Brute)
							StartCoroutine (COStunPause(1.4f));
						else if(warrior == Warrior.Sorceress)//ifcharacter is Sorceress
							StartCoroutine (COStunPause(1.1f));
						else if(warrior == Warrior.Mage)  //ifcharacter is Mage
							StartCoroutine (COStunPause(1.4f));
						else if(warrior == Warrior.TwoHanded)  //ifcharacter is 2Handed
							StartCoroutine (COStunPause(1.2f));
						else if(warrior == Warrior.Hammer)
							StartCoroutine (COStunPause(2.4f));
						else
							StartCoroutine (COStunPause(.9f));
					}
					if(Input.GetButtonDown("Fire3")){
						animator.SetTrigger("SpecialAttack1Trigger");
						if(warrior == Warrior.Brute)
							StartCoroutine (COStunPause(2f));
						else if(warrior == Warrior.Sorceress)
							StartCoroutine (COStunPause(1.5f));
						else if(warrior == Warrior.Knight)
							StartCoroutine (COStunPause(1.1f));
						else if(warrior == Warrior.Mage)  //ifcharacter is Mage
							StartCoroutine (COStunPause(1.8f));
						else if(warrior == Warrior.TwoHanded)
							StartCoroutine (COStunPause(1.2f));
						else if(warrior == Warrior.Swordsman)
							StartCoroutine (COStunPause(1f));
						else if(warrior == Warrior.Spearman)
							StartCoroutine (COStunPause(.9f));
						else if(warrior == Warrior.Hammer)
							StartCoroutine (COStunPause(1.6f));
						else
							StartCoroutine (COStunPause(1.7f));
					}
					if(Input.GetButtonDown("LightHit")){
						animator.SetTrigger("LightHitTrigger");
						StartCoroutine (COStunPause(2.8f));
					}
				}
				else{
					if(Input.GetButtonDown("Jump"))
						animator.SetTrigger("BlockHitReactTrigger");
					if(Input.GetButtonDown("Fire0"))
						animator.SetTrigger("BlockHitReactTrigger");
					if(Input.GetButtonDown("Fire1"))
						animator.SetTrigger("BlockHitReactTrigger");
					if(Input.GetButtonDown("Fire2"))
						animator.SetTrigger("BlockHitReactTrigger");
					if(Input.GetButtonDown("Fire3"))
						animator.SetTrigger("BlockHitReactTrigger");
					if(Input.GetButtonDown("LightHit"))
						animator.SetTrigger("BlockHitReactTrigger");
				}
				if(Input.GetAxis("DashVertical") > .5 || Input.GetAxis("DashVertical") < -.5 || Input.GetAxis("DashHorizontal") > .5 || Input.GetAxis("DashHorizontal") < -.5){
					StartCoroutine (CODirectionalDash(Input.GetAxis("DashVertical"), Input.GetAxis("DashHorizontal")));
				}
			}
		}
		else
			inputVec = new Vector3(0,0,0);
		if(!dead){  //ifcharacter isn't dead
			if(!isBlocking){  //ifnot blocking
				if(Input.GetButtonDown("Death")){
					animator.SetTrigger("DeathTrigger");
					dead = true;
				}
			}
		}
		else{
			if(Input.GetButtonDown("Death")){
				animator.SetTrigger("ReviveTrigger");
				if(warrior == Warrior.Brute)
					StartCoroutine (COStunPause(1.7f));
				else if(warrior == Warrior.Knight)
					StartCoroutine (COStunPause(1.6f));
				else
					StartCoroutine (COStunPause(1f));
				dead = false;
			}
		}
		UpdateMovement();  //update character position and facing
		InAir();
		if(Input.GetButtonDown("Special")){
			Debug.Log("Special");
			if(warrior == Warrior.Ninja){
				if(!isStealth){
					isStealth = true;
					animator.SetBool("Stealth", true);
				} else {
					isStealth = false;
					animator.SetBool("Stealth", false);
				}
			}
		}
	}
	
	void UpdateMovement(){
		if(!dead && !isBlocking && !blockGui && !isStunned){
			Vector3 motion = inputVec;  //get movement input from controls
			//reduce input for diagonal movement
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
			if(!isStrafing  && !isWall)
				if(!ledgeGui || !ledge)
					RotateTowardMovementDirection();  //ifnot strafing, face character along input direction
			if(isStrafing){  //ifstrafing, look at the target
				//make character point at target
				Quaternion targetRotation;
				Vector3 targetPos = target.transform.position;
				targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x,0,transform.position.z));
				transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,(rotationSpeed * Time.deltaTime) * rotationSpeed);
			}
			GetCameraRelativeMovement();  
		}
		else
			inputVec = new Vector3(0,0,0);
	}

	void InAir(){
		if(isInAir){
			if(Input.GetButtonDown("Fire1")){
				if(warrior == Warrior.Knight)
					animator.SetTrigger("JumpAttack1Trigger");
			}
			if(ledgeGui){
				animator.SetTrigger("Ledge-Catch");
				ledge = true;
			}
		}
	}

	void GetCameraRelativeMovement(){  //converts control input vectors into camera facing vectors
		Transform cameraTransform = Camera.main.transform;
		// Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		//directional inputs
		float v= Input.GetAxisRaw("Vertical");
		float h= Input.GetAxisRaw("Horizontal");
		float dv= Input.GetAxisRaw("DashVertical");
		float dh= Input.GetAxisRaw("DashHorizontal");
		// Target direction relative to the camera
		targetDirection = h * right + v * forward;
		// Target dash direction relative to the camera
		targetDashDirection = dh * right + dv * -forward;
	}

	void RotateTowardMovementDirection(){  //face character along input direction
		if(!dead){  //ifcharacter isn't dead
			if(!blockGui && !isBlocking && !isStunned){  //ifcharacter isn't stunned or blocking
				if(inputVec != Vector3.zero && !isStrafing){  //ifwe're not strafing
					//take the camera orientated input vector and apply it to our characters facing with smoothing
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
				}
			}
		}
	}

	public IEnumerator CODirectionalDash(float x, float v){
		//check which way the dash is pressed relative to the character facing
		float angle = Vector3.Angle(targetDashDirection,-transform.forward);
		float sign = Mathf.Sign(Vector3.Dot(transform.up,Vector3.Cross(targetDashDirection,transform.forward)));
		// angle in [-179,180]
		float signed_angle = angle * sign;
		//angle in 0-360
		float angle360 = (signed_angle + 180) % 360;
		//deternime the animation to play based on the angle
		if( angle360 > 315 || angle360 < 45){
			animator.SetBool("DashForwardBool", true);
			yield return null;
			animator.SetBool("DashForwardBool", false);
		}
		if(angle360 > 45 && angle360 < 135){
			animator.SetBool("DashRightBool", true);
			yield return null;
			animator.SetBool("DashRightBool", false);
		}
		if(angle360 > 135 && angle360 < 225){
			animator.SetBool("DashBackwardBool", true);
			yield return null;
			animator.SetBool("DashBackwardBool", false);
		}
		if(angle360 > 225 && angle360 < 315){
			animator.SetBool("DashLeftBool", true);
			yield return null;
			animator.SetBool("DashLeftBool", false);
		}
		yield return null;
	}
	
	public IEnumerator CODash(string direction){
		animator.SetBool(direction, true);
		yield return null;
		animator.SetBool(direction, false);
	}

	public IEnumerator COSetInAir(float timeToStart, float lenthOfTime){
		yield return new WaitForSeconds(timeToStart);
		isInAir = true;
		yield return new WaitForSeconds(lenthOfTime);
		isInAir = false;
	}
	
	public IEnumerator COStunPause(float pauseTime){
		isStunned = true;
		animator.SetFloat("Input X", 0);
		animator.SetFloat("Input Z", 0);
		animator.SetBool("Moving", false);
		yield return new WaitForSeconds(pauseTime);
		isStunned = false;
	}

	public IEnumerator COWeaponVisibility(float waitTime, bool weaponVisiblity){
		yield return new WaitForSeconds(waitTime);
		weaponModel.SetActive(weaponVisiblity);
	}

	public IEnumerator COSecondaryWeaponVisibility(float waitTime, bool weaponVisiblity){
		yield return new WaitForSeconds(waitTime);
		secondaryWeaponModel.GetComponent<Renderer>().enabled = weaponVisiblity;
	}
	
	public IEnumerator COArcherArrow(float waitTime){
		secondaryWeaponModel.gameObject.SetActive(true);
		yield return new WaitForSeconds(waitTime);
		secondaryWeaponModel.gameObject.SetActive(false);
	}

	public IEnumerator COSetLayerWeight(float time){
		animator.SetLayerWeight(1, 1);
		yield return new WaitForSeconds(time);
		float a = 1;
		for (int i = 0; i < 20; i++){
			a -= .05f;
			animator.SetLayerWeight(1, a);
			yield return new WaitForEndOfFrame();
		}
		animator.SetLayerWeight(1, 0);
	}

	public IEnumerator COChainWindow(float timeToWindow, float chainLength){
		yield return new WaitForSeconds(timeToWindow);
		animator.SetBool("CanChainBool", true);
		canChain = true;
		yield return new WaitForSeconds(chainLength);
		canChain = false;
		animator.SetBool("CanChainBool", false);
	}

	void OnGUI(){
		if(!dead){
			if(warrior == Warrior.Mage || warrior == Warrior.Ninja || warrior == Warrior.Knight || warrior == Warrior.Archer || warrior == Warrior.TwoHanded || warrior == Warrior.Swordsman || warrior == Warrior.Spearman || warrior == Warrior.Hammer || warrior == Warrior.Crossbow){
				if(!dead && weaponSheathed){
					if(GUI.Button (new Rect (30, 310, 100, 30), "Unsheath Weapon")){
						animator.SetTrigger("WeaponUnsheathTrigger");
						if(warrior == Warrior.Archer)
							StartCoroutine (COWeaponVisibility(.4f, true));
						else if(warrior == Warrior.TwoHanded)
							StartCoroutine (COWeaponVisibility(.35f, true));
						else if(warrior == Warrior.Swordsman){
							StartCoroutine (COWeaponVisibility(.35f, true));
							StartCoroutine (COSecondaryWeaponVisibility(.35f, true));
						}
						else if(warrior == Warrior.Spearman){
							StartCoroutine (COWeaponVisibility(.45f, true));
						}
						else
							StartCoroutine (COWeaponVisibility(.6f, true));
						weaponSheathed = false;
						isStunned = false;
					}
				}
			}
		}
		if(!dead && !weaponSheathed){  //ifcharacter isn't dead or weapon is sheathed
			if(!blockGui && !isBlocking){  //ifcharacter is not blocking
				if(GUI.Button (new Rect (25, 20, 100, 30), "Dash Forward")) 
					StartCoroutine(CODash("DashForwardBool"));
				if(GUI.Button (new Rect (135, 20, 100, 30), "Dash Right")) 
					StartCoroutine(CODash("DashRightBool"));
				if(!ledge){
					if(GUI.Button (new Rect (245, 20, 100, 30), "Jump")){
						if(isMoving){ //ifcharacter is moving
							if(!isStrafing){ //ifcharacter is running play Jump Forward anim
								animator.SetTrigger("JumpForwardTrigger");
								StartCoroutine(COSetInAir(.3f, .4f));
							}else{
								animator.SetTrigger("JumpTrigger");  //play regular jump anim
								StartCoroutine(COSetInAir(.3f, .4f));
							}
						}else{
							animator.SetTrigger("JumpTrigger");  //play regular jump anim
							StartCoroutine(COSetInAir(.3f, .4f));
						}
					}
				}
				if(warrior == Warrior.Ninja){
					ledgeGui = GUI.Toggle (new Rect (245, 60, 100, 30), ledgeGui, "Ledge Jump");
					if(ledge){
						if(GUI.Button (new Rect (245, 90, 100, 30), "Ledge Drop")){
							animator.SetTrigger("Ledge-Drop");
							ledge = false;
							animator.SetBool("Ledge-Catch", false);
						}
						if(GUI.Button (new Rect (245, 20, 100, 30), "Ledge Climb")){
							animator.SetTrigger("Ledge-Climb-Trigger");
							ledge = false;
							animator.SetBool("Ledge-Catch", false);
						}
					}
				}
				if(GUI.Button (new Rect (25, 50, 100, 30), "Dash Backward")) 
					StartCoroutine(CODash("DashBackwardBool"));
				if(GUI.Button (new Rect (135, 50, 100, 30), "Dash Left")) 
						StartCoroutine(CODash("DashLeftBool"));
				//2nd Dash/Roll animations for Knight
				if(warrior == Warrior.Knight){
					if(GUI.Button (new Rect (355, 20, 100, 30), "Roll Forward"))
						StartCoroutine(CODash("DashForward2Bool"));
					if(GUI.Button (new Rect (355, 50, 100, 30), "Roll Backward")) 
						StartCoroutine(CODash("DashBackward2Bool"));
					if(GUI.Button (new Rect (460, 20, 100, 30), "Roll Left")) 
						StartCoroutine(CODash("DashLeft2Bool"));
					if(GUI.Button (new Rect (460, 50, 100, 30), "Roll Right")) 
						StartCoroutine(CODash("DashRight2Bool"));
				}
				if(GUI.Button (new Rect (25, 85, 100, 30), "Attack Chain")){  //ATTACH CHAIN
					if(!canChain){ //used for characters who can chain attacks to chain to 2nd Attack
						if(isInAir){  //for Knight Air Smash Attack
							if(warrior == Warrior.Knight){
								animator.SetTrigger("JumpAttack1Trigger");
								StartCoroutine (COStunPause(2f));
							}
						} 
						else{  //ifcharater is not in air, do regular attack
							animator.SetTrigger("Attack1Trigger");
							if(warrior == Warrior.Knight){
								StartCoroutine(COChainWindow(.1f, .7f));
								StartCoroutine (COStunPause(.7f));
							}
							else if(warrior == Warrior.TwoHanded){
								StartCoroutine(COChainWindow(.6f, 1f));
								StartCoroutine (COStunPause(1f));
							}
							else if(warrior == Warrior.Brute || warrior == Warrior.Sorceress){
								StartCoroutine(COChainWindow(.6f, 1f));
								StartCoroutine (COStunPause(1.2f));
							}
							else if(warrior == Warrior.Swordsman){
								StartCoroutine(COChainWindow(.6f, 1.1f));
								StartCoroutine (COStunPause(1f));
							}
							else if(warrior == Warrior.Karate){
								StartCoroutine(COChainWindow(.2f, .6f));
								StartCoroutine (COStunPause(.8f));
							}
							else if(warrior == Warrior.Spearman){
								StartCoroutine(COChainWindow(.2f, .8f));
								StartCoroutine (COStunPause(.8f));
							}
							else if(warrior == Warrior.Hammer){
								StartCoroutine(COChainWindow(.6f, 1.2f));
								StartCoroutine (COStunPause(1.4f));
							}
							else if(warrior == Warrior.Mage){
								StartCoroutine(COChainWindow(.4f, 1.2f));
								StartCoroutine (COStunPause(1.1f));
							}
							else if(warrior == Warrior.Crossbow){
								StartCoroutine (COStunPause(.7f));
							}
							else{
								StartCoroutine(COChainWindow(.2f, .4f));
								StartCoroutine (COStunPause(.6f));
							}
							chain1 = true;
							chain2 = false;
						}
					}
					else{
						if(chain1){  //ifwithin chain time do ATTACK2
							animator.SetTrigger("Attack2Trigger");
							StopAllCoroutines();
							if(warrior == Warrior.Knight){
								StartCoroutine(COChainWindow(.1f, 1.1f));
								StartCoroutine (COStunPause(.6f));
							}
							else if(warrior == Warrior.TwoHanded){
								StartCoroutine(COChainWindow(.9f, 1.5f));
								StartCoroutine (COStunPause(.9f));
							}
							else if(warrior == Warrior.Brute || warrior == Warrior.Sorceress){
								StartCoroutine(COChainWindow(.5f, 1.2f));
								StartCoroutine (COStunPause(1.2f));
							}
							else if(warrior == Warrior.Karate){
								StartCoroutine(COChainWindow(.3f, .6f));
								StartCoroutine (COStunPause(.9f));
							}
							else if(warrior == Warrior.Swordsman){
								StartCoroutine(COChainWindow(.6f, 1.1f));
								StartCoroutine (COStunPause(1.1f));
							}else if(warrior == Warrior.Spearman){
								StartCoroutine(COChainWindow(.6f, 1.1f));
								StartCoroutine (COStunPause(1.1f));
							}
							else if(warrior == Warrior.Hammer){
								StartCoroutine(COChainWindow(.6f, 1.2f));
								StartCoroutine (COStunPause(1.4f));
							}
							else if(warrior == Warrior.Mage){
								StartCoroutine(COChainWindow(.4f, 1.2f));
								StartCoroutine (COStunPause(1.3f));
							}
							else
								StartCoroutine(COChainWindow(.1f, 2f));
							chain1 = false;
							chain2 = true;
						}
						else if(chain2){
							StopAllCoroutines();
							animator.SetTrigger("Attack3Trigger");
							chain1 = false;
							chain2 = false;
							if(warrior == Warrior.Knight)
								StartCoroutine (COStunPause(.9f));
							if(warrior == Warrior.Swordsman)
								StartCoroutine (COStunPause(1.4f));
							else if(warrior == Warrior.Hammer)
								StartCoroutine (COStunPause(1.5f));
							else if(warrior == Warrior.Karate)
								StartCoroutine (COStunPause(.8f));
							else if(warrior == Warrior.Brute)
								StartCoroutine (COStunPause(1.4f));
							else if(warrior == Warrior.TwoHanded)
								StartCoroutine (COStunPause(1f));
							else if(warrior == Warrior.Mage)
								StartCoroutine (COStunPause(1f));
							else{
								StartCoroutine (COStunPause(1.2f));
							}
							canChain = false;
						}
					}
				}
				if(warrior == Warrior.Crossbow){
					if(GUI.Button (new Rect (135, 85, 100, 30), "Reload")){
						StartCoroutine (COSetLayerWeight(1.2f));
						animator.SetTrigger("ReloadTrigger");
					}
				}
				if(warrior == Warrior.Ninja){
					if(GUI.Button (new Rect (135, 85, 100, 30), "Attack1_R")){
						animator.SetTrigger("Attack1RTrigger");
						StartCoroutine (COStunPause(.6f));
					}
					if(GUI.Button (new Rect (245, 85, 100, 30), "Attack2_R")){
						animator.SetTrigger("Attack2RTrigger");
						StartCoroutine (COStunPause(.7f));
					}
				}
			}
			blockGui = GUI.Toggle (new Rect (25, 215, 100, 30), blockGui, "Block");
			if(!blockGui)
				animator.SetBool("Block", false);
			else{
				animator.SetBool("Block", true);
				animator.SetFloat("Input X", 0);
				animator.SetFloat("Input Z", -0);
			}
			if(blockGui){
				if(GUI.Button (new Rect (30, 240, 100, 30), "BlockHitReact"))
					animator.SetTrigger("BlockHitReactTrigger");
			}
			else if(!inBlock){  //ifnot blocking
				if(!inBlock){
					if(GUI.Button (new Rect (30, 240, 100, 30), "Hit React")){
						animator.SetTrigger("LightHitTrigger");
						StartCoroutine (COStunPause(2.8f));
					}
				}
			}
			if(!blockGui && !isBlocking){  //if blocking
				if(GUI.Button (new Rect (25, 115, 100, 30), "RangeAttack1")){
					animator.SetTrigger("RangeAttack1Trigger");
					if(warrior == Warrior.Brute)  //if character is Brute
						StartCoroutine (COStunPause(2.4f));
					else if(warrior == Warrior.Mage)  //if character is Mage
						StartCoroutine (COStunPause(1.8f));
					else if(warrior == Warrior.Ninja)  //if character is Ninja
						StartCoroutine (COStunPause(1f));
					else if(warrior == Warrior.Swordsman)
						StartCoroutine (COStunPause(1f));
					else if(warrior == Warrior.Archer){  //if character is Archer
						StartCoroutine (COSetLayerWeight(.6f));
						StartCoroutine (COArcherArrow(.2f));
					}
					else if(warrior == Warrior.Crossbow){  //if character is Crossbow
						StartCoroutine (COSetLayerWeight(.6f));
					}
					else if(warrior == Warrior.TwoHanded){  //if character is 2Handed
						StartCoroutine (COStunPause(2.1f));
						StartCoroutine(COSecondaryWeaponVisibility(.66f, true));
						StartCoroutine(COWeaponVisibility(.66f, false));
						StartCoroutine(COSecondaryWeaponVisibility(2f, false));
						StartCoroutine(COWeaponVisibility(2f, true));
					}
					else if(warrior == Warrior.Hammer)
						StartCoroutine (COStunPause(1.7f));
					else
						StartCoroutine (COStunPause(1.2f));
				}
				if(warrior == Warrior.Ninja){
					if(GUI.Button(new Rect (135, 115, 100, 30), "RangeAttack2")){
						animator.SetTrigger("RangeAttack2Trigger");
						StartCoroutine (COStunPause(.7f));
					}
				}
				if(GUI.Button(new Rect (25, 145, 100, 30), "MoveAttack1")){
					animator.SetTrigger("MoveAttack1Trigger");
					if(warrior == Warrior.Brute)  //if character is Brute
						StartCoroutine (COStunPause(1.5f));
					else if(warrior == Warrior.Sorceress)  //if character is Sorceress
						StartCoroutine (COStunPause(1.1f));
					else if(warrior == Warrior.Mage)  //if character is Mage
						StartCoroutine (COStunPause(1.4f));
					else if(warrior == Warrior.TwoHanded)  //if character is 2Handed
						StartCoroutine (COStunPause(1.2f));
					else if(warrior == Warrior.Hammer)
						StartCoroutine (COStunPause(2.4f));
					else
						StartCoroutine (COStunPause(.9f));
				}
				if(warrior == Warrior.Archer){
					if(GUI.Button(new Rect (135, 145, 100, 30), "MoveAttack2")){
						animator.SetTrigger("MoveAttack2Trigger");
						StartCoroutine (COArcherArrow(.6f));
						StartCoroutine (COStunPause(1.4f));
					}
				}
				if(GUI.Button (new Rect (25, 175, 100, 30), "SpecialAttack1")){
					animator.SetTrigger("SpecialAttack1Trigger");
					if(warrior == Warrior.Brute)
						StartCoroutine (COStunPause(2f));
					else if(warrior == Warrior.Mage)
						StartCoroutine (COStunPause(1.8f));
					else if(warrior == Warrior.Sorceress)
						StartCoroutine (COStunPause(1.5f));
					else if(warrior == Warrior.TwoHanded)
						StartCoroutine (COStunPause(1.2f));
					else if(warrior == Warrior.Swordsman)
						StartCoroutine (COStunPause(1f));
					else if(warrior == Warrior.Spearman)
						StartCoroutine (COStunPause(.9f));
					else if(warrior == Warrior.Hammer)
						StartCoroutine (COStunPause(1.6f));
					else
						StartCoroutine (COStunPause(1.7f));
				}
				if(warrior == Warrior.Ninja || warrior == Warrior.Sorceress){
					if(GUI.Button (new Rect (135, 175, 100, 30), "SpecialAttack2")){
						if(warrior == Warrior.Sorceress){
							if(!specialAttack2Bool){
								animator.SetTrigger("SpecialAttack2Trigger");
								animator.SetBool("SpecialAttack2Bool", true);
								specialAttack2Bool = true;
							}
							else {
								specialAttack2Bool = false;
								animator.SetBool("SpecialAttack2Bool", false);
								animator.SetBool("SpecialAttack2Trigger", false);
							}
						}
						else {
							animator.SetTrigger("SpecialAttack2Trigger");
							StartCoroutine (COStunPause(1f));
						}
					}
				}
				if(GUI.Button (new Rect (30, 270, 100, 30), "Death")){
					animator.SetTrigger("DeathTrigger");
					dead = true;
				}
				if(warrior == Warrior.Mage || warrior == Warrior.Ninja || warrior == Warrior.Knight || warrior == Warrior.Archer || warrior == Warrior.TwoHanded || warrior == Warrior.Swordsman || warrior == Warrior.Spearman || warrior == Warrior.Hammer || warrior == Warrior.Crossbow){
					if(!dead && !weaponSheathed && !weaponSheathed2){
						if(GUI.Button (new Rect (30, 310, 100, 30), "Sheath Wpn")){
							animator.SetTrigger("WeaponSheathTrigger");
							if(warrior == Warrior.Archer)
								StartCoroutine (COWeaponVisibility(.4f, false));
							else if(warrior == Warrior.Swordsman){
								StartCoroutine (COWeaponVisibility(.4f, false));
								StartCoroutine (COSecondaryWeaponVisibility(.4f, false));
							}
							else if(warrior == Warrior.Spearman){
								StartCoroutine (COWeaponVisibility(.26f, false));
							}
							else
								StartCoroutine (COWeaponVisibility(.5f, false));
							weaponSheathed = true;
							if(warrior != Warrior.Ninja && warrior != Warrior.Knight){
								isStunned = true;
							}
						}
					}
				}

				if(warrior == Warrior.Knight && !weaponSheathed){
					if(!dead && !weaponSheathed2){
						if(GUI.Button (new Rect (140, 310, 100, 30), "Sheath Wpn2")){
							animator.SetTrigger("WeaponSheath2Trigger");
							StartCoroutine (COWeaponVisibility(.75f, false));
							weaponSheathed2 = true;
						}
					}
				}

				if(warrior == Warrior.Knight){
					if(!dead && weaponSheathed2){
						if(GUI.Button (new Rect (140, 310, 100, 30), "UnSheath Wpn2")){
							animator.SetTrigger("WeaponUnsheath2Trigger");
							StartCoroutine (COWeaponVisibility(.5f, true));
							weaponSheathed2 = false;
							weaponSheathed = false;
						}
					}
				}

				if(warrior == Warrior.Ninja && !isStealth){
					if(!dead && !weaponSheathed){
						if(GUI.Button (new Rect (30, 350, 100, 30), "Stealth")){
							animator.SetBool("Stealth", true);
							isStealth = true;
						}
					}
				}
				if(warrior == Warrior.Ninja && isStealth && !isWall){
					if(!dead && !weaponSheathed){
						if(GUI.Button (new Rect (30, 350, 100, 30), "UnStealth")){
							animator.SetBool("Stealth", false);
							isStealth = false;
						}
					}
				}
				if(warrior == Warrior.Ninja && isStealth){
					if(!dead && !weaponSheathed){
						if(!isWall){
							if(GUI.Button (new Rect (140, 350, 100, 30), " Wall On")){
								animator.SetBool("Stealth-Wall", true);
								isWall = true;
							}
						}else{
							if(GUI.Button (new Rect (140, 350, 100, 30), " Wall Off")){
								animator.SetBool("Stealth-Wall", false);
								isWall = false;
							}
						}
					}
				}
			}
		}
		if(dead){  //ifcharacter is dead
			if(GUI.Button (new Rect (30, 270, 100, 30), "Revive")){
				animator.SetTrigger("ReviveTrigger");
				if(warrior == Warrior.Brute)  //ifcharacter is Brute
					StartCoroutine (COStunPause(1.7f));
				else if(warrior == Warrior.Mage)  //ifcharacter is Mage
					StartCoroutine (COStunPause(1.2f));
				else
					StartCoroutine (COStunPause(1f));
				dead = false;
			}
		}
	}
}