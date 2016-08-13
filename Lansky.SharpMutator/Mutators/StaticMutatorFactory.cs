using System.Collections.Generic;

namespace Lansky.SharpMutator.Mutators
{
    /// <summary>
    /// Ugly temporary solution before applying Typed Factory Facility.
    /// </summary>
    class StaticMutatorFactory : IMutatorFactory
    {
        public IList<IMutator> GetAllMutators()
            => new List<IMutator> { new BinaryOpMutator(), new InequalityMutator() };
    }
}
