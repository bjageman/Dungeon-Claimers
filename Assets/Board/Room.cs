using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public HeroController claimedBy;
	public GameObject claimOverlay;
	public GameObject tile;
	public GameObject fog;
	
	public bool isVisited = false;
	public List<Room> pathWays = new List<Room>();
	public int x;
	public int y;

	MonsterTribe tribe;

	public void SetTribe(MonsterTribe tribe){
		tribe.room = this;
		this.tribe = tribe;
	}

	public MonsterTribe GetTribe(){
		return this.tribe;
	}


}
