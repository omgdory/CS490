using x86toCPP;

public class SymbolTable<T>
{
  private Dictionary<string, SymbolEntry<T>> Symbols;
  private SymbolTable<T>? Parent;
  private string Name;

  public SymbolTable(string name, SymbolTable<T>? parent = null) {
    Symbols = new Dictionary<string, SymbolEntry<T>>();
    Parent = parent;
    Name = name;
  }

  /// <summary>
  /// Adds a new symbol entry to the current scope. Throws exception if already exists in the table.
  /// </summary>
  /// <param name="name">The name of the symbol.</param>
  /// <param name="symbol">The symbol entry to be added.</param>
  /// <exception cref="Exception">Thrown if the symbol already exists in the current scope.</exception>
  public void AddSymbol(SymbolEntry<T> symbol) {
    if (Symbols.ContainsKey(symbol.Name)) {
      throw new Exception($"Symbol '{symbol.Name}' already exists in the current scope.");
    }
    Symbols[symbol.Name] = symbol;
  }

  /// <summary>
  /// Searches for a symbol by name, starting from the current scope and moving up to parent scopes if necessary.
  /// </summary>
  /// <param name="name">The name of the symbol to find.</param>
  /// <returns>The found <see cref="SymbolEntry{T}"/>, or null if not found.</returns>
  public SymbolEntry<T>? FindSymbol(string name) {
    if (Symbols.TryGetValue(name, out var symbol)) {
      return symbol;
    }
    return Parent?.FindSymbol(name);
  }

  public bool Exists(string name) {
    if (Symbols.TryGetValue(name, out var value)) {
      return true;
    }
    return false;
  }

  public SymbolTable<T> OpenScope(string name) {
    return new SymbolTable<T>(name, this);
  }

  public SymbolTable<T> CloseScope() {
    if (Parent == null) {
      throw new Exception("Cannot close scope: already global.");
    }
    return Parent;
  }
}
