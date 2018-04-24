using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PhsyicsObject 
 * This component tracks position and movement information for a GameObject
 * It knows how to ApplyForce (but some other script will call that)
 * It runs its own BounceCheck method (to keep the object in-bounds)
 * Movement happens in LateUpdate, so that some other script can call
 *   ApplyForce in its Update, and we can be sure all Forces have been
 *   applied before moving the PhysicsObject.
 */
public class PhysicsObject : MonoBehaviour {

	Vector3 velocity;
	Vector3 lastGoodVelocity;
	public Vector3 followPos;
	public Vector3 toLeader;
	public Vector3 target;

	// You can tweak these in the inspector.
	public float mass = 1f; // higher mass - more force needed to get moving
	public float radius = 0.25f;
	public float speedLimit = 0.125f;
	public float slowRadius = 0.5f;

	void Start () {

		// no forces to start off
		velocity = Vector3.zero;
		lastGoodVelocity = Vector3.up;
	}

	void ClampVelocity() {
		float mag = velocity.magnitude;
		if (mag > speedLimit) {
			float scale = speedLimit / mag;
			velocity *= scale;
		}
	}

	public void Arrive() {
		// find the direction we want to travel
		Vector3 desired = target - transform.position;

		// if I'm far enough to not need to slow down
		if (desired.magnitude > slowRadius) {
			// full steam ahead!
			velocity = desired.normalized * speedLimit;
		} else {
			// set velocity such that it approaches zero as we get closer to our target
			float percentToTarget = desired.magnitude / slowRadius;
			velocity = desired.normalized * (percentToTarget * speedLimit);
		}
	}
		
	void LateUpdate () {

		// obey the speed limit
		ClampVelocity ();

		// update position by velocity (respecting velocity as units per second)
		transform.position += velocity * Time.deltaTime;

		// set followPos based on a velocity that was actually moving
		this.followPos = transform.position - (lastGoodVelocity.normalized * 0.75f);
		// compare to
		// this.followPos = transform.position - (velocity.normalized * 0.75f);

		// update our lastGoodVelocity if actually moving
		if (velocity.sqrMagnitude > 0.125f) {
			lastGoodVelocity = velocity;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, radius);
		Gizmos.DrawLine (transform.position, this.followPos);
	}
}
