namespace ProjSaltHash
{
    public class SaltHash
    {
        public SaltHash(string salt, string hash)
        {
            this.Salt = salt;
            this.Hash = hash;
        }

        public string Salt { get; private set; }
        public string Hash { get; private set; }
    }
}
