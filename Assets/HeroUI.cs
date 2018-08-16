using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour {

	public Text heroName;
	public Text movesLeft;
	public Text health;
	public Image activeMarker;

	public void SetActiveMarker(bool active){
		activeMarker.gameObject.SetActive(active);
	}
}
