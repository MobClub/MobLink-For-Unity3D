//using UnityEngine;
//using System.Collections;
//
//public class MLConfigEditor : MonoBehaviour {
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//}

using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


namespace com.moblink.unity3d
{
	[CustomEditor(typeof(MobLink))]
	[ExecuteInEditMode]
	public class MLConfigEditor : Editor {

		private MobLinkConfig config;

		void Awake()
		{
			Prepare ();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space ();
			config.appKey = EditorGUILayout.TextField ("MobAppKey", config.appKey);
			config.appSecret = EditorGUILayout.TextField ("MobAppSecret", config.appSecret);
			Save ();
		}

		private void Prepare()
		{
			string filePath = Application.dataPath + "/MobLinkAutoPackage/Editor/SDKPorter/MobLinkConfig.bin";
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				Stream destream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				MobLinkConfig config = (MobLinkConfig)formatter.Deserialize(destream);
				destream.Flush();
				destream.Close();
				this.config = config;
			}
			catch(Exception)
			{
				this.config = new MobLinkConfig ();
			}
		}

		private void Save()
		{
			try
			{
				string filePath = Application.dataPath + "/MobLinkAutoPackage/Editor/SDKPorter/MobLinkConfig.bin";
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				formatter.Serialize(stream, this.config);
				stream.Flush();
				stream.Close();
			}
			catch (Exception e) 
			{
				Debug.Log ("save error:" + e);
			}
		}

	}
}