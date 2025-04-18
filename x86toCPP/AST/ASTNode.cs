namespace x86toCPP;

public abstract class ASTNode {
  public List<ASTNode> Children { get; }

  public ASTNode() {
    Children = new List<ASTNode>();
  }

  // handling visitor pattern
  public abstract void accept(Visitor visitor);

  // for outputting to the terminal
  public abstract void Print(int indent = 0);
  // only call from within (children too)
  protected void PrintChildren(int indent) {
    foreach (var child in Children) {
      child.Print(indent + 2);
    }
  }
}

public class RootNode : ASTNode {
  public override void accept(Visitor visitor) {
    visitor.visitRoot(this);
  }

  public override void Print(int indent = 0) {
    // indent must be 0, just setting as param so it compiles
    Console.WriteLine("Syntax Tree (Root):");
    PrintChildren(0);
  }
}

public class DefaultNode : ASTNode {
  public Token Token { get; set; }

  public DefaultNode(Token token) {
    Token = token;
  }

  public override void accept(Visitor visitor) {
    visitor.visitDefault(this);
  }

  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Token: {Token.Value}");
    PrintChildren(indent);
  }
}
