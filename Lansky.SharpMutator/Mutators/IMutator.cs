using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Lansky.SharpMutator.Mutators
{
    public interface IComplexMutator : IMutator
    {
        bool Mutate(Instruction instr, MethodBody methodBody, AssemblyDefinition assembly);
    }

    public interface IOpCodeReplacingMutator : IMutator
    {
        bool Mutate(Instruction instr);
    }

    /// <summary>
    /// Every implementation is a different way IL can
    /// be mutated. Every such mutator has a set of OpCodes
    /// it's supposed to act upon so the engine can decide
    /// quickly whether to run potentially expensive Mutate
    /// method that contains the core mutation logic. Even
    /// when the method is called, mutator can decide to not
    /// do anything by not changing it and returning false.
    /// </summary>
    public interface IMutator
    {
        /// <summary>
        /// List of instruction mutator can act upon.
        /// </summary>
        ISet<OpCode> CodesToApply { get; }
    }
}
