using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Lansky.SharpMutator.Mutators
{
    /// <summary>
    /// BinaryOpMutator replaces + for -, - for +,
    /// * for / and / for *.
    /// </summary>
    public class BinaryOpMutator : IOpCodeReplacingMutator
    {
        public ISet<OpCode> CodesToApply => new HashSet<OpCode> { OpCodes.Add, OpCodes.Sub, OpCodes.Mul, OpCodes.Div };

        public bool Mutate(Instruction instr)
        {
            if (!CodesToApply.Contains(instr.OpCode))
            {
                return false;
            }

            if (instr.OpCode == OpCodes.Add)
            {
                instr.OpCode = OpCodes.Sub;
            }
            else if (instr.OpCode == OpCodes.Sub)
            {
                instr.OpCode = OpCodes.Add;
            }
            else if (instr.OpCode == OpCodes.Mul)
            {
                instr.OpCode = OpCodes.Div;
            }
            else if (instr.OpCode == OpCodes.Div)
            {
                instr.OpCode = OpCodes.Mul;
            }

            return true;
        }
    }
}
