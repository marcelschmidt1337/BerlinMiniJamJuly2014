using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{

		public float stepStrength = 10.0f;
		public string stepL, stepR;
		public float cooldown = 1.0f;
		float lastPressed = 0.0f;

		// Use this for initialization
		void Start ()
		{
				lastPressed = Time.time;
		}

		bool canStep ()
		{
				return (Time.time - lastPressed) >= cooldown;
		}

		void registerStepTaken ()
		{
				lastPressed = Time.time;
		}

		// Update is called once per frame
		void Update ()
		{
				if (canStep ()) {
						if (Input.GetButtonDown (stepL)) {
								transform.FindChild ("Left Foot").rigidbody.AddRelativeForce ((new Vector3 (0.0f, 0.0f, 1.0f)) * stepStrength);
								//Debug.Log ("Left!");
								registerStepTaken ();
						}
						if (Input.GetButtonDown (stepR)) {
								transform.FindChild ("Right Foot").rigidbody.AddRelativeForce ((new Vector3 (0.0f, 0.0f, 1.0f)) * stepStrength);
								//Debug.Log ("Right!");
								registerStepTaken ();
						}
				}

		}
}
