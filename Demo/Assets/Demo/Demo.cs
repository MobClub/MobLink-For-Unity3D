using UnityEngine;
using System;
using System.Collections;
using com.moblink.unity3d;
using UnityEngine.SceneManagement;
using Alertify;
using System.Runtime.InteropServices;

public class Demo : MonoBehaviour {

	// 临时用来在Scene切换时传递参数
	public static MobLinkScene tempScene;

	public GUISkin demoSkin;

	private float scale = 1.0f;

	private static String[] PATH = new string[]{
		"/demo/a",
        "/demo/b",
        "/demo/c",
        "/demo/d"
    };

	// 用户选择path的id
	private int selectedPath;
    // private String source = "";
	private String key1 = "", value1 = "";
	private String key2 = "", value2 = "";
	private String key3 = "", value3 = "";
	private String mobId;

	// 0, 不展示; 1,获取modId; 2, 分享提示; 3, 场景还原时用
	private int boxId;

	// window rect(dialog)
	private Rect windowRect;

	void Start () {
        MobLink.setRestoreSceneListener(OnRestoreScene);

#if UNITY_IPHONE
        
#endif
	}

	void OnGUI ()
	{
        
        GUI.skin = demoSkin;
       
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
			scale = Screen.width / 320;
		} else if (Application.platform == RuntimePlatform.Android) {
			scale = Screen.width / 320;
		}

