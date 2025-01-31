namespace Lexer_x86;

public class Token {
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
  };
  public enum CHANNEL_TYPE {
    DEFAULT,
    WHITESPACE,
    COMMENT
  }

  public static Dictionary<string, TOKEN_TYPE> StringToTypeDict { get; }
  public TOKEN_TYPE TokenType { get; }
  public CHANNEL_TYPE ChannelType { get; }
  public string Value { get; }

  // static declarator
  static Token() {
    StringToTypeDict = stringToTypeDict;
  }
  // constructors
  public Token() {
    TokenType = TOKEN_TYPE.DEFAULT;
    ChannelType = CHANNEL_TYPE.DEFAULT;
    Value = "";
  }
  public Token(TOKEN_TYPE tokenType, CHANNEL_TYPE channel, string value) {
    TokenType = tokenType;
    ChannelType = channel;
    Value = value;
  }

  // for debugging
  public void printData() {
      // not print whitespace
      if(ChannelType == CHANNEL_TYPE.WHITESPACE) {
        return;
      }
      else if (ChannelType == CHANNEL_TYPE.COMMENT) {
        Console.Write("Token Category: " + TOKEN_TYPE.COMMENT.ToString());
        Console.WriteLine(", comment, value \"" + Value + "\"");
        return;
      }

      /* handle default channel type*/
      switch(TokenType) {
        default:
          break;
      }
    }
}
