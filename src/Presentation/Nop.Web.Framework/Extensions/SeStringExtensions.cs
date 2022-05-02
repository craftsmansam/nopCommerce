namespace Nop.Web.Framework.Extensions
{
    public static class SeStringExtensions
    {
        public static string GetSeNameWithoutPath(this string seNameWithOptionalPath)
        {
            if (seNameWithOptionalPath.Contains("/"))
            {
                return seNameWithOptionalPath.Split('/')[1];
            }

            return seNameWithOptionalPath;
        }

        public static string GetPathFromSeName(this string seNameWithPath)
        {
            if (seNameWithPath.Contains("/"))
            {
                return seNameWithPath.Split('/')[0];
            }

            return seNameWithPath;
        }
    }
}