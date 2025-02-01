namespace Lexer_x86;

public class Token {
  public int TokenType { get; }
  public int ChannelType { get; }
  public string Value { get; }
  public int Line { get; }
  
  // constructors
  public Token() {
    TokenType = -1;
    ChannelType = -1;
    Value = "";
  }
  public Token(int tokenType, int channelType, string value, int line) {
    TokenType = tokenType;
    ChannelType = channelType;
    Value = value;
    Line = line;
  }

  public override string ToString() {
    return "(" + Line + ") Value: \"" + Value + "\"\tToken Type: " + Enum.GetName(typeof(TOKEN_TYPE),TokenType);
  }
}
