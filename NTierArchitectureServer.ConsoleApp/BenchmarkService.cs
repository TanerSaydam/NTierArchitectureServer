using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.ConsoleApp
{
    [ShortRunJob,Config(typeof(Config))]
    public class BenchmarkService
    {
        private class Config :ManualConfig
        {
            public Config()
            {
                SummaryStyle = BenchmarkDotNet.Reports.SummaryStyle.Default.WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Trend);
            }
        }

        [Benchmark(Baseline =true)]
        public void Method1() => Thread.Sleep(10);

        [Benchmark]
        public void Method2() => Thread.Sleep(20);
    }
}
