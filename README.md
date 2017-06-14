# Unity For MobLink 快速集成文档

## 下载并导入MobLink

下载[Unity-For-MobLink](https://github.com/MobClub/Unity-For-MobLink),打开项目双击MobLink.unitypackage相关文件。注意该操作可能会覆盖您原来已经存在的文件！ 


## 拖入MobLink并配置应用信息

导入unitypackage后,在Plugins - MobLink 中找到MobLink.prefab。将其拖载到您的项目中,如图示
![](https://lh3.googleusercontent.com/-RvxnRpiii5w/WUDd_EPdm4I/AAAAAAAABlc/QwRZ5BngtOwnjifRJhfFvN1MAAFaBL33wCHMYCw/I/14974233545014.jpg)

### iOS配置应用信息

![](https://lh3.googleusercontent.com/-sN5_9Oe_iHg/WUDekgFs53I/AAAAAAAABlk/oEliwwhY0BwIQda9ney-K_8yPcfK3CbEACHMYCw/I/14974235048946.jpg)

### Android配置应用信息


## 调用接口及获取回调

### 调用获取mobid

```
Hashtable custom = new Hashtable ();
custom ["ChapterID"] = 1001;
custom ["ChapterName"] = "Dragon Fire";
//构造场景参数
MobLinkScene scene = new MobLinkScene ("/chapter1/dragon", "userid-123456", custom);
//获取mobid
MobLink.getMobId (scene);
```


### 设置回调

```
//获取mobid之回调
void mobIdHandler (string mobid)
{
	Console.Write ("Received MobId:" + mobid);
}
		
//场景恢复之回调(在应用唤醒时此回调会被触发)
void sceneHandler (MobLinkScene scene)
{
	Console.Write ("path:" + scene.path);
	Console.Write ("source:" + scene.source);
	Console.Write ("params:" + MiniJSON.jsonEncode (scene.customParams));
}
```

```
MobLink.onGetMobId += mobIdHandler;
MobLink.onRestoreScene += sceneHandler;
```


### 对于Android端的一些配置

1. 配置scheme: 
Android平台的scheme是在AndroidManifest里配置的，需要在Activity的声明处增加如下配置：

```
<intent-filter>
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.BROWSABLE"/>
    <category android:name="android.intent.category.DEFAULT"/>
    <data android:scheme="mlink" android:host="com.mob.moblink.demo">
</intent-filter>
```
scheme和host都由您自己定义(最好是唯一的)，但要与后端配置保持一致。
AppLinks配置(AppLinks的支持从SDK-v1.1.0开始支持)

```
<intent-filter android:autoVerify="true">
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.DEFAULT"/>
    <category android:name="android.intent.category.BROWSABLE"/>
    <data android:scheme="http" android:host="70r9.ulml.mob.com" />
    <data android:scheme="https" android:host="70r9.ulml.mob.com" />
</intent-filter>
```
这里的scheme只能是http或者https, host请从后端配置里读取
![aaaa](http://wiki.mob.com/wp-content/uploads/2017/02/aaaa.png)

2. 在所有Scene中增加如下代码

```
#if UNITY_ANDROID
protected virtual void OnApplicationPause(bool pauseStatus)
{
    // 您应该复制这段代码 -> 您的实现方法里.
    if (!pauseStatus) {
        MobLink.updateIntent ();
    }
}
#endif
```

3. 仅当你的Activity的launchMode为singleTop时，您才需要这样处理(Android代码)

```
protected void onNewIntent(Intent intent) {
    setIntent(intent);
}
```


### 对于iOS端的一些配置

预配置Scheme:
找到MobLinkAutoPackage - Editor - SDKPorter - MobLinkPostProcessBuild.cs
在EditInfoPlist方法中，修改CFBundleURLSchemes 下的值,将其设置为您在MobLink后台填入的 URI Scheme (注意不带'://')

![14937845549367](https://lh3.googleusercontent.com/-_le-4mpzKIw/WQlelJ3Q6uI/AAAAAAAABj4/443zqhF8bNAD1qPOwRathPkF4BXFslyBQCHM/I/14937845549367.jpg)


配置Universal Link:
在生成Xcode项目后,配置在MobLink后台所填入的Universal Link

![14937849253534](https://lh3.googleusercontent.com/-sj8hXdc0WUA/WQlemadRzLI/AAAAAAAABj8/Jh9JQ2YkEWIONNeqHXsAnhioSP16FCs_gCHM/I/14937849253534.png)


# 如何Build Demo For Android 

1. 使用git工具下载代码

```
git clone https://github.com/MobClub/Unity-For-MobLink.git
```

2. 使用Unity3d v5.x(推荐5.4以上版本)打开

3. 打开Player Settings, 切换到Android选项下
 
    请使用我们提供的签名文件， 文件地址：/Unity3D-MobLink/demokey.keystore
    签名密码：123456
    
    ![moblink_unity3d_sign](http://wiki.mob.com/wp-content/uploads/2014/11/moblink_unity3d_sign.png)
    
4. build 成功后，安装apk到手机上

5. 在不同的平台(如微信/浏览器)分享以下链接，然后访问链接
    
    http://f.moblink.mob.com/demo/a?mobid=up
    
    http://f.moblink.mob.com/demo/b?mobid=up
    
    http://f.moblink.mob.com/demo/c?mobid=up
    
    http://f.moblink.mob.com/demo/d?mobid=up
    


