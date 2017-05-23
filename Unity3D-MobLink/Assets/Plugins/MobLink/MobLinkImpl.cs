using UnityEngine;
using System.Collections;


namespace com.moblink.unity3d
{

	public abstract class MobLinkImpl
	{
		public virtual void InitSDK (string appKey) {
			
		}

		public virtual void GetMobId (MobLinkScene scene) {
			
		}

		#if UNITY_ANDROID
		public virtual void updateIntent() {
			
		}
		#endif
	}

}
	



