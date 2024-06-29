/*
 * Script that move on a Path using CinemaScene
 * Created by Misora Tanaka
 * 
 * date 24/02/15
 * --- Log ---
 * 02/14:Create Script
 * 02/15:Write comments for use in other projects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DollyCartController : MonoBehaviour
{
	// ----Variable Declarations----
	[SerializeField] private CinemachineDollyCart moveCart;     // Cart for walking on paths
	[SerializeField] private string tagName;                    // PathTag
	[SerializeField] private float speed;                       // WalkSpeed
	private CinemachinePath path;                               // SetPath
	private int count;

	// -----------------------------

	// Start is called before the first frame updates
	void Start()
	{
		moveCart.m_Speed = speed;
		count = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (moveCart.m_Position >= 1) ResetPath();
	}

	/// <summary>
	/// Find and set the nearest path
	/// </summary>
	public void SetPath()
	{
		// Find path with tag
		path = SearchTag(gameObject, tagName).GetComponent<CinemachinePath>();
		// Set path to moveCart
		if (path != null) moveCart.m_Path = path;
		count++;

	}

	/// <summary>
	/// Reset path and Position
	/// </summary>
	private void ResetPath()
	{
		if (count >= 2) return;
		moveCart.m_Position = 0;
		SetPath();
	}

	/// <summary>
	/// Search Object with Tag
	/// </summary>
	/// <param name="refObj">ReferenceObject</param>
	/// <param name="tag">SearchTag</param>
	/// <returns></returns>
	private GameObject SearchTag(GameObject refObj, string tag)
	{
		float tmpDistance;      // Distance to searched object
		float nearDistance = 0; // Distance to the nearest current object

		// Nearest current object
		GameObject targetObj = null;

		// Search for objects with the target tag
		foreach (GameObject objs in GameObject.FindGameObjectsWithTag(tag))
		{
			// Distance to found object
			tmpDistance = Vector3.Distance(objs.transform.position, refObj.transform.position);


			// Compare with stored distances
			if (nearDistance == 0 || nearDistance > tmpDistance)
			{
				// Update nearest object and its distance
				nearDistance = tmpDistance;
				targetObj = objs;
			}
		}

		// Returns the nearest object
		return targetObj;
	}
}
