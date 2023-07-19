namespace SilentMike.DietMenu.Mailing.Application.Shared;

using System.Diagnostics;
using System.Reflection;

public class ServiceConstants
{
    private static readonly FileVersionInfo FILE_VERSION_INFO = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    public static string ServiceName => FILE_VERSION_INFO.ProductName!;
    public static string ServiceVersion => FILE_VERSION_INFO.ProductVersion!;
}
