namespace SilentMike.DietMenu.Core.Application.Shared;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[ExcludeFromCodeCoverage]
public static class ServiceConstants
{
    private static readonly FileVersionInfo FILE_VERSION_INFO = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    public static string ServiceName => FILE_VERSION_INFO.ProductName!;
    public static string ServiceVersion => FILE_VERSION_INFO.ProductVersion!;
}
