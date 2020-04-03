using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
	// This is our prefab object that will be exposed in the inspector
	public GameObject prefab;

	// number of objects to create. Exposed in inspector
	public int numberToCreate;

	void Start()
	{
		Populate();
	}

	void Update()
	{

	}

	void Populate()
	{
		// Create GameObject instance
		GameObject newObj;

		for (int i = 0; i < numberToCreate; i++)
		{
			// Create new instances of our prefab until we've created as many as we specified
			newObj = (GameObject)Instantiate(prefab, transform);

			// Randomize the color of our image
			newObj.GetComponent<Image>().color = Random.ColorHSV();

		}

	}
}
