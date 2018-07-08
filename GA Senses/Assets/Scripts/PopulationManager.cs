using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GA_Senses
{
	public class PopulationManager : MonoBehaviour {

		public GameObject botPrefab;
		public int populationSize = 50;
		List<GameObject> population = new List<GameObject>();
		public static float elapsedTime = 0;
		public float trialTime = 5;
		int generation = 1;

		GUIStyle guiStyle = new GUIStyle();

		void OnGUI()
		{
			guiStyle.fontSize = 25;
			guiStyle.normal.textColor = Color.white;

			GUI.BeginGroup(new Rect(10, 10, 250, 150));
			
			GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
			GUI.Label(new Rect(10, 25, 200, 30), "Gen " + generation, guiStyle);
			GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsedTime), guiStyle);
			GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);

			GUI.EndGroup();	
		}
		// Use this for initialization
		void Start () {
			for (int i = 0; i < populationSize; i++)
			{
				Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-6,6),
					this.transform.position.y,
					this.transform.position.z + Random.Range(-6,6));

				GameObject b = Instantiate(botPrefab, startingPos, this.transform.rotation);
				b.GetComponent<Brain>().Init();
				population.Add(b);
			}
		}

		GameObject Breed(GameObject parent1, GameObject parent2)
		{
			Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-6,6),
				this.transform.position.y,
				this.transform.position.z + Random.Range(-6,6));

			GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
			Brain b = offspring.GetComponent<Brain>();

			//Mutation
			if (Random.Range(0, 100) == 1)
			{
				b.Init();
				b.dna.Mutate();
				b.mutagen.SetActive(true);
			}

			else
			{
				b.Init();
				b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
			}

			return offspring;	
		}
		
		void BreedNewPopulation()
		{
			
			List<GameObject> sortedList = population.OrderBy(
				o => (o.GetComponent<Brain>().timeAlive * 5f + o.GetComponent<Brain>().timeWalking))
				.ToList();
			/*
			List<GameObject> sortedList = population.OrderBy(
				o => Vector3.Distance(o.GetComponent<Brain>().m_Move, transform.position))
				.ToList();
			*/

			population.Clear();

			for (int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
			{
				population.Add(Breed(sortedList[i], sortedList[i+1]));
				population.Add(Breed(sortedList[i+1], sortedList[i]));
			}

			//Destroy previous population
			for (int i = 0; i < sortedList.Count; i++)
				Destroy(sortedList[i]);

			generation++;
		}
		// Update is called once per frame
		void Update () {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= trialTime)
			{
				BreedNewPopulation();
				elapsedTime = 0;
			}
		}
	}
}

