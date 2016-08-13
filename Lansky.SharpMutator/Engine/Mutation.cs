using Lansky.SharpMutator.Mutators;
using Mono.Cecil;
using System.IO;

namespace Lansky.SharpMutator.Engine
{
    class Mutation
    {
        /// <summary>
        /// Full name of the mutated method.
        /// </summary>
        public string FullMethodName;

        /// <summary>
        /// Full path to the source file obtained from PDBs.
        /// </summary>
        public string SourceFullPath;
        
        /// <summary>
        /// Start line of the mutated code obtained from PDBs.
        /// </summary>
        public int StartLine;

        /// <summary>
        /// Where is the compiled IL located at file system?
        /// </summary>
        public FileInfo Location;

        /// <summary>
        /// Type of the mutator that produced this mutation.
        /// </summary>
        public string Type;

        public Mutation(MethodDefinition method, FileInfo location, IMutator mutator)
        {
            FullMethodName = method.FullName;
            Location = location;
            Type = mutator.GetType().Name;
        }
    }
}
