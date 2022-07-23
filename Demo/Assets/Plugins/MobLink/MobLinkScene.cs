using System;
using UnityEngine;
using System.Collections;

namespace com.moblink.unity3d
{
	public class MobLinkScene {
		public string path;
		public Hashtable customParams;

		public MobLinkScene(string scenePath, Hashtable sceneCustomParams) {
			try {
				this.path = scenePath;
				this.customParams = sceneCustomParams;
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}


	}
}