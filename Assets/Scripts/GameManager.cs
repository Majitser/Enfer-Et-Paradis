﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public ScoreManager scoreManager;

	[Header("World")]
	public GameObject GameCanvas;
	public GameObject maskWorld;
	public float shakeTime = 0.5f;
	public float shakeAmount = 0.7f;
	[Header("Mob")]
	public RectTransform spawnPoint;
	public Transform[] mobGrids;
	public GameObject mobPrefab;
	public GameObject[] explosionParticle;
	public GameObject clickParticle;
	[Header("Door")]
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

			Debug.Log (nbSpawn);
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
			cooldownSpawn = 2.7f;
		}
	}

	public void Spawn(int aleaType, Transform parent)
	{
		GameObject mob = Instantiate (mobPrefab) as GameObject;
		//mob.GetComponent<MobProperty> ().timerDeath = 1.5f;
		mob.transform.SetParent (parent);
		mob.transform.localScale = Vector3.one;
		mob.GetComponent<MobProperty> ().type = aleaType;
		mob.GetComponent<Button> ().onClick.AddListener (delegate {
			StartCoroutine (TouchAction (aleaType, mob.transform.GetChild(0)));	
		});

		RectTransform mobRect = (RectTransform)mob.transform;
	}

	IEnumerator TouchAction(int type, Transform mob)
	{
		if (doors[type].GetComponent<DoorsScript>().isOpen) 
		{
			mob.parent.GetComponent<MobProperty> ().isMoving = true;
			GameObject click = Instantiate (clickParticle) as GameObject;
			click.transform.position = mob.parent.position;
			mob.GetChild (0).GetComponent<ParticleSystem> ().Play ();
			iTween.MoveTo (mob.gameObject, iTween.Hash ("x", doors [type].transform.position.x, "y", doors [type].transform.position.y, "time", 0.6, "easeType", iTween.EaseType.easeOutCubic));

			yield return new WaitForSeconds (0.6f);

			MenuManager.instance.FadeIn (mob.gameObject);
			mob.GetChild(0).GetComponent<ParticleSystem> ().Stop();
			scoreManager.addScore (1);
			GameObject explosion = Instantiate (explosionParticle [type]) as GameObject;
			explosion.transform.SetParent(maskWorld.transform.parent);
			explosion.transform.position = mob.position;
		} 
		else 
		{
			ScreenShaker ();
		}
	}

	void ScreenShaker()
	{
		maskWorld.GetComponent<Animation> ().Play ();
	}
}
