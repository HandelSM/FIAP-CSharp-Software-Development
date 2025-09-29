namespace FIAPApi.Models;

public class AlunoDTO
{
    public string Nome { get; set; }
    public string Matricula { get; set; }

    public override string ToString()
    {
        return $"Nome: {Nome}, Matricula: {Matricula}";
    }
}
