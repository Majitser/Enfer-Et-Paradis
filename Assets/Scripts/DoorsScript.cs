using UnityEngine;
using System.Collections;

public class DoorsScript : MonoBehaviour {

	public RectTransform cooldownBarBg;
	public RectTransform cooldownBar;

	public float maxTimeOpen = 8;
	public float minTimeOpen = 5;
	public float maxTimeClose = 3;
	public float minTimeClose = 2;
	public bool isOpen = true;

	private float openTime;
	private float closeTime;
	private float startCooldownDoor;

	// Use this for initialization
	void Start () {
		restartDoor ();
	}
	
	// Update is called once per frame
	void Update () {
		if (StatesManager.GameStates == StatesManager.states.GAME) 
		{
			if (openTime > 0 && isOpen) 
			{
				openTime -= Time.deltaTime;
				cooldownBar.offsetMin += new Vector2 ((cooldownBarBg.rect.width / startCooldownDoor) * Time.deltaTime, 0);
			}

			if (closeTime > 0 && !isOpen)
				closeTime -= Time.deltaTime;

			if (openTime <= 0 && isOpen) 
			{
				this.transform.GetChild (0).gameObject.SetActive (true);
				isOpen = false;
				closeTime = Random.Range (minTimeClose, maxTimeClose);
				cooldownBarBg.gameObject.SetActive (false);
			}

			if (closeTime <= 0 && !isOpen) 
			{
				this.transform.GetChild (0).gameObject.SetActive (false);
				isOpen = true;
				openTime = Random.Range (minTimeOpen, maxTimeOpen);
				startCooldownDoor = openTime;
				cooldownBarBg.gameObject.SetActive (true);
				cooldownBar.offsetMin = Vector2.zero;
			}
		}
	}

	public void restartDoor()
	{
		openTime = Random.Range (minTimeOpen, maxTimeOpen);
		startCooldownDoor = openTime;
		cooldownBar.offsetMin = Vector2.zero;
		this.transform.GetChild (0).gameObject.SetActive (false);
	}
}
