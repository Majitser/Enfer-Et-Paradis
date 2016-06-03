using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorsScript : MonoBehaviour {

	public RectTransform cooldownBarBg;
	public RectTransform cooldownBar;

	public float maxTimeOpen = 8;
	public float minTimeOpen = 5;
	public float maxTimeClose = 3;
	public float minTimeClose = 2;
	public bool isOpen = true;
	public Sprite[] sprites;

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
				isOpen = false;
				closeTime = Random.Range (minTimeClose, maxTimeClose);
				cooldownBarBg.gameObject.SetActive (false);
				this.gameObject.GetComponent<Image> ().sprite = sprites [0];
			}

			if (closeTime <= 0 && !isOpen) 
			{
				isOpen = true;
				openTime = Random.Range (minTimeOpen, maxTimeOpen);
				startCooldownDoor = openTime;
				cooldownBarBg.gameObject.SetActive (true);
				cooldownBar.offsetMin = Vector2.zero;
				this.gameObject.GetComponent<Image> ().sprite = sprites [1];
			}
		}
	}

	public void restartDoor()
	{
		openTime = Random.Range (minTimeOpen, maxTimeOpen);
		startCooldownDoor = openTime;
		cooldownBar.offsetMin = Vector2.zero;
		this.gameObject.GetComponent<Image> ().sprite = sprites [1];
	}
}
