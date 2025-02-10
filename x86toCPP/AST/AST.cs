namespace x86toCPP;

public abstract class AST {
  // TODO: review AST from CS 460
  private List<AST> children;

  public AST() {
    children = new List<AST>();
  }
}
