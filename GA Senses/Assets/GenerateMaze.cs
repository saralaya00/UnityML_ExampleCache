using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour {

	public GameObject blockPrefab;
	public List<GameObject> blockInstances;
	int width = 40;
	int depth = 40;

	// Use this for initialization
	public void CreateMaze () {
		for(int w = 0; w < width; w++)
		{
			for(int d = 0; d < depth; d++)
			{
				if( w == 0 || d == 0)	//outside walls bottom and left
				{
					Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y, d + this.transform.position.z), Quaternion.identity);
				}	
				else if( w < 7 && d < 7)
				{
					continue;
				}		
				else if(w == width-1 || d == depth-1) //outside walls top and right
				{
					Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y,d + this.transform.position.z), Quaternion.identity);
				}
				else if(Random.Range(0,5) < 1)
				{
					blockInstances.Add(Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y,d + this.transform.position.z), Quaternion.identity));
				}
			}	
		}
	}

	public void RepositionMaze()
	{
		int totalBlocks = blockInstances.Count;
		int blockI = 0;

		for (int w = 0; w < width; w++)
		{
			for (int d = 0; d < depth; d++)
			{
				if (w == 0 && d == 0)
				{
					continue;
				}

				else if (w < 7 && d < 7)
				{
					continue;
				}

				else if(w == width-1 || d == depth-1) 
				{
					continue;
				}

				else 
				{
					if (blockI < totalBlocks && Random.Range(0,5) < 1)
					{
						blockInstances[blockI].transform.position = new Vector3(w + this.transform.position.x, this.transform.position.y,d + this.transform.position.z);
						blockI++;
					}
				}
			}
		}
	}
}
