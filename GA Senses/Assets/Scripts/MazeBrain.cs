using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GA_Senses
{
	public class MazeBrain : MonoBehaviour 
	{
		public int DNALength = 2;
		Vector3 startPosition;
		public float distanceWalked;
		
		public DNA dna;
		public GameObject eyes;

		private bool isSeeingWall = true;
		bool isAlive = true;

		public GameObject mutagen;
		//public string ancestorKeys;

		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.tag == "dead")
			{
				isAlive = false;
				distanceWalked = 0;
			}		
		}

		public void Init()
		{
			//Initialize DNA
			//0-Forward, 1-Left, 2-Right
			dna = new DNA(DNALength, 360);
			//ancestorKeys = dna.ancestorKeys;
			startPosition = this.transform.position;
			isAlive = true;
		}

		void Update()
		{
			if (!isAlive) this.enabled = false;

			//Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
			isSeeingWall = false;

			RaycastHit hit;
			if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f))
			{
				if (hit.collider.gameObject.tag == "wall")
				{
					isSeeingWall = true;
				}
			}

		}

		void FixedUpdate()
		{
			if (!isAlive) this.enabled = false;

			//read DNA
			float h = 0f;
			float v = dna.GetGene(0);

			if (isSeeingWall)
			{
				h = Random.Range(0, dna.GetGene(1));
			}

			this.transform.Translate(0,0,v * 0.0003f);
			this.transform.Rotate(0,h,0);

			float currentDistance = Vector3.Distance(startPosition, this.transform.position);
			if (currentDistance > distanceWalked)
				distanceWalked = currentDistance;
		}
	}
}
