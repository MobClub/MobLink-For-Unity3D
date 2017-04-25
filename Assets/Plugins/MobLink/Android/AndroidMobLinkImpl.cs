using UnityEngine;
using System;
using System.Collections;

namespace com.moblink.unity3d
{

	#if UNITY_ANDROID
	public class AndroidMobLinkImpl : MobLinkImpl {

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public override void InitSDK (String appKey) 
		{

		}

		public override void GetMobId (MobLinkScene scene)
		{


		}
	}
	#endif
}



