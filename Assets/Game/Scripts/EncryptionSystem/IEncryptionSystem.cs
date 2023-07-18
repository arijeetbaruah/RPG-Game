namespace Game.Encryption
{
    public interface IEncryptionSystem
    {
        byte[] Encrypt(string data);
        string Decrypt(byte[] data);
    }
}
