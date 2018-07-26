using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerController : NetworkBehaviour {

	HeroController heroController;

	bool printOnce = true;

	void Start(){
		heroController = GetComponent<HeroController>();
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
