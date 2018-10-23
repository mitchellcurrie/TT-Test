using System.Collections;
using UnityEngine;

/// Moves the game object this script is attached to, towards the EndPos set in inspector
// If collided with, it destorys itself and adds to the score.
public class Target : MonoBehaviour 
{
	public Vector3 EndPos;
	public float SpeedMPS;

	void Update ()
	{
		// Move twowards end position - destroying oursleves when we get ther
		Vector3 toTarget = EndPos - transform.position;
		float distanceToMove = SpeedMPS * Time.deltaTime;
		distanceToMove = Mathf.Min(distanceToMove, toTarget.magnitude);
		if (distanceToMove > 0.0f)
		{
			Vector3 heading = Vector3.Normalize(toTarget);
			Vector3 move = heading * distanceToMove;
			transform.position += move;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		Score.Total++;
		
		GameObject.Destroy(gameObject);
	}
}
