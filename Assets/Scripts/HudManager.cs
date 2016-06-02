using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudManager : MonoBehaviour {

	public int score = 0;
	public Text scoreText;
	public Text scoreTextGameOver;

	public GameObject[] lifeGo;
	public int lifePoints = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addScore(int points)
	{
		score += points;
		scoreText.text = score.ToString ();
		scoreTextGameOver.text = "Score : " + score;
	}

	public void looseLife()
	{
		lifeGo [lifePoints - 1].transform.GetChild (0).gameObject.SetActive (false);
		lifeGo [lifePoints - 1].transform.GetChild (1).gameObject.SetActive (true);

		lifePoints--;
		if(lifePoints == 0)
			this.gameObject.GetComponent<StatesManager> ().SetStateOver ();
	}
}
