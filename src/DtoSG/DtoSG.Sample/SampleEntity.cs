//using Generators;
using Mapster;

namespace DtoSG.Sample;

// This code will not compile until you build the project with the Source Generators

//[Report]
//[ReportSG]
[AdaptTo("[name]DTO"), GenerateMapper]
public partial class SampleEntity
{
    public int Id { get; } = 42;
    public string? Name { get; } = "Sample";
}

//[ReportSG]
public partial class MyClass
{
}