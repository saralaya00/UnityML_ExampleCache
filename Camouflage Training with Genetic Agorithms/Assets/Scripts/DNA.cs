using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour {

	public Color rgb = new Color();
	bool stateDeath = false;
	public float timeOfDeath = 0f;

	SpriteRenderer spriteRenderer;
	Collider2D col;

	public float humanSize = 0f;
	public GameObject mutagen;

	void OnMouseDown(){
		stateDeath = true;
		timeOfDeath = PopulationManager.elapsed;
		
		mutagen.SetActive(false);
		spriteRenderer.enabled = false;
		col.enabled = false;
	}

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		
		spriteRenderer.material.color = rgb;

		//Hacky business
		if (humanSize == 0f)
		{
			//Default Value is around 0.3
			humanSize = transform.localScale.x * Random.Range(0.6f, 1f);
		}

		transform.localScale = new Vector3(humanSize, humanSize, humanSize);
	}
	
	public bool IsDead(){
		return stateDeath;
	}
}
