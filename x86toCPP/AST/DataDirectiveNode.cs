namespace x86toCPP;

public class DataDirectiveNode : ASTNode {

  public Token IdentifierToken { get; private set; }
  public DATA_DIRECTIVE_TOKEN Directive { get; private set; }

  public DataDirectiveNode(Token identifier, DATA_DIRECTIVE_TOKEN directive, List<ASTNode> operands) {
    IdentifierToken = identifier;
    Directive = directive;
    Children.AddRange(operands);
  }

  public override void accept(Visitor visitor) {
    visitor.visitDataDirective(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Data Directive: {IdentifierToken.Value}, {Directive}");
    PrintChildren(indent);
  }
}
