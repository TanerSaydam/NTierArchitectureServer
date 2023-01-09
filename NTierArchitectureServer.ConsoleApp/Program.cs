using BenchmarkDotNet.Running;

namespace NTierArchitectureServer.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Benchmark - Performans Kontrolleri Sağlayacağım


            BenchmarkRunner.Run<BenchmarkService>();


        }
    }
}