using System.Collections.Generic;
using Mono.Cecil.Cil;
using System;
using Mono.Cecil;

namespace Lansky.SharpMutator.Mutators
{
    /// <summary>
    /// Mutator replacing any op with 
    /// </summary>
    public class ExceptionMutator : IComplexMutator
    {
        /// <summary>
        /// Every opcode.
        /// </summary>
        public ISet<OpCode> CodesToApply => new HashSet<OpCode> { };

        /// <summary>
        /// Inserts throw before the instruction.
        /// </summary>
        public bool Mutate(Instruction instr, MethodBody methodBody, AssemblyDefinition assembly)
        {
            var processor = methodBody.GetILProcessor();
            
            var exceptionConstructor = typeof(Exception).GetConstructor(new Type[] { });
            var exceptionReference = assembly.MainModule.Import(exceptionConstructor);

            var instructions = new Instruction[] {
                processor.Create(OpCodes.Ldstr, "random string"),
                processor.Create(OpCodes.Newobj, exceptionReference),
                processor.Create(OpCodes.Throw) };

            foreach (var instrToInsert in instructions)
            {
                processor.InsertBefore(instr, instrToInsert);
            }
            
            return true;
        }
    }
}