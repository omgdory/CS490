namespace x86toCPP;

public class SegmentIdentifierToken : Token {
  private static Dictionary<string, SEGMENT_IDENTIFIER_TOKEN> _stringToType = new Dictionary<string, SEGMENT_IDENTIFIER_TOKEN>() {
    {".code", SEGMENT_IDENTIFIER_TOKEN.CODE_SEGMENT_IDENTIFIER},
    {".data", SEGMENT_IDENTIFIER_TOKEN.DATA_SEGMENT_IDENTIFIER},
    {".bss", SEGMENT_IDENTIFIER_TOKEN.BSS_SEGMENT_IDENTIFIER},
    {".text", SEGMENT_IDENTIFIER_TOKEN.TEXT_SEGMENT_IDENTIFIER},
    {".stack", SEGMENT_IDENTIFIER_TOKEN.STACK_SEGMENT_IDENTIFIER},
  };

  public static Dictionary<string, SEGMENT_IDENTIFIER_TOKEN> StringToType { get; }
  public SEGMENT_IDENTIFIER_TOKEN IdentifierType { get; }

  static SegmentIdentifierToken() {
    StringToType = _stringToType;
  }

  public SegmentIdentifierToken(string value, int line)
  : base((int)TOKEN_TYPE.SEGMENT_IDENTIFIER, (int)CHANNEL_TYPE.DEFAULT, value, line) {
    if(!StringToType.ContainsKey(value)) {
      throw new Exception("Invalid segment identifier string");
    }
    IdentifierType = StringToType[value];
  }
  
  public override string ToString() {
    string lineIndicator = "(" + Line + ")";
    string valueIndicator = "\"" + Value + "\"";
    string tokenTypeIndicator = "SegIdent: " + Enum.GetName(typeof(SEGMENT_IDENTIFIER_TOKEN), IdentifierType);
    return string.Format("{0,6} {1,32} {2,32}", lineIndicator, valueIndicator, tokenTypeIndicator);
  }
}
