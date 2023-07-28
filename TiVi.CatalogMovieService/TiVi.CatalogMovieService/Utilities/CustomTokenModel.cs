namespace TiVi.UserCatalogService.Utilities
{
    public class CustomTokenModel
    {
        public object actor { get; set; }
        public List<object> audiences { get; set; }
        public List<CustomClaim> claims { get; set; }
        public string encodedHeader { get; set; }
        public string encodedPayload { get; set; }
        public Header header { get; set; }
        public object id { get; set; }
        public object issuer { get; set; }
        public Payload payload { get; set; }
        public object innerToken { get; set; }
        public object rawAuthenticationTag { get; set; }
        public object rawCiphertext { get; set; }
        public string rawData { get; set; }
        public object rawEncryptedKey { get; set; }
        public object rawInitializationVector { get; set; }
        public string rawHeader { get; set; }
        public string rawPayload { get; set; }
        public string rawSignature { get; set; }
        public object securityKey { get; set; }
        public string signatureAlgorithm { get; set; }
        public object signingCredentials { get; set; }
        public object encryptingCredentials { get; set; }
        public object signingKey { get; set; }
        public object subject { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
    }

    public class Properties
    {
    }

    public class CustomClaim
    {
        public string issuer { get; set; }
        public string originalIssuer { get; set; }
        public Properties properties { get; set; }
        public object subject { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string valueType { get; set; }
    }

    public class Header
    {
        public string alg { get; set; }
        public string typ { get; set; }
    }

    public class Payload
    {
        public string unique_name { get; set; }
        public string BusinessGrupID { get; set; }
        public string Email { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
    }
}
