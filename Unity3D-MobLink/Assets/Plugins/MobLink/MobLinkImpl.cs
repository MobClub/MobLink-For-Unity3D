using UnityEngine;
using System.Collections;


namespace com.moblink.unity3d
{

	public abstract class MobLinkImpl
	{
		public virtual void InitSDK (string appKey) {
			
		}

		public virtual void GetMobId (MobLinkScene scene) {
			GetMobId (scene.path, scene.source, scene.customParams);
		}

		public virtual void GetMobId (string path, string source, Hashtable param) {
			// subclass impl this method
		}

		#if UNITY_ANDROID
		public virtual void setIntentHandler() {
			
		}

		public virtual void setIntentNull() {
			
		}

		public virtual string getIntentPath() {
			return null;
		}
		#endif
	}

}
	



