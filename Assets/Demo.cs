using UnityEngine;
using System;
using System.Collections;
using com.moblink.unity3d;

using System.Runtime.InteropServices;

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public MobLink moblink;

	void Start () {

		//如果block 中有引用其他东西，会将其持有
//		MobLink.onGetMobId += (mobId) => {
//
//
//		};

		//更建议以下方式，然后在本类中销毁
		MobLink.onGetMobId += mobIdHandler;
		MobLink.onRestoreScene += sceneHandler;
	}

	//销毁
//	void Destroy()
//	{
//		MobLink.onGetMobId -= mobIdHandler;
//	}

	void OnGUI ()
	{
		GUI.skin = demoSkin;

		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
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
			custom ["money"] = 1000;
			custom ["productName"] = "Game Authorization";

			MobLinkScene scene = new MobLinkScene ("/new/local", "uuid-123456", custom);

			MobLink.getMobId (scene);
		}
	}

	void mobIdHandler (string mobid)
	{
		Console.Write ("demoReceiveMobId:" + mobid);
	}
		
	void sceneHandler (Hashtable data)
	{
		Console.Write ("path:" + data["path"]);
		Console.Write ("source:" + data["source"]);
		Console.Write ("params:" + data["params"]);
	}
}
