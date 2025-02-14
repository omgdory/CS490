using System.Xml.Serialization;
using Token_x86;

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
  public RootNode ParseTokens() {
    RootNode result = new RootNode();
    while(tokensParsed < tokenCount) {
      if(Tokens[tokensParsed].TokenType == (int)TOKEN_TYPE.NEWLINE) {
        tokensParsed++;
        continue;
      }
      result.Children.Add(ParseSection());
    } 
    return result;
  }

  public SegmentNode ParseSection() {
    List<ASTNode> children = new List<ASTNode>();
    SEGMENT_IDENTIFIER_TOKEN segmentIdentifier;
    ParseSectionKeyword();
    segmentIdentifier = ParseSegmentIdentifier();
    while(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.EOF ||
      Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.SECTION) {
        children.Add(new OperandNode("rax", TOKEN_TYPE.REGISTER));
    }
    return new SegmentNode(segmentIdentifier, children);
  }

  public void ParseSectionKeyword() {
    if(Tokens[tokensParsed].TokenType != (int)TOKEN_TYPE.SECTION) {
      throw new Exception("Section keyword missing");
    }
    tokensParsed++;
  }

  public SEGMENT_IDENTIFIER_TOKEN ParseSegmentIdentifier() {
   if(Tokens[tokensParsed] is SegmentIdentifierToken t) {
      tokensParsed++; 
      return t.IdentifierType;
    }
    throw new Exception("Segment identifier invalid");
  }
}
