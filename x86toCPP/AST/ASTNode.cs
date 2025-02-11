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
