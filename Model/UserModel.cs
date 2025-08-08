using Microsoft.AspNetCore.Identity;

namespace CreditCardStatementApi.Model
{
    public class UserModel :IdentityUser
    {
        public string Type { get; set; } = "user";

        public string RefreshToken { get; set; } = "";

        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}
