using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace com.moblink.unity3d
{
	public class MobLink : MonoBehaviour
	{

#if UNITY_IPHONE || UNITY_IOS
		private static event getPolicyHandle getPolicy;
#endif

		private static bool isInit;
		private static MobLink _instance;
		private static MobLinkImpl moblinkUtils;
		private static event GetMobIdHandler onGetMobId;
		private static event RestoreSceneHandler onRestoreScene;
		public OnSubmitPolicyGrantResultCallback onSubmitPolicyGrantResultCallback;

		public delegate void getPolicyHandle(string content);

		public delegate void GetMobIdHandler(string mobId, string errorInfo);

		public delegate void RestoreSceneHandler(MobLinkScene scene);

		public delegate void OnSubmitPolicyGrantResultCallback(bool success);

		void Awake() {
			if (!isInit) {
				isInit = true;
			}

			if (_instance != null) {
				Destroy(_instance.gameObject);
			}

			_instance = this;

#if UNITY_ANDROID
			moblinkUtils = new AndroidMobLinkImpl();
#elif UNITY_IPHONE
			moblinkUtils = new iOSMobLinkImpl(gameObject);
#endif
			DontDestroyOnLoad(this.gameObject);
		}

		public void _PolicyGrantResultCallback(bool success) {
			onSubmitPolicyGrantResultCallback(success);
		}

		public static void submitPolicyGrantResult(bool granted) {
			moblinkUtils.submitPolicyGrantResult(granted);
		}

		public static void setAllowDialog(bool allowDialog) {
			moblinkUtils.setAllowDialog(allowDialog);
		}

		public static void setPolicyUi(String backgroundColorRes, String positiveBtnColorRes, String negativeBtnColorRes) {
			moblinkUtils.setPolicyUi(backgroundColorRes, positiveBtnColorRes, negativeBtnColorRes);
		}

		public static void setRestoreSceneListener(com.moblink.unity3d.MobLink.RestoreSceneHandler sceneHander) {
			moblinkUtils.setRestoreSceneListener();
			onRestoreScene += sceneHander;
		}

		public static void getMobId(MobLinkScene scene, GetMobIdHandler modIdHandler) {
			onGetMobId += modIdHandler;
			moblinkUtils.GetMobId(scene);
		}

		public void _MobIdCallback(string data) {
			Hashtable json = (Hashtable)MiniJSON.jsonDecode(data);
			if (json["mobid"] != null) {
				string mobId = json["mobid"].ToString();
				onGetMobId(mobId, null);
			} else if (json["errorMsg"] != null) {
				string errorInfo = json["errorMsg"].ToString();
				onGetMobId(null, errorInfo);
			}
			onGetMobId = null;
		}

		public void _RestoreCallBack(string data) {
			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}

			string path = res["path"].ToString();
			Hashtable customParams = (Hashtable)res["params"];

			MobLinkScene scene = new MobLinkScene(path, customParams);
			onRestoreScene(scene);
		}
#if UNITY_IPHONE || UNITY_IOS
		public static void getPrivacyPolicy(bool url, getPolicyHandle handle) {
			getPolicy += handle;
			 moblinkUtils.getPrivacyPolicy(url);
		}

		public void _Callback(string data) {
			if (data == null) {
				return;
			}

				Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}

			int status = Convert.ToInt32(res["status"]);
			int action = Convert.ToInt32(res["action"]);
			switch(status) {
				case 1: {
					if (action == 1) {
						if (getPolicy != null) {
							string url = (string)res["url"];
							getPolicy(url);
						}
					}
					break;
				}
				case 2: {
					break;
				}
				case 3: {
					break;
				}
			}
		}
#endif

// #if UNITY_ANDROID
		// public static void getPrivacyPolicy(bool url) {
		// 	getPolicy += handle;
		// 	 moblinkUtils.getPrivacyPolicy(url);
		// }
// #endif

	}
}