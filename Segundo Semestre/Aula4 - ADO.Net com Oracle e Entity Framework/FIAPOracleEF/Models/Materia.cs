using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAPOracleEF.Models;

[Table("MATERIAS")]
public class Materia
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Codigo { get; set; } = "";
}
