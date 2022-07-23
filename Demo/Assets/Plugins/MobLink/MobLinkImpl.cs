using System;
using UnityEngine;
using System.Collections;

namespace com.moblink.unity3d
{
	public abstract class MobLinkImpl
	{
		public virtual void setRestoreSceneListener() {}

		public virtual void GetMobId(MobLinkScene scene) {}

		/// <summary>
		/// 获取MobSDK隐私协议内容, url为true时返回MobTech隐私协议链接，false返回协议的内容
		/// <summary>
		public abstract string getPrivacyPolicy(bool url);

		/// <summary>
		/// 提交用户授权结果给MobSDK
		/// <summary>
		public abstract void submitPolicyGrantResult(bool granted);

		/// <summary>
		/// 是否允许展示二次确认框
		/// <summary>
		public abstract void setAllowDialog(bool allowDialog);

		/// <summary>
		/// 设置二次确认框样式
		/// <summary>
		public abstract void setPolicyUi(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes);

	}
}