namespace x86toCPP;

/// <summary>
/// Visitor pattern implementation to visit the parse tree.
/// Builds a Control Flow Graph given an appropriately validated parse tree.
/// </summary>
public class CFGBuilder : Visitor {
  // SymbolTable (but for CFG nodes)

  // Create new CFG for each macro

  // 1. iterate through label children (ASTNodes)
  // 2. jump instruction?
  //     yes: already in SymbolTable?
  //   yes: add pointer to that entry
  //   no: create new CFG node and add to symbol table
  //       - associate with label of the jump instruction
  //       - add pointer to newly-created CFG node
  //     no: add to current node and continue

  public VisitorType Type { get; private set; }
  public SymbolTable<CFGNode> NodesTable { get; private set; }
  public List<CFGNode> Graphs { get; private set;}
  
  private int id = 0;
  private CFGNode current;
  
  public CFGBuilder() {
    Type = VisitorType.CFGBuilder;
    NodesTable = new SymbolTable<CFGNode>("ControlFlowGraphBuilder");
    Graphs = new List<CFGNode>();
    current = new CFGNode(id++);
    // add first node to symbol table
    SymbolEntry<CFGNode> entry = new SymbolEntry<CFGNode>(current.Id.ToString(), SymbolType.DEFAULT, current);
    NodesTable.AddSymbol(entry);
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
    // if data segment, just make it its own node
    if(node.SegmentIdentifier == SEGMENT_IDENTIFIER_TOKEN.DATA_SEGMENT_IDENTIFIER) {
      
    }
  }
}
