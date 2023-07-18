namespace Game.UserState
{
    public interface IUserState
    {
        
    }

    public interface IUserStateHandler
    {
        void Save();
        void Load();
    }
}
