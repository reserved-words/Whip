
namespace Whip.Common.Validation
{
    public static class SocialMediaValidation
    {
        public const byte FacebookUsernameMaxLength = 15;
        public const string FacebookUsernameRegexPattern = @"^[0-9a-zA-Z\.]*$";

        public const byte TwitterUsernameMaxLength = 15;
        public const string TwitterUsernameRegexPattern = "^[0-9a-zA-Z_]*$";
    }
}
