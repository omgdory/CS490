namespace x86toCPP;

public class Parser {
  public bool IgnoreComments { get; set; }
  public RootNode SyntaxTreeRoot { get; }
  public List<Token> Tokens { get; set; }
  
  private int tokensParsed = 0;
  private int tokenCount;
  
  public Parser(List<Token> tokens) {
    IgnoreComments = false;
    SyntaxTreeRoot = new RootNode();
    Tokens = tokens;
    tokenCount = Tokens.Count;
    SyntaxTreeRoot = ParseTokens();
  }

  // should build the tree and return its root
  private RootNode ParseTokens() {
    RootNode result = new RootNode();
    while(tokensParsed < tokenCount &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF) {
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE) {
        tokensParsed++;
        continue;
      }
      result.Children.Add(ParseSection());
      Console.WriteLine("Section Parsed");
    } 
    return result;
  }

  private SegmentNode ParseSection() {
    List<ASTNode> children = new List<ASTNode>();
    SEGMENT_IDENTIFIER_TOKEN segmentIdentifier;
    ParseSectionKeyword();
    segmentIdentifier = ParseSegmentIdentifier();
    // newline expected
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.NEWLINE) {
      throw new Exception($"Newline expected: {Tokens[tokensParsed].Line}");
    }
    tokensParsed++;
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.SECTION) {
        // Console.WriteLine("(" + segmentIdentifier + ") " + Tokens[tokensParsed].TokenType + " : " + (int)TOKEN_TYPE.SECTION);
        switch(segmentIdentifier) {
          case SEGMENT_IDENTIFIER_TOKEN.DATA_SEGMENT_IDENTIFIER:
            if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE ||
            Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.EOF) {
              tokensParsed++;
              break;
            }
            children.Add(ParseDataDirective());
            break;
          case SEGMENT_IDENTIFIER_TOKEN.TEXT_SEGMENT_IDENTIFIER:
          if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE ||
            Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.EOF) {
            tokensParsed++;
            break;
          }
            children.Add(ParseInstruction());
            break;
          default:
            // children.Add(ParseProgram());
            break;
        }
        if(tokensParsed >= tokenCount) break;
    }
    return new SegmentNode(segmentIdentifier, children);
  }

  private MnemonicNode ParseInstruction() {
    // must be -> mnemonic, operand list
    // mnemonic
    MNEMONIC_TOKEN opcode = MNEMONIC_TOKEN.DEFAULT;
    List<ASTNode> operands = new List<ASTNode>();
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MNEMONIC) {
      throw new Exception($"Mnemonic expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\": {Tokens[tokensParsed].Line}");
    }
    if(Tokens[tokensParsed] is MnemonicToken t) {
      tokensParsed++;
      opcode = t.MnemonicType;
    }
    // operand
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.NEWLINE &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF) {
      ParsedTokensCountValid();
      operands.Add(new DefaultNode(Tokens[tokensParsed]));
      tokensParsed++;
    }
    return new MnemonicNode(opcode, operands);
  }

  private void ParseSectionKeyword() {
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.SECTION) {
      throw new Exception($"Section keyword missing: {Tokens[tokensParsed].Line}");
    }
    tokensParsed++;
  }

  private SEGMENT_IDENTIFIER_TOKEN ParseSegmentIdentifier() {
   if(Tokens[tokensParsed] is SegmentIdentifierToken t) {
      tokensParsed++; 
      return t.IdentifierType;
    }
    throw new Exception($"Segment identifier invalid: {Tokens[tokensParsed].Line}");
  }

  private DataDirectiveNode ParseDataDirective() {
    // must be IDENTIFIER -> DATA_DIRECTIVE -> operand list
    // identifier
    string identifier;
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.IDENTIFIER) {
      throw new Exception($"Identifier expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\": {Tokens[tokensParsed].Line}");
    }
    identifier = new DefaultNode(Tokens[tokensParsed]).Token.Value;
    tokensParsed++;
    // data directive
    DATA_DIRECTIVE_TOKEN directive;
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.DATA_DIRECTIVE &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EQU) {
      throw new Exception($"Directive expected, got \"{Tokens[tokensParsed].Value}\": {Tokens[tokensParsed].Line}");
    }
    if(Tokens[tokensParsed] is DataDirectiveToken t) {
      tokensParsed++;
      directive = t.DirectiveType;
    } else {
      directive = DATA_DIRECTIVE_TOKEN.DEFAULT;
    }
    // operand list
    List<ASTNode> operands = new List<ASTNode>();
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.NEWLINE &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF) {
      ParsedTokensCountValid();
      operands.Add(new DefaultNode(Tokens[tokensParsed]));
      tokensParsed++;
    }
    // skip newline or eof
    // tokensParsed++;
    return new DataDirectiveNode(identifier, directive, operands);
  }

  private void ParsedTokensCountValid() {
    if(tokensParsed >= tokenCount) {
      throw new Exception($"Tokens parsed overflowed: {tokensParsed}");
    }
  }
  private void SkipNewlines() {
    ParsedTokensCountValid();
    while(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE) {
      tokensParsed++;
      ParsedTokensCountValid();
    }
  }
}
