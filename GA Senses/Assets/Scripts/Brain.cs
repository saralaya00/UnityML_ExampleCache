using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GA_Senses
{
	public class Brain : MonoBehaviour 
	{
		public int DNALength = 2;
		public float timeAlive;
		public float timeWalking;
		
		public DNA dna;
		public GameObject eyes;

		private bool isSeeingGround = true;
		bool isAlive = true;

		public GameObject ethanPrefab;
		private GameObject ethanInstance;
		public GameObject mutagen;


		void OnDestroy()
		{
			Destroy(ethanInstance);			
		}

		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.tag == "dead")
			{
				isAlive = false;
				timeAlive = 0;
				timeWalking = 0;
			}		
		}

		public void Init()
		{
			//Initialize DNA
			//0-Forward, 1-Left, 2-Right
			dna = new DNA(DNALength, 3);
			timeAlive = 0;
			isAlive = true;

			if (ethanPrefab != null)
			{
				ethanInstance = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
				ethanInstance.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;	
			}
		}

		void Update()
		{
			if (!isAlive) this.enabled = false;

			Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
			isSeeingGround = false;

			RaycastHit hit;
			if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
			{
				if (hit.collider.gameObject.tag == "platform")
				{
					isSeeingGround = true;
				}
			}

			timeAlive = PopulationManager.elapsedTime;

			//read DNA
			float rotationZ = 0f;
			float velocityZ = 0f;

			if (isSeeingGround)
			{
				//velocityZshould be relative to character, to move forward
				switch(dna.GetGene(0))
				{
					case 0: {velocityZ = 1; timeWalking++; break;}
					case 1: rotationZ = -90; break;
					case 2 : rotationZ = 90; break;
				}
			}

			else
			{
				switch(dna.GetGene(1))
				{
					case 0: {velocityZ = 1; timeWalking++; break;}
					case 1: rotationZ = -90; break;
					case 2 : rotationZ = 90; break;
				}
			}

			this.transform.Translate(0,0,velocityZ* 0.1f);
			this.transform.Rotate(0,rotationZ,0);
		}
	}
}
