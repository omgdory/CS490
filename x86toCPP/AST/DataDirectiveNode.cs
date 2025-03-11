namespace x86toCPP;

public class DataDirectiveNode : ASTNode {

  string Identifier { get; set; }
  DATA_DIRECTIVE_TOKEN Directive { get; set; }

  public DataDirectiveNode(string identifier, DATA_DIRECTIVE_TOKEN directive, List<ASTNode> operands) {
    Identifier = identifier;
    Directive = directive;
    Children.AddRange(operands);
  }

  public override void accept(Visitor visitor) {
    visitor.visitDataDirective(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Data Directive: {Identifier}, {Directive}");
    PrintChildren(indent);
  }
}
