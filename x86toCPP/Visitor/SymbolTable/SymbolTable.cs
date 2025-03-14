namespace x86toCPP;

/// <summary>
/// Represents a Symbol Table that tracks identifiers, such as within a visitor pattern.
/// </summary>
public class SymbolTable {
  /// <summary>
  /// A dictionary storing symbols with their names as keys and SymbolEntry objects as values.
  /// </summary>
  public Dictionary<string, SymbolEntry> Symbols { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SymbolTable"/> class.
  /// </summary>
  public SymbolTable() {
    Symbols = new Dictionary<string, SymbolEntry>();
  }

  /// <summary>
  /// Adds a new identifier to the Symbol Table.
  /// Throws an exception if the entry name already exists.
  /// </summary>
  public void Add(SymbolEntry entry) {
    if (!Symbols.ContainsKey(entry.Name)) {
      Symbols[entry.Name] = entry;
    }
    else {
      throw new Exception($"Error: Identifier '{entry.Name}' is already defined.");
    }
  }

  /// <summary>
  /// Checks whether an identifier exists in the Symbol Table.
  /// </summary>
  public bool Exists(string name) {
    return Symbols.ContainsKey(name);
  }

  /// <summary>
  /// Retrieves the details of an identifier from the Symbol Table.
  /// Throws an exception if not found.
  /// </summary>
  public SymbolEntry Get(string name) {
    if (Symbols.TryGetValue(name, out var entry))
      return entry;
    throw new Exception($"Error: Identifier '{name}' is not defined.");
  }

  /// <summary>
  /// Prints all stored Symbols in the symbol Table to the console.
  /// </summary>
  public void PrintTable() {
    Console.WriteLine("\n--- Symbol Table ---");
    foreach (var entry in Symbols.Values) {
      Console.WriteLine($"Name: {entry.Name}, Type: {entry.Type}, Value: {entry.Value}");
    }
    Console.WriteLine("---------------------\n");
  }
}
