using Lansky.SharpMutator.Log;
using System;
using System.Diagnostics;
using System.IO;

namespace Lansky.SharpMutator.Engine
{
    sealed class Test
    {
        private readonly ILogger _log;

        private static Guid CurrentRunId = Guid.NewGuid();

        public Test(ILogger log)
        {
            _log = log;
        }

        public enum TestResult
        {
            Passed,
            Failed
        }

        public TestResult TestOriginal(FileInfo dllWithTest, FileInfo dllWithSut)
            => RunTests(null, dllWithTest, dllWithSut);

        public TestResult TestMutant(Mutation mutation, FileInfo dllWithTest, FileInfo dllWithSut)
            => RunTests(mutation, dllWithTest, dllWithSut);

        private TestResult RunTests(Mutation mutation, FileInfo dllWithTest, FileInfo dllWithSut)
        {
            var runnerPath = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe";
            var experimentPath = $@"C:\tmp\mtts\{CurrentRunId}";
            Directory.CreateDirectory(experimentPath);
            File.Copy(dllWithTest.FullName, $"{experimentPath}/Lansky.SharpMutator.ExampleTest.dll");
            if (mutation != null)
            {
                mutation.Location.MoveTo($"{experimentPath}/Lansky.SharpMutator.ExampleSut.dll");
            }
            else
            {
                File.Copy(dllWithSut.FullName, $"{experimentPath}/Lansky.SharpMutator.ExampleSut.dll");
            }

            var psi = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = runnerPath,
                Arguments = "Lansky.SharpMutator.ExampleTest.dll",
                WorkingDirectory = experimentPath,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = true
            };

            var result = TestResult.Failed;
            using (var exeProcess = Process.Start(psi))
            {
                exeProcess.WaitForExit();
                _log.Info($"Exit code: {exeProcess.ExitCode}");

                if (exeProcess.ExitCode == 0)
                {
                    if (mutation == null)
                    {
                        _log.Info("Non-mutated version passes the test, that's OK & required.");
                        result = TestResult.Passed;
                    }
                    else
                    {
                        _log.Info("There is a mutation that is not captured by any test! That's bad!");
                        result = TestResult.Failed;
                    }
                }
                else
                {
                    if (mutation == null)
                    {
                        _log.Info("Even non-mutated version doesn't pass the tests, that's bad.");
                        result = TestResult.Failed;
                    }
                    else
                    {
                        _log.Info($"This mutation (type {mutation.Type}, line {mutation.SourceFullPath}:{mutation.StartLine}) triggered some test to respond.");
                        result = TestResult.Passed;
                    }
                }
            }

            Directory.Delete(experimentPath, true);

            return result;
        }
    }
}
