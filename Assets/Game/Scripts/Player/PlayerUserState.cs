using Game.UserState;

namespace Game
{
    public class PlayerUserStateHandler : BaseUserStateHandler<PlayerUserState>
    {

    }

    public class PlayerUserState : IUserState
    {
        public string playerName;
    }
}
