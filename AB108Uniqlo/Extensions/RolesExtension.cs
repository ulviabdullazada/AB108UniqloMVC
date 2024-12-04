using AB108Uniqlo.Views.Account.Enums;

namespace AB108Uniqlo.Extensions
{
    public static class RolesExtension
    {
        public static string GetRole(this Roles role)
        {
            return role switch
            {
                Roles.User => nameof(Roles.User),
                Roles.Admin => nameof(Roles.Admin),
                Roles.Moderator => nameof(Roles.Moderator),
                Roles.Designer => nameof(Roles.Designer),
                Roles.Cashier => nameof(Roles.Cashier)
            };
        }
    }
}
