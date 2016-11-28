using UnityEngine;
using System.Collections;

public class BeanController : MonoBehaviour {
	public GameObject mapManager;

	void Start ()
	{
		MoveTheBean ();
	}

	void Update ()
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	public void MoveTheBean ()
	{
		Vector3[] illegalPositions = mapManager.GetComponent<MapManager> ().GetIllegalPositions ();
		int range = MapManager.WORLD_RANGE;
		Vector3 newPosition = new Vector3 ((float)Random.Range (-range, range), 0.5f, (float)Random.Range (-range, range));
		while (((IList)illegalPositions).Contains (newPosition)) 
		{
			newPosition = new Vector3 ((float)Random.Range (-range, range), 0.5f, (float)Random.Range (-range, range));
		}
		transform.position = newPosition;
	}
}
