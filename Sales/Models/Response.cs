using System.Diagnostics.CodeAnalysis;

namespace Sales.Models;

[ExcludeFromCodeCoverage]
public class Response<T>
{
    public Response(T data, List<string> errors)
    {
        Data = data;
        Errors = errors;
    }

    public Response(T data)
    {
        Data = data;
    }

    public Response(List<string> errors)
    {
        Errors = errors;
    }

    public Response(string error)
    {
        Errors.Add(error);
    }

    public T Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
}
