using Game.UserState;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;

namespace Game.Config
{
    public class UserStateConfig : BaseSingleConfig<UserStateConfigData, UserStateConfig>
    {
        
    }

    [System.Serializable]
    public class UserStateConfigData : IConfigData
    {
        public string ID => nameof(UserStateConfigData);

        [ReadOnly]
        public List<string> userStateTypes = new List<string>();

        [Button]
        public void Refresh()
        {
#if UNITY_EDITOR
            System.Type configType = typeof(IUserStateHandler);
            Dictionary<Type, object> userStateRegistry = new Dictionary<Type, object>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes()
                    .Where(config => configType.IsAssignableFrom(config) && !config.IsGenericType && !config.IsAbstract && !config.IsInterface).ToList())
                {
                    userStateRegistry[type] = type;
                }
            }

            userStateTypes = userStateRegistry.Select(userState => JsonConvert.SerializeObject(userState.Key)).ToList();
#endif
        }
    }
}
