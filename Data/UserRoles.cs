namespace Blazor.Server.Data
{
    public class UserRoles 
    {
        public const string User = "user-role";
        public const string Admin = "Admin-role";

        public static bool HasRole(string role)
        {
            if(role == User)
                return true;
            else if(role == Admin)
                return true;
            return false;
        }
    }
}