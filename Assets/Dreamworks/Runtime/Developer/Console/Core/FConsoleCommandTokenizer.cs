using System.Text;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Splits a command-line string into an array of tokens, respecting quoted substrings.
    /// </summary>
    /// <remarks>This method handles quoted substrings by treating them as single tokens, even if they contain
    /// spaces. Quotation marks themselves are not included in the resulting tokens. For example, the input <c>"arg1
    /// \"arg 2\" arg3"</c> will produce the tokens <c>{"arg1", "arg 2", "arg3"}</c>.</remarks>
    internal static class FConsoleCommandTokenizer
    {
        internal static string[] Tokenize(string commandLine)
        {
            List<string> tokens = new();

            StringBuilder current = new();

            bool quoted = false;

            foreach (char c in commandLine)
            {
                if (c == '"')
                {
                    quoted = !quoted;

                    continue;
                }

                if (c == ' ' && !quoted)
                {
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());

                        current.Clear();
                    }

                    continue;
                }

                current.Append(c);
            }

            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
            }

            return tokens.ToArray();
        }
    }
}