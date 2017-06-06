using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections;
using System.IO;
using com.moblink.unity3d;
using com.moblink.unity3d.sdkporter;
using System.Runtime.Serialization.Formatters.Binary;

public static class MobLinkPostProcessBuild{

	[PostProcessBuildAttribute(66)]
	public static void onPostProcessBuild(BuildTarget target,string targetPath)
	{
		string unityEditorAssetPath = Application.dataPath;

		if (target != BuildTarget.iOS) 
		{
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		XCProject project = new XCProject (targetPath);
		//var files = System.IO.Directory.GetFiles( unityEditorAssetPath, "*.projmods", System.IO.SearchOption.AllDirectories );
		var files = System.IO.Directory.GetFiles( unityEditorAssetPath + "/MobLinkAutoPackage/Editor/SDKPorter", "*.projmods", System.IO.SearchOption.AllDirectories);
		foreach( var file in files ) 
		{
			project.ApplyMod( file );
		}

		string projPath = Path.GetFullPath (targetPath);
		EditInfoPlist (projPath);

		//Finally save the xcode project
		project.Save();
	}

	private static void EditInfoPlist(string projPath)
	{
		MobLinkConfig theConfig;

		try
		{
			string filePath = Application.dataPath + "/MobLinkAutoPackage/Editor/SDKPorter/MobLinkConfig.bin";
			BinaryFormatter formatter = new BinaryFormatter();
			Stream destream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			MobLinkConfig config = (MobLinkConfig)formatter.Deserialize(destream);
			destream.Flush();
			destream.Close();
			theConfig = config;
		}
		catch(Exception)
		{
			theConfig = new MobLinkConfig ();
		}

		string AppKey = @"<key>MOBAppkey</key> <string>" + theConfig.appKey + "</string>";
		string AppSecret = @"<key>MOBAppSecret</key> <string>" + theConfig.appSecret + "</string>";

		XCPlist plist = new XCPlist (projPath);

		//URL Scheme 添加
		string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
			<array>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
					<string>mlink</string>
					</array>
				</dict>
			</array>";

		//在plist里面增加一行
		plist.AddKey(PlistAdd);
		plist.AddKey(AppKey);
		plist.AddKey(AppSecret);
		plist.Save();
	}
}
