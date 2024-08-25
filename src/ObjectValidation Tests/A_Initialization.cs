namespace ObjectValidation_Tests
{
    [TestClass]
    public class A_Initialization
    {
        [AssemblyInitialize]
        public static void Init(TestContext tc) => wan24.Tests.TestsInitialization.Init(tc);
    }
}
