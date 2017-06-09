using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour {

		public delegate void GetMobIdHandler(string mobId);
		public delegate void RestoreSceneHandler(MobLinkScene scene);

		private static event GetMobIdHandler onGetMobId;
		private static event RestoreSceneHandler onRestoreScene;

		private static bool isInit;
		private static MobLink _instance;
		private static MobLinkImpl moblinkUtils;

		private MobLinkConfig getConfig()
		{
			MobLinkConfig theConfig;

			try
			{
				string filePath = Application.dataPath + "/MobLinkAutoPackage/Editor/SDKPorter/MobLinkConfig.bin";
				BinaryFormatter formatter = new BinaryFormatter();
				Stream destream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				MobLinkConfig config = (MobLinkConfig)formatter.Deserialize(destream);
				destream.Flush();
				destream.Close();
				theConfig = config;
			}
			catch(Exception)
			{
				theConfig = new MobLinkConfig ();
			}

			return theConfig;
		}

		void Awake()
		{
			MobLinkConfig config = getConfig ();

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
			onGetMobId += modIdHandler;
			moblinkUtils.GetMobId(scene);
		}

		private void _MobIdCallback (string mobid)
		{
			onGetMobId (mobid);
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

		private void _MobIdCallbackFromAndroid(string data)
		{
			Hashtable json = (Hashtable) MiniJSON.jsonDecode(data);
		}

		private void _MobIdCallbackSuccess(string data)
		{
			Hashtable json = (Hashtable) MiniJSON.jsonDecode(data);
			string modId = json["mobID"].ToString();
			_MobIdCallback (modId);
		}

		private void _MobIdCallbackFail(string data)
		{
			onGetMobId = null;
		}
	}

}


