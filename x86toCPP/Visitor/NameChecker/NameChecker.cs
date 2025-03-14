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
    SymbolTable = new SymbolTable();
  }

  // visitRoot, visitDefault -> just visit children

  public void visitDataDirective(DataDirectiveNode node) {
    // check if it already exists
    Console.WriteLine($"Visiting identifier {node.IdentifierToken.Value}");
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

  }
}
