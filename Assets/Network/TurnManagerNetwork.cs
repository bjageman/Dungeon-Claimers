using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TurnManagerNetwork : NetworkBehaviour {

	List<HeroControllerNetwork> playerOrder;
	List<HeroControllerNetwork> savedPlayerOrder;
	
    int currentRound = 1;

	public static TurnManagerNetwork instance = null;

	public HeroControllerNetwork GetActiveHero(){
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
		playerOrder = new List<HeroControllerNetwork>();
	}

    public void StartGame()
    {
        playerOrder = new List<HeroControllerNetwork>(FindObjectsOfType<HeroControllerNetwork>());
		//RandomizeTurnOrder();
		savedPlayerOrder = new List<HeroControllerNetwork>(playerOrder);
    }

    private void RandomizeTurnOrder()
    {
        for (int i = 0; i < playerOrder.Count; i++) {
         HeroControllerNetwork temp = playerOrder[i];
         int randomIndex = UnityEngine.Random.Range(i, playerOrder.Count);
         playerOrder[i] = playerOrder[randomIndex];
         playerOrder[randomIndex] = temp;
     }
    }

    private void StartNewRound()
    {
        currentRound++;
        print("Starting New Round: " + currentRound);
        playerOrder = new List<HeroControllerNetwork>(savedPlayerOrder);
		foreach(HeroControllerNetwork player in playerOrder){
			player.ResetActions();
		}
    }

	public void SubmitTurn(HeroControllerNetwork hero){
		if (hero == GetActiveHero())
        {
            MoveToNextPlayer(hero);
        }
    }

    private void MoveToNextPlayer(HeroControllerNetwork hero)
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
