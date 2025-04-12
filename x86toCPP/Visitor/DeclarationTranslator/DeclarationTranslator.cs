namespace x86toCPP;

/// <summary>
/// Visitor pattern implementation to visit the parse tree.
/// Translates data declarations to CPP declarations.
/// </summary>
public class DeclarationTranslator : Visitor {
  public VisitorType Type { get; private set; }
  public CPPRootNode DeclarationTree { get; private set;}
  
  public DeclarationTranslator() {
    Type = VisitorType.DeclarationTranslator;
    DeclarationTree = new CPPRootNode();
  }

  public void PrintTranslation() {
    for(int i=0; i<DeclarationTree.Children.Count; i++) {
      Console.WriteLine(DeclarationTree.Children[i].ToString());
    }
  }

  // Call default implementation to visit the root
  public void Execute(RootNode rootNode) {
    Console.WriteLine($"Performing visitor pattern {Type}.");
    Console.WriteLine("Generating CPP declarations from data section...");
    rootNode.accept(this);
  }

  public void visitRoot(RootNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitDataDirective(DataDirectiveNode node) {
    Console.WriteLine($"Translating data identifier \"{node.IdentifierToken.Value}\".");
    // get the operands
    List<OperandNode> operands = new List<OperandNode>();
    for(int i=0; i<node.Children.Count; i++) {
      if(node.Children[i].GetType() == typeof(OperandNode))
        operands.Add((OperandNode)node.Children[i]);
      else
        throw new Exception($"({node.IdentifierToken.Line}) Operand not of type OperandNode");
    }
    // how many operands there are -> single value vs array
    // single value
    if(operands.Count == 1) {
      LocalDeclNode result = new LocalDeclNode(node.IdentifierToken.Value, operands[0].Value);
      DeclarationTree.Children.Add(result);
      return;
    }
    // array
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
    // only data segment
    if(node.SegmentIdentifier == SEGMENT_IDENTIFIER_TOKEN.DATA_SEGMENT_IDENTIFIER) {
      foreach(ASTNode child in node.Children) {
        child.accept(this);
      }
    }
  }
}
