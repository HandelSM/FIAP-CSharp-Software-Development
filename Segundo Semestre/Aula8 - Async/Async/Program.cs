int N = 10_000_000;

int sum = 0;

//for (int i = 0; i<N; i++)
//{
//    sum++;
//}

//Console.WriteLine(sum);

//Parallel.For(0, N, i =>
//{
//    sum += 1;
//});

//var gate = new Object();

//Parallel.For(0, N, i =>
//{
//    lock (gate)
//    {
//        sum += 1;
//    }
//});

Parallel.For(0, N, i =>
{
    //Interlocked.Add(ref sum, 1);
    Interlocked.Increment(ref sum);
});

Console.WriteLine(sum);

