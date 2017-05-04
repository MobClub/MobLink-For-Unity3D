using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InnerScene : MonoBehaviour {

	private float scale = 1.0f;

	private string currentScene;
	private string title;
	private string label;

	void Start() {
		currentScene = SceneManager.GetActiveScene().name;
		if ("SceneA" == currentScene) {
			title = "A界面";
			label = "A";
		} else if ("SceneB" == currentScene) {
			title = "B界面";
			label = "B";
		} else if ("SceneC" == currentScene) {
			title = "C界面";
			label = "C";
		} else if ("SceneD" == currentScene) {
			title = "D界面";
			label = "D";
		} else {
			title = "A界面";
			label = "A";
		}
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
		float FILL_WIDTH = Screen.width;
		float ITEM_HEIGHT = 30 * scale;
		float V_DIVIDER_HEIGHT = 20 * scale;
		float LEFT = 20 * scale;
		bool hit = false;

		GUI.skin.button.fontSize = (int)FONT_SIZE;
		GUI.skin.label.fontSize = (int)(30 * scale);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textField.fontSize = (int)FONT_SIZE;
		GUI.skin.box.fontSize = (int)FONT_SIZE;

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
	}

	void clickViewParam() {
		
	}

	void clickBack() {
		Application.LoadLevel ("Demo");
		
	}
}
