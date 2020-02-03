using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

	public GameObject model;
	public float rate = 1.0f;

	float progress = 0.0f;

	void Spawn()
	{
        GameObject next = Instantiate(model, transform.position, transform.rotation);
		next.transform.parent = transform;
		next.transform.localPosition = Vector3.zero;
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
