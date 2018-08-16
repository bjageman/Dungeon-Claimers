using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class HeroControllerNetwork : NetworkBehaviour {

	TurnManagerNetwork turnManager;
	
	SpriteRenderer spriteRenderer;

	public bool IsActivePlayer()
    {
		if (turnManager == null){ return false;}
        return this == turnManager.GetActiveHero();
    }

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		turnManager = FindObjectOfType<TurnManagerNetwork>();
	}

	public void SubmitTurn(){
		CmdResolveTurn();
	}

	public void ResetActions(){
		spriteRenderer.color = Color.white;
	}

	public void ResolveTurn()
    {  
		if (!IsActivePlayer()){	return;	}
		spriteRenderer.color = Color.green;
		turnManager.SubmitTurn(this);
    }

	[ClientRpc]
	void RpcPerformActions(){
		if(isServer){ return; }
		ResolveTurn();
	}

    [Command]
    void CmdResolveTurn()
    {
        ResolveTurn();
		RpcPerformActions();
    }

}
