using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{

	#if UNITY_ANDROID
	public class AndroidMobLinkImpl : MobLinkImpl {

		private AndroidJavaClass javaMoblink;
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public override void InitSDK (String appKey) 
		{
			initAndroidMoblink();
			AndroidJavaObject context = getAndroidContext ();
			object appkeyJava = getJavaString (appKey);
			javaMoblink.CallStatic ("initSDK", context, appkeyJava);
		}

		public override void GetMobId (MobLinkScene scene)
		{
			object path = getJavaString(scene.path);
			object source = getJavaString (scene.source);
			object map = hashtable2JavaMap(scene.customParams);
			object l = new AndroidJavaObject ("com.mob.moblink.unity.ActionListener", "MOBLink", "_MobIdCallback");

			// call java sdk 
			initAndroidMoblink ();
			javaMoblink.CallStatic ("getMobID", map, path, source, l);
		}

		private void initAndroidMoblink() 
		{
			if (null == javaMoblink) {
				javaMoblink = new AndroidJavaClass ("com.mob.moblink.MobLink");
			}
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

		/*
		private static void _callJava_GetMobId(AndroidJavaObject) {
			AndroidJavaObject a;

			Lcom/mob/moblink/MobLink;.getMobID(Ljava.util.HashMap;Ljava.lang.String;Ljava.lang.String;Lcom.mob.moblink.unity.MobIdActionListener;)V
			*/
	}
	#endif
}



