using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TurnManager : NetworkBehaviour {

	List<HeroController> playerOrder;
	List<HeroController> savedPlayerOrder;
    Queue<MonsterTribe> monsterOrder;
	
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
        monsterOrder = new Queue<MonsterTribe>();
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
        playerOrder = new List<HeroController>(savedPlayerOrder);
		foreach(HeroController player in playerOrder){
			player.ResetActions();
		}
    }

	public void SubmitTurn(HeroController hero){
		if (hero == GetActiveHero())
        {
            StartCoroutine(MoveToNextPlayer(hero));
        }
    }

    IEnumerator MoveToNextPlayer(HeroController hero)
    {
        hero.heroUI.SetActiveMarker(false);
        yield return new WaitForEndOfFrame();
        playerOrder.Remove(hero);
		if (playerOrder.Count == 0){
			ProcessMonsterActions();  
		}
    }

    private void ProcessMonsterActions()
    {
        monsterOrder = new Queue<MonsterTribe>(FindObjectsOfType<MonsterTribe>());
        while(monsterOrder.Count > 0){
            MonsterTribe monster = monsterOrder.Dequeue();
            if (monster != null){
                monster.AttackWeakerNeighbors();
                monster.MeldWithSameNeighbors();
                if (monster.numRoundsPerGrowth > 0 && currentRound % monster.numRoundsPerGrowth == 0){
                    monster.GrowTribe();
                }
            }
            
        }
        StartNewRound();
    }

    
}
