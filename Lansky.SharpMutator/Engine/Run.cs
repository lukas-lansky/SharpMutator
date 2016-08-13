using Lansky.SharpMutator.Log;
using Mono.Cecil;
using System.IO;

namespace Lansky.SharpMutator.Engine
{
    /// <summary>
    /// Single run of mutation process.
    /// </summary>
    sealed class Run
    {
        private readonly MutationFactory _mutationFactory;
        private readonly Test _test;
        private readonly ILogger _log;

        public Run(MutationFactory mutationFactory, Test test, ILogger log)
        {
            _mutationFactory = mutationFactory;
            _test = test;
            _log = log;
        }

        public void Process(FileInfo sutDll, FileInfo testDll)
        {
            var loadedAssembly = AssemblyDefinition.ReadAssembly(sutDll.FullName, new ReaderParameters { ReadSymbols = true });

            _test.TestOriginal(testDll, sutDll);

            var mutations = _mutationFactory.Mutate(loadedAssembly);
            foreach (var mutation in mutations)
            {
                _test.TestMutant(mutation, testDll, sutDll);
            }

            _log.Info("Mutation is done.");
        }
    }
}
