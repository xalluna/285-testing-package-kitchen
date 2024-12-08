using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LearningStarter.Common.EntityController;

public class ControllerMethods : StringEnum<ControllerMethods>
{
    public const string GetById = nameof(GetById);
    public const string GetAll = nameof(GetAll);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string DeleteById = nameof(DeleteById);
}

public class StringEnum<T> where T : class
{
    public static readonly List<string> List = typeof(T)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(x => x.FieldType == typeof(string))
        .Select(x => (string) x.GetValue(null))
        .ToList();
}