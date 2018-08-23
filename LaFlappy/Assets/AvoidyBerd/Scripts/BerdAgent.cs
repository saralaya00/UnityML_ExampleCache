using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//

public class BerdAgent : Agent
{
	public float rayLength = 100f;
	float force = 10.0f;
	Vector3 startPosition;
	Rigidbody2D rb;
	Collider2D col;
	bool crash = false;

	public override void InitializeAgent()
	{
		startPosition = this.transform.position;
		rb = this.GetComponent<Rigidbody2D>();
		col = this.GetComponent<Collider2D>();
	}

	// public override void CollectObservations()
	// {
	// 	RaycastHit2D rayTopHit2D, rayBottomHit2D; // rayLeftHit2D ...		
		
	// 	rayTopHit2D = Physics2D.Raycast(transform.position, Vector3.up, rayLength);
	// 	rayBottomHit2D = Physics2D.Raycast(transform.position, Vector3.down, rayLength);

	// 	AddVectorObs(rayTopHit2D.distance);
	// 	AddVectorObs(rb.velocity.x);
	// 	AddVectorObs(rb.velocity.y);
	// }

	public override void CollectObservations()
	{
		/*
		AddVectorObs(Vector3.Distance(topBorder.transform.position, this.transform.position));
		AddVectorObs(Vector3.Distance(bottomBorder.transform.position, this.transform.position));
		AddVectorObs(Vector3.Distance(leftBorder.transform.position, this.transform.position));
		AddVectorObs(Vector3.Distance(rightBorder.transform.position, this.transform.position));
*/
		RaycastHit2D rayTopHit2D;
		RaycastHit2D rayBottomHit2D;		
		RaycastHit2D rayLeftHit2D;
		RaycastHit2D rayRightHit2D;
		
		rayTopHit2D = Physics2D.Raycast(transform.position, Vector3.up, rayLength);
		rayBottomHit2D = Physics2D.Raycast(transform.position, Vector3.down, rayLength);
		rayLeftHit2D = Physics2D.Raycast(transform.position, Vector3.left, rayLength);
		rayRightHit2D = Physics2D.Raycast(transform.position, Vector3.right, rayLength);

		AddVectorObs(rayTopHit2D.distance);
		AddVectorObs(rayBottomHit2D.distance);
		AddVectorObs(rayLeftHit2D.distance);
		AddVectorObs(rayRightHit2D.distance);

		//Debug.Log(rayLeft.distance);
		
		Debug.DrawRay(transform.position, Vector3.up * rayLength, Color.white);
		Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white);
		Debug.DrawRay(transform.position, Vector3.left * rayLength, Color.white);
		Debug.DrawRay(transform.position, Vector3.right * rayLength, Color.white);

		AddVectorObs(rb.velocity.x);
		AddVectorObs(rb.velocity.y);
	}

	public override void AgentAction(float[] actions, string textAction)
	{
		int action = (int) actions[0];

		if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
		{
			//SpaceType.discrete reduces the Action down to a single valued integer.
			// Preferred when there are supposed be choices
			switch(action)
			{
				case 0: rb.AddForce(this.transform.up * force); break;
				case 1: rb.AddForce(this.transform.up * -force); break;
				case 2: rb.AddForce(this.transform.right * force); break;
				case 3: rb.AddForce(this.transform.right * -force); break;
			}

			SetReward(0.1f);
		}

		else
		{
			rb.AddForce(this.transform.up * force * actions[0]);
			rb.AddForce(this.transform.up * -force * actions[1]);
			rb.AddForce(this.transform.right * force * actions[2]);
			rb.AddForce(this.transform.right * -force * actions[3]);
		
			SetReward(0.1f);
		}

		if (crash)
		{
			Done();
			SetReward(-1.0f);
		}
	}

	public override void AgentReset()
	{
		this.transform.position = startPosition;
		rb.velocity = Vector3.zero;
		crash = false;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		crash = true;
	}
}
