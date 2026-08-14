using System.Collections.Generic;
using System.Text;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public static class FConsoleTokenizer
    {
        public static string[] Tokenize(string commandLine)
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