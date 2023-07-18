namespace Game.Config
{
    public interface IConfig
    {
        string ID { get; }

        void Initialize();
    }

    public interface IConfigData
    {
        string ID { get; }
    }
}
