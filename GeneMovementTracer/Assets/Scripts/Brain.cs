using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour 
{
	public int DNALength = 1;
	public float timeAlive;
	public DNA dna;

	private ThirdPersonCharacter m_Character;
	[HideInInspector]
	public Vector3 m_Move;
	private bool m_Jump;

	bool isAlive = true;

	//mutagen GO here since DNA is a non Monobehaviour class
	public GameObject mutagen;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "dead")
		{
			isAlive = false;
		}		
	}

	public void Init()
	{
		//Initialize DNA
		//0-Forward, 1-Back, 2-Left, 3-Right, 4-Jump, 5-Crouch
		dna = new DNA(DNALength, 6);
		m_Character = GetComponent<ThirdPersonCharacter>();
		timeAlive = 0;
		isAlive = true;
	}

	void FixedUpdate()
	{
		//Horizontal / Vertical Movement
		float h = 0;
		float v = 0;
		bool crouch = false;

		switch(dna.GetGene(0))
		{
			case 0: v =  1; break;
			case 1: v = -1; break;
			case 2: h = -1; break;
			case 3: h =  1; break;
			case 4: m_Jump = true; break;
			case 5: crouch = true; break;
		}

		m_Move = Vector3.forward * v + Vector3.right * h;
		m_Character.Move(m_Move, crouch, m_Jump);
		m_Jump = false;

		if (isAlive)
			timeAlive += Time.deltaTime; 

	}
}
