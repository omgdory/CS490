namespace x86toCPP;

public class SymbolEntry {
  public string Name { get; }
  public ASTNode? Node { get; }
  public SymbolTable? Scope { get; }
  public SymbolEntry(string name, ASTNode node) {
    Name = name;
    Node = node;
    Scope = null;
  }
  public SymbolEntry(string name, SymbolTable scope) {
    Name = name;
    Node = null;
    Scope = scope;
  }
}
