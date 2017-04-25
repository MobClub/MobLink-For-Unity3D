using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections;
using System.IO;
using com.moblink.unity3d;
using com.moblink.unity3d.sdkporter;

public static class MobLinkPostProcessBuild{

	[PostProcessBuildAttribute(66)]
	public static void onPostProcessBuild(BuildTarget target,string targetPath)
	{
		string projPath = Path.GetFullPath (targetPath);
		EditInfoPlist (projPath);
	}

	private static void EditInfoPlist(string projPath)
	{
		XCPlist plist = new XCPlist (projPath);

		//URL Scheme 添加
		string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
			<array>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
					<string>support</string>
					</array>
				</dict>
			</array>";

		//在plist里面增加一行
		plist.AddKey(PlistAdd);
		plist.Save();
	}
}
