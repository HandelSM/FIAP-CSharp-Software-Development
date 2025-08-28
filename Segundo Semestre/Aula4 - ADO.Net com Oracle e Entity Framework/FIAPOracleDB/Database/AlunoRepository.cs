using System.Data;
using Oracle.ManagedDataAccess.Client;
using FIAPOracleDB.Models;

namespace FIAPOracleDB.Database
{
    public class AlunoRepository
    {
        private readonly OracleConnection _con;
        public AlunoRepository(OracleConnection con) => _con = con;

        public List<Aluno> GetAll()
        {
            var list = new List<Aluno>();
            const string sql = "SELECT Id, Nome, Matricula FROM Alunos ORDER BY Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                list.Add(new Aluno
                {
                    Id = rd.GetInt32(0),
                    Nome = rd.GetString(1),
                    Matricula = rd.GetString(2)
                });
            }
            return list;
        }

        public Aluno? GetById(int id)
        {
            const string sql =
                "SELECT Id, Nome, Matricula FROM Alunos WHERE Id = :Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add(new OracleParameter("Id", OracleDbType.Int32) { Value = id });

            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                return new Aluno
                {
                    Id = rd.GetInt32(0),
                    Nome = rd.GetString(1),
                    Matricula = rd.GetString(2)
                };
            }
            return null;
        }

        public int Insert(Aluno a)
        {
            const string sql =
                @"INSERT INTO Alunos (Nome, Matricula)
                  VALUES (:Nome, :Matricula)
                  RETURNING Id INTO :Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Nome", OracleDbType.Varchar2, a.Nome, ParameterDirection.Input);
            cmd.Parameters.Add("Matricula", OracleDbType.Varchar2, a.Matricula, ParameterDirection.Input);

            var outId = new OracleParameter("Id", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);

            cmd.ExecuteNonQuery();
            a.Id = Convert.ToInt32(outId.Value.ToString());
            return a.Id;
        }

        public bool Update(Aluno a)
        {
            const string sql =
                @"UPDATE Alunos
                  SET Nome = :Nome, Matricula = :Matricula
                  WHERE Id = :Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Nome", OracleDbType.Varchar2, a.Nome, ParameterDirection.Input);
            cmd.Parameters.Add("Matricula", OracleDbType.Varchar2, a.Matricula, ParameterDirection.Input);
            cmd.Parameters.Add("Id", OracleDbType.Int32, a.Id, ParameterDirection.Input);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Alunos WHERE Id = :Id";
            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Id", OracleDbType.Int32, id, ParameterDirection.Input);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public List<Aluno> SearchByName(string term)
        {
            // Mantido como não implementado, exatamente como no original.
            throw new NotImplementedException();
        }
    }
}
