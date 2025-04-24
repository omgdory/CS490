namespace x86toCPP;

public class SymbolEntry<T>  {
  public string Name { get; }
  public SymbolType Type { get;}
  public T? Node { get; }
  public SymbolTable<T>? Scope { get; }
  public SymbolEntry(string name, SymbolType type, T node) {
    Name = name;
    Type = type;
    Node = node;
    Scope = null;
  }
  public SymbolEntry(string name, SymbolType type, SymbolTable<T> scope) {
    Name = name;
    Type = type;
    Node = default(T);
    Scope = scope;
  }
}
