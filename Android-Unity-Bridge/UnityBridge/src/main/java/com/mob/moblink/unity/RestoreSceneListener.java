package com.mob.moblink.unity;

import android.app.Activity;

import com.mob.moblink.AbstractRestoreSceneListener;
import com.unity3d.player.UnityPlayer;

import org.json.JSONObject;

import java.util.HashMap;

public class RestoreSceneListener extends AbstractRestoreSceneListener {

	private String gameObjectName;
	private String callbackMethod;

	/**
	 * 构造函数
	 * @param goName Name of GameObject.
	 * @param method callback.
	 */
	public RestoreSceneListener(String goName, String method) {
		super();
		gameObjectName = goName;
		callbackMethod = method;
	}

	@Override
	public void onReturnSceneData(Activity activity, HashMap<String, Object> result) {
		JSONObject json = new JSONObject(result);
		String value = json.toString();
		UnitySendMessage(gameObjectName, callbackMethod, value);
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
