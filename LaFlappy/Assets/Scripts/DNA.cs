using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaFlap
{
	public class DNA
	{
		List<int> genes = new List<int>();

		int dnaLength = 0;
		int maxValues = 0;

		public float rayLength = 1.0f;
		public float moveSpeed = 1.0f;

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
				genes.Add(Random.Range(-maxValues, maxValues));
			}

			rayLength = Random.Range(0.3f, 1f);
			moveSpeed = Random.Range(0.5f, 1.2f);
		}

		public void SetInt(int pos, int value)
		{
			genes[pos] = value;
		}

		public void Combine(DNA d1, DNA d2)
		{
			for (int i=0; i < dnaLength; i++)
			{
				genes[i] = Random.Range(0, 10) < 5 ? d1.genes[i] : d2.genes[i];
			}

			rayLength = (d1.rayLength > d2.rayLength) ? d1.rayLength : d2.rayLength;
			moveSpeed = (d1.moveSpeed > d2.moveSpeed) ? d1.moveSpeed : d2.moveSpeed;
		}

		public void Mutate()
		{
			genes[Random.Range(0, dnaLength)] = Random.Range(-maxValues, maxValues);
			rayLength = Random.Range(0.3f, 1f);
			moveSpeed = Random.Range(0.5f, 1.2f);
		}

		public int GetGene(int pos)
		{
			return genes[pos];
		}
	}
}

