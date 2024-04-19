using System.Reflection;
using Mapster;

namespace Api;

public class MappingRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        var rootType = typeof(App.Data.Models.Agg);
        // config Sample file https://github.com/MapsterMapper/Mapster/blob/master/src/Sample.CodeGen/MappingRegister.cs

        config.AdaptFrom("[name]Update")
            .ForAllTypesInNamespace(rootType.Assembly, rootType.Namespace!)
            .IgnoreAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute))
            .ExcludeTypes(type => type.IsEnum);

        config.GenerateMapper("[name]Mapper")
            .ForAllTypesInNamespace(rootType.Assembly, rootType.Namespace!);
    }
}