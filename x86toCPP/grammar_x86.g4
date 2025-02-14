grammar X86;

program: (label | instruction | directive)? comment? newline* program? EOF;

label: identifier ':' ;

instruction: mnemonic operand_list? ; // not all mnemonics need operands

mnemonic: 
    'mov'  | 'add'  | 'sub'  | 'mul'  | 'div'  | 'cmp'  | 'jmp'  | 'je'   | 'jne'
  | 'jl'   | 'jle'  | 'jg'   | 'jge'  | 'jb'   | 'jbe'  | 'ja'   | 'jae'  | 'call'
  | 'ret'  | 'push' | 'pop'  | 'and'  | 'or'   | 'xor'  | 'nop'  | 'inc'  | 'dec'
  | 'shl'  | 'shr'  | 'syscall'
  ;

operand_list: operand (',' operand)* ;

operand: register | immediate | memory ;

register: 
    'al'  | 'bl'  | 'cl'  | 'dl'  | 'sil' | 'dil' | 'bpl' | 'spl'
  | 'r8b' | 'r9b' | 'r10b' | 'r11b' | 'r12b' | 'r13b' | 'r14b' | 'r15b'
  | 'ax'  | 'bx'  | 'cx'  | 'dx'  | 'si'  | 'di'  | 'bp'  | 'sp'
  | 'r8w' | 'r9w' | 'r10w' | 'r11w' | 'r12w' | 'r13w' | 'r14w' | 'r15w'
  | 'eax' | 'ebx' | 'ecx' | 'edx' | 'esi'  | 'edi'  | 'ebp'  | 'esp'
  | 'r8d' | 'r9d' | 'r10d' | 'r11d' | 'r12d' | 'r13d' | 'r14d' | 'r15d'
  | 'rax' | 'rbx' | 'rcx' | 'rdx' | 'rsi' | 'rdi' | 'rbp' | 'rsp'
  | 'r8'  | 'r9'  | 'r10' | 'r11' | 'r12' | 'r13' | 'r14' | 'r15'
  ;

memory: '[' (register | (register '+' register ('*' scale)?) | immediate) ']' ;

scale: ('1' | '2' | '4' | '8');

immediate: number | identifier ;

number: ('-'? DIGIT+ | '0x' HEX_DIGIT+) ;

directive: data_directive | segment_directive | other_directive ;

data_directive: identifier ('db' | 'dw' | 'dd' | 'dq' | 'ddq' | 'dt' | 'resb' | 'resw' | 'resd' | 'resq') operand_list ;
segment_directive: 'section' ('.code' | '.data' | '.bss' | '.text' | '.stack') ;
other_directive:
      'org' INTEGER
    | 'equ' operand
    | 'align' INTEGER
    | 'include' string
    | '%macro' identifier macro_body '%endmacro'
    ;

macro_body: (instruction | directive | label)* ;

letter: 'A' | 'B' | 'C' | 'D' | 'E' | 'F' | 'G' | 'H' | 'I' | 'J' | 'K' | 'L' |
        'M' | 'N' | 'O' | 'P' | 'Q' | 'R' | 'S' | 'T' | 'U' | 'V' | 'W' | 'X' | 'Y' | 'Z' |
        'a' | 'b' | 'c' | 'd' | 'e' | 'f' | 'g' | 'h' | 'i' | 'j' | 'k' | 'l' |
        'm' | 'n' | 'o' | 'p' | 'q' | 'r' | 's' | 't' | 'u' | 'v' | 'w' | 'x' | 'y' | 'z' ;

identifier: (letter | '_') (letter | DIGIT | '_')* ;

string: '"' (letter)* '"' ; // everything except quotation

character: letter | DIGIT | special_character ;

special_character: '!' | '@' | '#' | '$' | '%' | '^' | '&' | '*' | '(' | ')'
                |  '-' | '_' | '+' | '=' | '{' | '}' | '[' | ']' | ';'
                |  ':' | '\'' | ',' | '.' | '/' | '?' | '\\' | '|' ;


HEX_DIGIT: (DIGIT | 'A' | 'B' | 'C' | 'D' | 'E' | 'F' | 'a' | 'b' | 'c' | 'd' | 'e' | 'f') ;

DIGIT: ('0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9') ;

INTEGER: DIGIT+ ;

// https://stackoverflow.com/questions/7070763/parse-comment-line
comment:  ';' ~( '\r' | '\n' )* ;

newline: ('\n' | '\r\n') ;
