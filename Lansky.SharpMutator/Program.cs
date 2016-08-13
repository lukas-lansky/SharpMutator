using Lansky.SharpMutator.Mutators;
using System;
using System.IO;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Lansky.SharpMutator.Log;
using Lansky.SharpMutator.Engine;

namespace Lansky.SharpMutator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sutPath = @"..\..\..\Lansky.SharpMutator.ExampleSut\bin\Debug\Lansky.SharpMutator.ExampleSut.dll";
            var testPath = @"..\..\..\Lansky.SharpMutator.ExampleTest\bin\Debug\Lansky.SharpMutator.ExampleTest.dll";

            var container = new WindsorContainer();
            container.Register(
                
                Component.For<ILogger>().ImplementedBy<ConsoleLogger>().LifestyleSingleton(),

                Component.For<IMutatorFactory>().ImplementedBy<StaticMutatorFactory>().LifestyleSingleton(),
                
                Component.For<MutationFactory>().LifestyleSingleton(),
                Component.For<Run>().LifestyleSingleton(),
                Component.For<Test>().LifestyleSingleton()
                
                );

            container
                .Resolve<Run>()
                .Process(new FileInfo(sutPath), new FileInfo(testPath));

            Console.ReadLine();
        }
    }
}
