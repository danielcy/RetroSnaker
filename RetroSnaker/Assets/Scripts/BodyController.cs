using UnityEngine;
using System.Collections;

public class BodyController : MonoBehaviour {
	public GameObject prefabsBody;
	public GameObject player;

	private GameObject[] bodyList;
	private int bodyCount;
	private BodyStatment bodyStatment;

	void Start ()
	{
		bodyCount = 0;
		bodyStatment = BodyStatment.Moving;
		ListInitialize ();
	}

	void Update () 
	{
		
	}

	void ListInitialize ()
	{
		AddBody ();
	}

	void AddBody ()
	{
		
	}
}

public enum BodyStatment
{
	Moving = 0,
	Growing = 1,
}