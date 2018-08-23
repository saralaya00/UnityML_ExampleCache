using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;
using UnityEngine.UI;

namespace CollectorML
{
	public class CollectorAcademy : Academy
	{
		public GameObject[] agents;
		public CollectorArea[] CollectorAreas;

		public int totalScore;
		public Text scoreText;
		
		public override void AcademyReset()
		{
			DestroyObjects(GameObject.FindGameObjectsWithTag("good-coin"));
			DestroyObjects(GameObject.FindGameObjectsWithTag("bad-coin"));

			agents = GameObject.FindGameObjectsWithTag("agent");
			CollectorAreas = FindObjectsOfType<CollectorArea>();

			foreach (CollectorArea ca in CollectorAreas)
			{
				ca.ResetCollecterArea(agents);
			}

			totalScore = 0;
		}

		private void DestroyObjects(GameObject[] gameObjects)
		{
			foreach (GameObject go in gameObjects)
			{
				Destroy(go);
			}
		}
		
		public override void AcademyStep()
		{
			scoreText.text = "Score: " + totalScore;
		}
	}	
}