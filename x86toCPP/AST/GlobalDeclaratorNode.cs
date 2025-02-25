namespace x86toCPP;

public class GlobalDeclaratorNode : ASTNode {
  Token GlobalToken { get; set;}
  Token TargetToken { get; set; }

  public GlobalDeclaratorNode(Token globalToken, Token targetToken) {
    GlobalToken = globalToken;
    TargetToken = targetToken;
    Children.Clear();
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Global: {GlobalToken.Value} , {TargetToken.Value}");
    PrintChildren(indent);
  }
}