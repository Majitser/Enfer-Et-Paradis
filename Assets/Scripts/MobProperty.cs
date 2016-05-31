using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobProperty : MonoBehaviour {

	public float maxSpeedY = 1f;
	public float minSpeedY = 1f;
	public float maxSpeedX = 0.2f;
	public float minSpeedX = 0f;
	public int type = 0;
	public Color[] colors;
	public float timerDeath = 3;
	[HideInInspector]
	public bool isMoving = false;

	RectTransform mobRect;
	float speedX;
	float speedY;

	// Use this for initialization
	void Start () {
		mobRect = (RectTransform)this.transform;
		this.transform.GetChild(0).GetComponent<Image> ().color = colors [type];
		speedX = Random.Range (minSpeedX, maxSpeedX);
		speedY = Random.Range (minSpeedY, maxSpeedY);
		iTween.ScaleTo (this.transform.GetChild (0).gameObject, iTween.Hash ("x", 1, "y", 1, "z", 1, "time", 0.2, "easeType", iTween.EaseType.easeOutQuad));
	}
	
	// Update is called once per frame
	void Update () {
		if (timerDeath > 0)
			timerDeath -= Time.deltaTime;
		else if(timerDeath <= 0 && !isMoving)
			Destroy (this.gameObject);
	}

	public void Move()
	{
		mobRect.anchoredPosition -= new Vector2 (speedX, speedY);
	}
}
