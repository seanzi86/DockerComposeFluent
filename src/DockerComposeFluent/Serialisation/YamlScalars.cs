using System.Text.RegularExpressions;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Decides when a string must be quoted so that YAML readers still treat it as a string.
    /// </summary>
    internal static class YamlScalars
    {
        private static readonly Regex _nonStringPattern = new Regex(
            "^(" +
            "~|null|Null|NULL" +
            "|true|True|TRUE|false|False|FALSE" +
            "|y|Y|yes|Yes|YES|n|N|no|No|NO|on|On|ON|off|Off|OFF" +
            "|[-+]?(\\d[\\d_]*)(\\.[\\d_]*)?([eE][-+]?\\d+)?" +
            "|[-+]?\\.\\d[\\d_]*([eE][-+]?\\d+)?" +
            "|[-+]?\\d+(:[0-5]?\\d)+(\\.\\d*)?" +
            "|0[xX][0-9a-fA-F_]+|0[oO]?[0-7_]+|0[bB][01_]+" +
            "|[-+]?\\.(inf|Inf|INF)|\\.(nan|NaN|NAN)" +
            "|\\d{4}-\\d{1,2}-\\d{1,2}([Tt ].*)?" +
            ")$",
            RegexOptions.CultureInvariant);

        /// <summary>
        /// Determines whether writing <paramref name="value"/> as a plain scalar would make it read back as
        /// something other than a string (a boolean, number, null or date).
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns><c>true</c> if the value must be quoted.</returns>
        internal static bool RequiresQuoting(string value)
        {
            return _nonStringPattern.IsMatch(value);
        }
    }
}
