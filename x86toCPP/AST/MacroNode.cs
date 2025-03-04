namespace x86toCPP;

public class MacroNode : ASTNode {
  Token Token { get; set;}
  string Identifier { get; set; }
  Token ArgumentsToken { get; set;}

  public MacroNode(Token token, List<ASTNode> followingInstructions, Token argumentsToken) {
    Token = token;
    Identifier = Token.Value;
    ArgumentsToken = argumentsToken;
    Children.AddRange(followingInstructions);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Macro: {Identifier}");
    PrintChildren(indent);
  }
}
