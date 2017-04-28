using UnityEngine;
using System;
using System.Collections;
using com.moblink.unity3d;

using System.Runtime.InteropServices;

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public MobLink moblink;

	private float scale = 1.0f;

	private static String[] PATH = new string[]{"/demo/a",
		"/demo/a",
		"/demo/b",
		"/demo/c",
		"/demo/d"
	};

	private int path;
	private String source = "";
	private String key1 = "", value1 = "";
	private String key2 = "", value2 = "";
	private String key3 = "", value3 = "";

	private String mobId;

	// 0, 不展示; 1,获取modId; 2, 分享提示; 3, 场景还原时用
	private int boxId;

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

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			scale = Screen.width / 320;
		} else if (Application.platform == RuntimePlatform.Android) {
			scale = Screen.width / 320;
		}

		float FONT_SIZE = (int)(18 * scale);
		float FILL_WIDTH = Screen.width;
		float ITEM_HEIGHT = 30 * scale;
		float V_DIVIDER_HEIGHT = 20 * scale;
		float LEFT = 20 * scale;
		bool hit = false;

		GUI.skin.button.fontSize = (int)FONT_SIZE;
		GUI.skin.label.fontSize = (int)FONT_SIZE;
		GUI.skin.textField.fontSize = (int)FONT_SIZE;
		GUI.skin.box.fontSize = (int)FONT_SIZE;

		float x, y, w, h;
		w = Screen.width - 40 * scale; x = 20 * scale; y = 40 * scale; h = ITEM_HEIGHT;
		GUI.Label(new Rect(x, y, w, ITEM_HEIGHT), "路径path");
		y += V_DIVIDER_HEIGHT + FONT_SIZE;
		path = GUI.Toolbar (new Rect (x, y, w, h), path, new string[]{ "A界面", "B界面", "C界面", "D界面" });

		y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
		GUI.Label(new Rect(x, y, w, h), "来源source");
		y += V_DIVIDER_HEIGHT + FONT_SIZE;
		source = GUI.TextField (new Rect (x, y, w, h), source);

		y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
		GUI.Label(new Rect(x, y, w, h), "自定义参数");
		float KEY_WIDTH = w * 0.23f;
		float VALUE_WIDTH = w * 0.73f; 
		float lineOffset = LEFT + KEY_WIDTH + (w * 0.04f);

		y += 20 * scale + FONT_SIZE;
		key1 = GUI.TextField (new Rect (x, y, KEY_WIDTH, h), key1);
		value1 = GUI.TextField (new Rect (lineOffset, y, VALUE_WIDTH, h), value1);

		y += 20 * scale + FONT_SIZE;
		key2 = GUI.TextField (new Rect (x, y, KEY_WIDTH, h), key2);
		value2 = GUI.TextField (new Rect (lineOffset, y, VALUE_WIDTH, h), value2);

		y += 20 * scale + FONT_SIZE;
		key3 = GUI.TextField (new Rect (x, y, KEY_WIDTH, h), key3);
		value3 = GUI.TextField (new Rect (lineOffset, y, VALUE_WIDTH, h), value3);

		y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
		hit = GUI.Button (new Rect (x, y, w, ITEM_HEIGHT), "获取MobId");
		if (hit)
			clickGetMobId ();

		y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
		hit = GUI.Button (new Rect (x, y, w, ITEM_HEIGHT), "分享");
		if (hit)
			clickShare ();

		GUI.Label(new Rect((w - 40 * scale) / 2, 5 * scale, 40 * scale, h), "演示");
		hit = GUI.Button (new Rect (w - 100 * scale, 5 * scale, 120 * scale, h), "填充默认值");
		if (hit)
			fillDefault ();

		guiBox ();
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

	void clickGetMobId()
	{
		Hashtable custom = new Hashtable ();

		String pathString = PATH [path];

		if (key1.Length > 0 && value1.Length > 0) {
			custom.Add (key1, value1);
		}
		if (key2.Length > 0 && value2.Length > 0) {
			custom.Add (key2, value2);
		}
		if (key3.Length > 0 && value3.Length > 0) {
			custom.Add (key3, value3);
		}
		MobLinkScene scene = new MobLinkScene (pathString, source, custom);
		MobLink.getMobId (scene);
	}

	void clickShare()
	{
		if (null == mobId || mobId.Length <= 0) {
			boxId = 1;
		} else {
			boxId = 2;
		}
	}

	void fillDefault()
	{
		path = 0;
		source = "MobLinkDemo";
		key1 = "key1"; value1 = "value1";
		key2 = "key2"; value2 = "value2";
		key3 = "key3"; value3 = "value3";
	}

	void guiBox() 
	{
		float width = Screen.width / 2;
		float height = Screen.height / 2;
		float x = width / 2;
		float y = height / 2;

		/*
		if (1 == boxId) {
			GUI.Box (new Rect (x, y, width, height), "请先获取mobId");
		} else if(2 == boxId) {
			String url = "http://f.moblink.mob.com" + PATH [path] + "?mobid=" + mobId;
			GUI.Box (new Rect (x, y, width, height), "请分享下面的链接地址: \r\n" + url + "\r\n然后就可以通过这个链接打开app, 并进行还原");
		} */
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
	}


	public Rect windowRect = new Rect(20, 20, 120, 50);
	void DoMyWindow(int windowID) {
		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
			print("Got a click");

	}
		
}
