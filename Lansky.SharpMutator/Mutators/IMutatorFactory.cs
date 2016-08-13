using System.Collections.Generic;

namespace Lansky.SharpMutator.Mutators
{
    /// <summary>
    /// Service for discovery mutators available in the system.
    /// </summary>
    public interface IMutatorFactory
    {
        IList<IMutator> GetAllMutators();
    }
}
