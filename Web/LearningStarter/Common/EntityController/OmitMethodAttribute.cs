using System;
using System.Linq;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace LearningStarter.Common.EntityController;

[AttributeUsage(AttributeTargets.Class)]
public class OmitMethodsAttribute : Attribute
{
    public string[] OmittedMethods { get; init; }

    public bool OmitCreateDto => OmittedMethods.Contains(ControllerMethods.Create);
    public bool OmitUpdateDto => OmittedMethods.Contains(ControllerMethods.Update);
    public bool OmitGetDto => OmittedMethods.Contains(ControllerMethods.GetAll)
                              || OmittedMethods.Contains(ControllerMethods.GetById)
                              || OmittedMethods.Contains(ControllerMethods.Create)
                              || OmittedMethods.Contains(ControllerMethods.Update);
    
    public OmitMethodsAttribute(params string[] omittedMethods)
    {
        if (omittedMethods.Any(method => !ControllerMethods.List.Contains(method)))
        {
            throw new InvalidConfigurationException();
        }

        OmittedMethods = omittedMethods;
    }
    
    public bool ShouldOmit(string methodName) => OmittedMethods.Contains(methodName);
}

