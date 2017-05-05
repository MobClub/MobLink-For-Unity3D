using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moblink.unity3d;

public class BaseScene : MonoBehaviour 
{

	void Start () {
		MobLink.onRestoreScene += OnRestoreScene;
	}


	void OnApplicationFocus(bool hasFocus)
	{
		Debug.Log ("OnApplicationFocus(), hasFocus:" + hasFocus);
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus) {
			MobLink.setIntentHandler (null);
			MobLink.clearIntent ();
		}
	}

	void OnRestoreScene(Hashtable param)
	{
		Debug.Log ("OnRestoreScene");
	}
}
