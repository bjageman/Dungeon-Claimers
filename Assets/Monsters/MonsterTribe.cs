using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTribe : MonoBehaviour {
	public string tribeName;

	[Header("Population")]
	public int powerPerMonster;
	public int growthRate = 1;
	public int numRoundsPerGrowth;
	public int maxPopulationDensity = 10;
	public int resetPopulation = 5;
	[SerializeField] int population;
	[SerializeField] TextMesh populationText;

	[Header("Other")]
	public Room room;
	public HeroController ally;
	[SerializeField] int requiredAttackMultiplier = 2;
	[SerializeField] TextMesh strengthText;

	public int Strength { get{ return population * powerPerMonster; } }

	public int Population { 
		get { return population; }
		set { 
			this.population = value; 
			populationText.text = this.population.ToString();
			strengthText.text = Strength.ToString();
		}					
	}

	void Start(){
		Population = population;
	}


	public void AttackWeakerNeighbors()
    {
        foreach (Room neighbor in room.pathWays){
			MonsterTribe nearbyTribe = neighbor.GetTribe();
            if (nearbyTribe != null && !IsSameTribe(nearbyTribe) && Strength > nearbyTribe.Strength * requiredAttackMultiplier){
                ReducePopulationByAttack(nearbyTribe.Strength);
				ExpandTribeToRoom(nearbyTribe.room);
				Destroy(nearbyTribe.gameObject);
            }
        }
    }

    private void ExpandTribeToRoom(Room room)
    {
        GameObject tribeObject = Instantiate(gameObject, room.tile.transform.position, Quaternion.identity);
		MonsterTribe newTribe = tribeObject.GetComponent<MonsterTribe>();
		newTribe.Population = Mathf.FloorToInt(Population / 2);
		Population = Mathf.FloorToInt(Population / 2) + Population % 2; 
		room.SetTribe(newTribe);
    }

    public void MeldWithSameNeighbors()
    {
        foreach (Room neighbor in room.pathWays)
        {
			MonsterTribe nearbyTribe = neighbor.GetTribe();
            if (IsSameTribe(nearbyTribe) && Strength > nearbyTribe.Strength)
            {
                int totalPopulation = Population + nearbyTribe.population;
				Population = Mathf.FloorToInt(totalPopulation / 2) + totalPopulation % 2; 
				nearbyTribe.population = Mathf.FloorToInt(totalPopulation / 2);
            }
        }
    }

	public void GrowTribe(){
		if (Population > maxPopulationDensity){
			Population = resetPopulation;
		}else{
			Population = Population + growthRate;
		}
		
	}

    private bool IsSameTribe(MonsterTribe neighbor)
    {
        return tribeName == neighbor.tribeName;
    }
	
	public void ReducePopulationByAttack(int attack)
    {
        int currentAttack = attack;
        for (int i = 0; Population > 0 && currentAttack >= powerPerMonster ; i++){
            currentAttack = currentAttack - powerPerMonster;
            Population--;
        }
    }
}
