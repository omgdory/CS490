namespace Lexer_x86;

public class Lexer {
  private static Dictionary<string, TOKEN_TYPE> stringToTypeDict = new Dictionary<string, TOKEN_TYPE>() {
    {"mov", TOKEN_TYPE.MOV},
    {"add", TOKEN_TYPE.ADD},
    {"sub", TOKEN_TYPE.SUB},
    {"mul", TOKEN_TYPE.MUL},
    {"div", TOKEN_TYPE.DIV},
    {"cmp", TOKEN_TYPE.CMP},
    {"jmp", TOKEN_TYPE.JMP},
    {"je", TOKEN_TYPE.JE},
    {"jne", TOKEN_TYPE.JNE},
    {"jl", TOKEN_TYPE.JL},
    {"jle", TOKEN_TYPE.JLE},
    {"jg", TOKEN_TYPE.JG},
    {"jge", TOKEN_TYPE.JGE},
    {"jb", TOKEN_TYPE.JB},
    {"jbe", TOKEN_TYPE.JBE},
    {"ja", TOKEN_TYPE.JA},
    {"jae", TOKEN_TYPE.JAE},
    {"call", TOKEN_TYPE.CALL},
    {"ret", TOKEN_TYPE.RET},
    {"push", TOKEN_TYPE.PUSH},
    {"pop", TOKEN_TYPE.POP},
    {"and", TOKEN_TYPE.AND},
    {"or", TOKEN_TYPE.OR},
    {"xor", TOKEN_TYPE.XOR},
    {"nop", TOKEN_TYPE.NOP},
    {"inc", TOKEN_TYPE.INC},
    {"dec", TOKEN_TYPE.DEC},
    {"shl", TOKEN_TYPE.SHL},
    {"shr", TOKEN_TYPE.SHR},
    {"syscall", TOKEN_TYPE.SYSCALL},
    {"section", TOKEN_TYPE.SECTION},
    {"equ", TOKEN_TYPE.EQU},
    {"db", TOKEN_TYPE.DB},
    {"dw", TOKEN_TYPE.DW},
    {"dd", TOKEN_TYPE.DD},
    {"dq", TOKEN_TYPE.DQ},
    {"ddq", TOKEN_TYPE.DDQ},
    {"dt", TOKEN_TYPE.DT},
    {"resb", TOKEN_TYPE.RESB},
    {"resw", TOKEN_TYPE.RESW},
    {"resd", TOKEN_TYPE.RESD},
    {"resq", TOKEN_TYPE.RESQ},
    {".code", TOKEN_TYPE.CODE_SEGMENT_IDENTIFIER},
    {".data", TOKEN_TYPE.DATA_SEGMENT_IDENTIFIER},
    {".bss", TOKEN_TYPE.BSS_SEGMENT_IDENTIFIER},
    {".text", TOKEN_TYPE.TEXT_SEGMENT_IDENTIFIER},
    {".stack", TOKEN_TYPE.STACK_SEGMENT_IDENTIFIER},
    {"global", TOKEN_TYPE.GLOBAL},
  };

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
        // one character at a time until can't read
        while ((character = reader.Read()) != -1) {
          char currChar = (char)character;
          // if(currChar=='\r') {
          //   character = reader.Read();
          //   if((char)character != '\n') {
          //     throw new Exception("CR not followed by LF");
          //   }
          // }
          HandleChar(currChar, ref inComment, ref currentValue, ref res);
        }
      }
    }
    catch (Exception ex) {
      Console.WriteLine("Error: " + ex.Message);
    }
    return res;
  }
  private static void HandleChar(char currChar, ref bool inComment, ref string currentValue, ref List<Token> result) {
    // if in comment mode, then just append to currentValue (comment contents)
    if(inComment) {
      if(currChar == '\r') {
        result.Add(new Token((int)TOKEN_TYPE.COMMENT, (int)CHANNEL_TYPE.COMMENT, currentValue));
        currentValue = "";
        inComment = false;
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
        // handle token type
        TOKEN_TYPE tokenType = stringToTypeDict.ContainsKey(currentValue) ?
          stringToTypeDict[currentValue]
          : TOKEN_TYPE.IDENTIFIER;
        // guaranteed not to be in comment, because it would have been caught earlier
        result.Add(new Token((int)tokenType, (int)CHANNEL_TYPE.DEFAULT, currentValue));
      }
      return;
    }
    // whitespace or comma -> handle current token, then add a newline token if necessary
    if(currChar == ' ' || currChar == '\r' || currChar == '\t' || currChar == '\n' || currChar == ',') {
      if(currentValue.Length == 0) return;
      // Console.WriteLine("Token found");
      // handle token type
      TOKEN_TYPE tokenType = stringToTypeDict.ContainsKey(currentValue) ?
        stringToTypeDict[currentValue]
        : TOKEN_TYPE.IDENTIFIER;
      result.Add(new Token((int)tokenType, (int)CHANNEL_TYPE.DEFAULT, currentValue));
      currentValue = "";
      // if(currChar=='\n')
      //   result.Add(new Token((int)TOKEN_TYPE.NEWLINE, (int)CHANNEL_TYPE.WHITESPACE, currChar.ToString()));
      if (currChar==',')
        result.Add(new Token((int)TOKEN_TYPE.COMMA, (int)CHANNEL_TYPE.DEFAULT, currChar.ToString()));
      return;
    }
    // anything else
    // Console.WriteLine("Current char: " + currChar);
    currentValue += currChar;
  }

  public static void PrintToken(Token token) {
    // special channels
    if((CHANNEL_TYPE)token.ChannelType==CHANNEL_TYPE.WHITESPACE) {
      return;
    } else if((CHANNEL_TYPE)token.ChannelType==CHANNEL_TYPE.COMMENT) {
      Console.Write("Token Type: " + token.TokenType);
      Console.WriteLine(", comment, value: \"" + token.Value + '\"');
      return;
    }
    // default channel
    Console.Write("Token Type: " + token.TokenType);
    switch(token.TokenType) {
      case (int)TOKEN_TYPE.IDENTIFIER:
        Console.WriteLine(", identifier, value: \"" + token.Value + '\"');
        break;
      default:
        Console.WriteLine(", keyword, value: \"" + token.Value + '\"');
        break;
    }
  }
  public static void Test() {
    Console.WriteLine("Hello world");
  }
}
