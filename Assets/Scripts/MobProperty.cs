using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobProperty : MonoBehaviour {

	public float maxSpeedY = 1f;
	public float minSpeedY = 1f;
	public float maxSpeedX = 0.2f;
	public float minSpeedX = 0f;
	public int type = 0;
	public Texture2D[] dragonImages;
	public RuntimeAnimatorController[] dragonAnimators;
	public Color[] colorsParticle;
	public float timerDeath = 3;
	[HideInInspector]
	public bool isMoving = false;

	RectTransform mobRect;
	bool isDying = false;
	float speedX;
	float speedY;

	// Use this for initialization
	void Start () {
		mobRect = (RectTransform)this.transform;
		this.transform.GetChild(0).GetComponent<Image> ().sprite = Sprite.Create(dragonImages[type], new Rect(0,0,dragonImages[type].width, dragonImages[type].height), new Vector2(0.5f, 0.5f));
		this.transform.GetChild (0).GetChild(0).GetComponent<ParticleSystem> ().startColor = colorsParticle [type];
		this.transform.GetChild (0).GetComponent<Animator> ().runtimeAnimatorController = dragonAnimators [type];
		speedX = Random.Range (minSpeedX, maxSpeedX);
		speedY = Random.Range (minSpeedY, maxSpeedY);
		iTween.ScaleTo (this.transform.GetChild (0).gameObject, iTween.Hash ("x", 1, "y", 1, "z", 1, "time", 0.2, "easeType", iTween.EaseType.easeOutQuad));

	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.GetChild (0).GetComponent<Animator> ().SetBool ("isMoving", isMoving);

		if (timerDeath > 0)
			timerDeath -= Time.deltaTime;
		else if (timerDeath <= 0 && !isMoving && !isDying)
			StartCoroutine (deathAnim ());
		else if (timerDeath <= 0 && isMoving && !this.transform.GetChild(0).gameObject.activeSelf)
			StartCoroutine (deathAnim ());
	}

	public void Move()
	{
		mobRect.anchoredPosition -= new Vector2 (speedX, speedY);
	}

	IEnumerator deathAnim()
	{
		isDying = true;
		this.gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		iTween.ScaleTo (this.transform.GetChild (0).gameObject, iTween.Hash ("x", 0, "y", 0, "z", 0, "time", 0.2, "easeType", iTween.EaseType.easeOutQuad));

		yield return new WaitForSeconds (0.2f);
		Destroy (this.gameObject);
	}
}
