using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace FIAPOracleCRUD.Database
{ 
    public static class Connection
    {
        private const string DataSource = "oracle.fiap.com.br:1521/ORCL";

        public static OracleConnection Create(string user, string password)
        {
            var csb = new OracleConnectionStringBuilder
            {
                DataSource = DataSource,
                UserID = user,
                Password = password
            };

            // "User Id=PF2380;Password=SUASENHA;Data Source=oracle.fiap.com.br:1521/ORCL"

            return new OracleConnection(csb.ToString());
        }

        public static string ReadPasswordMasked(string prompt = "Senha: ")
        {
            Console.Write(prompt);
            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0) { sb.Length--; continue; }
                if (!char.IsControl(key.KeyChar)) sb.Append(key.KeyChar);
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}