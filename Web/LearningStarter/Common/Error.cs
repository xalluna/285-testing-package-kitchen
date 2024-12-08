namespace LearningStarter.Common;

public class Error(string property, string message)
{
    public string Property { get; set; } = property;
    public string Message { get; set; } = message;
}
