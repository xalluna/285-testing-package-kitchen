#nullable enable
using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Common.EntityController;

public class EntityController<TEntity> : ControllerBase;


public class DtoSuffixes : StringEnum<DtoSuffixes>
{
    public const string Get = "GetDto";
    public const string Create = "CreateDto";
    public const string Update = "UpdateDto";
}

public static class ControllerMethodsExtensions
{
    public static bool ValidateControllerConfiguration(bool throwOnError = true)
    {
        var controllerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.BaseType is { IsGenericType: true } && x.BaseType.GetGenericTypeDefinition() == typeof(EntityController<>))
            .Select(x => new EntityControllerInfo(x))
            .ToList();

        return controllerTypes.TrueForAll(controllerInfo => {
            var response = controllerInfo.ValidateControllerMethods();

            if (!throwOnError || !response.HasErrors) return response.HasErrors;
            
            var errors = response.Errors.Aggregate("", (acc, error) => $"{acc}\n{error.Property}: {error.Message}");
            throw new Exception(errors);

        });
    }
}