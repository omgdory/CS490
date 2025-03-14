using System.Globalization;
using System.Runtime.InteropServices;

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
      switch(Tokens[tokensParsed].TokenType) {
        case (int)TOKEN_TYPE.SECTION:
          SegmentNode currSection = ParseSection(); 
          result.Children.Add(currSection);
          Console.WriteLine($"Section {currSection.SegmentIdentifier} Parsed");
          break;
        case (int)TOKEN_TYPE.MACRO_START:
          MacroNode currMacro = ParseMacro();
          result.Children.Add(currMacro);
          break;
        default:
          throw new Exception($"FATAL: Unhandled parse at token {tokensParsed}\nGot token type {Tokens[tokensParsed].TokenType}");
      }
    } 
    return result;
  }

  private MacroNode ParseMacro() {
    // macro_label -> identifier -> number -> newline
    CheckToken(TOKEN_TYPE.MACRO_START, CreateExceptionString_Expected("Macro"));
    tokensParsed++;
    CheckToken(TOKEN_TYPE.IDENTIFIER, CreateExceptionString_Expected("Identifier"));
    Token identifierToken = Tokens[tokensParsed];
    tokensParsed++;
    CheckToken(TOKEN_TYPE.NUMBER, CreateExceptionString_Expected("Number"));
    Token argumentsToken = Tokens[tokensParsed];
    tokensParsed++;
    CheckToken(TOKEN_TYPE.NEWLINE, CreateExceptionString_Expected("Newline"));
    tokensParsed++;
    List<ASTNode> body = new List<ASTNode>();
    while(tokensParsed < tokenCount &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MACRO_END) {
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.EOF) {
        throw new Exception($"({Tokens[tokensParsed].Line}) Macro unterminated.");
      }
      HandleTextSegment(ref body);
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.MACRO_END)
        break;
      // tokensParsed++;
    }
    // macro end
    tokensParsed++;
    return new MacroNode(identifierToken, body, argumentsToken);
  }

  private string CreateExceptionString_Expected(string expected) {
    return $"({Tokens[tokensParsed].Line}) " + expected + $" expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\"";
  }

  private void CheckToken(TOKEN_TYPE tt, string exceptionString) {
    ParsedTokensCountValid();
    if(Tokens == null) {
      throw new Exception($"FATAL: Tokens list is null. Tokens parsed = {tokensParsed}");
    }
    if(Tokens[tokensParsed].TokenType != (int)tt) {
      throw new Exception(exceptionString);
    }
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
      HandleSection(segmentIdentifier, ref children);
      if(tokensParsed >= tokenCount) break;
    }
    return new SegmentNode(segmentIdentifier, children);
  }

  private void HandleSection(SEGMENT_IDENTIFIER_TOKEN segmentIdentifier, ref List<ASTNode> children) {
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
        HandleTextSegment(ref children);
        break;
      default:
        // children.Add(ParseProgram());
        break;
    }
  }

  private void HandleTextSegment(ref List<ASTNode> children) {
    // lookahead
    switch(Tokens[tokensParsed].TokenType) {
      // if mnemonic, then add a phantom label as parent node
      case (int)TOKEN_TYPE.MNEMONIC:
        children.Add(ParseLabel(false));
        break;
      // skip new lines and EOF
      case (int)TOKEN_TYPE.NEWLINE:
      case (int)TOKEN_TYPE.EOF:
        tokensParsed++;
        break;
      // specially handle items that are not under labels
      case (int)TOKEN_TYPE.GLOBAL:
        children.Add(ParseGlobalDeclarator());
        break;
      // everything else should be under a label here
      case (int)TOKEN_TYPE.MACRO_LABEL:
      case (int)TOKEN_TYPE.LABEL:
        children.Add(ParseLabel(true));
        break;
      // throw exception for unaccounted/unknown tokens
      default:
        throw new Exception($"({Tokens[tokensParsed].Line}) Unhandled text segment object: {(TOKEN_TYPE)Tokens[tokensParsed].TokenType}");
    }
  }

  private GlobalDeclaratorNode ParseGlobalDeclarator() {
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.GLOBAL) {
      throw new Exception($"({Tokens[tokensParsed].Line}) Global declarator expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\"");
    }
    Token globalToken = Tokens[tokensParsed];
    tokensParsed++;
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.IDENTIFIER) {
      throw new Exception($"({Tokens[tokensParsed].Line}) Identifier expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\"");
    }
    Token identifierToken = Tokens[tokensParsed];
    tokensParsed++;
    return new GlobalDeclaratorNode(globalToken, identifierToken);
  }

  private MnemonicNode ParseInstruction() {
    // must be -> mnemonic, operand list
    // mnemonic
    Token token = new Token();
    MNEMONIC_TOKEN opcode = MNEMONIC_TOKEN.DEFAULT;
    List<ASTNode> operands = new List<ASTNode>();
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MNEMONIC &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.IDENTIFIER) {
      throw new Exception($"Mnemonic or (macro) identifier expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\": {Tokens[tokensParsed].Line}");
    }
    if(Tokens[tokensParsed] is MnemonicToken t) {
      tokensParsed++;
      token = t;
      opcode = t.MnemonicType;
    }
    // operand
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.NEWLINE &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF) {
      ParsedTokensCountValid();
      // skip commas for operands
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.COMMA) {
        tokensParsed++;
        continue;
      }
      // handle memory access
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.BYTE ||
      Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.WORD ||
      Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.DWORD ||
      Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.QWORD) {
        operands.Add(ParseMemoryAccess());
        // operands.Add(new OperandNode(new Token(0, 0, "mem access", Tokens[tokensParsed].Line)));
        tokensParsed++;
        continue;
      }
      // otherwise, normal operand
      operands.Add(new OperandNode(Tokens[tokensParsed]));
      tokensParsed++;
    }
    return new MnemonicNode(token, opcode, operands);
  }

  private MemoryAccessNode ParseMemoryAccess() {
    // access (dword)
    Token accessTypeToken = Tokens[tokensParsed];
    tokensParsed++;
    ParsedTokensCountValid();
    // [
    CheckToken(TOKEN_TYPE.OPEN_BRACK, CreateExceptionString_Expected("Open bracket '['"));
    tokensParsed++;
    ParsedTokensCountValid();
    // address (rbx)
    Token address = Tokens[tokensParsed];
    tokensParsed++;
    ParsedTokensCountValid();
    // if no offset, then just set them to default tokens
    if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.CLOSE_BRACK) {
      return new MemoryAccessNode(accessTypeToken, address, new Token(), new Token());
    }
    // offset operation (must be addition)
    if(Tokens[tokensParsed].Value != "+") {
      throw new Exception($"({Tokens[tokensParsed].Line}) Plus sign (\"+\") expected, got {Tokens[tokensParsed].Value}.");
    } else {
      tokensParsed++;
    }
    // offset itself
    ParsedTokensCountValid();
    Token offset = Tokens[tokensParsed];
    tokensParsed++;
    ParsedTokensCountValid();
    // if no offset multiplier, then just set to default token
    if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.CLOSE_BRACK) {
      return new MemoryAccessNode(accessTypeToken, address, offset, new Token());
    }
    // offset multiplier operation (must be multiplication)
    if(Tokens[tokensParsed].Value != "*") {
      throw new Exception($"({Tokens[tokensParsed].Line}) Multiplication sign (\"*\") expected, got {Tokens[tokensParsed].Value}.");
    } else {
      tokensParsed++;
    }
    // offset multiplier itself
    ParsedTokensCountValid();
    Token offsetMultiplier = Tokens[tokensParsed];
    tokensParsed++;
    // must be closed now
    CheckToken(TOKEN_TYPE.CLOSE_BRACK, CreateExceptionString_Expected("Close bracket ']'"));
    tokensParsed++;
    return new MemoryAccessNode(accessTypeToken, address, offset, offsetMultiplier);
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

  private LabelNode ParseLabel(bool labelExpected) {
    ParsedTokensCountValid();
    Token labelToken = new Token((int)TOKEN_TYPE.LABEL, (int)CHANNEL_TYPE.DEFAULT, "__DEFAULT__", Tokens[tokensParsed].Line);
    if(labelExpected) {
      if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.LABEL &&
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MACRO_LABEL) {
        throw new Exception($"Label expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\": {Tokens[tokensParsed].Line}");
      }
      labelToken = Tokens[tokensParsed];
      tokensParsed++;
    }
    List<ASTNode> followingInstructions = new List<ASTNode>();
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MACRO_END &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.LABEL &&
    Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.MACRO_LABEL) {
      // handle newlines if necessary
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE) {
        tokensParsed++;
        continue;
      }
      followingInstructions.Add(ParseInstruction());
      if(tokensParsed >= tokenCount) break;
    }
    return new LabelNode(labelToken, followingInstructions);
  }

  private DataDirectiveNode ParseDataDirective() {
    // must be IDENTIFIER -> DATA_DIRECTIVE -> operand list
    // identifier
    Token identifierToken;
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.IDENTIFIER) {
      throw new Exception($"Identifier expected, got \"{(TOKEN_TYPE)Tokens[tokensParsed].TokenType}\": {Tokens[tokensParsed].Line}");
    }
    identifierToken = Tokens[tokensParsed];
    tokensParsed++;
    // data directive
    DATA_DIRECTIVE_TOKEN directive;
    ParsedTokensCountValid();
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.DATA_DIRECTIVE) {
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
      // skip commas for operands
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.COMMA) {
        tokensParsed++;
        continue;
      }
      operands.Add(new OperandNode(Tokens[tokensParsed]));
      tokensParsed++;
    }
    // skip newline or eof
    // tokensParsed++;
    return new DataDirectiveNode(identifierToken, directive, operands);
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
