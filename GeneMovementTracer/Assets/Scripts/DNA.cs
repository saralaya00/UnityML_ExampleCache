using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
	List<int> genes = new List<int>();

	int dnaLength = 0;
	int maxValues = 0;

	public DNA(int dnaLength, int maxValues)
	{
		this.dnaLength = dnaLength;
		this.maxValues = maxValues;

		SetRandom();
	}

	public void SetRandom()
	{
		genes.Clear();
		for (int i=0; i < dnaLength; i++)
		{
			genes.Add(Random.Range(0, maxValues));
		}
	}

	public void Combine(DNA d1, DNA d2)
	{
		for (int i=0; i < dnaLength; i++)
		{
			//Split the dna in half based on i, and assign it to the gene
			if (i < dnaLength / 2.0f)
			{
				int geneSeq = d1.genes[i];
				genes[i] = geneSeq;
			}

			else
			{
			int geneSeq = d2.genes[i];
				genes[i] = geneSeq;
			}
		}
	}

	public void SetInt(int pos, int value)
	{
		genes[pos] = value;
	}
	
	public void Mutate()
	{
		genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
	}

	public int GetGene(int pos)
	{
		return genes[pos];
	}
}
