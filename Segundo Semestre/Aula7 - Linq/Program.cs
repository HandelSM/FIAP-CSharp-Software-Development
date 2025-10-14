using LinqRM.Apis;

var baseApi = new Uri("https://rickandmortyapi.com/api/");
var client = new RickAndMortyClient(baseApi);

var charactersTask = client.GetAllCharactersAsync();
var episodesTask = client.GetAllEpisodesAsync();
var locationsTask = client.GetAllLocationsAsync();

var characters = await charactersTask;
var episodes = await episodesTask;
var locations = await locationsTask;

var charByUrl = characters.ToDictionary(c => c.url);
var epByUrl = episodes.ToDictionary(e => e.url);
var locById = locations.ToDictionary(l => l.id);
var locByUrl = locations.ToDictionary(l => l.url);

// -----------------------------------------------------------------------------

// 01) (Select/OrderBy/Take)
//    Crie uma lista de objetos anônimos com { Id, Name, Species } de todos os personagens e
//    exiba os 5 primeiros em ordem alfabética por Name.

// 02) (Select com índice, .Select((c, i) => ... ))
//    A lista characters, tem o seu Id baseado no Id da API, mas e se quisermos o índice de acordo com a ordem alfabética?
//    Refaça a questão 1, mas agora ao invés de Id, vamos usar nosso próprio Id.

// 03) (Where/char.IsDigit)
//    Na questão 1 e 2, ordenando de forma alfabética primeiro aparecem os personagens cujos nomes começam com números (e.g. "26 Years Old Morty").
//    refaça a questão 1, mas agora filtrando os personagens que começam com números.

// 04) Agora refaça a questão 2, fazendo o filtro dos personagens que começam com números.

// 05) (Distinct)
//    Liste todas as espécies (Species) de personagens, sem repetições, em ordem alfabética.

// 06) (Cast/OfType): Dado um List<object> { "rick", 42, "morty", 1.5 }, filtre apenas strings
//    e junte-as numa única string separada por vírgula.

var q6 = new List<object> { "rick", 70, "morty", 14 };

// 07) (Append / Prepend): Pegue os 3 primeiros nomes de personagens e:
//    - Prepend("BEGIN")
//    - Append("END")
//    Imprima a sequência resultante.

// 08) Liste todos os episódios.

// 09) (Chunk)
//     Divida a lista de episódios em chunks de 10. Para cada chunk,
//     imprima "Lote X: {primeiroCodigo}..{ultimoCodigo}".

// 10) Liste personagens "Alive" E espécie "Human". Ordene por Name e mostre 10.

// 11) (SkipWhile)
//     Ordene episódios por Code e pule enquanto o Code começar com "S02".
//     Ou seja, pule todos da segunda temporada.

// 12) (TakeWhile/Count)
//     Pegue episódios enquanto o Code começar com "S02". Conte-os.

// 13) (SkipLast/TakeLast)
//     Ordene personagens por número de episódios decrescente.
//     Pegue os 5 últimos (TakeLast) e depois pule o último (SkipLast(1)) dessa seleção.

// 14) (DistinctBy)
//     Use DistinctBy para criar uma lista de personagens únicos por Species, ou seja, um representante de cada espécie.

// 15) (Union)
//     Liste todas as personagens que tem "Rick" no nome, depois liste todos as personagens que tem "Morty" no nome.
//     Una as listas usando Union.

// 16) (UnionBy)
//     Já está listado os 20 personagens que mais aparecem em episódios.
//     Também já está listado todos os personagens vivos e que são aliens.
//     Use UnionBy para adicionar em top20 os alives alines.

var top20 = characters.OrderByDescending(c => c.episode.Count).Take(20);
var aliveAliens = characters.Where(c => c.status == "Alive" && c.species == "Alien");

// 17) (Intersect)
//     Agora veja quem são os que estão em top20 e também são alives aliens.

// 18) (IntersectBy)
//     A 17 também pode ser feita usando IntersectBy. Tente fazer.

// 19) (Except)
//     Veja quem são os personagens que estão em top20 mas não estão vivos.

// 20) (ExceptBy)
//     Também temos o ExceptBy. Union, Intersect e Except compara se os objetos são iguais em todos os campos, aqueles com By você
//     pode escolher um campo para comparar.

// 21) Dado o código de um episódio (tente vários), liste os nomes dos personagens que aparecem nele. (faça com select)

var q21EpisodeCode = "S01E01";
var q21Episode = episodes.First(e => e.episode == q21EpisodeCode);

// 22) (Join)
//     Tente fazer a questão 21 usando Join.

// 23) (GroupBy)
//     Agrupe os personagens por espécie (Species). Para cada espécie, mostre a contagem de personagens.

// -----------------------------------------------------------------------------
// Funções da static class Enumerable
// -----------------------------------------------------------------------------

// 24) (Enumerable.Empty)
//     Crie uma lista vazia de inteiros.

// 25) (Enumerable.Range)
//     Crie uma lista de inteiros de 1 a 20.

// 26) Crie uma lista dos quadrados dos números de 1 a 20.

// 27) (Enumerable.Repeat)
//     Crie uma lista com 10 vezes o valor "Rick".

// -----------------------------------------------------------------------------
// -----------------------------------------------------------------------------

// 28) (Zip)
//     Crie uma lista da soma das listas dos exercicios 25 e 26.

// 29) (SequenceEqual)
//     Cheque se quadrados1 e quadrados2 são iguais.
var quadrados1 = Enumerable.Range(1, 20).Select(n => n * n);
var quadrados2 = Enumerable.Range(1, 20).Zip(Enumerable.Range(1, 20), (x, y) => x * y);

// 30)
// Chegamos ao fim, vimos várias funções de Linq, mas ainda temos muitas outras. Explore elas, aqui vão algumas que faltaram, procurem saber sobre:
// Concat, Reverse, GroupJoin, ToLookup, Aggregate, Sum, Average, Min, Max, MinBy, MaxBy, Count, LongCount, Any, All, Contains,
// DefaultIfEmpty, First/FirstOrDefault, Last/LastOrDefault, Single/SingleOrDefault, ElementAt/ElementAtOrDefault, AsEnumerable
// ToArray, ToList, ToDictionary, ToHashSet...
// Como última questão use alguma nova que não foi usada nas outras questões para obter uma informação interessante sobre o universo de Rick & Morty.
