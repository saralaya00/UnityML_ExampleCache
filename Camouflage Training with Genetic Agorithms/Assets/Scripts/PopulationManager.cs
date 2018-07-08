using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

	public GameObject personPrefab;
	public int populationSize = 10;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;

	int trialTime = 10;
	int generation = 1;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 50;
		guiStyle.normal.textColor = Color.white;	
		GUI.Label(new Rect(10,10,100,20), "Generation: " + generation, guiStyle);
		GUI.Label(new Rect(10,65,100,20), "Trial Time: " + (int)elapsed, guiStyle);
	}

	// Use this for initialization
	void Start () 
	{
		for	(int i=0; i< populationSize; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-9f,9f), Random.Range(-2.5f,5f),0f);
			GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
			
			Color rgb = new Color();
			rgb.r = Random.Range(0.0f, 1.0f);
			rgb.g = Random.Range(0.0f, 1.0f);
			rgb.b = Random.Range(0.0f, 1.0f);
			go.GetComponent<DNA>().rgb = rgb;
			population.Add(go);
		}
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if (elapsed > trialTime)
		{
			BreedNewPopulation();
			elapsed = 0;
		}
	}

	void BreedNewPopulation()
	{
		List <GameObject> newPopulation = new List<GameObject>();
		
		// Removing unfit Individuals
		/*List<GameObject> sortedList = population.OrderBy(
			o => o.GetComponent<DNA>().timeOfDeath).ToList();*/

		//Remove people who arn't Alive
		population.RemoveAll(o => o.GetComponent<DNA>().IsDead() == true);
		List <GameObject> sortedList = population.ToList();
		

		population.Clear();

		switch(sortedList.Count)
		{
			case 0:{
				Application.Quit();
				break;
			}

			case 1:	{
				for (int i =0; i < populationSize / 2; i++)
				{
					population.Add(Breed(sortedList[0], sortedList[0]));
					population.Add(Breed(sortedList[0], sortedList[0]));
				}
					
				break;
			}

			case 2:	{
				for (int i =0; i < populationSize / 2; i++)
				{
					population.Add(Breed(sortedList[0], sortedList[1]));
					population.Add(Breed(sortedList[0], sortedList[1]));
				}
				break;
			}

			default:{
				//Note this Section selects the sotedList[mid] and greater
				//Requires repair
				for (int i = (int) (sortedList.Count / 2) - 1; i < sortedList.Count -1; i++)
				{
					population.Add(Breed(sortedList[i], sortedList[i-1]));
					population.Add(Breed(sortedList[i+1], sortedList[i]));
				}

				break;
			}
		}

		foreach (GameObject go in sortedList)
		{
			Destroy(go);
		}
		generation++;
	}

	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		Vector3 pos = new Vector3(Random.Range(-9f,9f), Random.Range(-2.5f,5f),0f);
		GameObject child = Instantiate(personPrefab, pos, Quaternion.identity);
	
		Color colP1 = parent1.GetComponent<DNA>().rgb;	
		Color colP2 = parent2.GetComponent<DNA>().rgb;
		float childSize;

		Color hybridColor = new Color();

		if(Random.value < 0.8f)
		{
			hybridColor.r = Random.value < 0.5f ? colP1.r : colP2.r;
			hybridColor.g = Random.value < 0.5f ? colP1.g : colP2.g;
			hybridColor.b = Random.value < 0.5f ? colP1.b : colP2.b;
			hybridColor.a = 1.0f;

			childSize = (Random.value > 0.5f) ? parent1.GetComponent<DNA>().humanSize : parent2.GetComponent<DNA>().humanSize; 

		}
		else
		{
			//Debug.Log("Mutated!");
			hybridColor.r = Random.value;
			hybridColor.g = Random.value;
			hybridColor.b = Random.value;
			hybridColor.a = 0.5f;

			childSize = Random.Range(0.2f, 0.3f);
			child.GetComponent<DNA>().mutagen.SetActive(true);
		}

		child.GetComponent<DNA>().rgb = hybridColor;
		child.GetComponent<DNA>().humanSize = childSize; 
		
		return child;
	}
}
