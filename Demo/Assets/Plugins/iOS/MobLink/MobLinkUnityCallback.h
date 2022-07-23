//
//  MobLinkUnityCallback.h
//  Unity-iPhone
//
//  Created by 陈剑东 on 2017/4/17.
//
//

#import <Foundation/Foundation.h>
#import <MobLinkPro/MobLink.h>

@interface MobLinkUnityCallback : NSObject <IMLSDKRestoreDelegate>

@property (nonatomic,strong) NSString * callBackObesever;
+ (MobLinkUnityCallback *)defaultCallBack;

@end
