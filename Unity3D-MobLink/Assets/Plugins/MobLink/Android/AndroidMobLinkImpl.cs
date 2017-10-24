using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{

	#if UNITY_ANDROID
	public class AndroidMobLinkImpl : MobLinkImpl {

		public const string MOB_GAMEOBJECT_NAME = "MobLink";
		public const string MOB_GETMOBID_CALLBACK_SUCCESS_METHOD = "_MobIdCallback";
		public const string MOB_GETMOBID_CALLBACK_FAIL_METHOD = "_MobIdCallback";
		public const string MOB_RESTORE_CALLBACK_METHOD = "_RestoreCallBack";

		private bool isInited = false;
		private bool isUpdateIntent = true;

		public override void setRestoreSceneListener () 
		{
			initMobSdk ();
			AndroidJavaClass javaMoblink = getAndroidMoblink();;
			AndroidJavaObject l = new AndroidJavaObject ("com.mob.moblink.unity.RestoreSceneListener", MOB_GAMEOBJECT_NAME, MOB_RESTORE_CALLBACK_METHOD);
			javaMoblink.CallStatic ("setRestoreSceneListener", l);
			if (isUpdateIntent) {
				isUpdateIntent = false;
				updateIntent ();
			}
		}

		public override void GetMobId (MobLinkScene scene) {
			GetMobId (scene.path, scene.source, scene.customParams);
		}

		private void GetMobId (string path, string source, Hashtable param) {
			initMobSdk ();
			object map = hashtable2JavaMap(param);
			AndroidJavaClass javaClazz = new AndroidJavaClass ("com.mob.moblink.Scene");
			object scene = javaClazz.CallStatic<AndroidJavaObject> ("fromMap", map);
			object l = new AndroidJavaObject ("com.mob.moblink.unity.ActionListener", MOB_GAMEOBJECT_NAME, MOB_GETMOBID_CALLBACK_SUCCESS_METHOD, MOB_GETMOBID_CALLBACK_FAIL_METHOD);

			// call java sdk 
			AndroidJavaClass javaMoblink = getAndroidMoblink ();
			javaMoblink.CallStatic ("getMobID", scene, l);
		}

		private void updateIntent() {
			AndroidJavaObject activity = getAndroidContext ();
			object intent = activity.Call<AndroidJavaObject> ("getIntent");
			AndroidJavaClass javaMoblink = getAndroidMoblink ();
			javaMoblink.CallStatic ("updateIntent", activity, intent);
		}

		private void initMobSdk() {
			if (!isInited) {
				AndroidJavaObject jo = getAndroidContext ();
				AndroidJavaClass jc = new AndroidJavaClass ("com.mob.MobSDK");
				jc.CallStatic ("init", jo);
			}
			isInited = true;
		}

		private static AndroidJavaClass getAndroidMoblink() 
		{
			return new AndroidJavaClass ("com.mob.moblink.MobLink");
		}

		private static AndroidJavaObject getAndroidContext() 
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			return jo;
		}

		private static object getJavaString(String value) 
		{
			return new AndroidJavaObject("java.lang.String", value);
		}

		private static object hashtable2JavaMap(Hashtable map) 
		{
			AndroidJavaObject javaMap = new AndroidJavaObject ("java.util.HashMap");
			IntPtr putMethod = AndroidJNIHelper.GetMethodID (javaMap.GetRawClass(), "put", "Ljava.lang.Object;(Ljava.lang.Object;Ljava.lang.Object;)", false);

			foreach (string key in map.Keys)  
			{
				string value = map[key].ToString();
				object javaKey = getJavaString(key);
				object javaValue = getJavaString(value);
				object[] arr = new object[2];
				arr[0] = javaKey; arr[1] = javaValue;

				jvalue[] param = AndroidJNIHelper.CreateJNIArgArray(arr);
				AndroidJNI.CallObjectMethod(javaMap.GetRawObject(), putMethod, param);
			}
			return javaMap;
		}
	}
	#endif
}



