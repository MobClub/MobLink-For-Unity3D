using UnityEngine;
using System;
using System.Collections;

using System.Runtime.InteropServices;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour {

		public string AppKey = "1bf42e96da8f0";

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

		private void _MobIdCallback (string mobid)
		{
			onGetMobId (mobid);
		}
			
		private void _RestoreCallBack (string data)
		{
			Hashtable res = (Hashtable) MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) 
			{
				return;
			}
				
			onRestoreScene (res);
		}
	}

}


