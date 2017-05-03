using UnityEngine;
using System.Collections;


namespace com.moblink.unity3d
{

	public abstract class MobLinkImpl
	{
		public abstract void InitSDK (string appKey);
		public abstract void GetMobId (MobLinkScene scene);
	}

}
	



