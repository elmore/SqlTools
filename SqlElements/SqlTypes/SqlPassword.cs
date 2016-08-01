namespace SqlElements.SqlTypes
{
    public class SqlPassword
    {
        private readonly string _password;

        public SqlPassword(string password)
        {
            _password = password;
        }

        public override string ToString()
        {
            return Encrypt(_password);
        }

        private string Encrypt(string password)
        {
            throw new System.NotImplementedException();
        }
    }
}