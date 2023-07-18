using Game.UserState;
using System.Collections.Generic;

namespace Game
{
    public class PlayerWalletUserStateHandler : BaseUserStateHandler<PlayerWalletUserState>
    {
        
    }

    public class PlayerWalletUserState : IUserState
    {
        public Dictionary<string, float> wallet;
    }
}
