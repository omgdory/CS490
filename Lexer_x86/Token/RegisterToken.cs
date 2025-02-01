namespace Lexer_x86;

public class RegisterToken : Token {
  private static Dictionary<string, REGISTER_TYPE> _stringToType = new Dictionary<string, REGISTER_TYPE>() {
    {"al", REGISTER_TYPE.AL},
    {"ah", REGISTER_TYPE.AH},
    {"bl", REGISTER_TYPE.BL},
    {"bh", REGISTER_TYPE.BH},
    {"cl", REGISTER_TYPE.CL},
    {"ch", REGISTER_TYPE.CH},
    {"dl", REGISTER_TYPE.DL},
    {"dh", REGISTER_TYPE.DH},

    {"spl", REGISTER_TYPE.SPL},
    {"bpl", REGISTER_TYPE.BPL},
    {"sil", REGISTER_TYPE.SIL},
    {"dil", REGISTER_TYPE.DIL},
    {"r8b", REGISTER_TYPE.R8B},
    {"r9b", REGISTER_TYPE.R9B},
    {"r10b", REGISTER_TYPE.R10B},
    {"r11b", REGISTER_TYPE.R11B},
    {"r12b", REGISTER_TYPE.R12B},
    {"r13b", REGISTER_TYPE.R13B},
    {"r14b", REGISTER_TYPE.R14B},
    {"r15b", REGISTER_TYPE.R15B},

    {"ax", REGISTER_TYPE.AX},
    {"bx", REGISTER_TYPE.BX},
    {"cx", REGISTER_TYPE.CX},
    {"dx", REGISTER_TYPE.DX},
    {"sp", REGISTER_TYPE.SP},
    {"bp", REGISTER_TYPE.BP},
    {"si", REGISTER_TYPE.SI},
    {"di", REGISTER_TYPE.DI},
    {"r8w", REGISTER_TYPE.R8W},
    {"r9w", REGISTER_TYPE.R9W},
    {"r10w", REGISTER_TYPE.R10W},
    {"r11w", REGISTER_TYPE.R11W},
    {"r12w", REGISTER_TYPE.R12W},
    {"r13w", REGISTER_TYPE.R13W},
    {"r14w", REGISTER_TYPE.R14W},
    {"r15w", REGISTER_TYPE.R15W},

    {"eax", REGISTER_TYPE.EAX},
    {"ebx", REGISTER_TYPE.EBX},
    {"ecx", REGISTER_TYPE.ECX},
    {"edx", REGISTER_TYPE.EDX},
    {"esp", REGISTER_TYPE.ESP},
    {"ebp", REGISTER_TYPE.EBP},
    {"esi", REGISTER_TYPE.ESI},
    {"edi", REGISTER_TYPE.EDI},
    {"r8d", REGISTER_TYPE.R8D},
    {"r9d", REGISTER_TYPE.R9D},
    {"r10d", REGISTER_TYPE.R10D},
    {"r11d", REGISTER_TYPE.R11D},
    {"r12d", REGISTER_TYPE.R12D},
    {"r13d", REGISTER_TYPE.R13D},
    {"r14d", REGISTER_TYPE.R14D},
    {"r15d", REGISTER_TYPE.R15D},

    {"rax", REGISTER_TYPE.RAX},
    {"rbx", REGISTER_TYPE.RBX},
    {"rcx", REGISTER_TYPE.RCX},
    {"rdx", REGISTER_TYPE.RDX},
    {"rsp", REGISTER_TYPE.RSP},
    {"rbp", REGISTER_TYPE.RBP},
    {"rsi", REGISTER_TYPE.RSI},
    {"rdi", REGISTER_TYPE.RDI},
    {"r8", REGISTER_TYPE.R8},
    {"r9", REGISTER_TYPE.R9},
    {"r10", REGISTER_TYPE.R10},
    {"r11", REGISTER_TYPE.R11},
    {"r12", REGISTER_TYPE.R12},
    {"r13", REGISTER_TYPE.R13},
    {"r14", REGISTER_TYPE.R14},
    {"r15", REGISTER_TYPE.R15},
  };
  public static Dictionary<string, REGISTER_TYPE> StringToType { get; }
  public REGISTER_TYPE RegisterType { get; }

  static RegisterToken() {
    StringToType = _stringToType;
  }

  public RegisterToken(string value, int line)
  : base((int)TOKEN_TYPE.REGISTER, (int)CHANNEL_TYPE.DEFAULT, value, line) {
    if(!StringToType.ContainsKey(value)) {
      throw new Exception("Invalid register string");
    }
    RegisterType = StringToType[value];
  }
  
  public override string ToString() {
    return "(" + Line + ") Value: \"" + Value + "\"\tRegister: " + Enum.GetName(typeof(REGISTER_TYPE), RegisterType);
  }
}
