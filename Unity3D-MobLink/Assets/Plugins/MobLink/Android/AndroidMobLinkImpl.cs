using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{

	#if UNITY_ANDROID
	public class AndroidMobLinkImpl : MobLinkImpl {

		public const string MOB_GAMEOBJECT_NAME = "MobLink";
		public const string MOB_GETMOBID_CALLBACK_METHOD = "_MobIdCallback";
		public const string MOB_RESTORE_CALLBACK_METHOD = "_RestoreCallBack";

		public override void InitSDK (String appKey) 
		{
			AndroidJavaClass javaMoblink = getAndroidMoblink();;
			AndroidJavaObject context = getAndroidContext ();
			javaMoblink.CallStatic ("initSDK", context, appKey);
		}

		public override void GetMobId (MobLinkScene scene) {
			GetMobId (scene.path, scene.source, scene.customParams);
		}

		private void GetMobId (string path, string source, Hashtable param) {
			object map = hashtable2JavaMap(param);
			object l = new AndroidJavaObject ("com.mob.moblink.unity.GetMobIdListener", MOB_GAMEOBJECT_NAME, MOB_GETMOBID_CALLBACK_METHOD);

			// call java sdk 
			AndroidJavaClass javaMoblink = getAndroidMoblink ();
			javaMoblink.CallStatic ("getMobID", map, path, source, l);
		}

		public override void setIntentHandler() {
			AndroidJavaObject activity = getAndroidContext ();
			object intent = activity.Call<AndroidJavaObject> ("getIntent");
			object l = new AndroidJavaObject ("com.mob.moblink.unity.ActionListener", MOB_GAMEOBJECT_NAME, MOB_RESTORE_CALLBACK_METHOD);
			AndroidJavaClass javaMoblink = getAndroidMoblink ();
			javaMoblink.CallStatic ("setIntentHandler", intent, l);
		}

		public override void setIntentNull() {
			AndroidJavaObject activity = getAndroidContext ();
			activity.Call ("setIntent", null);
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
			//return AndroidJNI.NewStringUTF (value);
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



