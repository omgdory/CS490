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

  public void visitRoot(RootNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitDataDirective(DataDirectiveNode node) {
    // check if it already exists
    Console.WriteLine($"Namechecking data identifier \"{node.IdentifierToken.Value}\".");
    try {
      SymbolEntry entry = new SymbolEntry(node.IdentifierToken.Value, SymbolType.DATA_ID, node);
      SymbolTable.AddSymbol(entry);
    } catch {
      throw new Exception($"Redefinition of identifier with name {node.IdentifierToken.Value} as data identifier on line {node.IdentifierToken.Line}.");
    }
  }

  public void visitGlobalDeclarator(GlobalDeclaratorNode node) {

  }

  public void visitLabel(LabelNode node) {
    // check if it already exists
    Console.WriteLine($"Namechecking label \"{node.Identifier}\".");
    try {
      SymbolEntry entry = new SymbolEntry(node.Identifier, SymbolType.LABEL, node);
      SymbolTable.AddSymbol(entry);
    } catch {
      throw new Exception($"Redefinition of identifier with name {node.Identifier} as label on line {node.Token.Line}.");
    }
    // now, visit the innards
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitMacro(MacroNode node) {
    // check if it already exists
    Console.WriteLine($"Namechecking macro \"{node.Identifier}\".");
    try {
      SymbolEntry entry = new SymbolEntry(node.Identifier, SymbolType.MACRO, node);
      SymbolTable.AddSymbol(entry);
    } catch {
      throw new Exception($"Redefinition of identifier with name {node.Identifier} as macro on line {node.Token.Line}.");
    }
    // no need to visit macro innards... -> consider later?
  }

  public void visitMemoryAccess(MemoryAccessNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitMnemonic(MnemonicNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitOperand(OperandNode node) {
    // only namecheck for identifiers -> skip if it isn't one
    if(node.OperandType != TOKEN_TYPE.IDENTIFIER) {
      return;
    }
    Console.WriteLine($"Namechecking identifier \"{node.Value}\".");
    if(!SymbolTable.Exists(node.Value)) { 
      throw new Exception($"Use of unfound identifier \"{node.Value}\".");
    }
  }

  public void visitSegment(SegmentNode node) {
    Console.WriteLine($"Namechecking segment \"{node.SegmentIdentifier}\".");
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }
}
