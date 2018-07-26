using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	HeroController heroController;

	bool printOnce = true;

	void Start(){
		heroController = GetComponent<HeroController>();
	}

	public void SubmitTurn()
    {
        heroController.ResolveTurn();

    }

	void Update () {
		if (heroController.IsActivePlayer()){
			if (printOnce){ 
				print(gameObject.name + " is active");	
				printOnce = false;
				}
			SubmitTurn();
		}else{
			printOnce = true;
		}
	}
}
