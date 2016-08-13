using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Lansky.SharpMutator.Mutators
{
    /// <summary>
    /// Inequality mutator replaces "greater" for "lesser" and vice versa.
    /// </summary>
    public class InequalityMutator : IMutator
    {
        public ISet<OpCode> CodesToApply => new HashSet<OpCode> { OpCodes.Cgt, OpCodes.Clt };

        public bool Mutate(Instruction instr)
        {
            if (!CodesToApply.Contains(instr.OpCode))
            {
                return false;
            }

            if (instr.OpCode == OpCodes.Cgt)
            {
                instr.OpCode = OpCodes.Clt;
            }
            else if (instr.OpCode == OpCodes.Clt)
            {
                instr.OpCode = OpCodes.Cgt;
            }

            return true;
        }
    }
}
