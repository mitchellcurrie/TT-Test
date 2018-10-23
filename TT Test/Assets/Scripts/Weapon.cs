using UnityEngine;
using System.Collections;

/// Pullback and release weapon
/// Works by user clicking, holding and pulling back on the gameobject this script is attached to. 
/// Letting go then fires in the opposite direction wo which it was pulled back.
public class Weapon : MonoBehaviour 
{
	public float FireTime = 1.0f;
	public float FireSpeed = 40.0f;
	
	enum State
	{
		ready,
		beingPulledBack,
		fired,
	};
	State currentState = State.ready;
	float timeInCurrentState;
	
	Vector3 startPos;
	
	// Velocity set each time we're fired
	Vector3 fireVelocity;
	
	// Unity calls this once on strt up
	void Start()
	{
		startPos = transform.position;
	}
	
	// Unity calls this once per frame
	void Update()
	{
		State previousState = currentState;
		State newState = previousState;
		
		switch (previousState)
		{
			
		case State.ready:
			newState = UpdateReady();
			break;
			
		case State.beingPulledBack:
			newState = UpdateBeingPulledBack();
			break;
			
		case State.fired:
			newState = UpdateFired();
			break;
		}		
		
		if (newState != previousState)
		{
			timeInCurrentState = 0.0f;
			currentState = newState;
		}
		else
		{
			timeInCurrentState += Time.deltaTime;
		}
		
		UpdateRotationAndScale();
	}
	
	State UpdateReady()
	{
		// As soon as mouse click is down on top of us then go to pulledBack state
		if (MousePosTouchesUs())
		{
			return State.beingPulledBack;
		}
		
		return State.ready;
	}
	
	State UpdateBeingPulledBack()
	{
		// if no longer being held then fire
		if (!Input.GetMouseButton(0))
		{
			SetFireVelocity();
			return State.fired;
		}
		
		// Position ourselves halfway between start position and mouse position
		transform.position = MouseGroundPosition() + startPos;
		transform.position /= 2;
		
		return State.beingPulledBack;
	}
	
	void SetFireVelocity()
	{
		fireVelocity = startPos - transform.position;
		fireVelocity.Normalize();
		fireVelocity *= FireSpeed;
	}
	
	State UpdateFired()
	{		
		// move
		transform.position += fireVelocity * Time.deltaTime;
		
		// check if we've been fired long enough to stop
		if (timeInCurrentState > FireTime)
		{
			transform.position = startPos;
			return State.ready;
		}
		
		return State.fired;
	}
	
	/// Is the current mouse position over our gameobject
	bool MousePosTouchesUs()
	{
		if (Input.GetMouseButton(0))
		{
			// cast a ray from the camera at the mouse position
			// and see if it intersects the collider component of this gameobject
			Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;			
			bool hit = this.GetComponent<Collider>().Raycast(rayFromMouse, out hitInfo, 100.0f);
			
			return hit;
		}
			
		return false;		
	}
	
	/// get the position of the mouse on the ground plane
	Vector3 MouseGroundPosition()
	{
		// Using a plane representing flayt ground at 0 height
		// take a ray from the mouse position and see where it hits the ground
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitDistance;
		groundPlane.Raycast(rayFromMouse, out hitDistance);
		Vector3 groundPos = rayFromMouse.GetPoint(hitDistance);
		
		return groundPos;
	}
	
	/// rotate and scale to help show direction we'll be fired in
	void UpdateRotationAndScale()
	{
		// when not being pulled back use no scale or rotation
		if (currentState != State.beingPulledBack)
		{
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			return;
		}
		
		Vector3 pullBackVector = transform.position - startPos;
		
		// scale to stretch between mouse position and start position
		float pullBackDistance = pullBackVector.magnitude * 2;
		Vector3 scale = Vector3.one;
		scale.z = pullBackDistance + 1;
		transform.localScale = scale;
		
		// rotate to point at start position
		pullBackVector.Normalize();
		transform.LookAt(startPos);
		
	}
}						
