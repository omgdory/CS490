namespace x86toCPP;

/// <summary>
/// Visitor pattern implementation to visit the parse tree.
/// Ensures that identifiers are initialized and subsequently referenced appropriately.
/// </summary>
public class NameChecker : Visitor {
  // name
  // outer scope (label)
  // map string to "SymbolEntry"
  //  macro, block (label), variable (identifier)
  //  name + value
  
  // add to prefix of macros
  public static string MACRO_PREFIX = "__$$";

  public VisitorType Type { get; private set; }
  public SymbolTable SymbolTable { get; private set; }
  
  public NameChecker() {
    Type = VisitorType.NameChecker;
    SymbolTable = new SymbolTable("NameChecker");
  }

  // Call default implementation to visit the root
  public void Execute(RootNode rootNode) {
    Console.WriteLine($"Performing visitor pattern {Type}.");
    rootNode.accept(this);
  }

  // visitDefault -> just visit children
  public void visitRoot(RootNode node) {
    Console.WriteLine("Visitng root.");
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitDataDirective(DataDirectiveNode node) {
    // check if it already exists
    Console.WriteLine($"Visiting identifier {node.IdentifierToken.Value}");
    try {
      SymbolEntry entry = new SymbolEntry(node.IdentifierToken.Value, node);
      SymbolTable.AddSymbol(entry);
    } catch {
      throw new Exception($"Redefinition of data directive {node.IdentifierToken.Value} on line {node.IdentifierToken.Line}.");
    }
  }

  public void visitGlobalDeclarator(GlobalDeclaratorNode node) {

  }

  public void visitLabel(LabelNode node) {

  }

  public void visitMacro(MacroNode node) {

  }

  public void visitMemoryAccess(MemoryAccessNode node) {

  }

  public void visitMnemonic(MnemonicNode node) {

  }

  public void visitOperand(OperandNode node) {

  }

  public void visitSegment(SegmentNode node) {
    Console.WriteLine($"Visiting segment {node.SegmentIdentifier}.");
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }
}
