using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaleidoscopeCsharp
{
    internal enum Token : int
    {
        EndOfFile = -1,

        // commands
        Def = -2,
        Extern = -3,

        // primary
        Identifier = -4,
        Number = -5
    }

    internal class Lexer
    {
        private readonly string input;
        private int currentIndex;

        internal string IdentifierString { get; private set; }
        internal double NumberValue { get; private set; }

        internal Lexer(string input)
        {
            this.input = input;
        }

        internal IEnumerable<int> Tokens()
        {
            char lastChar = ' ';

            while (currentIndex < input.Length)
            {
                while (char.IsWhiteSpace(lastChar))
                {
                    lastChar = input[currentIndex++];
                }

                // 文字列から開始する場合
                if (char.IsLetter(lastChar))
                {
                    IdentifierString = lastChar.ToString();
                    while (char.IsLetterOrDigit(lastChar = input[currentIndex++]))
                    {
                        IdentifierString += lastChar.ToString();
                    }

                    switch (IdentifierString)
                    {
                        case "def":
                            yield return (int) Token.Def;
                            break;
                        case "extern":
                            yield return (int) Token.Extern;
                            break;
                        default:
                            yield return (int) Token.Identifier;
                            break;
                    }

                    continue;
                }

                // 数字かピリオド（小数点）で始まる場合
                if (char.IsDigit(lastChar) || lastChar == '.')
                {
                    string numString = string.Empty;
                    do
                    {
                        numString += lastChar.ToString();
                        lastChar = input[currentIndex++];
                    } while (char.IsDigit(lastChar) || lastChar == '.');

                    // TODO: try-catchで囲いたい
                    NumberValue = double.Parse(numString);
                    yield return (int) Token.Number;

                    continue;
                }

                // # から始まる場合（コメント）
                if (lastChar == '#')
                {
                    // EOFか改行までスキップ
                    do
                    {
                        lastChar = input[currentIndex++];
                    } while (currentIndex < input.Length && lastChar != '\n' && lastChar != '\r');

                    continue;
                }

                // 文字、数字、コメント以外（＋など）そのままAsciiコードで返す
                int thisChar = lastChar;
                lastChar = input[currentIndex++];
                yield return thisChar;
            }

            yield return (int) Token.EndOfFile;
        }
    }
}
