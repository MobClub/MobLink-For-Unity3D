using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using cn.mob.unity3d.sdkporter;

namespace com.moblink.unity3d
{
	#if UNITY_ANDROID
	[CustomEditor(typeof(MobLink))]
	[ExecuteInEditMode]
	public class MLConfigEditor : Editor {

		void Awake()
		{
			PrepareManifest ();
		}
			
		private void PrepareManifest()
		{
			string text = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
			FileInfo destFi = new FileInfo (text);
			if (destFi.Exists) {
				return;
			}

			text = Application.dataPath + "/Demo/Android/AndroidManifest.xml";
			FileInfo srcFi = new FileInfo (text);
			if (srcFi.Exists) {
				srcFi.CopyTo (destFi.FullName);
			}
		}
	}

	#endif
}