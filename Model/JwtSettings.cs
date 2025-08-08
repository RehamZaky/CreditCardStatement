namespace CreditCardStatementApi.Model
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Key { get; set; }
        public double Expires { get; set; }

    }
}
