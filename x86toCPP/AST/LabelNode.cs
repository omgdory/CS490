namespace x86toCPP;

public class LabelNode : ASTNode {
  Token Token { get; set;}
  string Identifier { get; set; }

  public LabelNode(Token token, List<ASTNode> followingInstructions) {
    Token = token;
    Identifier = Token.Value;
    Children.AddRange(followingInstructions);
  }

  public override void accept(Visitor visitor) {
    visitor.visitLabel(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Label: {Identifier}, line {Token.Line}");
    PrintChildren(indent);
  }
}
