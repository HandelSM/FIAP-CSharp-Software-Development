
using Amazon.S3;
using Amazon.S3.Model;
using FIAPApi.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// Adicionando txt no S3 exemplo: (SDK)

var bucket = "nomeDoBucket";
var key = "nomeDoArquivoNoS3";
var content = "Conteudo do Arquivo";

using var s3 = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);

var putRequest = new PutObjectRequest()
{
    BucketName = bucket,
    Key = key,
    ContentBody = content,
    ContentType = "text/plain"
};

var putResponse = await s3.PutObjectAsync(putRequest);

Console.WriteLine($"Upload feito! {putResponse.ETag}");

// Requests para nossa Web API Local (HttpClient)

//using (var client = new HttpClient() )
//{
// client.BaseAddress = new Uri("https://localhost:7146/");

//var results = await client.GetFromJsonAsync<IEnumerable<AlunoDTO>>("https://localhost:7146/api/FIAP");

//foreach(var result in results)
//{
//    Console.WriteLine(result);
//}

//var novoAluno = new AlunoDTO()
//{
//    Nome = "Jefferson2",
//    Matricula = "1231234"
//};

//Console.WriteLine("############################");
//Console.WriteLine("############################");
//Console.WriteLine("############################");
//Console.WriteLine("############################");

//var novoAlunoJson =  JsonSerializer.Serialize(novoAluno);

//var message = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7146/api/FIAP");

//Console.WriteLine(novoAlunoJson);

//message.Content = new StringContent(novoAlunoJson, Encoding.UTF8, "application/json");

//var postResult = await client.SendAsync(message);

//Console.WriteLine(postResult.StatusCode);

//Console.WriteLine("############################");
//Console.WriteLine("############################");
//Console.WriteLine("############################");
//Console.WriteLine("############################");

//var results2 = await client.GetFromJsonAsync<IEnumerable<AlunoDTO>>("https://localhost:7146/api/FIAP");

//foreach (var result in results2)
//{
//    Console.WriteLine(result);
//}

// Requests para nossa API externa (HttpClient)

//var result = await client.GetStringAsync("https://restcountries.com/v3.1/name/brazil");

//Console.WriteLine( result );
//}
