//
//  MobLinkUnity3DBridge.m
//  Unity-iPhone
//
//  Created by 陈剑东 on 2017/4/17.
//
//

#import "MobLinkUnity3DBridge.h"
#import "MobLinkUnityCallback.h"
#import <MobLinkPro/MobLink.h>
#import <MobLinkPro/MLSDKScene.h>
#import <MOBFoundation/MOBFJson.h>
#import <MOBFoundation/MobSDK+Privacy.h>
#import "UnityAppController.h"

static MobLinkUnityCallback *_callback = nil;
#if defined (__cplusplus)
extern "C" {
#endif

    extern void __iosMobLinkGetMobId (void *scenepath, void *params,void * observer);
    extern void __iosMobLinkSDKGetPolicy(int type , void * observer);
    extern void __iosMobLinkSDKSubmitPolicyGrantResult(int granted);
    extern void __iosMobLinkSDKSetAllowDialog(int allow);
    extern void __iosMobLinkSDKSetPolicyUI(void * backgroundColorRes , void * positiveBtnColorRes, void * negativeBtnColorRes);
    extern void __iosMobLinkSetOnRestoreCallBackObject(void * observer);
#if defined (__cplusplus)
}
#endif


#if defined (__cplusplus)
extern "C" {
#endif
    
    void __iosMobLinkSetOnRestoreCallBackObject(void * observer){
        if (observer != nil) {
            NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
            if (observerStr && [observerStr isKindOfClass:NSString.class]) {
                [MobLinkUnityCallback defaultCallBack].callBackObesever = observerStr;
            }
        }
    }
    void __iosMobLinkGetMobId (void *path, void *params,void * observer)
    {
        NSString *thePath = [NSString stringWithCString:path encoding:NSUTF8StringEncoding];
        NSString *theParamsStr = [NSString stringWithCString:params encoding:NSUTF8StringEncoding];
        NSDictionary *theParams = [MOBFJson objectFromJSONString:theParamsStr];
        NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        MLSDKScene *scene = [MLSDKScene sceneForPath:thePath params:theParams];
        [MobLink getMobId:scene result:^(NSString *mobid, NSString *domain, NSError *error) {
            
            NSString *str  = @"";
            if (mobid)
            {
                NSDictionary *result = @{@"mobid" : mobid};
                str = [MOBFJson jsonStringFromObject:result];
            }
            
            if (error && error.userInfo[@"error"])
            {
                NSDictionary *result = @{@"errorMsg" : error.userInfo[@"error"]};
                str = [MOBFJson jsonStringFromObject:result];
            }
            
            UnitySendMessage([observerStr UTF8String], "_MobIdCallback", [str UTF8String]);
        }];
        
    }
    static inline NSUInteger MOBLinkProhexStrToInt(NSString *str) {
        uint32_t result = 0;
        sscanf([str UTF8String], "%X", &result);
        return result;
    }

    static BOOL MOBLinkProhexStrToRGBA(NSString *str,
                             CGFloat *r, CGFloat *g, CGFloat *b, CGFloat *a) {
        NSCharacterSet *set = [NSCharacterSet whitespaceAndNewlineCharacterSet];
        str = [[str stringByTrimmingCharactersInSet:set] uppercaseString];
        if ([str hasPrefix:@"#"]) {
            str = [str substringFromIndex:1];
        } else if ([str hasPrefix:@"0X"]) {
            str = [str substringFromIndex:2];
        }
        
        NSUInteger length = [str length];
        if (length != 3 && length != 4 && length != 6 && length != 8) {
            return NO;
        }
        
        if (length < 5) {
            *r = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(0, 1)]) / 255.0f;
            *g = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(1, 1)]) / 255.0f;
            *b = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(2, 1)]) / 255.0f;
            if (length == 4)  *a = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(3, 1)]) / 255.0f;
            else *a = 1;
        } else {
            *r = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(0, 2)]) / 255.0f;
            *g = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(2, 2)]) / 255.0f;
            *b = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(4, 2)]) / 255.0f;
            if (length == 8) *a = MOBLinkProhexStrToInt([str substringWithRange:NSMakeRange(6, 2)]) / 255.0f;
            else *a = 1;
        }
        return YES;
    }
    
    static UIColor * MOBLinkProColorWithHexStr(NSString * hexStr){
        CGFloat r, g, b, a;
        if (MOBLinkProhexStrToRGBA(hexStr, &r, &g, &b, &a)) {
            if (@available(iOS 10.0, *)) {
                return [UIColor colorWithDisplayP3Red:r green:g blue:b alpha:a];
            }else{
                return [UIColor colorWithRed:r green:g blue:b alpha:a];
            }
        }
        return [UIColor whiteColor];
    }
    extern void __iosMobLinkSDKGetPolicy(int type , void * observer){
         NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        [MobSDK getPrivacyPolicy:[NSString stringWithFormat:@"%d",type] compeletion:^(NSDictionary * dic, NSError *error){
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            if (!error) {
                [resultDict setObject:[NSNumber numberWithInteger:1] forKey:@"status"];
            }else{
                [resultDict setObject:[NSNumber numberWithInteger:2] forKey:@"status"];
            }
            [resultDict setObject:[NSNumber numberWithInteger:1] forKey:@"action"];
            [resultDict setObject:dic[@"content"]?:@"" forKey:@"url"];
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
        }];
    }
    extern void __iosMobLinkSDKSubmitPolicyGrantResult(int granted){
        [MobSDK uploadPrivacyPermissionStatus:granted onResult:nil];
    }
    extern void __iosMobLinkSDKSetAllowDialog(int allow){
        [MobSDK setAllowShowPrivacyWindow:allow];
    }
    extern void __iosMobLinkSDKSetPolicyUI(void * backgroundColorRes , void * positiveBtnColorRes, void * negativeBtnColorRes){
        NSString *backgroundColorResString = [NSString stringWithCString:backgroundColorRes encoding:NSUTF8StringEncoding];
        NSString *positiveBtnColorResString = [NSString stringWithCString:positiveBtnColorRes encoding:NSUTF8StringEncoding];
        NSString *negativeBtnColorResString = [NSString stringWithCString:negativeBtnColorRes encoding:NSUTF8StringEncoding];
        [MobSDK setPrivacyBackgroundColor:MOBLinkProColorWithHexStr(backgroundColorResString) operationButtonColor:@[MOBLinkProColorWithHexStr(negativeBtnColorResString),MOBLinkProColorWithHexStr(positiveBtnColorResString)]];
    }
#if defined (__cplusplus)
}
#endif
@implementation MobLinkUnity3DBridge

@end
