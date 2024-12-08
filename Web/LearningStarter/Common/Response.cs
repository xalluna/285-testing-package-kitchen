using System.Collections.Generic;

namespace LearningStarter.Common;

public class Response<T>
{
    public T Data { get; set; }
    public List<Error> Errors { get; set; } = [];
    public bool HasErrors => Errors.Count > 0;

    public void AddError(string property, string message)
    {
        Errors.Add(new Error(property, message));
    }
}

public class Response : Response<object>;