namespace x86toCPP;

public enum TOKEN_TYPE {
  DEFAULT,
  EOF,
  MNEMONIC,
  REGISTER,
  NUMBER,
  HEX_NUMBER,
  ERROR,
  LABEL,
  COMMENT,
  IDENTIFIER,
  WHITESPACE,
  NEWLINE,
  SPECIAL_CHAR,
  COLON,
  OPEN_BRACK,
  CLOSE_BRACK,
  COMMA,
  GLOBAL,
  SECTION,
  MACRO_START,
  MACRO_END,
  DATA_DIRECTIVE,
  SEGMENT_IDENTIFIER,
  MACRO_ARGUMENT,
  MACRO_LABEL,
  BYTE,
  WORD,
  DWORD,
  QWORD,
}
