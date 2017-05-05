using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moblink.unity3d;

public class BaseScene : MonoBehaviour 
{
	private string path;
	protected virtual void Start () {
		MobLink.onRestoreScene += OnRestoreScene;
	}

	protected virtual void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus) {
			MobLink.setIntentHandler (null);
			path = MobLink.getIntentPath ();
			MobLink.clearIntent ();
		}
	}

	protected virtual void OnRestoreScene(Hashtable res)
	{
		string source = res ["source"].ToString();
		if ("/demo/a" == path) {
			Application.LoadLevel ("SceneA");
		} else if ("/demo/b" == path) {
			Application.LoadLevel ("SceneB");
		} else if ("/demo/c" == path) {
			Application.LoadLevel ("SceneC");
		} else if ("/demo/d" == path) {
			Application.LoadLevel ("SceneD");
		} else {
			// do nothing
		}

		Hashtable customParams = (Hashtable)res ["params"];

		Debug.Log ("OnRestoreScene(), param:" + customParams);
	}

	protected virtual void OnDestroy() {
		MobLink.onRestoreScene -= OnRestoreScene;
	}
}
