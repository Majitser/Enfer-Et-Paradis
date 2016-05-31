using UnityEngine;
using System.Collections;

public class DoorsScript : MonoBehaviour {

	public GameManager gameManager;

	public float maxTimeOpen = 4;
	public float minTimeOpen = 2;
	public float maxTimeClose = 2;
	public float minTimeClose = 0.5f;
	public bool isOpen = true;

	private float openTime;
	private float closeTime;

	// Use this for initialization
	void Start () {
		openTime = Random.Range (minTimeOpen, maxTimeOpen);
	}
	
	// Update is called once per frame
	void Update () {
		if (openTime > 0 && isOpen) 
		{
			openTime -= Time.deltaTime;
			gameManager.cooldownBar.offsetMin -= new Vector2 ((gameManager.cooldownBarBg.rect.width / gameManager.startCooldownDoor) * Time.deltaTime, 0);
		}

		if (closeTime > 0 && !isOpen)
			closeTime -= Time.deltaTime;

		if (openTime <= 0 && isOpen) 
		{
			this.transform.GetChild (0).gameObject.SetActive (true);
			isOpen = false;
			closeTime = Random.Range (minTimeClose, maxTimeClose);
		}

		if (closeTime <= 0 && !isOpen) 
		{
			this.transform.GetChild (0).gameObject.SetActive (false);
			isOpen = true;
			openTime = Random.Range (minTimeClose, maxTimeClose);
		}
	}
}
