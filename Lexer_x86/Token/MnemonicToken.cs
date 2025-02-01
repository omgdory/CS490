namespace Lexer_x86;

public class MnemonicToken : Token {
  private static Dictionary<string, MNEMONIC_TYPE> _stringToType = new Dictionary<string, MNEMONIC_TYPE>() {
    {"mov", MNEMONIC_TYPE.MOV},
    {"add", MNEMONIC_TYPE.ADD},
    {"sub", MNEMONIC_TYPE.SUB},
    {"mul", MNEMONIC_TYPE.MUL},
    {"div", MNEMONIC_TYPE.DIV},
    {"cmp", MNEMONIC_TYPE.CMP},
    {"jmp", MNEMONIC_TYPE.JMP},
    {"je", MNEMONIC_TYPE.JE},
    {"jne", MNEMONIC_TYPE.JNE},
    {"jl", MNEMONIC_TYPE.JL},
    {"jle", MNEMONIC_TYPE.JLE},
    {"jg", MNEMONIC_TYPE.JG},
    {"jge", MNEMONIC_TYPE.JGE},
    {"jb", MNEMONIC_TYPE.JB},
    {"jbe", MNEMONIC_TYPE.JBE},
    {"ja", MNEMONIC_TYPE.JA},
    {"jae", MNEMONIC_TYPE.JAE},
    {"call", MNEMONIC_TYPE.CALL},
    {"ret", MNEMONIC_TYPE.RET},
    {"push", MNEMONIC_TYPE.PUSH},
    {"pop", MNEMONIC_TYPE.POP},
    {"and", MNEMONIC_TYPE.AND},
    {"or", MNEMONIC_TYPE.OR},
    {"xor", MNEMONIC_TYPE.XOR},
    {"nop", MNEMONIC_TYPE.NOP},
    {"inc", MNEMONIC_TYPE.INC},
    {"dec", MNEMONIC_TYPE.DEC},
    {"shl", MNEMONIC_TYPE.SHL},
    {"shr", MNEMONIC_TYPE.SHR},
    {"syscall", MNEMONIC_TYPE.SYSCALL},
  };
  public static Dictionary<string, MNEMONIC_TYPE> StringToType { get; }
  public MNEMONIC_TYPE MnemonicType { get; }
  
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
    return "(" + Line + ") Value: \"" + Value + "\"\tMnemonic: " + Enum.GetName(typeof(MNEMONIC_TYPE), MnemonicType);
  }
}
