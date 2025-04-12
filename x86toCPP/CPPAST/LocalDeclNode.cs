namespace x86toCPP;

public class LocalDeclNode : CPPASTNode {
  public string Identifier { get; private set; }
  public string Value { get; private set; } 

  public LocalDeclNode(string identifier, string value) {
    Identifier = identifier;
    Value = value;
  }

  public override string ToString() {
    return "const int " + Identifier + " = " + Value + ";";
  }
}
