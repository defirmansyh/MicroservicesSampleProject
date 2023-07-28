namespace TiVi.MLService.Utilities
{
    public static class SettingHelper
    {
        public static string GetAppsettingValue(string key)
        {
            //Calling Example
            //var x = DataHelper.GetAppsettingValue("ConnectionStrings:LocalConnection"); 
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            return configuration.GetSection(key).Value;

        }

        public static string GenerateEndpoint(string keyBase, string keyEndpoint)
        {
            var baseUrl = GetAppsettingValue($"BaseUrl:{keyBase}");
            var endpoint = GetAppsettingValue($"EndpointUrl:{keyEndpoint}");
            return baseUrl + endpoint;
        }
    }
}
