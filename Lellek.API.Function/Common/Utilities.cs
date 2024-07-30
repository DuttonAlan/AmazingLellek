
using Microsoft.Azure.Management.ResourceManager.Fluent;


namespace PlanB.DPF.Manager.UserManager.Common;

public static class Utilities
{
    public static string CreatePassword()
    {
        return SdkContext.RandomResourceName("Pa5$", 15);
    }
}
