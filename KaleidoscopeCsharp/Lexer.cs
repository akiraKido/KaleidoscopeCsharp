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
        private char lastChar = ' ';

        internal string IdentifierString { get; private set; }
        internal double NumberValue { get; private set; }
        internal int CurrentToken { get; private set; }

        internal int CurrentLine { get; private set; }
        internal int CurrentIndex => currentIndex;

        internal Lexer(string input)
        {
            this.input = input;
        }

        // TODO: substrを使ったらもうちょっと処理が早くなる
        internal int NextToken()
        {
            // ファイルの最後だったらEOFにして終了
            if (currentIndex >= input.Length)
            {
                CurrentToken = (int)Token.EndOfFile;
                return CurrentToken;
            }

            // 空白文字をスキップ
            while (char.IsWhiteSpace(lastChar))
            {
                lastChar = input[currentIndex++];
                if (lastChar == '\n') CurrentLine++;
                if (currentIndex >= input.Length) break;
            }

            // 文字列から開始する場合
            if (char.IsLetter(lastChar))
            {
                IdentifierString = lastChar.ToString();
                while (char.IsLetterOrDigit(lastChar = input[currentIndex++]))
                {
                    IdentifierString += lastChar.ToString();

                    if (currentIndex >= input.Length) break;
                }

                switch (IdentifierString)
                {
                    case "def":
                        CurrentToken = (int)Token.Def;
                        break;
                    case "extern":
                        CurrentToken = (int)Token.Extern;
                        break;
                    default:
                        CurrentToken = (int)Token.Identifier;
                        break;
                }
                
                return CurrentToken;
            }

            // 数字かピリオド（小数点）で始まる場合
            if (char.IsDigit(lastChar) || lastChar == '.')
            {
                string numString = string.Empty;
                do
                {
                    numString += lastChar.ToString();
                    lastChar = input[currentIndex++];

                    if (currentIndex >= input.Length) break;
                } while (char.IsDigit(lastChar) || lastChar == '.');

                // TODO: try-catchで囲いたい
                NumberValue = double.Parse(numString);
                CurrentToken = (int)Token.Number;
                
                return CurrentToken;
            }

            // # から始まる場合（コメント）
            if (lastChar == '#')
            {
                // EOFか改行までスキップ
                do
                {
                    lastChar = input[currentIndex++];
                } while (currentIndex < input.Length && lastChar != '\n' && lastChar != '\r');
                
                // TODO: 再起関数は場合によってはスタックオーバーフローが発生するので
                // TODO: 問題になったらループにする
                return NextToken();
            }

            // 文字、数字、コメント以外（＋など）そのままAsciiコードで返す
            int thisChar = lastChar;
            if (currentIndex < input.Length)
            {
                // 進められるなら一個進める
                lastChar = input[currentIndex++];
            }
            CurrentToken = thisChar;
            return CurrentToken;
        }
    }
}
