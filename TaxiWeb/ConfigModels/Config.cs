namespace TaxiWeb.ConfigModels
{
    public class Config
    {
        public object Logging { get;set; }
        public string AllowedHosts { get; set; }
        public JWTConfig JWT { get; set; } 
        public AzureStorage AzureStorage { get; set; }

    }

    public class JWTConfig
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }

    public class AzureStorage
    {
        public string ConnectionString { get; set; }
    }
}
