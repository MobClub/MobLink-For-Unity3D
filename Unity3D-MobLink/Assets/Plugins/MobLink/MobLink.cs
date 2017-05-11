using UnityEngine;
using System;
using System.Collections;

using System.Runtime.InteropServices;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour {

		public string AppKey = "1b8898cb51ccb";

		public delegate void GetMobIdHandler(string mobId);
		public delegate void RestoreSceneHandler(MobLinkScene scene);

		public static event GetMobIdHandler onGetMobId;
		public static event RestoreSceneHandler onRestoreScene;

		private static bool isInit;
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

		#if UNITY_ANDROID
		/**
		 * 本方法仅支持安卓
		 * 解析intent中，跟服务器匹配的scheme数据
		 * 解析成功后会回调_RestoreCallBack函数
		 */
		public static void setIntentHandler() 
		{
			moblinkUtils.setIntentHandler ();
			moblinkUtils.setIntentNull ();
		}

		#endif

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

			string path = res ["path"].ToString();
			string source = res ["source"].ToString();
			Hashtable customParams = (Hashtable)res ["params"];

			MobLinkScene scene = new MobLinkScene (path, source, customParams);
			onRestoreScene (scene);
		}
	}

}


