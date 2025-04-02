namespace x86toCPP;

public class SymbolEntry {
  public string Name { get; }
  public SymbolType Type { get;}
  public ASTNode? Node { get; }
  public SymbolTable? Scope { get; }
  public SymbolEntry(string name, SymbolType type, ASTNode node) {
    Name = name;
    Type = type;
    Node = node;
    Scope = null;
  }
  public SymbolEntry(string name, SymbolType type, SymbolTable scope) {
    Name = name;
    Type = type;
    Node = null;
    Scope = scope;
  }
}
