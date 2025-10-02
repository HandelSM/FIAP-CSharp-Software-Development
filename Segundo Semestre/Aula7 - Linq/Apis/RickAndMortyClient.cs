using System.Net.Http.Json;
using LinqRM.Models;

namespace LinqRM.Apis;

public class RickAndMortyClient
{
    private readonly HttpClient _http_client;

    public RickAndMortyClient(Uri baseApi)
    {
        _http_client = new HttpClient { BaseAddress = baseApi };
    }

    public async Task<List<T>> GetAllPagesAsync<T>(string relativePath, CancellationToken ct = default)
    {
        var all = new List<T>();
        string? next = relativePath;

        while (!string.IsNullOrWhiteSpace(next))
        {
            var page = await _http_client.GetFromJsonAsync<Page<T>>(next, ct);

            if (page == null)
                break;

            all.AddRange(page.results);

            next = page.info.next;
        }

        return all;
    }

    public Task<List<Character>> GetAllCharactersAsync(CancellationToken ct = default)
        => GetAllPagesAsync<Character>("character", ct);

    public Task<List<Episode>> GetAllEpisodesAsync(CancellationToken ct = default)
        => GetAllPagesAsync<Episode>("episode", ct);

    public Task<List<Location>> GetAllLocationsAsync(CancellationToken ct = default)
        => GetAllPagesAsync<Location>("location", ct);
}
