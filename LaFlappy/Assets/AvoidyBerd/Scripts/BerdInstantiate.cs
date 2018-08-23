using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerdInstantiate : MonoBehaviour 
{
	public GameObject badBerdPrefab;
	public float startTime = 0f;
	public float interval = 10f;
	public List<GameObject> locations;
	public int count;

	void Spawn()
	{
		for (int i = 0; i < count; i++)
		{
			GameObject loc = locations[Random.Range(0, locations.Count)];
			GameObject badBerd =  Instantiate(badBerdPrefab, loc.transform.position, Quaternion.identity);
			badBerd.GetComponent<Rigidbody2D>().AddForce(loc.transform.position * 100);
			Destroy(badBerd, interval * .6f);
		}
	}

	private void Start()
	{
		// InvokeRepeating("Spawn", startTime, interval);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			GameObject badBerd = Instantiate(badBerdPrefab, pos, Quaternion.identity);
			Destroy(badBerd, 10f);
		}
	}
}
