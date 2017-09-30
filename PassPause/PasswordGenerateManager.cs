using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Betarium.PassPause
{
    class PasswordGenerateManager
    {
        private Random rand = new Random();
        private char[] candidate = new char[62];

        public PasswordGenerateManager()
        {
        }

        public string Generate(int length)
        {
            Regex ptn1 = new Regex("[a-z]");
            Regex ptn2 = new Regex("[A-Z]");
            Regex ptn3 = new Regex("[0-9]");
            while (true)
            {
                var temp = GenerateInner(length);
                var match1 = ptn1.Match(temp);
                var match2 = ptn2.Match(temp);
                var match3 = ptn3.Match(temp);
                if (match1.Length > 0 && match2.Length > 0 && match3.Length > 0)
                {
                    return temp;
                }
            }
        }

        public string GenerateInner(int length)
        {
            for (int i = 0; i < candidate.Length; i++)
            {
                if (i < 10)
                {
                    candidate[i] = Char.ConvertFromUtf32('0' + i)[0];
                }
                else if (i >= 10 && i < 36)
                {
                    candidate[i] = Char.ConvertFromUtf32('A' + i - 10)[0];
                }
                else if (i >= 36)
                {
                    candidate[i] = Char.ConvertFromUtf32('a' + i - 36)[0];
                }
            }

            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += candidate[rand.Next() % candidate.Length];
            }

            return result;
        }
    }
}
