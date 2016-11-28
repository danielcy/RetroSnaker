using UnityEngine;
using System.Collections;

public class BodyController : MonoBehaviour {
	public GameObject prefabsBody;
	public GameObject player;
	public GameObject bean;
	public GameObject timerObject;
	public GameObject mapManager;

	private float maxMoveFrames;
	private BodyState bodyState;
	private MovingState movingState;
	private GameObject[] bodyList;
	private Vector3[] directionList;
	private int bodyCount;
	private GameTimer timer;
	private bool stuckByGrowing;

	private const int MAX_BODY_COUNT = 500;

	void Start ()
	{
		timer = timerObject.GetComponent<GameTimer> ();
		maxMoveFrames = timer.GetMaxMoveFrames ();
		movingState = MovingState.Waiting;
		bodyCount = 0;
		stuckByGrowing = false;
		bodyState = BodyState.Moving;
		ListInitialize ();
		UpdatePositionsToMap ();
	}

	void Update () 
	{
		movingState = timer.GetMovingState ();
		switch (movingState) 
		{
		case MovingState.Waiting:
			if (stuckByGrowing) {
				stuckByGrowing = false;
			}
			break;
		case MovingState.Moving:
			if (!stuckByGrowing) {
				MoveOneStep ();
			}
			break;
		default:
			break;
		}
	}

	void MoveOneStep()
	{
		if (timer.IsFirstStepOfMoving ()) 
		{
			SetAllDirection ();
			if (player.GetComponent<PlayerController> ().NeedToGrow ()) {
				stuckByGrowing = true;
				AddBody ();
				return;
			}
		}
		if (timer.IsLastStepOfMoving ()) 
		{
			FixAllPosition ();
			UpdatePositionsToMap ();
			return;
		}
		MoveAll ();
	}

	void ListInitialize ()
	{
		bodyList = new GameObject[MAX_BODY_COUNT];
		directionList = new Vector3[MAX_BODY_COUNT];
		Vector3 playerPostion = player.transform.position;
		GameObject firstbody = AddBody ();
		firstbody.transform.position = new Vector3 (playerPostion.x + 2, playerPostion.y, playerPostion.z);
		GameObject secondbody = AddBody ();
		secondbody.transform.position = new Vector3 (playerPostion.x + 1, playerPostion.y, playerPostion.z);

	}

	GameObject AddBody ()
	{
		GameObject newBody = Instantiate (prefabsBody);
		newBody.name = "Body" + bodyCount;
		newBody.transform.position = FixedPlayerPosition();
		newBody.transform.parent = transform;
		bodyList [bodyCount] = newBody;
		bodyCount = bodyCount + 1;
		return newBody;
	}

	void SetAllDirection ()
	{
		for (int i = 0; i < bodyCount-1; i++) {
			Vector3 currentPosition = bodyList [i].transform.position;
			Vector3 prePosition = bodyList [i + 1].transform.position;
			directionList [i] = prePosition - currentPosition;
			directionList [i].Normalize ();
		}
		Vector3 playerPosition = player.transform.position;
		Vector3 firstPosition = bodyList [bodyCount - 1].transform.position;
		directionList [bodyCount - 1] = playerPosition - firstPosition;
		directionList [bodyCount - 1].Normalize ();
	}

	void MoveAll ()
	{
		for (int i = 0; i < bodyCount; i++) {
			Vector3 direction = directionList [i];
			bodyList [i].transform.Translate (direction * (1 / maxMoveFrames));
		}
	}

	void FixAllPosition ()
	{
		for (int i = 0; i < bodyCount; i++) {
			float fixedX = Mathf.Round (bodyList [i].transform.position.x);
			float fixedY = bodyList [i].transform.position.y;
			float fixedZ = Mathf.Round (bodyList [i].transform.position.z);
			bodyList [i].transform.position = new Vector3 (fixedX, fixedY, fixedZ);
		}
	}

	Vector3 FixedPlayerPosition()
	{
		float fixedX = Mathf.Round (player.transform.position.x);
		float fixedY = player.transform.position.y;
		float fixedZ = Mathf.Round ( player.transform.position.z);
		Vector3 result = new Vector3 (fixedX, fixedY, fixedZ);
		return result;
	}

	Vector3[] GetAllPositions ()
	{
		Vector3[] result = new Vector3[bodyCount];
		for (int i = 0; i < bodyCount; i++) {
			result [i] = bodyList [i].transform.position;
		}
		return result;
	}

	public GameObject GetLatestBody ()
	{
		return bodyList[bodyCount - 1];
	}

	void UpdatePositionsToMap()
	{
		Vector3[] bodyPositonList = GetAllPositions ();
		Vector3[] inputList = new Vector3[bodyPositonList.Length + 2];
		bodyPositonList.CopyTo (inputList, 0);
		inputList [bodyPositonList.Length] = player.transform.position;
		inputList [bodyPositonList.Length + 1] = bean.transform.position;
		mapManager.GetComponent<MapManager> ().UpdateIllegalPositions (inputList);
	}
}

public enum BodyState
{
	Moving = 0,
	Growing = 1,
	Waiting = 2,
}

