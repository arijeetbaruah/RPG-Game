using Game.Config;
using Game.Service;
using Game.UserState;

namespace Game
{
    public class WalletService : IService
    {
        private CurrencyConfig currencyConfig;
        private PlayerWalletUserStateHandler wallet;

        public void Initialize()
        {
            currencyConfig = ServiceRegistry.Instance.Get<ConfigService>().Get<CurrencyConfig>();
            ServiceRegistry.Instance.Get<UserStateService>().Get<PlayerWalletUserStateHandler>();
        }

        public void Release()
        {
            
        }

        public void Update()
        {
           
        }

        public void Add(string currencyID, float amount)
        {

        }
    }
}
