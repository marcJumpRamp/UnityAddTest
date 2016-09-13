//=============================================================================
//
//  Copy this file into your Unity project's Assets/Plugins/iOS folder.
//
//=============================================================================

#import <TremorVideoAd/TremorVideoAd.h>



@interface UnityIOSDelegate : NSObject<TremorVideoAdDelegate>
{
}

- (void)adReady:(BOOL)success;
- (void)adStart;
- (void)adSkipped;
- (void)adComplete:(BOOL)didShowAd responseCode:(NSInteger)responseCode;

@end


static UnityIOSDelegate* iosDelegate = nil;
static NSString *appID = nil;


@implementation UnityIOSDelegate

- (void)adReady:(BOOL)success {
    UnitySendMessage( "TremorVideo", "adReady", success? "true": "false" );
}

- (void)adStart {
    UnitySendMessage( "TremorVideo", "adStart", "");
}
- (void)adSkipped {
    UnitySendMessage( "TremorVideo", "adSkipped", "");
}

- (void)adComplete:(BOOL)didShowAd responseCode:(NSInteger)responseCode {
    char param[80];
    sprintf( param, "%s: %li", didShowAd? "true": "false", (long)responseCode);
    UnitySendMessage( "TremorVideo", "adComplete", param);
}

@end


#include <iostream>
using namespace std;
extern void UnityPause(bool);
extern UIViewController* UnityGetGLViewController();
extern "C" {
    
    char* MMMakeStringCopy ( const char* sourceString ) {
        if (sourceString == NULL) {
            return NULL;
        }
        
        char* copyString = (char*) malloc(strlen(sourceString) + 1);
        strcpy(copyString, sourceString);
        
        return copyString;
    }
    
    void MMinitWithID( const char* app_id ) {
        if( iosDelegate == nil )
            iosDelegate = [[UnityIOSDelegate alloc] init];
        appID = [NSString stringWithUTF8String:app_id];
        [TremorVideoAd initWithAppID: appID];
        [TremorVideoAd setDelegate: iosDelegate];
    }
    
    bool MMisAdReady ( void ) {
        return [TremorVideoAd isAdReady];
    }
    
    void MMloadAd ( void ) {
        [TremorVideoAd loadAd];
    }

    void MMshowAd ( void ) {
        [TremorVideoAd showAd: UnityGetGLViewController()];
    }
    
    void MMstop( void ) {
        [TremorVideoAd stop];
    }
    
    
    void MMdestroy( void ) {
        [TremorVideoAd destroy];
    }
    
    const char* MMgetVersion( void ) {
        const char* sdkVersion = MMMakeStringCopy([[TremorVideoAd getVersion] UTF8String]);
        return sdkVersion;
    }
    
    bool MMshowVASTAd( const char* url, int skipDelay, bool waterMark ) {
        if( iosDelegate == nil )
            iosDelegate = [[UnityIOSDelegate alloc] init];
        if(url == NULL)
            return false;
        else {
            bool result = [TremorVideoAd showVASTAd:UnityGetGLViewController() vastURL:[NSString stringWithUTF8String:url] skipDelay:skipDelay waterMark:waterMark];
            [TremorVideoAd setDelegate: iosDelegate]; 
            return result;
        }
    }

    void MMfireConversion( const char* advertizer_id,const char* conversion_id ){
        NSString *sAdvertizerID = [NSString stringWithUTF8String:advertizer_id];
        NSString *sConversionID = [NSString stringWithUTF8String:conversion_id];
        [TremorVideoAd fireConversion:sAdvertizerID : sConversionID];

    }
    
    
    
}