		float FONT_SIZE = (int)(18 * scale);
		float ITEM_HEIGHT = 30 * scale;
		float V_DIVIDER_HEIGHT = 20 * scale;
		float LEFT = 20 * scale;
		bool hit = false;



        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            scale = Screen.width / 320;
        }

        //float btnWidth = 165 * scale;
        float btnWidth = Screen.width / 5 * 2;
        float btnWidth2 = btnWidth + 80 * scale;

        float btnHeight = Screen.height / 25;
        float btnTop = 30 * scale;
        float btnGap = 20 * scale;
        GUI.skin.button.fontSize = Convert.ToInt32(13 * scale);
        GUI.skin.button.fontSize = (int)FONT_SIZE;
		GUI.skin.label.fontSize = (int)FONT_SIZE;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.textField.fontSize = (int)FONT_SIZE;
		GUI.skin.box.fontSize = (int)FONT_SIZE;
		GUI.skin.window.fontSize = (int)FONT_SIZE;

		float x, y, w, h;
		w = Screen.width - 40 * scale; x = 20 * scale; y = 40 * scale; h = ITEM_HEIGHT;
		GUI.Label(new Rect(x, y, w, ITEM_HEIGHT), "路径path");
		y += V_DIVIDER_HEIGHT + FONT_SIZE;
		selectedPath = GUI.Toolbar (new Rect (x, y, w, h), selectedPath, new string[]{ "A界面", "B界面", "C界面", "D界面" });

        // y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
        // GUI.Label(new Rect(x, y, w, h), "来源source");
        // y += V_DIVIDER_HEIGHT + FONT_SIZE;
        // source = GUI.TextField (new Rect (x, y, w, h), source);

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
		hit = GUI.Button (new Rect (x, y, w, ITEM_HEIGHT), "获取MobId", "button");
		if (hit)
			clickGetMobId ();

		y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
		hit = GUI.Button (new Rect (x, y, w, ITEM_HEIGHT), "分享");
		if (hit)
			clickShare ();

        // y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
        // hit = GUI.Button(new Rect(x, y, w, ITEM_HEIGHT), "获取隐私协议");
        // if (hit)
            // getPrivacyPolicyUrl();

        y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
        hit = GUI.Button(new Rect(x, y, w, ITEM_HEIGHT), "设置隐私协议状态");
        if (hit)
            submitPolicyGrantResult();


        y += V_DIVIDER_HEIGHT + ITEM_HEIGHT;
        hit = GUI.Button(new Rect(x, y, w, ITEM_HEIGHT), "设置隐私协议是否隐藏");
        if (hit)
            setAllowDialog();

        GUI.Label(new Rect((w - 40 * scale) / 2, 5 * scale, 40 * scale, h), "演示");
		hit = GUI.Button (new Rect (w - 100 * scale, 5 * scale, 120 * scale, h), "填充默认值");
		if (hit)
			fillDefault ();

		//renderWindow ();
	}



	// 创造符合委托格式的函数
	void mobIdHandler (string mobid, string errorInfo)
	{
		mobId = mobid;

		String message = "[moblink-unity]Received MobId: " + mobid + ", errorInfo: " + errorInfo;

		Debug.Log(message);

		Dialog.Alert(message, () => Notification.Warning("Warning!"));
	}
		
	void clickGetMobId()
	{
		Hashtable custom = new Hashtable ();

		String pathString = PATH [selectedPath];

		if (key1.Length > 0 && value1.Length > 0) {
			custom.Add (key1, value1);
		}
		if (key2.Length > 0 && value2.Length > 0) {
			custom.Add (key2, value2);
		}
		if (key3.Length > 0 && value3.Length > 0) {
			custom.Add (key3, value3);
		}
        // MobLinkScene scene = new MobLinkScene (pathString, source, custom);
        MobLinkScene scene = new MobLinkScene(pathString, custom);
		MobLink.getMobId (scene, mobIdHandler);
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
		selectedPath = 0;
        // source = "MobLinkDemo";
		key1 = "key1"; value1 = "value1";
		key2 = "key2"; value2 = "value2";
		key3 = "key3"; value3 = "value3";
	}

	void renderWindow() 
	{
		{
			float width = Screen.width - 80 * scale;
			float height = width;
			float x = 40 * scale;
			float y = (Screen.height - height) / 2;
			windowRect = new Rect (x, y, width, width);
		}

		if (0 != boxId) {
			GUI.ModalWindow(0, windowRect, renderWindowCallback, "提示");
		}
	}

	void renderWindowCallback(int windowID) {
		String message = "";
		if (1 == boxId) {
			message = "请先获取mobId";
		} else if (2 == boxId) {
			String url = "http://f.moblink.mob.com/demoPro" + PATH [selectedPath] + "?mobid=" + mobId;
			message = "请分享下面的链接地址: \r\n" + url + "\r\n然后就可以通过这个链接打开app, 并进行还原";
		}
		Rect winRect = windowRect;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(10 * scale, 50 * scale, winRect.width - 20 * scale, winRect.height), message);
		if (GUI.Button (new Rect (10 * scale, winRect.height - 50 * scale, winRect.width - 20 * scale, 30 * scale), "关闭")) {
			boxId = 0;
		}
	}

	/*
	 * 全局的场景还原监听函数 
	 */
	protected static void OnRestoreScene(MobLinkScene scene)
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

        if (customParams != null)
        {
			String message = "[moblink-unity]OnRestoreScene(). path: " + scene.path + ", params: " + customParams.toJson();

			Debug.Log(message);
			Dialog.Alert(message, () => Notification.Warning("Warning!"));

		}
		else
        {
			String message = "[moblink-unity]OnRestoreScene(). path: " + scene.path + ", params: " + null;
			Debug.Log(message);
			Dialog.Alert(message, () => Notification.Warning("Warning!"));

		}

	}

    void submitPolicyGrantResult()
    {
		Debug.Log("submitPolicyGrantResult");
        MobLink.submitPolicyGrantResult(true);

		Dialog.Alert("Call MobLink.submitPolicyGrantResult(true)", () => Notification.Warning("Warning!"));
	}

    // void getPrivacyPolicyUrl() {
    //     MobLink.getPrivacyPolicy(true);
    // }
    void setAllowDialog() {
        MobLink.setAllowDialog(false);
    }

    void OnFollowGetPolicy(string url)
    {
        Debug.Log("[OnFollowGetPolicy:" + url);
    }
}
