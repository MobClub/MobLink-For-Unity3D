using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
//using cn.mob.unity3d.sdkporter;

namespace com.moblink.unity3d
{
#if UNITY_IPHONE
	[CustomEditor(typeof(MobLink))]
	[ExecuteInEditMode]
	public class MLConfigEditor : Editor {
		string appKey = "";
		string appSecret = "";
		string mobNetLater = "";
		string scheme = "";
		string universalLink = "";
		string entitlements = "";
		void Awake()
		{
			Prepare ();
		}
			
		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space ();
			appKey = EditorGUILayout.TextField ("MobAppKey", appKey);
			appSecret = EditorGUILayout.TextField ("MobAppSecret", appSecret);
			mobNetLater = EditorGUILayout.TextField ("MOBNetLater", mobNetLater);
			scheme = EditorGUILayout.TextField("MobScheme", scheme);
			universalLink = EditorGUILayout.TextField("MobUniversalLink", universalLink);
			entitlements = EditorGUILayout.TextField("MobEntitlements", entitlements);
			Save ();
		}
			
		private void Prepare()
		{
			try
			{
				var files = System.IO.Directory.GetFiles(Application.dataPath , "MOB.keypds", System.IO.SearchOption.AllDirectories);
				string filePath = files [0];
				FileInfo projectFileInfo = new FileInfo( filePath );
				if(projectFileInfo.Exists)
				{
					StreamReader sReader = projectFileInfo.OpenText();
					string contents = sReader.ReadToEnd();
					sReader.Close();
					sReader.Dispose();
					Hashtable datastore = (Hashtable)MiniJSON.jsonDecode( contents );
					appKey = (string)datastore["MobAppKey"];
					appSecret = (string)datastore["MobAppSecret"];
					mobNetLater = (string)datastore["MOBNetLater"];
					scheme = (string)datastore["MobScheme"];
					universalLink = (string)datastore["MobUniversalLink"];
					entitlements = (string)datastore["MobEntitlements"];
				}
				else
				{
					Debug.LogWarning ("MOB.keypds no find");
				}
			}
			catch(Exception e) 
			{
				if(appKey.Length == 0)
				{
					appKey = "moba6b6c6d6";
					appSecret = "b89d2427a3bc7ad1aea1e1e8c1d36bf3";
					mobNetLater = "2";
					scheme = "moblink";
					universalLink = "applinks:z.t4m.cn";
					entitlements = "Base.entitlements";
				}
				Debug.LogException (e);
			}
		}
			
		private void Save()
		{
			try
			{
				var files = System.IO.Directory.GetFiles(Application.dataPath , "MOB.keypds", System.IO.SearchOption.AllDirectories);
				string filePath = files [0];
				if(File.Exists(filePath))
				{
					Hashtable datastore = new Hashtable();
					datastore["MobAppKey"] = appKey;
					datastore["MobAppSecret"] = appSecret;
					datastore["MOBNetLater"] = mobNetLater;
					datastore["MobScheme"] = scheme;
					datastore["MobUniversalLink"] = universalLink;
					datastore["MobEntitlements"] = entitlements;
                    var json = MiniJSON.jsonEncode(datastore);
					StreamWriter sWriter = new StreamWriter(filePath);
					sWriter.WriteLine(json);
					sWriter.Close();
					sWriter.Dispose();
				}
				else
				{
					Debug.LogWarning ("MOB.keypds no find");
				}
			}
			catch(Exception e) 
			{
				Debug.LogWarning ("error");
				Debug.LogException (e);
			}
		}
	}
#endif
}