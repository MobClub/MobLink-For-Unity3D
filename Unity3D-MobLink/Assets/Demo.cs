using UnityEngine;
using System;
using System.Collections;
using com.moblink.unity3d;

using System.Runtime.InteropServices;

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public MobLink moblink;

	void Start () {

		//获取回调的方式一：
		//注意,如果block中有引用其他,会将其持有
//		MobLink.onGetMobId += (mobId) => {
//
//
//		};

//		MobLink.onRestoreScene += (Hashtable scene) => {
//			
//		};


		//获取回调的方式二(推荐)【更建议此种方式,然后在本类中销毁,参看本类Destroy()】：
		MobLink.onGetMobId += mobIdHandler;
		MobLink.onRestoreScene += sceneHandler;
	}

	//对应”方式二“的销毁
	void Destroy()
	{
		MobLink.onGetMobId -= mobIdHandler;
	}

	void OnGUI ()
	{
		GUI.skin = demoSkin;

		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			scale = Screen.width / 320;
		} else if (Application.platform == RuntimePlatform.Android) {
			scale = Screen.width / 320;
		}

		float btnWidth = 165 * scale;
		float btnHeight = 30 * scale;
		float btnTop = 20 * scale;
		float btnGap = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(14 * scale);

		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "GetMobID"))
		{
			Hashtable custom = new Hashtable ();
			custom ["ChapterID"] = 1001;
			custom ["ChapterName"] = "Dragon Fire";

			MobLinkScene scene = new MobLinkScene ("/chapter1/dragon", "userid-123456", custom);

			MobLink.getMobId (scene);
		}
	}

	//获取mobid之回调
	void mobIdHandler (string mobid)
	{
		Console.Write ("Received MobId:" + mobid);
	}
		
	//场景恢复之回调
	void sceneHandler (MobLinkScene scene)
	{
		Console.Write ("path:" + scene.path);
		Console.Write ("source:" + scene.source);
		Console.Write ("params:" + MiniJSON.jsonEncode (scene.customParams));
	}
}
