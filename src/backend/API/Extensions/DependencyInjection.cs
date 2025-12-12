using System.Reflection;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        var assemblyBLL = Assembly.Load("BLL");
        var assemblyDAL = Assembly.Load("DAL");

        // BLL
        var bllTypes = assemblyBLL.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace.EndsWith("Services"));

        foreach (var impl in bllTypes)
        {
            var iface = impl.GetInterfaces().FirstOrDefault();
            if (iface != null)
            {
                services.AddScoped(iface, impl);
            }
        }

        // DAL
        var dalTypes = assemblyDAL.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace.EndsWith("Services"));

        foreach (var impl in dalTypes)
        {
            var iface = impl.GetInterfaces().FirstOrDefault();
            if (iface != null)
            {
                services.AddScoped(iface, impl);
            }
        }
    }
}
