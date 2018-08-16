using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	HeroController heroController;

	void Start(){
		heroController = GetComponent<HeroController>();
	}

	// Update is called once per frame
	void Update () {
		if (heroController.MovesLeft > 0 ){
			Move();
		}
		if (Input.GetKeyDown(KeyCode.Space)){
			heroController.SubmitTurn();
		}
	}

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)){
			heroController.Move(Vector2Int.up);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			heroController.Move(Vector2Int.left);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			heroController.Move(Vector2Int.right);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			heroController.Move(Vector2Int.down);
		}
    }
}
