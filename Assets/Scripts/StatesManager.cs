using UnityEngine;
using System.Collections;

public class StatesManager : MonoBehaviour {

	public enum states
	{
		MENU,
		GAME,
		OVER
	}

	public static states GameStates { get; set;}

	public GameObject gameHud;
	public GameObject gameOverHud;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetStateOver()
	{
		GameStates = states.OVER;
		MenuManager.instance.FadeIn (gameHud);
		MenuManager.instance.FadeOut (gameOverHud);
	}

	public void SetStateGame()
	{
		GameStates = states.GAME;
		MenuManager.instance.FadeOut (gameHud);
		if (gameOverHud.activeSelf)
			MenuManager.instance.FadeIn (gameOverHud);
		this.gameObject.GetComponent<GameManager> ().initGame ();
	}
}
