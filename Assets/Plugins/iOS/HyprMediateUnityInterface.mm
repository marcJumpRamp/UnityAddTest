#import "HYPRMediateInterpreter.h"

#ifdef __cplusplus
extern "C" {
    
    void _initializeHyprMediate(const char* apiToken, const char* userID, const char* unityInfo) {
        NSString *apiTokenString = [NSString stringWithUTF8String:apiToken];
        NSString *userIDString = [NSString stringWithUTF8String:userID];
        NSString *unityInfoString = [NSString stringWithUTF8String:unityInfo];
        [HYPRMediateInterpreter initializeHyprMediateWithAPIToken:apiTokenString userID:userIDString customInfo:unityInfoString];
    }
    
    void _checkInventory() {
        [HYPRMediateInterpreter checkInventory];
    }
    
    void _showAd() {
        [HYPRMediateInterpreter showAd];
    }
    
    void _setUserID(const char* userID) {
        NSString *userIDString = [NSString stringWithUTF8String:userID];
        [HYPRMediateInterpreter setUserID:userIDString];
    }
    
    void _setLogLevel(int logLevel) {
        [HYPRMediateInterpreter setLogLevel:(NSUInteger)logLevel];
    }
    
    int _logLevel() {
        return (int)[HYPRMediateInterpreter logLevel];
    }
    
    int _hyprMediateSDKVersionNumber() {
        return (int)[HYPRMediateInterpreter hyprMediateSDKVersionNumber];
    }
}
#endif