using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudManager : MonoBehaviour {

	public int score = 0;
	public Text scoreText;
	public Text scoreTextGameOver;

	public GameObject[] lifeGo;
	public int lifePoints = 3;

	public Color baseColor;
	public Color goodColor;
	public Color niceColor;
	public Color greatColor;
	public bool scoreIsAnimating = false;

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

		if (!scoreIsAnimating) 
		{
			Color targetColor = baseColor;
			float targetScale = 1.2f;
			float speedAnim = 4;

			if (points == 5) 
			{
				targetColor = niceColor;
				targetScale = 1.4f;
				speedAnim = 2;
			} 
			else if (points == 20) 
			{
				targetColor = greatColor;
				targetScale = 1.6f;
				speedAnim = 1.5f;
			}

			StartCoroutine (animScore (targetColor, targetScale, speedAnim));
		}
	}

	public void looseLife()
	{
		lifeGo [lifePoints - 1].transform.GetChild (0).gameObject.SetActive (false);
		lifeGo [lifePoints - 1].transform.GetChild (1).gameObject.SetActive (true);

		lifePoints--;
		if(lifePoints == 0)
			this.gameObject.GetComponent<StatesManager> ().SetStateOver ();
	}

	IEnumerator animScore(Color targetColor, float targetScale, float speedAnim)
	{
		float t = 0f;
		float scale = 1f;
		scoreIsAnimating = true;
		while (t < 1.0f) 
		{
			t += 4 * Time.deltaTime;
			scale = Mathf.SmoothStep (1f, targetScale, t);
			scoreText.transform.localScale = new Vector3 (scale, scale, scale);
			scoreText.color = Color.Lerp (baseColor, targetColor, t);
			yield return true;
		}

		t = 0f;

		while (t < 1.0f) 
		{
			t += speedAnim * Time.deltaTime;
			scale = Mathf.SmoothStep (targetScale, 1f, t);
			scoreText.color = Color.Lerp (targetColor, baseColor, t);
			scoreText.transform.localScale = new Vector3 (scale, scale, scale);
			yield return true;
		}

		scoreIsAnimating = false;
		yield return true;
	}
}
