using Lansky.SharpMutator.Log;
using Lansky.SharpMutator.Mutators;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace Lansky.SharpMutator.Engine
{
    class MutationFactory
    {
        private readonly IMutatorFactory _mutatorFactory;
        private readonly ILogger _log;

        public MutationFactory(IMutatorFactory mutatorFactory, ILogger log)
        {
            _mutatorFactory = mutatorFactory;
            _log = log;
        }
        
        public IEnumerable<Mutation> Mutate(AssemblyDefinition loadedAssembly)
        {
            var mutators = _mutatorFactory.GetAllMutators();
            var mutationId = 0;

            var types = loadedAssembly.MainModule.Types;
            foreach (var type in types)
            {
                if (type.BaseType == null)
                {
                    continue;
                }

                _log.Info(type.FullName);

                foreach (var method in type.Methods)
                {
                    _log.Info($"    {method.FullName}");

                    foreach (var mutation in Mutate(method, loadedAssembly, mutators, mutationId))
                    {
                        yield return mutation;
                        mutationId++;
                    }
                }
            }
        }

        public IEnumerable<Mutation> Mutate(MethodDefinition method, AssemblyDefinition loadedAssembly, IList<IMutator> mutators, int mutationId)
        {
            var currentLine = -1;
            var currentFile = "";
            foreach (var instr in method.Body.Instructions)
            {
                _log.Info($"        {instr.SequencePoint?.StartLine ?? 0:0000} {instr}");
                if (instr.SequencePoint != null)
                {
                    currentLine = instr.SequencePoint.StartLine;
                    currentFile = instr.SequencePoint.Document.Url.Split('\\').LastOrDefault() ?? "";
                }

                foreach (var mutator in mutators.Where(m => m.CodesToApply.Contains(instr.OpCode)))
                {
                    var originalInstrOpCode = instr.OpCode;

                    _log.Info($"            Mutating using {mutator.GetType().Name}");
                    var success = mutator.Mutate(instr);

                    if (!success)
                    {
                        continue;
                    }

                    var filePath = System.IO.Path.GetFullPath($"mutant-{mutationId++}.dll");
                    
                    _log.Info($"            Saving as mutant {mutationId}");
                    loadedAssembly.Write(filePath);
                    
                    _log.Info($"            Restoring to original version");
                    instr.OpCode = originalInstrOpCode;

                    yield return new Mutation(method, new System.IO.FileInfo(filePath), mutator);
                }
            }
        }
    }
}
