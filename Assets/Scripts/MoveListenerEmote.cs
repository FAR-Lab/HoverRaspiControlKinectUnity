using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class MoveListenerEmote : ImageResultsListener
{
	private float joy, fear, surprise, disgust;
	private bool faceFound;

	public GameObject cube;
	public float posx = 0.0f, posy = 0.5f, posz = 0.0f;

	public override void onFaceFound(float timestamp, int faceId)
	{
		Debug.Log("Found the face");
	}

	public override void onFaceLost(float timestamp, int faceId)
	{
		Debug.Log("Lost the face");
	}

	public override void onImageResults(Dictionary<int, Affdex.Face> faces)
	{
		if (faces.Count > 0)
		{
			faceFound = true;
			foreach (KeyValuePair<int, Affdex.Face> pair in faces)
			{
				int FaceId = pair.Key;  // The Face Unique Id.
				Affdex.Face face = pair.Value;  // Instance of the face class containing emotions, and facial expression values.

				//Retrieve the Emotions Scores
				//face.Expressions.TryGetValue(Expressions.BrowRaise, out raise);
				//face.Expressions.TryGetValue(Expressions.BrowFurrow, out furrow);
				//face.Expressions.TryGetValue(Expressions.LipSuck, out suck);
				//face.Expressions.TryGetValue(Expressions.LipPucker, out pucker);
				face.Emotions.TryGetValue(Emotions.Joy, out joy);
				//face.Emotions.TryGetValue(Emotions.Sadness, out sadness);
				face.Emotions.TryGetValue(Emotions.Fear, out fear);
				face.Emotions.TryGetValue(Emotions.Surprise, out surprise);
				face.Emotions.TryGetValue(Emotions.Disgust, out disgust);

				//Retrieve the Interocular distance, the distance between two outer eye corners.
				//currentInterocularDistance = face.Measurements.interOcularDistance;

				//Retrieve the coordinates of the facial landmarks (face feature points)
				//featurePointsList = face.FeaturePoints;
			}
		}
		else
		{
			faceFound = false;
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (faceFound)
		{
			if (joy > 90)
			{
				posx += 0.1f;
			}
			if (disgust > 75)
			{
				posx -= 0.1f;
			}
			if (surprise > 65)
			{
				posz += 0.1f;
			}
			if (fear > 65)
			{
				posz -= 0.1f;
			}
		}
		else
		{
			Debug.Log("Face not found");
		}
		cube.gameObject.transform.position = new Vector3(posx, posy, posz);
	}
}