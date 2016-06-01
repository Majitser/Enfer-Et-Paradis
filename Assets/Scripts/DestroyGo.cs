using UnityEngine;
using System.Collections;

public class DestroyGo : MonoBehaviour {

	public float lifeTime = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (lifeTime > 0)
			lifeTime -= Time.deltaTime;
		else
			Destroy (this.gameObject);
	}
}
