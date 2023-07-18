using Game.Config;
using Game.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;

namespace Game.UserState
{
    public class UserStateService : IService
    {
        public Dictionary<Type, IUserStateHandler> userStateRegistry = new Dictionary<Type, IUserStateHandler>();

        public UserStateConfig config;

        public void Initialize()
        {
            config = ServiceRegistry.Instance.Get<ConfigService>().Get<UserStateConfig>();

            userStateRegistry = config.Data.userStateTypes.Select(type => JsonConvert.DeserializeObject<Type>(type)).ToDictionary(type => type, type => Activator.CreateInstance(type) as IUserStateHandler);
        }

        public void Release()
        {
            
        }

        public void Update()
        {
            
        }

        public TUserStateHandler Get<TUserStateHandler>() where TUserStateHandler : class, IUserStateHandler
        {
            if (userStateRegistry.TryGetValue(typeof(TUserStateHandler), out var handler))
            {
                return handler as TUserStateHandler;
            }

            return null;
        }
    }
}
