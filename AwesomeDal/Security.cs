namespace AwesomeDal
{
    using System.Linq;

    /// <summary>
    /// Contains the security features of <see cref="AwesomeDal" />.
    /// </summary>
    internal static class Security
    {
        /// <summary>
        /// The malicious tags that are checked.
        /// </summary>
        private static readonly string[] Tags =
        {
            "&lt;script", "<script", "&lt;embed", "<embed", "&lt;object", "<object",
            "&lt;iframe", "<iframe", "&lt;link", "<link", "onclick", "droptable",
            "1=1", "exec("
        };

        /// <summary>
        /// Determines whether <paramref name="parameters" /> has any malicious tags.
        /// </summary>
        /// <param name="parameters">The parameters to check for malicious tags.</param>
        /// <returns>
        /// <c>true</c> if a malicious tag is found.
        /// </returns>
        internal static bool ContainsMaliciousTags(params string[] parameters)
        {
            return parameters.Select(
                parameter => Tags.Any(
                    tag => parameter
                    .ToLower()
                    .Replace("\n\r", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "")
                    .Contains(tag)
                    )
                ).FirstOrDefault();
        }
    }
}