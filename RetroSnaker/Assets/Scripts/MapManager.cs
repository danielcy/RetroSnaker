using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
	private Vector3[] illegalPositions;
	public const int WORLD_RANGE = 9;

	public void UpdateIllegalPositions (Vector3[] inputList)
	{
		illegalPositions = new Vector3[inputList.Length];
		inputList.CopyTo (illegalPositions, 0);
	}

	public Vector3[] GetIllegalPositions ()
	{
		return illegalPositions;
	}
}
