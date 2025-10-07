using System.Reflection;
using Xunit.Sdk;

namespace Api.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class TestBeforeAfter : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        Console.WriteLine(methodUnderTest.Name + " STARTED " + DateTime.UtcNow);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        Console.WriteLine(methodUnderTest.Name + " END " + DateTime.UtcNow);
    }
}