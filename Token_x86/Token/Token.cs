namespace Token_x86;

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
    string lineIndicator = "(" + Line + ")";
    string valueIndicator = "\"" + Value + "\"";
    string tokenTypeIndicator = "" + Enum.GetName(typeof(TOKEN_TYPE),TokenType);
    return string.Format("{0,6} {1,32} {2,32}", lineIndicator, valueIndicator, tokenTypeIndicator);
  }
}
