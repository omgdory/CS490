using Token_x86;

namespace x86toCPP;

public class Parser {
  public bool IgnoreComments { get; set; }
  public RootNode SyntaxTreeRoot { get; }
  public Parser() {
    IgnoreComments = false;
    SyntaxTreeRoot = new RootNode();
  }

  // should build the tree and return its root
  public ASTNode ParseTokens(List<Token> tokens) {
    int n = tokens.Count;
    int counter = 0;
    while(counter < n) {

    } 
    List<ASTNode> lst = new List<ASTNode>() {
      new OperandNode("rax", TOKEN_TYPE.REGISTER),
      new OperandNode("2", TOKEN_TYPE.NUMBER),
    };
    return new MnemonicNode(MNEMONIC_TOKEN.ADD, lst);
  }
}
