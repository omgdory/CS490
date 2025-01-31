namespace Lexer_x86;

public class Token {
  public int TokenType { get; }
  public int ChannelType { get; }
  public string Value { get; }
  
  // constructors
  public Token() {
    TokenType = -1;
    ChannelType = -1;
    Value = "";
  }
  public Token(int tokenType, int channelType, string value) {
    TokenType = tokenType;
    ChannelType = channelType;
    Value = value;
  }
}
