
using System.Text.Json;

namespace LinqRM.Models;

public class Page<T>
{
    public Info info { get; set; }
    public List<T> results { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}

public class Info
{
    public int count { get; set; }
    public int pages { get; set; }
    public string? next { get; set; }
    public string? prev { get; set; }
}
