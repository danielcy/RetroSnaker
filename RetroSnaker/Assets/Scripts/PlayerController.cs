using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public GameObject bean;

	private MovingState movingState;
	private float cycle;
	private float currentTime;
	private Vector3 direction;
	private Vector3 preDirection;
	private int currentMoveFrame;
	private float maxMoveFrames;

	void Start () 
	{
		cycle = 1;
		currentTime = 0;
		movingState = MovingState.Waiting;
		direction = new Vector3 (-1.0f, 0.0f, 0.0f);
		preDirection = direction;
		currentMoveFrame = 0;
		maxMoveFrames = 10;
	}

	void Update () 
	{
		Debug.Log ("PlayerController");
		SetPreDirection ();
		switch (movingState) 
		{
		case MovingState.Waiting:
			currentTime = currentTime + Time.deltaTime;
			if (currentTime >= cycle) 
			{
				direction = preDirection;
				movingState = MovingState.Moving;
				currentTime = 0;
			}
			break;
		case MovingState.Moving:
			MoveOneStep ();
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log (movingState);
	}

	void MoveOneStep()
	{
		if (currentMoveFrame == maxMoveFrames) 
		{
			currentMoveFrame = 0;
			movingState = MovingState.Waiting;
			FixPosition ();
			return;
		}
		currentMoveFrame = currentMoveFrame + 1;
		transform.Translate (direction * (1 / maxMoveFrames));
	}

	void FixPosition()
	{
		float fixedX = Mathf.Round (transform.position.x);
		float fixedY = transform.position.y;
		float fixedZ = Mathf.Round ( transform.position.z);
		transform.position = new Vector3 (fixedX, fixedY, fixedZ);
	}

	void SetPreDirection()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// To ensure only one key is pressed.
		if (moveHorizontal * moveVertical == 0 && moveHorizontal + moveVertical != 0) {
			//TODO This is not elegent. Can we have another way to change the direction?
			if (moveHorizontal > 0 && direction.x != -1) {
				preDirection.x = 1;
				preDirection.z = 0;
			} else if (moveHorizontal < 0 && direction.x != 1) {
				preDirection.x = -1;
				preDirection.z = 0;
			} else if (moveVertical > 0 && direction.z != -1) {
				preDirection.z = 1;
				preDirection.x = 0;
			} else if (moveVertical < 0 && direction.z != 1) {
				preDirection.z = -1;
				preDirection.x = 0;
			}
		}
	}

	bool NeedToGrow()
	{
		if (transform.position + direction == bean.transform.position) {
			return true;
		} else
			return false;
	}
}

public enum MovingState 
{
	Moving = 0,
	Waiting = 1,
}