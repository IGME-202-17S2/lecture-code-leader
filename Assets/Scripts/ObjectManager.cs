using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ObjectManager 
 * This component tracks collections of PhysicsObjects
 * It also orchestrates the forces and applies them.
 */
public class ObjectManager : MonoBehaviour {

	// for PhysicsObjects generated at runtime
	// (populated with the GeneratePOs() call
	List<PhysicsObject> movers = new List<PhysicsObject>();

	// a reference to the prefab used for PhysicsObjects
	// (also populated in the inspector)
	public GameObject moverPrefab;

	// the leader to follow
	public MouseLeader leader;

	void Start () {
		GenerateMovers ();
	}

	void GenerateMovers() {
		for (int i = 0; i < 5; i++) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (
				new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 0));
			pos.z = 0;
			GameObject go = Instantiate (moverPrefab, pos, Quaternion.identity);
			PhysicsObject po = go.GetComponent<PhysicsObject> ();
			movers.Add (po);
		}
	}

	static int SortByCloseness(PhysicsObject po1, PhysicsObject po2) {
		return po1.toLeader.sqrMagnitude.CompareTo (po2.toLeader.sqrMagnitude);
	}

	void DetermineOrder() {
		// calculate the toLeader vector for each mover
		for (int i = 0; i < movers.Count; i++) {
			movers [i].toLeader = leader.followPos - movers [i].transform.position;
		}

		// sort based on that information
		movers.Sort (SortByCloseness);

		// setup the closest mover to go at the leader
		PhysicsObject previous = movers [0];
		movers [0].target = leader.followPos;

		// setup every mover after that to go after the previous mover
		for (int j = 1; j < movers.Count; j++) {
			movers [j].target = previous.followPos;
			previous = movers [j];
		}
	}

	void Update () {
		DetermineOrder ();
		for (int i = movers.Count - 1; i >= 0; i--) {
			movers [i].Arrive();
		}
	}
}
