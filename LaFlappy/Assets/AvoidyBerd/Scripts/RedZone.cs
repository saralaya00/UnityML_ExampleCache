using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour {

	public GameObject topBorder;
	public GameObject bottomBorder;
	public GameObject leftBorder;
	public GameObject rightBorder;

	public float checkRate = 10f;
	float waitTime = 0f;
	float nextCheck;
	float moveCheck;
	int ctr = 1;
		
	// Use this for initialization
	void Start () {
		// Time.timeScale = 5;
		nextCheck = Time.time + checkRate;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		waitTime += Time.deltaTime;
		moveCheck = nextCheck - checkRate + .5f;

		if (waitTime > nextCheck || waitTime < moveCheck)
		{
			

			if (!(waitTime < moveCheck))
			{
				nextCheck = Time.time + checkRate;
				ctr = (ctr >= 8) ? 1 : ++ctr;
			}	

			//Border_Train();
			Border_Test();
		}
	}

	void Border_Train()
	{
		float v = Random.Range(0.5f,0.8f);
		float h = Random.Range(0.5f,0.8f);

		switch(ctr)
		{
			case 1: topBorder.transform.Translate(new Vector3(0, -v * Time.deltaTime, 0)); break;
			case 2: bottomBorder.transform.Translate(new Vector3(0, v * Time.deltaTime, 0)); break;
			case 3: leftBorder.transform.Translate(new Vector3(0, -h * Time.deltaTime, 0)); break;
			case 4: rightBorder.transform.Translate(new Vector3(0, h * Time.deltaTime,0)); break;
			case 5: topBorder.transform.Translate(new Vector3(0, v * Time.deltaTime, 0)); break;
			case 6: bottomBorder.transform.Translate(new Vector3(0, -v * Time.deltaTime, 0)); break;
			case 7: leftBorder.transform.Translate(new Vector3(0, h * Time.deltaTime, 0)); break;
			case 8: rightBorder.transform.Translate(new Vector3(0, -h * Time.deltaTime,0)); break;
		}
	}

	void Border_Test()
	{
		float v = Random.Range(0.5f,0.8f) * 6;
		float h = Random.Range(0.5f,0.8f) * 12;

		switch(ctr)
		{
			case 1: leftBorder.transform.Translate(new Vector3(0, -h * Time.deltaTime, 0)); break;
			case 2: rightBorder.transform.Translate(new Vector3(0, h * Time.deltaTime,0)); break;
			case 3: topBorder.transform.Translate(new Vector3(0, -v * Time.deltaTime, 0)); break;
			case 4: bottomBorder.transform.Translate(new Vector3(0, v * Time.deltaTime, 0)); break;
			case 5: leftBorder.transform.Translate(new Vector3(0, h * Time.deltaTime, 0)); break;
			case 6: rightBorder.transform.Translate(new Vector3(0, -h * Time.deltaTime,0)); break;
			case 7: topBorder.transform.Translate(new Vector3(0, v * Time.deltaTime, 0)); break;
			case 8: bottomBorder.transform.Translate(new Vector3(0, -v * Time.deltaTime, 0)); break;

		}
	}
}
