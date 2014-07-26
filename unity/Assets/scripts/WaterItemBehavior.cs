using UnityEngine;
using System.Collections;

public class WaterItemBehavior : GameComponent {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other){
		Debug.Log ("WATER! Finally!");
				Destroy(this.gameObject);
		(GLogic as GameLogic).IncreaseItemCount (other.GetComponent<PlayerBehavior> ().playerID);


	}
}
