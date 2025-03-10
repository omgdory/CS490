namespace x86toCPP;

// Visitor class for visiting AST parse tree nodes
// functionality must be added -> abstract
public abstract class Visitor {
  private RootNode ParseTree { get; set; }
  private VisitorType Type { get; set; }

  public Visitor(RootNode parseTree, VisitorType type) {
    ParseTree = parseTree;
    Type = type;
  }

  // wrapper function with error checking to allow Visitor instance to be interacted with
  public void visit() {
    if (ParseTree == null) {
      throw new Exception($"FATAL: visitor pattern \"{Type}\" has a null parse tree reference.");
    }
    visitRoot();
  }

  // where the visitor pattern starts
  protected void visitRoot() {
    foreach(ASTNode child in ParseTree.Children) {
      // visit(child);
    }
  }

  // each node

}
