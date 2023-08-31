namespace Api;

public interface IRole
{
    string AsConstString();
}
public static class Roles
{
    public const string Admin = "Admin";
}

public struct RoleClass
{
    public struct Admin : IRole
    {
        public string AsConstString() => Roles.Admin;
    }
}