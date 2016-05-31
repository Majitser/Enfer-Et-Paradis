using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public ScoreManager scoreManager;

	public GameObject GameCanvas;
	public RectTransform spawnPoint;
	public GameObject mobPrefab;
	public GameObject[] doors;
	public RectTransform cooldownBarBg;
	public RectTransform cooldownBar;

	public float cooldownSpawn = 0.5f;
	public float cooldownDoors = 1f;

	private int currentNb = 0;
	private float startCooldownDoor;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < doors.Length; i++) 
		{
			if (i == currentNb)
				doors [i].transform.GetChild (0).gameObject.SetActive (false);
			else
				doors [i].transform.GetChild (0).gameObject.SetActive (true);
		}
		cooldownBar.offsetMin = new Vector2 (cooldownBarBg.rect.width, 0);
		RectTransform doorRect = (RectTransform)doors [currentNb].transform;
		cooldownBarBg.anchoredPosition = new Vector2 (doorRect.anchoredPosition.x, cooldownBarBg.anchoredPosition.y);
		startCooldownDoor = cooldownDoors;
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldownSpawn > 0)
			cooldownSpawn -= Time.deltaTime;

		if(cooldownSpawn <= 0)
		{
			int randType = Random.Range (0, 2);
			Spawn (randType);
			cooldownSpawn = 0.5f;
		}

		if (cooldownDoors > 0)
		{
			cooldownDoors -= Time.deltaTime;
			cooldownBar.offsetMin -= new Vector2 ((cooldownBarBg.rect.width / startCooldownDoor) * Time.deltaTime, 0);
		}

		if (cooldownDoors <= 0) 
		{
			int newNb = Random.Range(0, doors.Length);
			if (newNb != currentNb)
				warningDoorChanging (newNb);
		}
	}

	void warningDoorChanging(int newNb)
	{
		for (int i = 0; i < doors.Length; i++) 
		{
			if (i == newNb)
				doors [i].transform.GetChild (0).gameObject.SetActive (false);
			else
				doors [i].transform.GetChild (0).gameObject.SetActive (true);
		}

		currentNb = newNb;
		cooldownDoors = Random.Range(3, 5);
		cooldownBar.offsetMin = new Vector2 (cooldownBarBg.rect.width, 0);
		RectTransform doorRect = (RectTransform)doors [currentNb].transform;
		cooldownBarBg.anchoredPosition = new Vector2 (doorRect.anchoredPosition.x, cooldownBarBg.anchoredPosition.y);
		startCooldownDoor = cooldownDoors;
	}

	public void Spawn(int aleaType)
	{
		GameObject mob = Instantiate (mobPrefab) as GameObject;
		mob.transform.SetParent (GameCanvas.transform);
		mob.transform.localScale = Vector3.one;
		mob.GetComponent<MobProperty> ().type = aleaType;
		mob.GetComponent<Button> ().onClick.AddListener (delegate {
			StartCoroutine(TouchAction(aleaType, mob));	
		});

		RectTransform mobRect = (RectTransform)mob.transform;
		mobRect.anchoredPosition = spawnPoint.anchoredPosition;
	}

	IEnumerator TouchAction(int type, GameObject mob)
	{
		Debug.Log (type + " _ " + currentNb);
		if (type == currentNb) 
		{
			iTween.MoveTo (mob, iTween.Hash ("x", doors [type].transform.position.x, "y", doors [type].transform.position.y, "time", 0.3, "easeType", iTween.EaseType.easeOutCubic));

			yield return new WaitForSeconds (0.3f);

			MenuManager.instance.FadeIn (mob);
			scoreManager.addScore (1);
		} 
		else 
		{
			Debug.Log ("fail !");
		}
	}
}
