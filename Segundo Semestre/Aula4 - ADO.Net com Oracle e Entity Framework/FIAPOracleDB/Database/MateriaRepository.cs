using System.Data;
using FIAPOracleDB.Models;
using Oracle.ManagedDataAccess.Client;

namespace FIAPOracleDB.Database
{
    public class MateriaRepository
    {
        private readonly OracleConnection _con;
        public MateriaRepository(OracleConnection con) => _con = con;

        public List<Materia> GetAll()
        {
            var list = new List<Materia>();
            const string sql = "SELECT Id, Nome, Codigo FROM Materias ORDER BY Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                list.Add(new Materia
                {
                    Id = rd.GetInt32(0),
                    Nome = rd.GetString(1),
                    Codigo = rd.GetString(2)
                });
            }
            return list;
        }

        public Materia? GetById(int id)
        {
            const string sql = "SELECT Id, Nome, Codigo FROM Materias WHERE Id = :Id";
            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Id", OracleDbType.Int32, id, ParameterDirection.Input);

            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                return new Materia
                {
                    Id = rd.GetInt32(0),
                    Nome = rd.GetString(1),
                    Codigo = rd.GetString(2)
                };
            }
            return null;
        }

        public int Insert(Materia m)
        {
            const string sql =
                @"INSERT INTO Materias (Nome, Codigo)
                  VALUES (:Nome, :Codigo)
                  RETURNING Id INTO :Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Nome", OracleDbType.Varchar2, m.Nome, ParameterDirection.Input);
            cmd.Parameters.Add("Codigo", OracleDbType.Varchar2, m.Codigo, ParameterDirection.Input);

            var outId = new OracleParameter("Id", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);

            cmd.ExecuteNonQuery();
            m.Id = Convert.ToInt32(outId.Value.ToString());
            return m.Id;
        }

        public bool Update(Materia m)
        {
            const string sql =
                @"UPDATE Materias
                  SET Nome = :Nome, Codigo = :Codigo
                  WHERE Id = :Id";

            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Nome", OracleDbType.Varchar2, m.Nome, ParameterDirection.Input);
            cmd.Parameters.Add("Codigo", OracleDbType.Varchar2, m.Codigo, ParameterDirection.Input);
            cmd.Parameters.Add("Id", OracleDbType.Int32, m.Id, ParameterDirection.Input);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Materias WHERE Id = :Id";
            using var cmd = new OracleCommand(sql, _con) { BindByName = true };
            cmd.Parameters.Add("Id", OracleDbType.Int32, id, ParameterDirection.Input);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}
