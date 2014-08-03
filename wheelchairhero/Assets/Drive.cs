using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drive : MonoBehaviour {
	//Controllers and control parts
	public WheelCollider rwheelL;
	public WheelCollider rwheelR;	
	public RUISPSMoveWand RWheelController;
	public RUISPSMoveWand LWheelController;

	//Command mode IF both false normal PSmove controll
	public bool keyboardMode = true;
	public bool wheelchairMode = false;

	//Torgue manipulation values
	public static float maxTorque = 4;
	public float maxTorqueLimit = 4;
	public float torqueGain = 0.8f;
	public float torqueReduce = 4f;
	public float breakTorque = 4f;
	public float breakHoldRange = 0.002f;
	public double breakIngnoreTime = 0.3;
	public static float torqueR;
	public static float torqueL;
	public float torgueREdit;
	public float torgueLedit;
	private double lastMovementR;
	private double lastMovementL;

	//Wheelchairmode control limits
	public float activationLimit = 0.002f;
	public float angActLimit = 65f;
	public float torqueMultiplL = 1.0f; //If axis goes mad
	public float torqueMultiplR = 1.0f; //If axis goes mad
	public float lastVelocityR;
	public float lastVelovityL;

	//Normal PSmove only controll values
	public float contrMovAdj = 0.005f;
	public float boostLimit = 0.02f;
	private Vector3 lastPosRController;
	private Vector3 lastPosLController;

	//Stuck handling values
	private Vector3 lastWheelChairPos;
	private float stuckTimerStart;
	private bool stuckDetecting = false;
	public float stuckTime = 2.0f;
	public float stuckRange = 0.01f;
	public float stuckForce = 0.5f;

	//Debugging values for keyboard usage. Set to public incase needed
	private bool RWheelTorqueReduce = true;
	private bool LWheelTorqueReduce = true;

	//Keyboard controll maps
	private Dictionary<string, float> keyControllLeft = new Dictionary<string, float> (){
		{"w", -1f},
		{"s", 1f},
	};
	private Dictionary<string, float> keyControllRight = new Dictionary<string, float> (){
		{"e", -1f},
		{"d", 1f}
	};


	void Start() {

	}
 	/*
     * Convert user input into movement/rotation
     */
	void FixedUpdate () {
		Drive.maxTorque = this.maxTorqueLimit;
		this.torgueLedit = Drive.torqueL;
		this.torgueREdit = Drive.torqueR;
		
		if (keyboardMode) {
			handleKeyboardControll();
		} else {

			//PSMOVE controlling
			//Calc distances for both controllers and update positions
			float distanceR = 0;
			float distanceL = 0;

			//Using real wheelchair as a controller
			if ( wheelchairMode ) {
				//Debug.Log (this.RWheelController.angularVelocity);
				float x = this.RWheelController.angularVelocity.x * this.torqueMultiplR;
				if (x > this.angActLimit ) {
					distanceR = this.activationLimit * 1.05f;
				} else if ( x < -this.angActLimit ) {
					distanceR = -this.activationLimit * 1.05f;
				}
				this.lastVelocityR = x;

				x = this.LWheelController.angularVelocity.x * this.torqueMultiplL;
				if (x > this.angActLimit ) {
					distanceL = this.activationLimit * 1.05f;
				} else if ( x < -this.angActLimit ) {
					distanceL = -this.activationLimit * 1.05f;
				}
				this.lastVelovityL = x;
			//using two psmoves as controllers
			} else {
				distanceR = this.RWheelController.localPosition.z - this.lastPosRController.z;
				distanceL = this.LWheelController.localPosition.z - this.lastPosLController.z;
				this.lastPosRController = this.RWheelController.localPosition; 
				this.lastPosLController = this.LWheelController.localPosition;
			}


			//Incase distances are close enough movent of both controllers cause
			//back or forward movement into same direction
			if (this.RWheelController.triggerValue > 0 && this.LWheelController.triggerValue > 0) {
				if ( Mathf.Abs(distanceL - distanceR) <= this.contrMovAdj &&
				    	distanceL != 0 && distanceR != 0) {
					distanceR = (distanceL + distanceR) / 2;
					distanceL = distanceR;
				}
			}


			//Update Right wheel
			//User holds trigger down
			if (this.RWheelController.triggerValue > 0 || wheelchairMode) {
				//Increase breaking torque if wheel is hold still
				float breakVal = breakValue(distanceR);
				if ( breakVal > 0) {
					if ( this.lastMovementR + this.breakIngnoreTime <= Time.realtimeSinceStartup ) {
						this.rwheelR.brakeTorque = breakVal;
						//Debug.Log ( "Break R");
					}
				} else {
					this.lastMovementR = Time.realtimeSinceStartup;
					this.rwheelR.brakeTorque = 0;
					//Debug.Log ( "No Break R");
				}
				//Update variables
				Drive.torqueR = updateTorque(distanceR, Drive.torqueR);
			} else {
				//reduce torque gradually and keep tracking controller position
				this.rwheelR.brakeTorque = 0;
				this.lastMovementR = Time.realtimeSinceStartup;
				Drive.torqueR = decreaseTorque(Drive.torqueR);
			}

			//Update Left wheel
			//User holds trigger down
			if (this.LWheelController.triggerValue > 0 || wheelchairMode) {
				//Increase breaking torque if wheel is hold still;
				float breakVal = breakValue(distanceL);
				if ( breakVal > 0 ) {
					if ( this.lastMovementL + this.breakIngnoreTime <= Time.realtimeSinceStartup ) {
					    this.rwheelL.brakeTorque = breakVal;
						//Debug.Log ( "Break L");
					}
				} else {
					//Debug.Log ( "No Break L");
					this.rwheelL.brakeTorque = 0;
					this.lastMovementL = Time.realtimeSinceStartup;
				}
				//Update variables
				Drive.torqueL = updateTorque(distanceL, Drive.torqueL);
			} else {
				//reduce torque gradually and keep tracking controller position
				this.rwheelL.brakeTorque = 0;
				this.lastMovementL = Time.realtimeSinceStartup;
				Drive.torqueL = decreaseTorque(Drive.torqueL);
			}

			//If controllers movement is adjusted average torgue values
			// to keep torgues balance
			if (this.RWheelController.triggerValue > 0 && this.LWheelController.triggerValue > 0) {
				if ( distanceL == distanceR && distanceL != 0 && distanceR != 0) {
					Drive.torqueL = (Drive.torqueL + Drive.torqueR) / 2;
					Drive.torqueR = Drive.torqueL;
				}
			}

			//Set torque values for both wheels
			this.rwheelL.motorTorque = Drive.torqueL;
			this.rwheelR.motorTorque = Drive.torqueR;
		}

		//Determinates if chair is stucked and needs a bit push
		Vector3 currentPos = this.gameObject.transform.localPosition;
		if (isStuck()) {
			if (Drive.torqueL == Drive.maxTorque && Drive.torqueR == Drive.maxTorque) {
				this.gameObject.transform.localPosition = new Vector3(
					currentPos.x + this.stuckForce,
					currentPos.y + this.stuckForce,
					currentPos.z);
			} else if (Drive.torqueL == -Drive.maxTorque && Drive.torqueR == -Drive.maxTorque) {
				this.gameObject.transform.localPosition = new Vector3(
					currentPos.x - this.stuckForce,
					currentPos.y - this.stuckForce,
					currentPos.z);
			}
		} else {
			this.lastWheelChairPos = currentPos;
		}	
	}


	//Add torque to given direction depending of movement
	//return torque value if 0 wheel is hold still
	float updateTorque (float distance, float oldTorque) {
		float newTorque = 0;

		if ( distance < -activationLimit) {
			newTorque = oldTorque + this.torqueGain;
			//Tries to simulate feeling that speed of your controller movement
			//really matters
			if ( distance > boostLimit ) {
				newTorque += this.torqueGain;
			}
			//Limit torque to max torque
			newTorque = Mathf.Min(Drive.maxTorque, newTorque);

		} else if ( distance > activationLimit ) {
			newTorque = oldTorque - this.torqueGain;
			//Tries to simulate feeling that speed of your controller movement
			//really matters
			if ( distance < -boostLimit ) {
				newTorque -= this.torqueGain;
			}
			//Limit torque to max torque
			newTorque = Mathf.Max(-Drive.maxTorque, newTorque);
		} else {
			newTorque = decreaseTorque(oldTorque);
		}
		return newTorque;
	}


	//Reduce current torque toward zero by default torque reduce
	float decreaseTorque (float oldtorque) {
		if ( oldtorque > 0 ) {
			return Mathf.Max(0, oldtorque - this.torqueReduce);
		} else if ( oldtorque < 0 ) {
			return Mathf.Min(0, oldtorque + this.torqueReduce);
		}
		return oldtorque;
	}


	//Return breaking value if needed
	float breakValue(float distance) {

		//Set break to one since wheel is hold in place
		if (distance > -breakHoldRange && distance < breakHoldRange) {
			return breakTorque;
			//Release break since there is torque
		} else {
			return 0;
		}
	}


	//Handles keyboard controlling
	void handleKeyboardControll() {
		//Rigth wheel controll
		foreach(KeyValuePair<string, float> pair in keyControllRight) {
			if (Input.GetKeyDown (pair.Key)) {
				RWheelTorqueReduce = false;
				Drive.torqueR = pair.Value * torqueGain;
			} else if (Input.GetKeyUp (pair.Key)) {
				RWheelTorqueReduce = true;
			}
		}

		//Left wheel controll
		foreach(KeyValuePair<string, float> pair in keyControllLeft) {
			if (Input.GetKeyDown (pair.Key)) {
				LWheelTorqueReduce = false;
				Drive.torqueL = pair.Value * torqueGain;
			} else if (Input.GetKeyUp (pair.Key)) {
				LWheelTorqueReduce = true;
			}
		}

		//no torque added -> reduce existing torque
		if (RWheelTorqueReduce) {
			Drive.torqueR = decreaseTorque(Drive.torqueR);
		}
		if (LWheelTorqueReduce) {
			Drive.torqueL = decreaseTorque(Drive.torqueL);
		}

		//Set new torque values in use
		rwheelL.motorTorque = Drive.torqueL;
		rwheelR.motorTorque = Drive.torqueR;
	}

	bool isStuck() {
		if (Vector3.Distance(this.lastWheelChairPos, this.gameObject.transform.localPosition) < this.stuckRange) {
			if (Time.time > this.stuckTimerStart + this.stuckTime && this.stuckDetecting) {
				this.stuckDetecting = false;
				return true;
			} else if (!this.stuckDetecting && Drive.torqueL == Drive.torqueR && Drive.torqueL != 0 ) {
				this.stuckTimerStart = Time.time;
				this.stuckDetecting = true;
			}
		} else {
			this.stuckDetecting = false;
		}
		return false;
	}
}