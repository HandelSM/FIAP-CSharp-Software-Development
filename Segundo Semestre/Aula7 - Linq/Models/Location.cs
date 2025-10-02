using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinqRM.Models;


public class Location
{
    public int id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string dimension { get; set; }
    public string[] residents { get; set; }
    public string url { get; set; }
    public DateTime created { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
