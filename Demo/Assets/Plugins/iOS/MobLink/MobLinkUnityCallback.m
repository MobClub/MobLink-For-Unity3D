//
//  MobLinkUnityCallback.m
//  Unity-iPhone
//
//  Created by 陈剑东 on 2017/4/17.
//
//

#import "MobLinkUnityCallback.h"
#import <MobLinkPro/MLSDKScene.h>
#import <MOBFoundation/MOBFJson.h>
@implementation MobLinkUnityCallback

+ (MobLinkUnityCallback *)defaultCallBack
{
    static MobLinkUnityCallback * _instance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _instance = [[MobLinkUnityCallback alloc] init];
    });
    return _instance;
}

- (void)IMLSDKWillRestoreScene:(MLSDKScene *)scene Restore:(void (^)(BOOL isRestore, RestoreStyle style))restoreHandler
{
    NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
    
    if (scene.path.length > 0)
    {
        resultDict[@"path"] = scene.path;
    }

    if (scene.params && scene.params.count > 0)
    {
        resultDict[@"params"] = scene.params;
    }
    
    NSString *resultStr  = @"";
    if (resultDict.count > 0)
    {
        resultStr = [MOBFJson jsonStringFromObject:resultDict];
    }
    
    NSString * callBackObseerver = @"MobLink";
    if (self.callBackObesever && [self.callBackObesever isKindOfClass:[NSString class]]) {
        callBackObseerver = self.callBackObesever;
    }
    UnitySendMessage([callBackObseerver UTF8String], "_RestoreCallBack", [resultStr UTF8String]);
}

@end
