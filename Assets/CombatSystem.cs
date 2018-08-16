using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour {
	public int health = 20;
	public int attack = 5;

	//TODO Move to a combat system component
    public bool FightMonster(Room room)
    {
        MonsterTribe tribe = room.GetTribe().GetComponent<MonsterTribe>();
        tribe.ReducePopulationByAttack(attack);
        ChangeHealth(-tribe.Strength);
        if (tribe.Population <= 0)
        {
            //TODO Keep or Kill?
            //KillTribe(tribe);
            KeepTribe(tribe);
            return true;
        }
        return false;
    }

    private void KeepTribe(MonsterTribe tribe)
    {
        tribe.Population = 1;
        tribe.ally = GetComponent<HeroController>();
    }

    private static void KillTribe(MonsterTribe tribe)
    {
        Destroy(tribe.gameObject);
    }

    

    private void ChangeHealth(int value)
    {
        health = health + value;
		GetComponent<HeroController>().heroUI.health.text = health.ToString();
    }
}
