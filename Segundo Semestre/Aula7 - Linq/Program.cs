using LinqRM.Apis;

var baseApi = new Uri("https://rickandmortyapi.com/api/");
var client = new RickAndMortyClient(baseApi);

var characters = await client.GetAllCharactersAsync();
var episodes = await client.GetAllEpisodesAsync();
var locations = await client.GetAllLocationsAsync();

// -----------------------------------------------------------------------------
// ÍNDICES ÚTEIS PARA JOINS (prepare antes dos exercícios)
// -----------------------------------------------------------------------------
var charByUrl = characters.ToDictionary(c => c.url);
var epByUrl = episodes.ToDictionary(e => e.url);
var locById = locations.ToDictionary(l => l.id);
var locByUrl = locations.ToDictionary(l => l.url);

Console.WriteLine("hello world");

// -----------------------------------------------------------------------------
// PROJEÇÃO E TRANSFORMAÇÃO
// -----------------------------------------------------------------------------

// 1) (Select)
//    Crie uma lista de objetos anônimos com { Id, Name, Species } de todos os personagens e
//    exiba os 5 primeiros em ordem alfabética por Name.

// 2) (Select com índice, .Select((c, i) => ... )) 
//    A lista characters, tem o seu Id baseado no Id da API, mas e se quisermos o índice de acordo com a ordem alfabética?
//    Refaça a questão 1, mas agora ao invés de Id, vamos usar nosso próprio Id.

// 3) (SelectMany)
//    Liste todas as URLs de episódios (sem repetição) a partir de characters (só vale usar a lista characters!),
//    e conte quantas URLs distintas existem.

// 4) (Cast)

// 5) (OfType)

// 6) (Append / Prepend)

// 7) (Chunk)

// -----------------------------------------------------------------------------
// FILTRAGEM E PARTICIONAMENTO
// -----------------------------------------------------------------------------

// 8) (Where)

// 9) (Skip / Take)

// 10) (SkipWhile)

// 11) (TakeWhile)

// 12) (SkipLast / TakeLast)

// 13) (DefaultIfEmpty)

// -----------------------------------------------------------------------------
// ORDENAÇÃO
// -----------------------------------------------------------------------------

// 14) (OrderBy / OrderByDescending)

// 15) (ThenBy / ThenByDescending)

// 16) (Reverse)

// -----------------------------------------------------------------------------
// CONJUNTOS (SET OPERATIONS)
// -----------------------------------------------------------------------------

// 17) (Distinct)

// 18) (DistinctBy)

// 19) (Union)

// 20) (UnionBy)

// 21) (Intersect)

// 22) (IntersectBy)

// 23) (Except)

// 24) (ExceptBy)

// -----------------------------------------------------------------------------
// JUNÇÕES E AGRUPAMENTO
// -----------------------------------------------------------------------------

// 25) (Join)

// 26) (GroupJoin)

// 27) (GroupBy)

// 28) (ToLookup)

// -----------------------------------------------------------------------------
// AGREGAÇÃO E ESTATÍSTICA
// -----------------------------------------------------------------------------

// 29) (Aggregate)

// 30) (Sum / Average / Min / Max)

// 31) (MinBy / MaxBy)

// 32) (Count / LongCount)

// -----------------------------------------------------------------------------
// QUANTIFICADORES E COMPARAÇÃO
// -----------------------------------------------------------------------------

// 33) (Any / All)

// 34) (Contains)

// 35) (SequenceEqual)

// -----------------------------------------------------------------------------
// ELEMENTOS (ACESSO PONTUAL)
// -----------------------------------------------------------------------------

// 36) (First / FirstOrDefault)

// 37) (Last / LastOrDefault)

// 38) (Single / SingleOrDefault)

// 39) (ElementAt / ElementAtOrDefault)

// -----------------------------------------------------------------------------
// CRIAÇÃO/GERAÇÃO E CONVERSÕES
// -----------------------------------------------------------------------------

// 40) (Enumerable.Empty<T>)

// 41) (Range / Repeat)

// 42) (AsEnumerable)

// 43) (ToArray / ToList / ToDictionary / ToHashSet)

// 44) (Zip)

// 45) (TryGetNonEnumeratedCount)

// -----------------------------------------------------------------------------

Console.WriteLine("bye");
