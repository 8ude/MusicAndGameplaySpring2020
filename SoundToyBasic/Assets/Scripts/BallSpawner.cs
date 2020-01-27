using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

	public Transform model;
	public float rate = 1.0f;

	float progress = 0.0f;

	void Spawn()
	{
		Transform next = Instantiate(model, transform.position, transform.rotation) as Transform;
		next.parent = transform;
		next.localPosition = Vector3.zero;
	}

	void Update()
	{
		progress += rate * Time.deltaTime;
		if (progress >= 1.0f)
		{
			Spawn();
			progress -= 1.0f;
		}
	}
}
