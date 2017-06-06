using System;
using System.Collections;
using UnityEngine;

namespace com.moblink.unity3d
{
	[Serializable]
	public class MobLinkConfig
	{
		public string appKey;
		public string appSecret;

		public MobLinkConfig()
		{
			this.appKey = "mob_a6b6c6d6";
			this.appSecret = "b89d2427a3bc7ad1aea1e1e8c1d36bf3";
		}
	}		

}
