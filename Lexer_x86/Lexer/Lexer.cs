using System.ComponentModel;
using System.Runtime.CompilerServices;
using Token_x86;

namespace Lexer_x86;

public class Lexer {
  private static Dictionary<string, TOKEN_TYPE> stringToTypeDict = new Dictionary<string, TOKEN_TYPE>() {
    {"section", TOKEN_TYPE.SECTION},
    {"equ", TOKEN_TYPE.EQU},
    {"global", TOKEN_TYPE.GLOBAL},
    {"byte", TOKEN_TYPE.BYTE},
    {"word", TOKEN_TYPE.WORD},
    {"dword", TOKEN_TYPE.DWORD},
    {"qword", TOKEN_TYPE.QWORD},
  };

  private static List<char> special_chars = new List<char>
    {'!','@','#','$','%','^','&','*','(',')'
    ,',','-','+','=','{','}','[',']',';'
    ,':','\'','.','/','?','\\','|'};

  private const string NEWLINE_LITERAL = "\\n"; // ASCII representation of \n

  /// <summary>
  /// Get tokens in a given assembly file
  /// </summary>
  /// <param name="filepath">Path of the specified file (with extension)</param>
  /// <returns>List of Token objects in the file</returns>
  public static List<Token> LexTokens(string filepath) {
    List<Token> res = new List<Token>();
    
    try {
      using (StreamReader reader = new StreamReader(filepath)) {
        int character;
        bool inComment = false;
        string currentValue = "";
        int lineTracker = 1;
        // one character at a time until can't read
        while ((character = reader.Read()) != -1) {
          char currChar = (char)character;
          HandleChar(currChar, ref inComment, ref currentValue, ref res, lineTracker);
          if(currChar=='\n') lineTracker++;
        }
        // deal with leftover currentValue if necessary
        if(inComment) res.Add(new Token((int)TOKEN_TYPE.COMMENT, (int)CHANNEL_TYPE.COMMENT, currentValue, lineTracker));
        else if(!String.IsNullOrWhiteSpace(currentValue)) res.Add(CreateTokenFromString(currentValue, lineTracker));
      }
    }
    catch (Exception ex) {
      Console.WriteLine("Error: " + ex.Message);
    }
    return res;
  }
  private static void HandleChar(char currChar, ref bool inComment,
  ref string currentValue, ref List<Token> result, int lineTracker) {
    // if in comment mode, then just append to currentValue (comment contents)
    if(inComment) {
      if(currChar == '\r' || currChar == '\n') {
        result.Add(new Token((int)TOKEN_TYPE.COMMENT, (int)CHANNEL_TYPE.COMMENT, currentValue, lineTracker));
        currentValue = "";
        inComment = false;
        if(currChar == '\r' || currChar == '\n')
          result.Add(new Token((int)TOKEN_TYPE.NEWLINE, (int)CHANNEL_TYPE.DEFAULT, NEWLINE_LITERAL, lineTracker));
        return;
      }
      currentValue += currChar;
      return;
    }
    // semicolon -> enter comment
    if(currChar == ';') {
      inComment = true;
      // handle previous token if necessary
      if(currentValue.Length > 0) {
        // ignore whitespace
        // guaranteed not to be in comment, because it would have been caught earlier
        if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      }
      return;
    }
    // percent -> macro
    if(currChar == '%') {
      currentValue += currChar;
      return;
    }
    // comma -> operand separator
    if(currChar == ',') {
      if(currentValue.Length == 0) return;
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      result.Add(new Token((int)TOKEN_TYPE.COMMA, (int)CHANNEL_TYPE.DEFAULT, currChar.ToString(), lineTracker));
      return;
    }
    // colon
    if(currChar == ':') {
      Token currentFoundToken = CreateTokenFromString(currentValue, lineTracker);
      TOKEN_TYPE foundType = TOKEN_TYPE.ERROR;
      switch(currentFoundToken.TokenType) {
        case (int)TOKEN_TYPE.IDENTIFIER:
          foundType = TOKEN_TYPE.LABEL;
          break;
        case (int)TOKEN_TYPE.MACRO_LABEL:
          foundType = TOKEN_TYPE.MACRO_LABEL;
          break;
        default:
          break;
      }
      result.Add(new Token((int)foundType, (int)CHANNEL_TYPE.DEFAULT, currentValue+currChar.ToString(), lineTracker));
      currentValue = "";
      return;
    }
    // any other special char not previously handled -> deal
    // ignore dot for segment identifiers
    if(special_chars.Contains(currChar) && currChar != '.') {
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      TOKEN_TYPE tokenType = TOKEN_TYPE.SPECIAL_CHAR;
      if(currChar == '[') tokenType = TOKEN_TYPE.OPEN_BRACK;
      else if(currChar == ']') tokenType = TOKEN_TYPE.CLOSE_BRACK;
      result.Add(new Token((int)tokenType, (int)CHANNEL_TYPE.DEFAULT, currChar.ToString(), lineTracker));
      return;
    }
    // whitespace -> handle current token, then add a newline token if necessary
    if(currChar == ' ' || currChar == '\r' || currChar == '\t' || currChar == '\n') {
      if(currentValue.Length == 0) return;
      // ignore whitespace
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      if(currChar == '\r' || currChar == '\n')
        result.Add(new Token((int)TOKEN_TYPE.NEWLINE, (int)CHANNEL_TYPE.DEFAULT, NEWLINE_LITERAL, lineTracker));
      return;
    }
    // anything else
    currentValue += currChar;
  }

