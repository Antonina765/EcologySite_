//using MultyTreadConsole;

//var counter = new Counter();

//var taskForIvan = new Task(() => counter.Count("Ivan"));
//var taskForLera= new Task(() => counter.Count("********* Lera"));

//taskForIvan.Start();
//taskForLera.Start();

//Console.ReadLine();
//Console.WriteLine("End of the console app");

using MultyTreadConsole;

var start = DateTime.Now;
var api = new ApiService();
var tasks = new List<Task>();
for (int i = 0; i < 10; i++)
{
    var task = api.GetDataFromAsync(i);//.Wait();
    tasks.Add(task);
}

Task.WaitAll(tasks.ToArray());

var end = DateTime.Now;

Console.WriteLine((end - start).TotalMilliseconds);