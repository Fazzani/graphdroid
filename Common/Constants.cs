namespace Common
{
    public class Constants
    {
        public class Permissions
        {
            public const string ADMIN = nameof(ADMIN);
            public const string READ_ONLY = nameof(READ_ONLY);
        }

        public class Policies
        {
            public const string AdminPolicy = nameof(AdminPolicy);
            public const string ViewerPolicy = nameof(ViewerPolicy);
        }
    }
}
