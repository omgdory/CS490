namespace x86toCPP;

public class SymbolEntry {
  public string Name { get; private set; }
  public SymbolType Type { get; private set; }
  public string Value { get; set; } // for variables

  public SymbolEntry(string name, SymbolType type, string value = "") {
    Name = name;
    Type = type;
    Value = value;
  }
}
