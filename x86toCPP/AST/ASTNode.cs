using Token_x86;

namespace x86toCPP;

public abstract class ASTNode {
  public List<ASTNode> Children { get; }

  public ASTNode() {
    Children = new List<ASTNode>();
  }

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

  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Token: {Token.Value} (DEFAULT)");
    PrintChildren(indent);
  }
}
