using UnityEngine;
using System.Collections;


namespace com.moblink.unity3d
{

	public abstract class MobLinkImpl
	{
		public virtual void InitSDK (string appKey) {
			
		}

		public virtual void GetMobId (MobLinkScene scene) {
			GetMobId (scene.path, scene.source, scene.customParams, MobLink.MOB_GAMEOBJECT_NAME, MobLink.MOB_GETMOBID_CALLBACK_METHOD);
		}

		public virtual void GetMobId (string path, string source, Hashtable param, string goName, string method) {
			
		}

		public virtual void setIntentHandler(string goName, string method) {
			
		}

		public virtual void clearIntent() {
			
		}
	}

}
	



