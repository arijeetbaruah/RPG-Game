using Game.Config;
using Game.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.HUDSystem
{
    public class HUDService : IService
    {
        public HUDConfig hUDConfig;

        private GameObject HUDParent;
        private Dictionary<Type, IHUD> HUDregistry = new Dictionary<Type, IHUD>();

        public void Initialize()
        {
            hUDConfig = ServiceRegistry.Instance.Get<ConfigService>().Get<HUDConfig>();
            HUDParent = new GameObject();
            HUDParent.name = "HUD";
        }

        public void Update()
        {
            
        }

        public void Release()
        {
            
        }

        public async Task<THUD> GetHUD<THUD>() where THUD : class, IHUD
        {
            IHUD hud;
            if (HUDregistry.TryGetValue(typeof(THUD), out hud))
            {
                return hud as THUD;
            }

            if (hUDConfig.Data.TryGetValue(nameof(THUD), out var dataConfig))
            {
                GameObject hudGO = await dataConfig.assetReference.InstantiateAsync(HUDParent.transform).Task;
                return hudGO.GetComponent<THUD>();
            }

            return null;
        }
    }
}
