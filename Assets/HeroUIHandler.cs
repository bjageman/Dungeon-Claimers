using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUIHandler : MonoBehaviour {
	[SerializeField] HeroUI[] HeroUIs;


	// Use this for initialization
	void Start () {
		HeroController[] heroes = FindObjectsOfType<HeroController>();
		int heroUIIndex = 0;
		foreach ( HeroController hero in heroes ){
			HeroUIs[heroUIIndex].gameObject.SetActive(true);
			hero.heroUI = HeroUIs[heroUIIndex];
			hero.heroUI.heroName.text = hero.heroName;
			hero.heroUI.movesLeft.text = hero.MovesLeft.ToString();
			hero.heroUI.health.text = hero.GetComponent<CombatSystem>().health.ToString();
			heroUIIndex++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
