using Microsoft.Extensions.Configuration;

public class DebugMode
{
    public bool Debug { get; set; }

    public DebugMode()
    {
        LoadConfiguration();
    }

    private void LoadConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");

        IConfigurationRoot configuration = builder.Build();
        Debug = configuration.GetValue<bool>("Debug");
    }
}
