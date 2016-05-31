using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public ScoreManager scoreManager;

	public GameObject GameCanvas;
	public RectTransform spawnPoint;
	public Transform[] mobGrids;
	public GameObject mobPrefab;
	public GameObject[] doors;
	public RectTransform cooldownBarBg;
	public RectTransform cooldownBar;
	[HideInInspector]
	public float startCooldownDoor;

	public float cooldownSpawn = 0.5f;
	public float cooldownDoors = 1f;

	private int currentNb = 0;
	private float nbSpawn = 1;

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
			nbSpawn = Mathf.Floor (1 + scoreManager.score /5);
			if (nbSpawn > 4)
				nbSpawn = 4;
			for (int i = 0; i < nbSpawn; i++)
			{
				int randType = Random.Range (0, doors.Length);
				if (nbSpawn <= 3)
					Spawn (randType, mobGrids [1]);
				else if (nbSpawn == 4)
					Spawn (randType, mobGrids [(int)Mathf.Floor (i / 2)]);
				else if (nbSpawn == 5) 
				{
					if (i < 1)
						Spawn (randType, mobGrids [0]);
					else if (i >= 1 && i < 4)
						Spawn (randType, mobGrids [1]);
					else
						Spawn (randType, mobGrids [2]);
				} 
				else if (nbSpawn == 6 || nbSpawn == 9)
					Spawn (randType, mobGrids [(int)Mathf.Floor (i / 3)]);
				else if (nbSpawn == 7) 
				{
					if (i < 2)
						Spawn (randType, mobGrids [0]);
					else if (i >= 2 && i < 5)
						Spawn (randType, mobGrids [1]);
					else
						Spawn (randType, mobGrids [2]);
				}
				else if (nbSpawn == 8)
				{
					if (i < 3)
						Spawn (randType, mobGrids [0]);
					else if (i >= 3 && i < 5)
						Spawn (randType, mobGrids [1]);
					else
						Spawn (randType, mobGrids [2]);
				}
			}
			for(int i = 0; i < mobGrids.Length; i++)
			{
				if (mobGrids [i].transform.childCount > 0)
					mobGrids [i].gameObject.SetActive (true);
				else
					mobGrids [i].gameObject.SetActive (false);
			}
			cooldownSpawn = 2;
		}

		if (cooldownDoors > 0)
		{
			cooldownDoors -= Time.deltaTime;

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

	public void Spawn(int aleaType, Transform parent)
	{
		GameObject mob = Instantiate (mobPrefab) as GameObject;
		mob.transform.SetParent (parent);
		mob.transform.localScale = Vector3.one;
		mob.GetComponent<MobProperty> ().type = aleaType;
		mob.GetComponent<Button> ().onClick.AddListener (delegate {
			StartCoroutine (TouchAction (aleaType, mob));	
		});

		RectTransform mobRect = (RectTransform)mob.transform;
	}

	IEnumerator TouchAction(int type, GameObject mob)
	{
		if (type == currentNb) 
		{
			mob.GetComponent<MobProperty> ().isMoving = true;
			iTween.MoveTo (mob, iTween.Hash ("x", doors [type].transform.position.x, "y", doors [type].transform.position.y, "time", 0.3, "easeType", iTween.EaseType.easeOutCubic));

			yield return new WaitForSeconds (0.3f);

			MenuManager.instance.FadeIn (mob);
			scoreManager.addScore (1);
			yield return new WaitForSeconds (0.35f);
			mob.GetComponent<MobProperty> ().isMoving = false;
		} 
		else 
		{
			Debug.Log ("fail !");
		}
	}
}
