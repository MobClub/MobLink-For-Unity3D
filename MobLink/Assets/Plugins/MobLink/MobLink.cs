using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour {

        // 第一步：定义委托
		public delegate void GetMobIdHandler(string mobId);
		public delegate void RestoreSceneHandler(MobLinkScene scene);

        // 第二步：创建委托对象
		private static event GetMobIdHandler onGetMobId;
		private static event RestoreSceneHandler onRestoreScene;

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
				isInit = true;
			}

			if (_instance != null) 
			{
				DestroyObject (_instance.gameObject);
			}
			_instance = this;

			DontDestroyOnLoad(this.gameObject);
		}

		public static void setRestoreSceneListener (com.moblink.unity3d.MobLink.RestoreSceneHandler sceneHander) {
			moblinkUtils.setRestoreSceneListener ();
			onRestoreScene += sceneHander;
		}

		public static void getMobId(MobLinkScene scene, GetMobIdHandler modIdHandler)
		{
            // 第三步：将函数名赋值给委托（函数名在Demo.cs中创建）
			onGetMobId += modIdHandler;
			moblinkUtils.GetMobId(scene);
		}

		private void _MobIdCallback (string data)
		{
			// 解析出mobId
			Hashtable json = (Hashtable) MiniJSON.jsonDecode(data);
			string modId = json["mobid"].ToString();

            // 第四步：调用委托实例，执行方法
			onGetMobId (modId);
			onGetMobId = null;
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


