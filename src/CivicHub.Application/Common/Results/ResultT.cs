namespace CivicHub.Application.Common.Results;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value)
    {
        _value = value;
    }

    protected internal Result(string errorMessage)
        : base(errorMessage)
    {

    }

    public TValue Value => _value!;

    public static implicit operator Result<TValue>(TValue value) => new(value);

}