using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.moblink.unity3d;

public class BaseScene : MonoBehaviour 
{
	// 临时用来在Scene切换时传递参数
	public static Hashtable tempParam;

	// 需还原场景的path
	private string restorePath;
	protected virtual void Start () {
		MobLink.onRestoreScene += OnRestoreScene;
	}

	#if UNITY_ANDROID
	protected virtual void OnApplicationPause(bool pauseStatus)
	{
		// 您应该复制这段代码 -> 您的实现方法里.
		if (!pauseStatus) {
			restorePath = MobLink.getIntentPath ();
			MobLink.setIntentHandler ();
		}
	}
	#endif

	protected virtual void OnRestoreScene(Hashtable res)
	{
		tempParam = res;
		if ("/demo/a" == restorePath) {
			SceneManager.LoadScene ("SceneA");
		} else if ("/demo/b" == restorePath) {
			SceneManager.LoadScene ("SceneB");
		} else if ("/demo/c" == restorePath) {
			SceneManager.LoadScene ("SceneC");
		} else if ("/demo/d" == restorePath) {
			SceneManager.LoadScene ("SceneD");
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
