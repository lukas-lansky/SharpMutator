namespace Lansky.SharpMutator.ExampleSut
{
    public static class SomeClass
    {
        public static int Max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }

            return b;
        }

        public static int Adder(int a, int b)
        {
            return a + b;
        }
    }
}
