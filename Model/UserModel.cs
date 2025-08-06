using Microsoft.AspNetCore.Identity;

namespace CreditCardStatementApi.Model
{
    public class UserModel :IdentityUser
    {
        public string Type { get; set; }

        public string RefreshToken { get; set; }

        public string RefreshTokenExpiryTime { get; set; }


    }
}
