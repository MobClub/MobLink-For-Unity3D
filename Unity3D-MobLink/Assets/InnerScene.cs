using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InnerScene : BaseScene {

	private float scale = 1.0f;

	/**
	 * 用于标识当前Scene, SceneA/B/C/D 共用此脚本
	 */
	private string currentScene;
	private string title;
	private string label;
	private Hashtable param;

	// window rect(dialog)
	private Rect windowRect;
	private int boxId;

	// 表示当前scene的path
	private string currentScenePath;

	protected override void Start() {
		base.Start ();
		currentScene = SceneManager.GetActiveScene().name;
		param = BaseScene.tempParam;
		if ("SceneA" == currentScene) {
			title = "A界面";
			label = "A";
			currentScenePath = "/demo/a";
		} else if ("SceneB" == currentScene) {
			title = "B界面";
			label = "B";
			currentScenePath = "/demo/b";
		} else if ("SceneC" == currentScene) {
			title = "C界面";
			label = "C";
			currentScenePath = "/demo/c";
		} else if ("SceneD" == currentScene) {
			title = "D界面";
			label = "D";
			currentScenePath = "/demo/d";
		} else {
			title = "A界面";
			label = "A";
			currentScenePath = "/demo/a";
		}

		boxId = 1;
	}
	
	void Update () {
		
	}

	void OnGUI ()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			scale = Screen.width / 320;
		} else if (Application.platform == RuntimePlatform.Android) {
			scale = Screen.width / 320;
		}

		float FONT_SIZE = (int)(18 * scale);
		float ITEM_HEIGHT = 30 * scale;
		bool hit = false;

		GUI.skin.button.fontSize = (int)FONT_SIZE;
		GUI.skin.label.fontSize = (int)(30 * scale);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textField.fontSize = (int)FONT_SIZE;
		GUI.skin.box.fontSize = (int)FONT_SIZE;
		GUI.skin.window.fontSize = (int)FONT_SIZE;

		float x, y, w, h;
		w = Screen.width - 40 * scale; x = 20 * scale; y = (Screen.height / 2 - 60 * scale); h = ITEM_HEIGHT;
		GUI.Label(new Rect(x, y, w, ITEM_HEIGHT), label);

		y += 40 * scale + ITEM_HEIGHT;
		hit = GUI.Button (new Rect (x, y, w, ITEM_HEIGHT), "查看参数");
		if (hit) {
			clickViewParam ();
		}
			
		GUI.skin.label.fontSize = (int)FONT_SIZE;
		GUI.Label(new Rect((w - 100 * scale) / 2, 5 * scale, 100 * scale, h), title);

		hit = GUI.Button (new Rect (20 * scale, 5 * scale, 60 * scale, h), "返回");
		if (hit) {
			clickBack ();
		}

		guiDialogWindow ();
	}

	void clickViewParam() {
		boxId = 1;
	}

	void clickBack() {
		SceneManager.LoadScene ("Demo");
	}

	void guiDialogWindow() {
		{
			float width = Screen.width - 80 * scale;
			float height = width;
			float x = 40 * scale;
			float y = (Screen.height - height) / 2;
			windowRect = new Rect (x, y, width, width);
		}
		if (0 != boxId && null != param && param.Count > 0) {
			GUI.ModalWindow(0, windowRect, renderWindowCallback, "参数");
		}
	}

	void renderWindowCallback(int windowID) {
		string message = "路径innerPath\n";
		message += currentScenePath + "\n";
		message += "\n";

		message += "来源source\n";
		message += param ["source"] + "\n";
		message += "\n";

		message += "参数\n";
		Hashtable temp = (Hashtable)param ["params"];
		foreach(string key in temp.Keys) {
			message += key + ":" + temp[key] + "\n";
		}
		Rect winRect = windowRect;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(10 * scale, 20 * scale, winRect.width - 20 * scale, winRect.height), message);
		if (GUI.Button (new Rect (10 * scale, winRect.height - 50 * scale, winRect.width - 20 * scale, 30 * scale), "关闭")) {
			boxId = 0;
		}
	}
}
