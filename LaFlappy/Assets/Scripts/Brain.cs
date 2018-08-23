using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaFlap
{
	public class Brain : MonoBehaviour 
	{
		public int DNALength = 5;
		public DNA dna;
		public GameObject eyes;

		//Location Bools
		bool seeingBottomWall = false;
		bool seeingTopWall = false;
		bool seeingBottom = false;
		bool seeingTop = false;

		Vector3 startingPos;

		public float timeAlive;
		public float distanceTravelled = 0f;
		public int crash = 0;

		bool isAlive = true;
		Rigidbody2D rb;
		public GameObject mutagen;

		public void Init()
		{
			//Initialize DNA
			//0-Forward, 1-UpWall, 2-DownWall, 3-Normal Up
			dna = new DNA(DNALength, 200);
			this.transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
			startingPos = transform.position;
			rb = GetComponent<Rigidbody2D>();
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			switch(other.gameObject.tag)
			{
				case "dead": isAlive = false; break;
			}
		}

		void OnCollisionStay2D(Collision2D other)
		{
			switch(other.gameObject.tag)
			{
				case "top":
				case "bottom":
				case "arrowTop":
				case "arrowBottom": crash++; break;
				case "dead": isAlive = false; break;
			}
		}

		void Update()
		{
			if (!isAlive) this.enabled = false;

			seeingBottomWall = false;
			seeingTopWall = false;
			seeingBottom = false;
			seeingTop = false;
			
			RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, dna.rayLength);

			Debug.DrawRay(eyes.transform.position, eyes.transform.forward * dna.rayLength, Color.red);
			Debug.DrawRay(eyes.transform.position, eyes.transform.up * dna.rayLength, Color.red);
			Debug.DrawRay(eyes.transform.position, -eyes.transform.up * dna.rayLength, Color.red);
			
			if (hit.collider != null)
			{
				switch(hit.collider.gameObject.tag)
				{
					case "arrowTop" : 	seeingTopWall = true; break;
					case "arrowBottom": seeingBottomWall = true; break;
				}
			}
			
			hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, dna.rayLength);
			if (hit.collider != null)
			{
				if (hit.collider.tag == "top") seeingTop = true;
			}

			hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, dna.rayLength);
			if (hit.collider != null)
			{
				if (hit.collider.tag == "bottom") seeingBottom = true;
			}

			timeAlive = PopulationManager.elapsedTime;
		}

		void FixedUpdate()
		{
			if (!isAlive) this.enabled = false;

			float forceUp = 0;
			float forceForward = dna.moveSpeed;

			if (seeingTopWall)
			{
				forceUp = dna.GetGene(0);
			}

			else if (seeingBottomWall)
			{
				forceUp = dna.GetGene(1);
			}

			else if (seeingTop)
			{
				forceUp = dna.GetGene(2);
			}

			else if (seeingBottom)
			{
				forceUp = dna.GetGene(3);
			}

			else
			{
				forceUp = dna.GetGene(4);
			}

			rb.AddForce(transform.right * forceForward);
			rb.AddForce(transform.up * forceUp * .1f);

			distanceTravelled = Vector3.Distance(startingPos, transform.position);
		}
	}
}
