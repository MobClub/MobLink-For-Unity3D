# Unity For MobLink 快速集成文档

## 下载并导入MobLink

下载[Unity-For-MobLink](https://github.com/MobClub/Unity-For-MobLink),打开项目双击MobLink.unitypackage相关文件。注意该操作可能会覆盖您原来已经存在的文件！ 


## 拖入MobLink并配置应用信息

导入unitypackage后,在Plugins - MobLink 中找到MobLink.prefab。将其拖载到您的项目中,如图示

![](https://lh3.googleusercontent.com/-RvxnRpiii5w/WUDd_EPdm4I/AAAAAAAABlc/QwRZ5BngtOwnjifRJhfFvN1MAAFaBL33wCHMYCw/I/14974233545014.jpg)

#### iOS配置及注意事项(Android开发者可忽略)
1.Unity切换到iOS环境后，点击以被拖进去的MobLink,在编辑器右侧填入您的AppKey,AppSecret

![](https://lh3.googleusercontent.com/-sN5_9Oe_iHg/WUDekgFs53I/AAAAAAAABlk/oEliwwhY0BwIQda9ney-K_8yPcfK3CbEACHMYCw/I/14974235048946.jpg)

2.预配置Scheme
找到MobLinkAutoPackage - Editor - SDKPorter - MobLinkPostProcessBuild.cs
在EditInfoPlist方法中，修改CFBundleURLSchemes 下的值,将其设置为您在MobLink后台填入的 URI Scheme (注意不带'://')

![14937845549367](https://lh3.googleusercontent.com/-_le-4mpzKIw/WQlelJ3Q6uI/AAAAAAAABj4/443zqhF8bNAD1qPOwRathPkF4BXFslyBQCHM/I/14937845549367.jpg)

3.配置Universal Link(**本步骤在生成的Xcode中操作**)

在生成Xcode项目后,配置在MobLink后台所填入的Universal Link

![14937849253534](https://lh3.googleusercontent.com/-sj8hXdc0WUA/WQlemadRzLI/AAAAAAAABj8/Jh9JQ2YkEWIONNeqHXsAnhioSP16FCs_gCHM/I/14937849253534.png)


#### Android配置及注意事项(iOS开发者可忽略)

在这里只需要配置AndroidManifest部分, 请参考原生文档[http://wiki.mob.com/sdk-moblink-android-2-0-0/](http://wiki.mob.com/sdk-moblink-android-2-0-0/)) 中的"配置AndroidManiFest.xml文件"进行配置

## 调用接口及获取回调

### 编写和设置场景还原的回调

```
// 全局的场景还原监听函数
protected static void OnRestoreScene(MobLinkScene scene)
{
	Console.Write ("path:" + scene.path);
	Console.Write ("source:" + scene.source);
	Console.Write ("params:" + MiniJSON.jsonEncode (scene.customParams));
}
```

一般在第一个场景的MonoBehaviour.Start()函数中设置监听

```
protected void Start () {
	MobLink.setRestoreSceneListener (OnRestoreScene);
}
```

### 编写modId的回调和调用获取mobid

```
// 获取mobid的回调
void mobIdHandler (string mobid)
{
	Console.Write ("Received MobId:" + mobid);
}
```

```
Hashtable custom = new Hashtable ();
custom ["ChapterID"] = 1001;
custom ["ChapterName"] = "Dragon Fire";

//构造场景参数
MobLinkScene scene = new MobLinkScene ("/chapter1/dragon", "userid-123456", custom);

// 获取mobid
MobLink.getMobId (scene, mobIdHandler);
```

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
    
    adfadsfadf
    
    http://f.moblink.mob.com/demo/b?mobid=up
    
    sdfasdf
    http://f.moblink.mob.com/demo/c?mobid=up
    
    http://f.moblink.mob.com/demo/d?mobid=up
    
