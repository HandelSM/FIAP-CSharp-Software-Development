using LinqRM.Models;
using System.Net.Http.Json;
using System.Text.Json;

using (var client = new HttpClient())
{
    var characters = new List<Character>();

    var currentPage = await client.GetFromJsonAsync<CharacterPage>("https://rickandmortyapi.com/api/character");

    characters.AddRange(currentPage.results);

    while(currentPage.info.next != null)
    {
        currentPage = await client.GetFromJsonAsync<CharacterPage>(currentPage.info.next);
        characters.AddRange(currentPage.results);
    }

    //var filteredChars = characters.Where( c => c.status == "Alive")
    //                              .Where(c => c.species == "Human")
    //                              .OrderByDescending(c => c.episode.Length)
    //                              .ThenBy(c => c.name)
    //                              .ToList();


    var filteredChars = characters.Where(c => c.status == "Alive")
                                  //.Where(c => c.species == "Human")
                                  //.Where(c => c.episode.Length > 2)
                                  .GroupBy(c => c.species);

    foreach (var group in filteredChars)
    {
        Console.WriteLine(group.Key);
    }

    Console.WriteLine(filteredChars.Count());

}
