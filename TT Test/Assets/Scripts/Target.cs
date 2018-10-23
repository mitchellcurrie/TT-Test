using System.Collections;
using UnityEngine;

/// Moves the game object this script is attached to, towards the EndPos set in inspector
// If collided with, it destorys itself and adds to the score.
public class Target : MonoBehaviour 
{
	
	public float SpeedMPS;
	private enum targetType{Horizontal, Vertical};  // target types
	private targetType type;

	// Spawning boundaries
	private const float spawnXBoundary = 20.0f;
	private const float lowerZSpawnBoundary = -3.0f;
	private const float upperZSpawnBoundary = 12.0f;

	private Vector3 StartPos;
	private Vector3 EndPos;
	private const float playerSafeZone = 3.0f; // the space on the x axis either side of the player weapon where vertical enemies can't spawn

	void Start() 
	{
		// Get type, start and end positions, and color
		SetRandomType();
		SetMovementPosition();
		SetColor();
		transform.position = StartPos;
	}
	void Update()
	{
		Move();
	}
	
	void OnTriggerEnter(Collider other)
	{
		// Score increases when you hit a green horizontal target, and decreases by 3 if you hit a red vertical flying target
		if (type == targetType.Horizontal)
		{
			Score.Total++;
		}

		else if (type == targetType.Vertical)
		{
			Score.Total -= 5;
		}
		
		GameObject.Destroy(gameObject);
	}

	void Move ()
	{
		// Move twowards end position - destroying oursleves when we get there
		Vector3 toTarget = EndPos - transform.position;
		float distanceToMove = SpeedMPS * Time.deltaTime;
		distanceToMove = Mathf.Min(distanceToMove, toTarget.magnitude);
		if (distanceToMove > 0.0f)
		{
			Vector3 heading = Vector3.Normalize(toTarget);
			Vector3 move = heading * distanceToMove;
			transform.position += move;
		}

		else
		{
			Destroy(gameObject);
		}
	}

	void SetRandomType()
	{
		// Assign random target type
		int random = Random.Range(0,2);

		if (targetType.IsDefined(typeof(targetType), random)) // check if random number is within enum scope
		{
			type = (targetType)random;
		}
		else
		{
			Debug.LogError("Random number outside of targetType enum scope");
		}
	}

	void SetMovementPosition()
	{
		if (type == targetType.Horizontal)
		{
			StartPos = new Vector3 (-spawnXBoundary, 0, Random.Range(lowerZSpawnBoundary, upperZSpawnBoundary));
			EndPos = new Vector3 (spawnXBoundary, 0, StartPos.z);
		}

		else if (type == targetType.Vertical)
		{
			// Randomly spawn on left or right of player - can't spawn with same x as player, as the red target will collide
			// with the player even if they don't shoot.
			int spawnSide = Random.Range(0,2);
			
			// Spawn point (StartPos) and EndPos are based on boundaries
			if (spawnSide == 0)
			{
				StartPos = new Vector3 (Random.Range(-spawnXBoundary, 0 - playerSafeZone), 0, spawnXBoundary);
			}
			else 
			{
				StartPos = new Vector3 (Random.Range(0 + playerSafeZone, spawnXBoundary), 0, spawnXBoundary);
			}
			
			EndPos = new Vector3 (StartPos.x, 0, lowerZSpawnBoundary);
		}
	}
	void SetColor()
	{
		// Assign colors based on type
		if (type == targetType.Horizontal)
		{
			GetComponent<MeshRenderer>().material.color = Color.green;
		}

		if (type == targetType.Vertical)
		{
			GetComponent<MeshRenderer>().material.color = Color.red;
		}
	}
}
