namespace Token_x86;

public class DataDirectiveToken : Token {
  private static Dictionary<string, DATA_DIRECTIVE_TOKEN> _stringToType = new Dictionary<string, DATA_DIRECTIVE_TOKEN>() {
    {"db", DATA_DIRECTIVE_TOKEN.DB},
    {"dw", DATA_DIRECTIVE_TOKEN.DW},
    {"dd", DATA_DIRECTIVE_TOKEN.DD},
    {"dq", DATA_DIRECTIVE_TOKEN.DQ},
    {"ddq", DATA_DIRECTIVE_TOKEN.DDQ},
    {"dt", DATA_DIRECTIVE_TOKEN.DT},
    {"resb", DATA_DIRECTIVE_TOKEN.RESB},
    {"resw", DATA_DIRECTIVE_TOKEN.RESW},
    {"resd", DATA_DIRECTIVE_TOKEN.RESD},
    {"resq", DATA_DIRECTIVE_TOKEN.RESQ},
  };

  public static Dictionary<string, DATA_DIRECTIVE_TOKEN> StringToType { get; }
  public DATA_DIRECTIVE_TOKEN DirectiveType { get; }

  static DataDirectiveToken() {
    StringToType = _stringToType;
  }

  public DataDirectiveToken(string value, int line)
  : base((int)TOKEN_TYPE.DATA_DIRECTIVE, (int)CHANNEL_TYPE.DEFAULT, value, line) {
    if(!StringToType.ContainsKey(value)) {
      throw new Exception("Invalid data directive string");
    }
    DirectiveType = StringToType[value];
  }
  
  public override string ToString() {
    string lineIndicator = "(" + Line + ")";
    string valueIndicator = "\"" + Value + "\"";
    string tokenTypeIndicator = "DataDirective: " + Enum.GetName(typeof(DATA_DIRECTIVE_TOKEN),DirectiveType);
    return string.Format("{0,6} {1,32} {2,32}", lineIndicator, valueIndicator, tokenTypeIndicator);
  }
}
