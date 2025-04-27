namespace SocialMediaApi.Constants;

public static class ApiRoutes
{
    public static class Posts
    {
        public const string Base = "/posts";
        public const string Create = Base;
        public const string GetAll = Base;
        public const string Like = Base + "/like";
    }

    public static class Users
    {
        public const string Base = "/users";
        public const string Create = Base;
        public const string Follow = Base + "/follow";
    }
}