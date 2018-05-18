package com.mob.moblink.unity;

import android.app.Activity;
import android.content.Intent;

import com.mob.moblink.Scene;
import com.unity3d.player.UnityPlayer;

import org.json.JSONObject;

import java.util.HashMap;

public class RestoreSceneListener extends Object implements com.mob.moblink.RestoreSceneListener {

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

	@Override
	public boolean onReturnSceneIntent(String s, Intent intent) {
		return false;
	}

	@Override
	public void onReturnSceneData(Activity activity, Scene scene) {
		HashMap<String, Object> result = new HashMap<String, Object>();
		result.put("path", scene.path);
		result.put("source", scene.source);
		result.put("params", scene.params);

		JSONObject json = new JSONObject(result);
		String value = json.toString();
		UnitySendMessage(gameObjectName, callbackMethod, value);
	}
}
