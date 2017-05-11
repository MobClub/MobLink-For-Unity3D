using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.moblink.unity3d;

public class BaseScene : MonoBehaviour 
{
	// 临时用来在Scene切换时传递参数
	public static MobLinkScene tempScene;

	protected virtual void Start () {
		MobLink.onRestoreScene += OnRestoreScene;
	}

	#if UNITY_ANDROID
	protected virtual void OnApplicationPause(bool pauseStatus)
	{
		// 您应该复制这段代码 -> 您的实现方法里.
		if (!pauseStatus) {
			MobLink.setIntentHandler ();
		}
	}
	#endif

	protected virtual void OnRestoreScene(MobLinkScene scene)
	{
		tempScene = scene;
		if (scene.path == "/demo/a") 
		{
			SceneManager.LoadScene ("SceneA");
		} 
		else if (scene.path == "/demo/b") 
		{
			SceneManager.LoadScene ("SceneB");
		} 
		else if (scene.path == "/demo/c") 
		{
			SceneManager.LoadScene ("SceneC");
		}
		else if (scene.path == "/demo/d") 
		{
			SceneManager.LoadScene ("SceneD");
		} else {
			// do nothing
		}
			
		Hashtable customParams = scene.customParams;

		Debug.Log ("OnRestoreScene(), param:" + customParams);
	}

	protected virtual void OnDestroy() {
		MobLink.onRestoreScene -= OnRestoreScene;
	}
		
}
