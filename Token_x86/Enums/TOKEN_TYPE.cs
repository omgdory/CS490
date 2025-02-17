namespace Token_x86;

public enum TOKEN_TYPE {
  DEFAULT,
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
  EQU,
  DATA_DIRECTIVE,
  SEGMENT_IDENTIFIER,
  MACRO_ARGUMENT,
  MACRO_LABEL,
  BYTE,
  WORD,
  DWORD,
  QWORD,
}
