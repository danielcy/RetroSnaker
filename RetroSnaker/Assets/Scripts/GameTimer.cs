using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {
	private float cycle;
	private float currentTime;
	private int currentMoveFrame;
	private float maxMoveFrames;
	private MovingState movingState;
	private bool firstStepOfMoving;
	private bool lastStepOfMoving;

	void Start () 
	{
		cycle = 0.7f;
		currentTime = 0;
		currentMoveFrame = 0;
		maxMoveFrames = 12;
		movingState = MovingState.Waiting;
		firstStepOfMoving = false;
		lastStepOfMoving = false;
	}

	void Update () 
	{
		switch (movingState) {
		case MovingState.Waiting:
			currentTime = currentTime + Time.deltaTime;
			if (currentTime >= cycle) {
				movingState = MovingState.Moving;
				currentTime = 0;
				firstStepOfMoving = true;
			}
			break;
		case MovingState.Moving:
			if (firstStepOfMoving) {
				firstStepOfMoving = false;
			}
			if (currentMoveFrame == maxMoveFrames - 1) {
				lastStepOfMoving = true;
			}
			if (currentMoveFrame == maxMoveFrames) {
				currentMoveFrame = 0;
				movingState = MovingState.Waiting;
				lastStepOfMoving = false;
				return;
			}
			currentMoveFrame = currentMoveFrame + 1;
			break;
		default:
			break;
		}
	}

	public MovingState GetMovingState ()
	{
		return movingState;
	}

	public bool IsFirstStepOfMoving ()
	{
		return firstStepOfMoving;
	}

	public bool IsLastStepOfMoving ()
	{
		return lastStepOfMoving;
	}

	public float GetMaxMoveFrames ()
	{
		return maxMoveFrames;
	}
}

public enum MovingState 
{
	Moving = 0,
	Waiting = 1,
}
