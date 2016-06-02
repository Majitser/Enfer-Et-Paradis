using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public static MenuManager instance;

	// Use this for initialization
	void Start () {
		if (instance == null) 
		{
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		} 
		else
			DestroyImmediate (this.gameObject);
	}

	public void FadeIn(GameObject go)
	{
		StartCoroutine (animFade(go, true));
	}

	public void FadeOut(GameObject go)
	{
		StartCoroutine (animFade (go, false));
	}

	IEnumerator animFade(GameObject go, bool isIn)
	{
		float t = 0f;
		float alpha = 0f;

		if (isIn)
			alpha = 1f;
		else
			go.SetActive (true);

		while (t < 1.0f) 
		{
			t += 3 * Time.deltaTime;
			if (isIn)
				alpha = Mathf.SmoothStep (1.0f, 0.0f, t);
			else
				alpha = Mathf.SmoothStep (0.0f, 1.0f, t);

			go.GetComponent<CanvasGroup> ().alpha = alpha;

			yield return true;
		}

		if(isIn)
			go.SetActive (false);
	}
}
