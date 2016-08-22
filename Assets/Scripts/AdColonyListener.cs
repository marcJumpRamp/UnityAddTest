using UnityEngine;
using System.Collections;

public class AdColonyListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitializeAdColony ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void playAdd(){
		bool isVideoAvialable;
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			Debug.Log ("HAHAHA NO VIDEO");
			break;
		case RuntimePlatform.Android:
			var isVideoAvailable = AdColony.IsVideoAvailable ("vzc2764759bb2b491cbd");
			break;
		}

		//vzc2764759bb2b491cbd -- preroll


	}

	private void InitializeAdColony()
	{
		
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			AdColony.Configure("1", "app12ddf1be6b274e9686", "vzf7ae4ea9e25a4fe9a9");
			break;
		case RuntimePlatform.Android:
			AdColony.Configure("1", "appda09488279a44455b5", "vz8c648262bb9b4e919a", "vz14d4b63976904ba1a2", "vz0944befdb62e49969d", "vzc2764759bb2b491cbd");
			break;
		}

	}
}
