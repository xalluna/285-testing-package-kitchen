#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Common.EntityController;

public class EntityControllerInfo
{
    private Type ControllerType { get; }
    private Type EntityType { get; }
    private Type? GetDtoType { get; }
    private Type? CreateDtoType { get; }
    private Type? UpdateDtoType { get; }
    
    private OmitMethodsAttribute? OmitMethods { get; init; } 

    public EntityControllerInfo(Type controllerType)
    {
        ControllerType = controllerType;
        EntityType = ControllerType.BaseType!.GetGenericArguments().First();
        
        var assembly = EntityType.Assembly;
        
        var entityFullName = EntityType.FullName;
        var entityName = EntityType.Name;

        GetDtoType = assembly.GetType(entityFullName!.Replace(entityName, $"{entityName}{DtoSuffixes.Get}"));
        CreateDtoType = assembly.GetType(entityFullName.Replace(entityName, $"{entityName}{DtoSuffixes.Create}"));
        UpdateDtoType = assembly.GetType(entityFullName.Replace(entityName, $"{entityName}{DtoSuffixes.Update}"));

        OmitMethods = controllerType.GetCustomAttribute<OmitMethodsAttribute>();
    }

    public Response ValidateControllerMethods()
    {
        var response = new Response();
        
        if (OmitMethods is { OmitGetDto: false } && GetDtoType is null)
        {
            response.AddError(DtoSuffixes.Get, $"{EntityType.Name}{DtoSuffixes.Get} is not defined");
        }

        if (OmitMethods is { OmitCreateDto: false } && CreateDtoType is null)
        {
            response.AddError(DtoSuffixes.Create, $"{EntityType.Name}{DtoSuffixes.Create} is not defined");
        }

        if (OmitMethods is { OmitUpdateDto: false } && UpdateDtoType is null)
        {
            response.AddError(DtoSuffixes.Update, $"{EntityType.Name}{DtoSuffixes.Update} is not defined");
        }

        if (response.HasErrors)
        {
            return response;
        }

        response.Errors.AddRange(ValidateGetAll().Errors);
        response.Errors.AddRange(ValidateGetById().Errors);
        response.Errors.AddRange(ValidateCreate().Errors);
        response.Errors.AddRange(ValidateUpdate().Errors);
        response.Errors.AddRange(ValidateDeleteById().Errors);
        
        return response;
    }

    private Response ValidateGetAll()
    {
        if (OmitMethods?.ShouldOmit(ControllerMethods.GetAll) ?? false)
        {
            return new Response();
        }
        
        if (GetDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Get} is null");
        }
        
        var returnType = typeof(ActionResult<>)
            .MakeGenericType(typeof(Response<>)
                .MakeGenericType(typeof(List<>)
                    .MakeGenericType(GetDtoType)));
        
        return ValidateMethod(ControllerMethods.GetAll, returnType);
    }

    private Response ValidateGetById()
    {
        if (OmitMethods?.ShouldOmit(ControllerMethods.GetById) ?? false)
        {
            return new Response();
        }
        
        if (GetDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Get} is null");
        }
        
        var returnType = typeof(ActionResult<>)
            .MakeGenericType(typeof(Response<>)
                .MakeGenericType(GetDtoType));

        Type[] parameters = [typeof(int)];

        return ValidateMethod(ControllerMethods.GetById, returnType, parameters);
    }

    private Response ValidateCreate()
    {
        if (OmitMethods?.ShouldOmit(ControllerMethods.Create) ?? false)
        {
            return new Response();
        }
        
        if (GetDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Get} is null");
        }

        if (CreateDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Create} is null");
        }
        
        var returnType = typeof(ActionResult<>)
            .MakeGenericType(typeof(Response<>)
                .MakeGenericType(GetDtoType));
        
        Type[] parameters = [CreateDtoType];
        
        return ValidateMethod(ControllerMethods.Create, returnType, parameters);
    }

    private Response ValidateUpdate()
    {
        if (OmitMethods?.ShouldOmit(ControllerMethods.Update) ?? false)
        {
            return new Response();
        }
        
        if (GetDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Get} is null");
        }

        if (UpdateDtoType is null)
        {
            throw new NullReferenceException($"{EntityType.Name}{DtoSuffixes.Update} is null");
        }
        
        var returnType = typeof(ActionResult<>)
            .MakeGenericType(typeof(Response<>)
                .MakeGenericType(GetDtoType));

        Type[] parameters = [typeof(int), UpdateDtoType];
        
        return ValidateMethod(ControllerMethods.Update, returnType, parameters);
    }

    private Response ValidateDeleteById()
    {
        if (OmitMethods?.ShouldOmit(ControllerMethods.DeleteById) ?? false)
        {
            return new Response();
        }
        
        var returnType = typeof(ActionResult<>)
            .MakeGenericType(typeof(Response));

        Type[] parameters = [typeof(int)];
        
        return ValidateMethod(ControllerMethods.DeleteById, returnType, parameters);
    }

    private Response ValidateMethod(string methodName, Type returnType, Type[]? parameters = null)
    {
        var response = new Response();
        
        var method = ControllerType.GetMethod(methodName);
        
        if (method is null)
        {
            response.AddError(methodName, $"Method {methodName} not found");
            return response;
        }

        if (method.ReturnType != returnType)
        {
            response.AddError(methodName, $"Method {methodName} return type \"{method.ReturnType}\" does not match return type \"{returnType}\"");
        }

        if (parameters is null) return response;
        
        var methodParameters = method.GetParameters()
            .Select(x => x.ParameterType)
            .ToArray();

        if (parameters.Length != methodParameters.Length)
        {
            response.AddError(methodName, $"Method {methodName} parameter count mismatch");
            return response;
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i] != methodParameters[i])
            {
                response.AddError(methodName, $"Method {methodName} parameter at position {i} not match expected type {parameters[i].Name}");
            }
        }

        return response;
    }
}
