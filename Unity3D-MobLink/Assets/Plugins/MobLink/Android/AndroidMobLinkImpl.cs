using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{

	#if UNITY_ANDROID
	public class AndroidMobLinkImpl : MobLinkImpl {

		public override void InitSDK (String appKey) 
		{
			AndroidJavaClass javaMoblink = getAndroidMoblink();;
			AndroidJavaObject context = getAndroidContext ();
			javaMoblink.CallStatic ("initSDK", context, appKey);
		}

		public override void GetMobId (string path, string source, Hashtable param, string goName, string method) {
			object map = hashtable2JavaMap(param);
			object l = new AndroidJavaObject ("com.mob.moblink.unity.GetMobIdListener", goName, method);

			// call java sdk 
			AndroidJavaClass javaMoblink = getAndroidMoblink ();
			javaMoblink.CallStatic ("getMobID", map, path, source, l);
		}

		public void setIntentHandler(string goName, string method) {
			object l = new AndroidJavaObject ("com.mob.moblink.unity.ActionListener", goName, method);
			AndroidJavaObject activity = getAndroidContext ();
			object intent = activity.Call<AndroidJavaObject> ("getIntent");
			activity.CallStatic ("setIntentHandler", intent, l);
		}

		public void clearIntent() {
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



