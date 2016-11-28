using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public GameObject bean;
	public GameObject bodies;
	public GameObject timerObject;
	public Text scoreText;
	public Text loseText;

	private MovingState movingState;
	private Vector3 direction;
	private Vector3 preDirection;
	private float maxMoveFrames;
	private GameTimer timer;
	private int scoreCount;

	void Start () 
	{
		movingState = MovingState.Waiting;
		direction = new Vector3 (-1.0f, 0.0f, 0.0f);
		preDirection = direction;
		timer = timerObject.GetComponent<GameTimer> ();
		maxMoveFrames = timer.GetMaxMoveFrames ();
		scoreCount = -1;
		AddAndUpdateScore ();
	}

	void Update () 
	{
		SetPreDirection ();
		movingState = timer.GetMovingState ();
		switch (movingState) 
		{
		case MovingState.Waiting:
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
		if (other.gameObject.tag == "Bean") 
		{
			other.gameObject.GetComponent<BeanController> ().MoveTheBean ();
			AddAndUpdateScore ();
		}
		else if (other.gameObject.tag == "Wall" ||
			(other.gameObject.tag == "Body" && other.gameObject != bodies.GetComponent<BodyController>() .GetLatestBody() )) 
		{
			loseText.gameObject.SetActive (true);
			Time.timeScale = 0;
		}
	}

	void MoveOneStep()
	{
		if (timer.IsFirstStepOfMoving ()) 
		{
			direction = preDirection;
		}
		if (timer.IsLastStepOfMoving ()) 
		{
			FixPosition ();
			return;
		}
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

	public bool NeedToGrow()
	{
		if ((transform.position + direction - bean.transform.position).magnitude <= 0.5) {
			return true;
		} else
			return false;
	}

	void AddAndUpdateScore ()
	{
		scoreCount++;
		scoreText.text = "SCORE: " + scoreCount.ToString ();
	}
}