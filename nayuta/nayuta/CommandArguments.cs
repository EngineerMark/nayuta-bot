using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace nayuta
{
    public class CommandArguments
    {
        /// <summary>
        /// List of command arguments
        /// </summary>
        public Dictionary<string, string> ArgumentValues { get; set; }
        
        /// <summary>
        /// Input string without command arguments
        /// </summary>
        public string LeftOverCommand { get; set; }

        /// <summary>
        /// Creates a list of 
        /// </summary>
        /// <param name="input"></param>
        public CommandArguments(string input)
        {
            
            MatchCollection matches = Regex.Matches(@""+input, @"(?<=-)[^-]+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //int c = matches.Count;
            LeftOverCommand = input;

            ArgumentValues = new Dictionary<string, string>();
            if (matches != null && matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string value = match.Value;
                    if (string.IsNullOrEmpty(value))
                        continue;
                    
                    if(value[value.Length-1]==' ')
                        value = value.Remove(value.Length - 1);
                    
                    if (string.IsNullOrEmpty(value))
                        continue;
                    
                    string prefix = value.Substring(0,(value.Contains(" "))?value.IndexOf(" "):value.Length);
                    string remainder = (value.Contains(" "))?value.Substring(value.IndexOf(" "), value.Length-value.IndexOf(" ")).Remove(0, 1):string.Empty;
                    ArgumentValues.Add(prefix, remainder);

                    LeftOverCommand = LeftOverCommand.Replace(" -"+value, "");
                }
            }
        }

        public string Get(string argumentName)
        {
            return (ArgumentValues.ContainsKey(argumentName) ? ArgumentValues[argumentName] : null);
        }
    }
}