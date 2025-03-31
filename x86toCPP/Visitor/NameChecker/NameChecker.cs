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
    ((Visitor)this).visitRoot(rootNode);
  }

  // visitRoot, visitDefault -> just visit children

  private void visitDataDirective(DataDirectiveNode node) {
    // check if it already exists
    Console.WriteLine($"Visiting identifier {node.IdentifierToken.Value}");
    try {
      SymbolEntry entry = new SymbolEntry(node.IdentifierToken.Value, node);
      SymbolTable.AddSymbol(entry);
    } catch {
      throw new Exception($"Redefinition of data directive {node.IdentifierToken.Value} on line {node.IdentifierToken.Line}.");
    }
  }

  private void visitGlobalDeclarator(GlobalDeclaratorNode node) {

  }

  private void visitLabel(LabelNode node) {

  }

  private void visitMacro(MacroNode node) {

  }

  private void visitMemoryAccess(MemoryAccessNode node) {

  }

  private void visitMnemonic(MnemonicNode node) {

  }

  private void visitOperand(OperandNode node) {

  }

  private void visitSegment(SegmentNode node) {
    Console.WriteLine($"Visiting segment {node.SegmentIdentifier}.");
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }
}
