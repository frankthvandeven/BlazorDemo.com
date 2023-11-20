using System;
using System.Text.RegularExpressions;

namespace VenturaSQL.Dynamite.Parsing
{
    /// <summary>
    /// Exception thrown when an expression cannot be parsed.
    /// </summary>
    [global::System.Serializable]
    public class ParserException : ApplicationException
    {
        /// <summary>
        /// Gets the position at which the error was found (as a zero-based index). -1 if position not applicable or not known.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the expression being parsed when error was found (null if expression not known).
        /// </summary>
        public String Expression { get; private set; }

        public ParserException() { Position = -1;  }

        /// <summary>
        /// Creates a new ParserException specifying the position, expression and a custom message.
        /// </summary>
        /// <param name="position">Position in expression that error was found.</param>
        /// <param name="expression">Expression being parsed.</param>
        /// <param name="message">Custom error message</param>
        public ParserException(int position, String expression, String message)
            : base(message)
        {
            Position = position;
            Expression = expression;
        }
        public ParserException(string message) : base(message) { Position = -1; }
        public ParserException(string message, Exception inner) : base(message, inner) { Position = -1; }
        protected ParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    
    /// <summary>
    /// Provides simple but efficient parsing of string expressions into "tokens".
    /// </summary>
    public struct SimpleTokenizer
    {
        private String expression;

        private int position;

        /// <summary>
        /// Creates a new SimpleParser specifying an expression to be parsed.
        /// </summary>
        /// <param name="expression">Expression to be parsed</param>
        /// <remarks>
        /// Any leading whitespace is automatically skipped when using this method.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">If Expression is set to null.</exception>
        public SimpleTokenizer(String expression)
        {
            if( expression == null ) throw new ArgumentNullException();
            this.expression = expression;
            int i;
            for( i = 0; i < expression.Length && expression[i] == ' '; i++);
            this.position = i;
        }

        /// <summary>
        /// Gets or sets the expression being parsed.
        /// </summary>
        /// <remarks>
        /// Position is reset to the beginning of the expression. Any leading whitespace is automatically skipped.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">If Expression is set to null.</exception>
        public String Expression
        {
            get { return expression; }
            set
            {
                if( value == null ) throw new ArgumentNullException();
                expression = value;
                int i;
                for( i = 0; i < expression.Length && expression[i] == ' '; i++);
                position = i;
            }
        }

        /// <summary>
        /// Gets the current position in expression.
        /// </summary>
        public int Position
        {
            get { return position; }
        }

        /// <summary>
        /// Advances the current position and returns true if the token at the current position is an identity equal to the specified identity.
        /// </summary>
        /// <param name="identity">Alphanumeric identity token to be tested for</param>
        /// <returns>True if token at current matched the specified one.</returns>
        /// <exception cref="System.NullPointerException">If identity is null.</exception>
        public bool AdvanceIfIdent(String identity)
        {
            int testLen = identity.Length;
            int endPos = position + testLen;
            if (endPos <= expression.Length && (endPos == expression.Length || ((Char.IsLetterOrDigit(expression, endPos) == false) && expression[endPos] != '_')))
            {
                if (String.Compare(expression, position, identity, 0, testLen, true) == 0)
                {
                    while (endPos < expression.Length && expression[endPos] == ' ') { endPos++; }
                    position = endPos;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Advances the current position and returns true if the token at the current position is the specified symbol character.
        /// </summary>
        /// <param name="symbol">Symbol character</param>
        /// <returns>True if token at current matched the specified symbol, false otherwise</returns>
        public bool AdvanceIfSymbol(Char symbol)
        {
            if (position < expression.Length && expression[position] == symbol)
            {
                position++;
                while (position < expression.Length && expression[position] == ' ') { position++; }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Advances the current position and returns true if the token at the current position is the specified symbol string.
        /// </summary>
        /// <param name="symbol">Symbol string</param>
        /// <returns>True if token at current matched the specified symbol, false otherwise</returns>
        public bool AdvanceIfSymbol(String symbol)
        {
            
            if (String.Compare(expression, position, symbol, 0, symbol.Length, true) == 0)
            {
                position += symbol.Length;
                while (position < expression.Length && expression[position] == ' ') { position++; }
                return true;
                
            }
            return false;
        }

        /// <summary>
        /// Gets the identity at current position and advances the current position to the next token.
        /// </summary>
        /// <returns>Next identity token or empty string if no simple word token at current position.</returns>
        public String ReadIdentity()
        {
            int startPos = position;
            while(position < expression.Length && (Char.IsLetterOrDigit(expression, position) || expression[position] == '_') ) { position++; }
            String token = expression.Substring(startPos, position - startPos);
            while (position < expression.Length && expression[position] == ' ') { position++; }
            return token;
        }

        /// <summary>
        /// Gets the next part of the expression that matches the given regular expression and advances the position to the next token after that.
        /// </summary>
        /// <returns>Next matching string or empty string if no match was found.</returns>
        public String ReadNextMatch(Regex matchPattern)
        {
            Match m = matchPattern.Match(expression, position);
            if (m.Success)
            {
                position = m.Index + m.Length;
                while (position < expression.Length && expression[position] == ' ') { position++; }
                return m.Value;
            }
            return String.Empty;
        }
            
        /// <summary>
        /// Verifies that the current token is the specified identity.
        /// </summary>
        /// <remarks>
        /// Current position is advanced passed the given token in case of a match.
        /// </remarks>
        /// <param name="token">Simple alpha-numeric identity token to be tested for</param>
        public void ExpectIdentity(String token)
        {
            if (AdvanceIfIdent(token) == false)
            {
                throw new ParserException(position, expression, "'" + token + "' expected.");
            }
        }

        /// <summary>
        /// Verifies that the current token matches a given symbol and throws a ParserException if not.
        /// </summary>
        /// <remarks>
        /// Current position is advanced passed the given symbol in case of a match.
        /// </remarks>
        /// <param name="symbol">Symbolic token to be tested for</param>
        public void ExpectSymbol(String symbol)
        {
            if (AdvanceIfSymbol(symbol) == false)
            {
                throw new ParserException(position, expression, "'" + symbol + "' expected.");
            }
        }

        /// <summary>
        /// Verifies that the current token matches a given symbol and throws a ParserException if not.
        /// </summary>
        /// <remarks>
        /// Current position is advanced passed the given symbol in case of a match.
        /// </remarks>
        /// <param name="symbol">Symbolic token to be tested for</param>
        public void ExpectSymbol(Char symbol)
        {
            if (AdvanceIfSymbol(symbol) == false)
            {
                throw new ParserException(position, expression, "'" + symbol + "' expected.");
            }
        }

        public void ExpectEnd()
        {
            if (position < expression.Length)
            {
                throw new ParserException(position, expression, "End of expression expected.");
            }
        }

        /// <summary>
        /// Advances the current position if token at current position matches any of the specified alphanumeric tokens.
        /// </summary>
        /// <param name="testValues">Alphanumeric tokens to test for.</param>
        /// <returns>The matching token or null if none of the specified tokens matched.</returns>
        public String AdvanceIfTokenAnyOf(params String[] testValues)
        {
            foreach (String testValue in testValues)
            {
                if (AdvanceIfIdent(testValue))
                {
                    return testValue;
                }
            }
            return null;
        }
        
    }
}
