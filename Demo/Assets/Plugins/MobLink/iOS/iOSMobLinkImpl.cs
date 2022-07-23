using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace com.moblink.unity3d
{
#if UNITY_IPHONE || UNITY_IOS
	public class iOSMobLinkImpl : MobLinkImpl {
		private string _callbackObjectName = "MobLink";

		[DllImport("__Internal")]
		private static extern void __iosMobLinkGetMobId(string path, string customParamsStr, string observer);

		[DllImport("__Internal")]
		private static extern string __iosMobLinkSDKGetPolicy(bool type, string observer);

		[DllImport("__Internal")]
		private static extern void __iosMobLinkSDKSubmitPolicyGrantResult(bool granted);

		[DllImport("__Internal")]
		private static extern void __iosMobLinkSDKSetAllowDialog(bool allowDialog);

		[DllImport("__Internal")]
		private static extern void __iosMobLinkSDKSetPolicyUI(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes);

		[DllImport("__Internal")]
		private static extern void __iosMobLinkSetOnRestoreCallBackObject(string observer);

		public iOSMobLinkImpl(GameObject go) {
			try {
				_callbackObjectName = go.name;
				setOnRestoreCallBackObject();
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public void setOnRestoreCallBackObject() {
			__iosMobLinkSetOnRestoreCallBackObject(_callbackObjectName);
		}

		public override void GetMobId(MobLinkScene scene) {
			string customParamsStr = MiniJSON.jsonEncode(scene.customParams);
			__iosMobLinkGetMobId(scene.path, customParamsStr, _callbackObjectName);
		}

		public override string getPrivacyPolicy(bool url) {
			__iosMobLinkSDKGetPolicy(url, _callbackObjectName);
			return @"";
		}

		public override void submitPolicyGrantResult(bool granted) {
			__iosMobLinkSDKSubmitPolicyGrantResult(granted);
		}

		public override void setAllowDialog(bool allowDialog) {
			__iosMobLinkSDKSetAllowDialog(allowDialog);
		}

		public override void setPolicyUi(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes) {
			__iosMobLinkSDKSetPolicyUI(backgroundColorRes, positiveBtnColorRes, negativeBtnColorRes);
		}
	}
#endif
}