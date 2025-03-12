namespace x86toCPP;

using System.Collections;
using System.Runtime.InteropServices;


public class NameChecker : Visitor {
  // name
  // outer scope (label)
  // map string to "SymbolEntry"
  //  macro, block (label), variable (identifier)
  //  name + value
  public string Name { get; private set; }
  public Hashtable SymbolTable { get; private set; }
  
  public NameChecker() {
    Name = "NameChecker";
    SymbolTable = new Hashtable();
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
