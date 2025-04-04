namespace x86toCPP;

public class MacroNode : ASTNode {
  public Token Token { get; }
  public string Identifier { get; }
  Token ArgumentsToken { get; set;}

  public MacroNode(Token token, List<ASTNode> followingInstructions, Token argumentsToken) {
    Token = token;
    Identifier = Token.Value;
    ArgumentsToken = argumentsToken;
    Children.AddRange(followingInstructions);
  }

  public override void accept(Visitor visitor) {
    visitor.visitMacro(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Macro: {Identifier}");
    PrintChildren(indent);
  }
}
