namespace Nop.Web.Framework.Extensions
{
    public static class SeStringExtensions
    {
        public static string GetSeNameWithoutPath(this string seNameWithOptionalPath)
        {
            return GetChunkFromSeName(seNameWithOptionalPath, 1);
        }

        public static string GetPathFromSeName(this string seNameWithPath)
        {
            return GetChunkFromSeName(seNameWithPath, 0);
        }

        private static string GetChunkFromSeName(string seNameWithPath, int index)
        {
            return seNameWithPath.Contains("/") ? seNameWithPath.Split('/')[index] : seNameWithPath;
        }
    }
}