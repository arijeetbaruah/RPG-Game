using System.IO;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Game.UserState
{
    public class BaseUserStateHandler<TUserState> : IUserStateHandler where TUserState : IUserState
    {
        public TUserState userState;
        private readonly string path = $"{Application.persistentDataPath}/{typeof(TUserState).Name}.sav";
        

        public void Load()
        {
            byte[] data = File.ReadAllBytes(path);
            string dataStr = Encoding.ASCII.GetString(data);

            userState = JsonConvert.DeserializeObject<TUserState>(dataStr);
        }

        public void Save()
        {
            string data = JsonConvert.SerializeObject(userState);

            File.WriteAllBytes(path, Encoding.ASCII.GetBytes(data));
        }
    }
}
