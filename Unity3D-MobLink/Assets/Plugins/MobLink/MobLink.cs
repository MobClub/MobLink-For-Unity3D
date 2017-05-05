using UnityEngine;
using System;
using System.Collections;

using System.Runtime.InteropServices;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour {

		public string AppKey = "1b8898cb51ccb";
		public const string MOB_GAMEOBJECT_NAME = "MobLink";
		public const string MOB_GETMOBID_CALLBACK_METHOD = "_MobIdCallback";
		public const string MOB_RESTORE_CALLBACK_METHOD = "_RestoreCallBack";

		private static bool isInit;

		public delegate void GetMobIdHandler(string mobId);
		public delegate void RestoreSceneHandler(Hashtable scene);

		public static event GetMobIdHandler onGetMobId;
		public static event RestoreSceneHandler onRestoreScene;

		private static MobLink _instance;
		private static MobLinkImpl moblinkUtils;

		void Awake()
		{
			if (!isInit) 
			{
				#if UNITY_ANDROID
				moblinkUtils = new AndroidMobLinkImpl();
				#elif UNITY_IPHONE
				moblinkUtils = new iOSMobLinkImpl();
				#endif
				moblinkUtils.InitSDK (AppKey);
				isInit = true;
			}

			if (_instance != null) 
			{
				DestroyObject (_instance.gameObject);
			}
			_instance = this;

			DontDestroyOnLoad(this.gameObject);
		}

		public static void getMobId(MobLinkScene scene)
		{
			moblinkUtils.GetMobId(scene);
		}

		public static void setIntentHandler(object l) {
			moblinkUtils.setIntentHandler (MOB_GAMEOBJECT_NAME, MOB_RESTORE_CALLBACK_METHOD);
		}

		public static void clearIntent() {
			moblinkUtils.clearIntent ();
		}

		public static string getIntentPath() {
			return moblinkUtils.getIntentPath ();
		}

		private void _MobIdCallback (string mobid)
		{
			onGetMobId (mobid);
		}
			
		private void _RestoreCallBack (string data)
		{
			Debug.Log ("data" + data);
			Hashtable res = (Hashtable) MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) 
			{
				return;
			}
			onRestoreScene (res);
		}
	}

}


