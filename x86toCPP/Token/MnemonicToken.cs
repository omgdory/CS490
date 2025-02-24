namespace x86toCPP;

public class MnemonicToken : Token {
  private static Dictionary<string, MNEMONIC_TOKEN> _stringToType = new Dictionary<string, MNEMONIC_TOKEN>() {
    {"mov", MNEMONIC_TOKEN.MOV},
    {"add", MNEMONIC_TOKEN.ADD},
    {"sub", MNEMONIC_TOKEN.SUB},
    {"mul", MNEMONIC_TOKEN.MUL},
    {"div", MNEMONIC_TOKEN.DIV},
    {"cmp", MNEMONIC_TOKEN.CMP},
    {"jmp", MNEMONIC_TOKEN.JMP},
    {"je", MNEMONIC_TOKEN.JE},
    {"jne", MNEMONIC_TOKEN.JNE},
    {"jl", MNEMONIC_TOKEN.JL},
    {"jle", MNEMONIC_TOKEN.JLE},
    {"jg", MNEMONIC_TOKEN.JG},
    {"jge", MNEMONIC_TOKEN.JGE},
    {"jb", MNEMONIC_TOKEN.JB},
    {"jbe", MNEMONIC_TOKEN.JBE},
    {"ja", MNEMONIC_TOKEN.JA},
    {"jae", MNEMONIC_TOKEN.JAE},
    {"call", MNEMONIC_TOKEN.CALL},
    {"ret", MNEMONIC_TOKEN.RET},
    {"push", MNEMONIC_TOKEN.PUSH},
    {"pop", MNEMONIC_TOKEN.POP},
    {"and", MNEMONIC_TOKEN.AND},
    {"or", MNEMONIC_TOKEN.OR},
    {"xor", MNEMONIC_TOKEN.XOR},
    {"nop", MNEMONIC_TOKEN.NOP},
    {"inc", MNEMONIC_TOKEN.INC},
    {"dec", MNEMONIC_TOKEN.DEC},
    {"shl", MNEMONIC_TOKEN.SHL},
    {"shr", MNEMONIC_TOKEN.SHR},
    {"syscall", MNEMONIC_TOKEN.SYSCALL},
    {"%macro", MNEMONIC_TOKEN.MACRO_START},
    {"%endmacro", MNEMONIC_TOKEN.MACRO_END},
  };
  public static Dictionary<string, MNEMONIC_TOKEN> StringToType { get; }
  public MNEMONIC_TOKEN MnemonicType { get; }
  
  static MnemonicToken() {
    StringToType = _stringToType;
  }

  public MnemonicToken(string value, int line)
  : base((int)TOKEN_TYPE.MNEMONIC, (int)CHANNEL_TYPE.DEFAULT, value, line) {
    if(!StringToType.ContainsKey(value)) {
      throw new Exception("Invalid mnemonic string");
    }
    MnemonicType = StringToType[value];
  }
  
  public override string ToString() {
    string lineIndicator = "(" + Line + ")";
    string valueIndicator = "\"" + Value + "\"";
    string tokenTypeIndicator = "Mnemonic: " + Enum.GetName(typeof(MNEMONIC_TOKEN), MnemonicType);
    return string.Format("{0,6} {1,32} {2,32}", lineIndicator, valueIndicator, tokenTypeIndicator);
  }
}
