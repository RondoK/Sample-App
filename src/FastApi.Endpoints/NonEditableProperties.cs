using System.Linq.Expressions;
using System.Reflection;
using SystemTextJsonPatch;

namespace FastApi.Endpoints;

public class NonEditableProperties
{
    private readonly Dictionary<Type, string[]?> _dict = new();

    public NonEditableProperties(params Config[] configs)
    {
        foreach (var config in configs)
            _dict[config.Type] = config.PropertyNames;
    }

    //TODO: Add support for multi-level entity validation
    /// <summary>
    /// Works ONLY for 1st entity level. 2nd+ level changes are always valid
    /// </summary>
    /// <param name="patchData"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsValidPatch<T>(JsonPatchDocument<T> patchData) where T: class
    {
        if (!_dict.TryGetValue(typeof(T), out var properties))
            return true;
        
        var changedPropertyNames = patchData.Operations
            .Select(o => o.Path!.LastIndexOf('/') != 0 ? "" : o.Path![1..])
            .Where(p => !string.IsNullOrEmpty(p));

        return !changedPropertyNames.Intersect(properties!, StringComparer.OrdinalIgnoreCase).Any();
    }
}
public class Config
{
    public Type Type { get; }
    public string[] PropertyNames { get; }

    private Config(Type type, string[] propertyNames)
    {
        Type = type;
        PropertyNames = propertyNames;
    }

    public static Config Create<T>(params Expression<Func<T, object>>[] @params) =>
        new(typeof(T), @params.Select(p => GetPropertyInfo(p).Name).ToArray());
    
    /// <summary>
    /// Gets the corresponding <see cref="PropertyInfo" /> from an <see cref="Expression" />.
    /// Source: https://stackoverflow.com/a/49695423
    /// </summary>
    private static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression) =>
        expression?.Body switch
        {
            null => throw new ArgumentNullException(nameof(expression)),
            UnaryExpression { Operand: MemberExpression me } => (PropertyInfo)me.Member,
            MemberExpression me => (PropertyInfo)me.Member,
            _ => throw new ArgumentException($"The expression doesn't indicate a valid property. [ {expression} ]")
        };
}



