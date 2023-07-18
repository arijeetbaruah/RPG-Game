using Game.Config;
using Game.Service;
using Game.UserState;
using UnityEngine;

namespace Game
{
    public class MainEntry : MonoBehaviour
    {
        private void Start()
        {
            ServiceRegistry.Instance.AddService(new ConfigService());
            ServiceRegistry.Instance.AddService(new UserStateService());
        }
    }
}
