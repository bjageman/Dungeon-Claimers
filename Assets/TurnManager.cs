using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TurnManager : NetworkBehaviour {

	List<HeroController> playerOrder;
	List<HeroController> savedPlayerOrder;
	
    int currentRound = 1;

	public static TurnManager instance = null;

	public HeroController GetActiveHero(){
		if (playerOrder.Count == 0){ return null;}
		return playerOrder[0];
	}

	void Awake()
        {
            //Check if instance already exists
            if (instance == null){
                instance = this;
			}else if (instance != this){
				Destroy(gameObject);
			}       
            DontDestroyOnLoad(gameObject);
            
        }

	void Start(){
		playerOrder = new List<HeroController>();
	}

    public void StartGame()
    {
        playerOrder = new List<HeroController>(FindObjectsOfType<HeroController>());
		//RandomizeTurnOrder();
		savedPlayerOrder = new List<HeroController>(playerOrder);
    }

    private void RandomizeTurnOrder()
    {
        for (int i = 0; i < playerOrder.Count; i++) {
         HeroController temp = playerOrder[i];
         int randomIndex = UnityEngine.Random.Range(i, playerOrder.Count);
         playerOrder[i] = playerOrder[randomIndex];
         playerOrder[randomIndex] = temp;
     }
    }

    private void StartNewRound()
    {
        currentRound++;
        print("Starting New Round: " + currentRound);
        playerOrder = new List<HeroController>(savedPlayerOrder);
		foreach(HeroController player in playerOrder){
			player.ResetActions();
		}
    }

	public void SubmitTurn(HeroController hero){
		if (hero == GetActiveHero())
        {
            MoveToNextPlayer(hero);
        }
    }

    private void MoveToNextPlayer(HeroController hero)
    {
        print(hero.gameObject.name + " submitted turn");
        playerOrder.Remove(hero);
		if (playerOrder.Count == 0){
			StartNewRound();
		}
    }

    

    // Update is called once per frame
    void Update () {
		
	}
}
