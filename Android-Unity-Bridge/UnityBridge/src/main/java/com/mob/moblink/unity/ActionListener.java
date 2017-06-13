package com.mob.moblink.unity;

import com.unity3d.player.UnityPlayer;

import org.json.JSONObject;

import java.util.HashMap;

/**
 * ActionListener for Unity3d.<br/>
 * 使用demo, step by step:
 * <ul>
 *     <li>C#代码, 创建本类的实现, 像这样 new AndroidJavaObject ();</li>
 *     <li>goName是the Name of gameObject(可查找unity3d文档了解)</li>
 *     <li>method 是回调函数名, 注意此函数签名与 UnitySendMessage中传递的函数签名一致</li>
 *     <li>回调的参数是json字符串</li>
 * </ul>
 */
public class ActionListener extends Object implements com.mob.moblink.ActionListener {
	private String gameObjectName;
	private String callbackSuccessMethod;
	private String callbackFailMethod;

	/**
	 * 构造函数
	 * @param goName Name of GameObject.
	 * @param successMethod 成功回调函数.
	 * @param failMethod 失败回调函数.
	 */
	public ActionListener(String goName, String successMethod, String failMethod) {
		super();
		gameObjectName = goName;
		callbackSuccessMethod = successMethod;
		callbackFailMethod = failMethod;
	}

	@Override
	public void onResult(HashMap<String, Object> params) {
		JSONObject json = new JSONObject(params);
		String value = json.toString();
		UnitySendMessage(gameObjectName, callbackSuccessMethod, value);
	}

	@Override
	public void onError(Throwable t) {
		JSONObject json = new JSONObject();
		String value = json.toString();
		UnitySendMessage(gameObjectName, callbackFailMethod, value);
	}

	/**
	 * 使用反射的方式调用UnityPlayer.UnitySendMessage函数.<br/>
	 * 此函数不该放置此处, 稍后处理
	 * @param goName gameObject.name
	 * @param method 函数
	 * @param params 参数
	 */
	protected static void UnitySendMessage(String goName, String method, String params) {
		UnityPlayer.UnitySendMessage(goName, method, params);
	}
}
