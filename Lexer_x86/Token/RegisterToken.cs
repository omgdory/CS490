namespace Lexer_x86;

public class RegisterToken : Token {
  private static Dictionary<string, REGISTER_TOKEN> _stringToType = new Dictionary<string, REGISTER_TOKEN>() {
    {"al", REGISTER_TOKEN.AL},
    {"ah", REGISTER_TOKEN.AH},
    {"bl", REGISTER_TOKEN.BL},
    {"bh", REGISTER_TOKEN.BH},
    {"cl", REGISTER_TOKEN.CL},
    {"ch", REGISTER_TOKEN.CH},
    {"dl", REGISTER_TOKEN.DL},
    {"dh", REGISTER_TOKEN.DH},

    {"spl", REGISTER_TOKEN.SPL},
    {"bpl", REGISTER_TOKEN.BPL},
    {"sil", REGISTER_TOKEN.SIL},
    {"dil", REGISTER_TOKEN.DIL},
    {"r8b", REGISTER_TOKEN.R8B},
    {"r9b", REGISTER_TOKEN.R9B},
    {"r10b", REGISTER_TOKEN.R10B},
    {"r11b", REGISTER_TOKEN.R11B},
    {"r12b", REGISTER_TOKEN.R12B},
    {"r13b", REGISTER_TOKEN.R13B},
    {"r14b", REGISTER_TOKEN.R14B},
    {"r15b", REGISTER_TOKEN.R15B},

    {"ax", REGISTER_TOKEN.AX},
    {"bx", REGISTER_TOKEN.BX},
    {"cx", REGISTER_TOKEN.CX},
    {"dx", REGISTER_TOKEN.DX},
    {"sp", REGISTER_TOKEN.SP},
    {"bp", REGISTER_TOKEN.BP},
    {"si", REGISTER_TOKEN.SI},
    {"di", REGISTER_TOKEN.DI},
    {"r8w", REGISTER_TOKEN.R8W},
    {"r9w", REGISTER_TOKEN.R9W},
    {"r10w", REGISTER_TOKEN.R10W},
    {"r11w", REGISTER_TOKEN.R11W},
    {"r12w", REGISTER_TOKEN.R12W},
    {"r13w", REGISTER_TOKEN.R13W},
    {"r14w", REGISTER_TOKEN.R14W},
    {"r15w", REGISTER_TOKEN.R15W},

    {"eax", REGISTER_TOKEN.EAX},
    {"ebx", REGISTER_TOKEN.EBX},
    {"ecx", REGISTER_TOKEN.ECX},
    {"edx", REGISTER_TOKEN.EDX},
    {"esp", REGISTER_TOKEN.ESP},
    {"ebp", REGISTER_TOKEN.EBP},
    {"esi", REGISTER_TOKEN.ESI},
    {"edi", REGISTER_TOKEN.EDI},
    {"r8d", REGISTER_TOKEN.R8D},
    {"r9d", REGISTER_TOKEN.R9D},
    {"r10d", REGISTER_TOKEN.R10D},
    {"r11d", REGISTER_TOKEN.R11D},
    {"r12d", REGISTER_TOKEN.R12D},
    {"r13d", REGISTER_TOKEN.R13D},
    {"r14d", REGISTER_TOKEN.R14D},
    {"r15d", REGISTER_TOKEN.R15D},

    {"rax", REGISTER_TOKEN.RAX},
    {"rbx", REGISTER_TOKEN.RBX},
    {"rcx", REGISTER_TOKEN.RCX},
    {"rdx", REGISTER_TOKEN.RDX},
    {"rsp", REGISTER_TOKEN.RSP},
    {"rbp", REGISTER_TOKEN.RBP},
    {"rsi", REGISTER_TOKEN.RSI},
    {"rdi", REGISTER_TOKEN.RDI},
    {"r8", REGISTER_TOKEN.R8},
    {"r9", REGISTER_TOKEN.R9},
    {"r10", REGISTER_TOKEN.R10},
    {"r11", REGISTER_TOKEN.R11},
    {"r12", REGISTER_TOKEN.R12},
    {"r13", REGISTER_TOKEN.R13},
    {"r14", REGISTER_TOKEN.R14},
    {"r15", REGISTER_TOKEN.R15},
  };
  public static Dictionary<string, REGISTER_TOKEN> StringToType { get; }
  public REGISTER_TOKEN RegisterType { get; }

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
    return "(" + Line + ") Value: \"" + Value + "\"\tRegister: " + Enum.GetName(typeof(REGISTER_TOKEN), RegisterType);
  }
}
