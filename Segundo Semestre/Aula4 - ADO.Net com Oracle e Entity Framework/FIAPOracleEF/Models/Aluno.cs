using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIAPOracleEF.Models;


[Table("ALUNOS")]
public class Aluno
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }
    [Column("NOME")]
    public string Nome { get; set; } = "";
    [Column("MATRICULA")]
    public string Matricula { get; set; } = "";
}
