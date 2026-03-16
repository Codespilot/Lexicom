namespace Lexicom.Scalar;

public class ScalarDefaultParameterAttribute : Attribute
{
    /// <exception cref="ArgumentNullException"/>
    public ScalarDefaultParameterAttribute(
        string paramName,
        object? defaultValue)
    {
        ArgumentNullException.ThrowIfNull(paramName);

        ParamName = paramName;
        DefaultValue = defaultValue;
    }

    public string ParamName { get; }
    public object? DefaultValue { get; }
}
