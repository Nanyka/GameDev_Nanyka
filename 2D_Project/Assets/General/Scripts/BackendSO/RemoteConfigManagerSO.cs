using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "RemoteConfigManager", menuName = "TheAiAlchemist/BackEnd/RemoteConfigManager")]
    public class RemoteConfigManagerSO : ScriptableObject
    {
        private Dictionary<string, int> numericConfig = new();
        // private Dictionary<string, string> stringConfig = new();
        // private Dictionary<string, string> jsonConfig = new();

        public async Task FetchRemoteConfigs()
        {
            try
            {
                await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
                FetchNumericConfig();
                // FetchStringConfig();
                // FetchJsonConfig();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        // Remote Config's FetchConfigs call requires passing two non-nullable objects to the method, regardless of
        // whether any data needs to be passed in them. Candidates for what you may want to pass in the UserAttributes
        // struct could be things like device type, however it is completely customizable.
        public struct UserAttributes
        {
        }

        // Candidates for what you can pass in the AppAttributes struct could be things like what level the player
        // is on, or what version of the app is installed. The candidates are completely customizable.
        public struct AppAttributes
        {
        }

        private void FetchNumericConfig()
        {
            // GetNumericConfig(NumericConfigName.GAME_DURATION.ToString());

            foreach (var configKey in Enum.GetNames(typeof(NumericConfigName)))
            {
                var numericValue = RemoteConfigService.Instance.appConfig.GetInt(configKey);
                numericConfig[configKey] = numericValue;
                // Debug.Log($"Get {configKey}: {numericValue}");
            }
        }

        // Connect with other scripts
        public int GetNumericConfig(NumericConfigName key)
        {
            return numericConfig[key.ToString()];
        }

        // private void FetchStringConfig()
        // {
        //     foreach (var configKey in Enum.GetNames(typeof(StringConfigName)))
        //     {
        //         var stringValue = RemoteConfigService.Instance.appConfig.GetString(configKey);
        //         stringConfig[configKey] = stringValue;
        //         // Debug.Log($"Get {configKey}: {stringValue}");
        //     }
        // }
        //
        // // Connect with other scripts
        // public string GetStringConfig(StringConfigName key)
        // {
        //     return stringConfig[key.ToString()];
        // }
        //
        // private void FetchJsonConfig()
        // {
        //     foreach (var configKey in Enum.GetNames(typeof(JsonConfigName)))
        //     {
        //         var stringValue = RemoteConfigService.Instance.appConfig.GetJson(configKey);
        //         jsonConfig[configKey] = stringValue;
        //         // Debug.Log($"Get {configKey}: {stringValue}");
        //     }
        // }
        //
        // // Connect with other scripts
        // public string GetJsonConfig(JsonConfigName key)
        // {
        //     return jsonConfig[key.ToString()];
        // }
    }

    public enum NumericConfigName
    {
        COFFEE_ASK,
    }

    public enum StringConfigName
    {
        GET_CONTENT_BY_TAG,
    }

    public enum JsonConfigName
    {
        DEFAULT_FEATURES
    }
}