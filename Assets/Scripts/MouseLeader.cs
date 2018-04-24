using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLeader : MonoBehaviour {

	public Vector3 followPos;

	void Start () {
		// set a default followPos
		this.followPos = transform.position - (transform.up * 0.75f);
	}

	void Update () {
		// find the mouse position in the world
		Vector3 mouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		// keep things level
		mouse.z = 0;

		// update the position and followPos
		this.transform.position = mouse;
		this.followPos = transform.position - (transform.up * 0.75f);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawLine (transform.position, transform.position - (transform.up * 0.75f));
		Gizmos.DrawWireSphere (transform.position, 0.25f);
	}
}
