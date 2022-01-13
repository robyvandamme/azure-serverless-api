using System;

namespace IsolatedProcessIoC.Configuration;

public class FunctionConfiguration : IFunctionConfiguration
{
    public FunctionConfiguration()
    {
        FunctionUrl = Environment.GetEnvironmentVariable("FunctionUrl");
    }

    public string? FunctionUrl { get; }
}
