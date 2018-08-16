using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CombatSystem))]
public class HeroController : MonoBehaviour {
	[SerializeField] int movesPerTurn = 3;
	
	public string heroName = "PLAYER";
	public HeroUI heroUI;
	public Color heroColor;

	BoardGenerator board;

	CombatSystem combat;
	SpriteRenderer spriteRenderer;
	Room currentRoom;
	int movesLeft = 3;

	public int MovesLeft { get { return movesLeft; }}

	public bool IsActivePlayer()
    {
		if (TurnManager.instance == null){ return false;}
        return this == TurnManager.instance.GetActiveHero();
    }

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		combat = GetComponent<CombatSystem>();
		board = BoardGenerator.instance;
	}

	void Update(){
		if (IsActivePlayer()) { 
			heroUI.SetActiveMarker(true); 
		}else{
			heroUI.SetActiveMarker(false); 
		}
	}

	public void SetHeroOnBoard(int x, int y)
    {
        board = BoardGenerator.instance;
		currentRoom = board.GetRoom(x, y);
        transform.position = currentRoom.tile.transform.position;
        ClaimTile();
    }

    private void ClaimTile()
    {
        Destroy(currentRoom.fog);
		currentRoom.claimedBy = this;
		currentRoom.claimOverlay.gameObject.SetActive(true);
		currentRoom.claimOverlay.GetComponent<SpriteRenderer>().color = heroColor;
        foreach (Room nearbyRoom in currentRoom.pathWays)
        {
            Destroy(nearbyRoom.fog);
        }
    }

    public void Move(Vector2Int direction){
		if (!IsActivePlayer()) { return; }
		Vector2Int newPosition = new Vector2Int(currentRoom.x, currentRoom.y) + direction;
		Room nextRoom = board.GetRoom(newPosition.x, newPosition.y);
		if (currentRoom.pathWays.Contains(nextRoom))
        {
			SpendMovement();
			MonsterTribe tribe = nextRoom.GetTribe();
			if (tribe != null && tribe.ally != this){
				bool monsterDefeated = combat.FightMonster(nextRoom);
				if (!monsterDefeated){ return; }
			}
            SetHeroOnBoard(newPosition.x, newPosition.y);
            
        }
    }

	

    public void ResetActions(){
		spriteRenderer.color = Color.white;
	}

	public void SubmitTurn()
    {
        if (!IsActivePlayer()) { return; }
        spriteRenderer.color = Color.green;
        ResetMovement();
        TurnManager.instance.SubmitTurn(this);
    }

	private void SpendMovement()
    {
        movesLeft--;
        heroUI.movesLeft.text = movesLeft.ToString() + " / " + movesPerTurn.ToString();
    }
	
    private void ResetMovement()
    {
        movesLeft = movesPerTurn;
        heroUI.movesLeft.text = movesLeft.ToString() + " / " + movesPerTurn.ToString();
    }
}
