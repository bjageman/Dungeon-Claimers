using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerControllerNetwork : NetworkBehaviour {

	HeroControllerNetwork heroController;

	void Start(){
		heroController = GetComponent<HeroControllerNetwork>();
	}

    public void SubmitTurn()
    {
		heroController.SubmitTurn();
    }

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer){ return; }
		if (heroController.IsActivePlayer()){
			if (Input.GetKeyDown(KeyCode.Space)){
				SubmitTurn();
			}
		}
	}
}