  private static Token CreateTokenFromString(string value, int lineTracker) {
    // if just whitespace, then add to whitespace channel
    if(String.IsNullOrWhiteSpace(value)) return new Token((int)TOKEN_TYPE.WHITESPACE, (int)CHANNEL_TYPE.WHITESPACE, value, lineTracker);
    // first, check if it is a register
    else if(RegisterToken.StringToType.ContainsKey(value)) {
      return new RegisterToken(value, lineTracker);
    }
    // then, check if it is a mnemonic
    else if(MnemonicToken.StringToType.ContainsKey(value)) {
      return new MnemonicToken(value, lineTracker);
    }
    // then, check if it's a data directive
    else if(DataDirectiveToken.StringToType.ContainsKey(value)) {
      return new DataDirectiveToken(value, lineTracker);
    }
    // then, check if it's a segment identifier
    else if(SegmentIdentifierToken.StringToType.ContainsKey(value)) {
      return new SegmentIdentifierToken(value, lineTracker);
    }
    // any other keyword
    else if(stringToTypeDict.ContainsKey(value.ToLower())) {
      return new Token((int)stringToTypeDict[value.ToLower()], (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // special: single character
    else if(value.Length == 1) {
      if(Char.IsDigit(value[0])) return new Token((int)TOKEN_TYPE.NUMBER,(int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      else if(Char.IsAsciiLetter(value[0])) return new Token((int)TOKEN_TYPE.IDENTIFIER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      else if(special_chars.Contains(value[0])) return new Token((int)TOKEN_TYPE.SPECIAL_CHAR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      // error otherwise
      return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // any more than one char
    else if(value.Length > 1) {
      // check macro
      if(value[0]=='%') {
        if(Char.IsDigit(value[1])) {
          // make sure it is digits
          bool validArg = true;
          for(int i=1; i<value.Length; i++) {
            if(!Char.IsDigit(value[i])) {
              validArg = false;
              break;
            }
          }
          if(!validArg)
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          // now valid
          return new Token((int)TOKEN_TYPE.MACRO_ARGUMENT, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
        // label
        else if(value[1]=='%') {
          // just two percent signs...
          if(value.Length <= 2) {
            Console.WriteLine("(" + lineTracker + ") Error: two percent signs");
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
          for(int i=2; i<value.Length; i++) {
            if(!Char.IsAsciiLetter(value[i]) && !Char.IsDigit(value[i]) && value[i] != '_') {
              Console.WriteLine("(" + lineTracker + ") Error: invalid label");
              return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
            }
          }
          return new Token((int)TOKEN_TYPE.MACRO_LABEL, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
      }
      // check hex
      if(value[0] == '0' && value[1]=='x') {
        // if not a power of 2 plus 2, error
        if(value.Length != 4 && value.Length != 6 && value.Length != 10 && value.Length != 18) {
          return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
        // if just "0x", error
        if(value.Length == 2) {
          return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
        List<char> valid_hex_char = new List<char>
          {'A','B','C','D','E','F','a','b','c','d','e','f'};
        for(int i=2; i<value.Length; i++) {
          if(!valid_hex_char.Contains(value[i]) && !Char.IsDigit(value[i])) {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.HEX_NUMBER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // check valid number
      else if(Char.IsDigit(value[0])) {
        for(int i=1; i<value.Length; i++) {
          if(!Char.IsDigit(value[i])) {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.NUMBER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // check valid identifier
      else if(Char.IsAsciiLetter(value[0]) || value[0] == '_') {
        for(int i=1; i<value.Length; i++) {
          if(!Char.IsAsciiLetter(value[0]) && !Char.IsDigit(value[0]) && value[0] != '_') {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.IDENTIFIER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // error otherwise
      return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // anything else should be an error (will test continuously)
    return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
  }
  public static void Test() {
    Console.WriteLine("Hello world");
  }
}
