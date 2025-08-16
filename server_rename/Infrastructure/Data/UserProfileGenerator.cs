using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.User;
using MoneyManager.Infrastructure.Interfaces.Utilitary;

namespace MoneyManager.Infrastructure.Data
{
    public class UserProfileGenerator : IDataGenerator<UserProfile>
    {
        private const string DefaultLanguageCode = "EN";

        public UserProfile[] Generate()
        {
            return new UserProfile[]
            {
                new() 
                { 
                    Id = UserProfileConstants.UserProfileId, 
                    CurrencyId = CurrencyConstants.USD, 
                    LanguageCode = DefaultLanguageCode
                }
            };
        }
    }
}