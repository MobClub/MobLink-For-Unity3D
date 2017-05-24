# Unity For MobLink 快速集成文档

## 下载并导入MobLink

下载[Unity-For-MobLink](https://github.com/MobClub/Unity-For-MobLink),打开项目双击MobLink.unitypackage相关文件。注意该操作可能会覆盖您原来已经存在的文件！ 


## 拖入MobLink并配置MobLink AppKey

导入unitypackage后,在Plugins - MobLink 中找到MobLink.prefab。将其拖载到您的项目中,如图示

![14937771831429](https://lh3.googleusercontent.com/-lKH-XGIWld4/WQlee37dhVI/AAAAAAAABj0/Z1ZSL55KF0EYmRpC9VCSWF_ZJRuHqUb9QCHM/I/14937771831429.jpg)

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
                <data android:scheme="http" android:host="7pb6.ulml.mob.com" />
                <data android:scheme="https" android:host="7pb6.ulml.mob.com" />
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


