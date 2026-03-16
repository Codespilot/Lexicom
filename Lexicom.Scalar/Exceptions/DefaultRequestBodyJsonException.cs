namespace Lexicom.Scalar.Exceptions;

public class DefaultRequestBodyJsonException(string? json, Exception? innerException) : Exception($"An unexpected error occured while converting the json string '{json ?? "null"}' to an actual json node.", innerException)
{
}
