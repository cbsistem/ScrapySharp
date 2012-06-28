using System.Globalization;
using System.Text;

namespace ScrapySharp.Html
{
    public class CodeReader
    {
        private readonly string sourceCode;
        private readonly StringBuilder buffer;
        private int position;
        private CodeReadingContext context;

        private int lineNumber = 1;
        private int linePosition = 1;

        public CodeReader(string sourceCode)
        {
            if (sourceCode.EndsWith("\n"))
                this.sourceCode = sourceCode;
            else
                this.sourceCode = sourceCode + "\n";

            buffer = new StringBuilder();
            context = CodeReadingContext.None;
        }

        public Word ReadWord()
        {
            buffer.Remove(0, buffer.Length);
            var c = ReadChar();

            //while (char.IsWhiteSpace(c))
            //    c = ReadChar();

            if (char.IsWhiteSpace(c))
                return new Word(c.ToString(CultureInfo.InvariantCulture), lineNumber, linePosition, false);

            if (context != CodeReadingContext.InQuotes && c == Tokens.Quote)
            {
                context = CodeReadingContext.InQuotes;
                return ReadQuotedString();
            }

            buffer.Append(c);

            var letterOrDigit = char.IsLetterOrDigit(c);

            while (char.IsLetterOrDigit(GetNextChar()) == letterOrDigit && !char.IsWhiteSpace(GetNextChar()))
            {
                c = ReadChar();
                if (c == Tokens.Quote)
                {
                    position--;
                    break;
                }

                buffer.Append(c);
            }

            return new Word(buffer.ToString(), lineNumber, linePosition, false);
        }

        private Word ReadQuotedString()
        {
            var c = ReadChar();
            
            while (!End && context == CodeReadingContext.InQuotes)
            {
                if (c == Tokens.Quote)
                    break;

                var nextChar = GetNextChar();
                if (nextChar == Tokens.TagBegin || nextChar == Tokens.TagEnd)
                    break;

                buffer.Append(c);

                if (c == Tokens.TagBegin || c == Tokens.TagEnd)
                    break;

                c = ReadChar();
            }

            context = CodeReadingContext.None;

            return new Word(buffer.ToString(), lineNumber, linePosition, true);
        }


        public char GetNextChar()
        {
            if (position >= sourceCode.Length)
                return (char)0;
            return sourceCode[position];
        }

        public char GetPreviousChar()
        {
            if (position <= 1)
                return (char)0;
            return sourceCode[position - 2];
        }

        public char ReadChar()
        {
            if (End)
                return (char)0;
            var c = sourceCode[position++];
            linePosition++;

            if (c == '\n')
            {
                lineNumber++;
                linePosition = 1;
            }

            return c;
        }

        public bool End
        {
            get { return position >= sourceCode.Length; }
        }

        public int LineNumber
        {
            get { return lineNumber; }
        }

        public int LinePosition
        {
            get { return linePosition; }
        }
    }
}