using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA_Senses
{
public class DNA
{
	List<int> genes = new List<int>();

	int dnaLength = 0;
	int maxValues = 0;
	//public string myKey;
	//public string ancestorKeys;
	public Color color = new Color();

	public DNA(int dnaLength, int maxValues)
	{
		this.dnaLength = dnaLength;
		this.maxValues = maxValues;
		//this.myKey = Random.Range(0.1f, 0.99f).ToString();
		//this.ancestorKeys = ancestorKeys + " " + myKey;
		SetRandom();
	}

	public void SetRandom()
	{
		genes.Clear();
		for (int i=0; i < dnaLength; i++)
		{
			genes.Add(Random.Range(0, maxValues));
		}

		color = Random.ColorHSV();
	}

	public void SetInt(int pos, int value)
	{
		genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2)
	{
		int crossPoint = Random.Range(0, dnaLength);

		for (int i = 0; i < crossPoint; i++)
		{
			int geneSeq = d1.genes[i];
			genes[i] = geneSeq;
			
		}

		for (int i = crossPoint; i < dnaLength; i++)
		{
			int geneSeq = d2.genes[i];
			genes[i] = geneSeq;
			
		}

		color = (Random.value < 0.5f) ? d1.color : d2.color; 
		//Debug.Log("Combine: 0 " + crossPoint + " "+dnaLength);
	}

	public void Mutate()
	{
		genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
		color = Random.ColorHSV(); 
	}

	public int GetGene(int pos)
	{
		return genes[pos];
	}
}
}

