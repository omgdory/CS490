namespace x86toCPP;

/// <summary>
/// Visitor pattern implementation to visit the parse tree.
/// Builds a Control Flow Graph given an appropriately validated parse tree.
/// </summary>
public class CFGBuilder : Visitor {

  public VisitorType Type { get; private set; }

  private Dictionary<string, int> labelMap; // label to ID
  private Dictionary<int, CFGNode> nodeMap; // ID to CFGNode
  private int id;
  private CFGNode? current;
  
  public CFGBuilder() {
    // public attributes
    Type = VisitorType.CFGBuilder;
    // private attributes
    labelMap = new Dictionary<string, int>();
    nodeMap = new Dictionary<int, CFGNode>();
    id = 0;
    current = null;
  }

  public void Print() {
    foreach(KeyValuePair<int, CFGNode> entry in nodeMap) {
      entry.Value.Print();
    }
  }

  /// <summary>
  /// Instantiates a new <see cref="CFGNode"/> as the current node.
  /// </summary>
  /// <remarks>
  /// Exception thrown if <c>current</c> is already assigned,
  /// indicating improper usage or unexpected flow in CFG construction.
  /// </remarks>
  private void InstantiateCurrent() {
    if(current != null) throw new Exception("Current instantiation while still extant.");
    current = new CFGNode(id++);
    nodeMap[id-1] = current;
  }

  /// <summary>
  /// Instantiates a new <see cref="CFGNode"/> and creates an edge from the current node.
  /// </summary>
  /// <remarks>
  /// Exception thrown if <c>current</c> is <c>null</c>
  /// </remarks>
  private CFGNode CreateTargetNode() {
    if(current == null) throw new Exception("New edge creation while current null.");
    CFGNode target = new CFGNode(id++);
    nodeMap[id-1] = target;
    current.Edges.Add(target);
    return target;
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
    // null current CFG node -> make a new one!
    if(current == null) {
      InstantiateCurrent();
    }
    // current CFG node extant -> create a new node and make it point to new one!
    else {
      CFGNode target = CreateTargetNode();
      current = target;
    }
    // add to labelMap
    labelMap[node.Identifier] = id;
    current?.ParseNodes.Add(node);
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitMacro(MacroNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  public void visitMemoryAccess(MemoryAccessNode node) {

  }

  public void visitMnemonic(MnemonicNode node) {
    switch(node.Opcode) {
      // handle the jumps
      case MNEMONIC_TOKEN.JMP:
      case MNEMONIC_TOKEN.JE:
      case MNEMONIC_TOKEN.JNE:
      case MNEMONIC_TOKEN.JL:
      case MNEMONIC_TOKEN.JLE:
      case MNEMONIC_TOKEN.JG:
      case MNEMONIC_TOKEN.JGE:
      case MNEMONIC_TOKEN.JB:
      case MNEMONIC_TOKEN.JBE:
      case MNEMONIC_TOKEN.JA:
      case MNEMONIC_TOKEN.JAE:
        // get the label
        string targetLabel = ((OperandNode)node.Children[0]).Value;
        // does the target already exist?
        bool nodeInstantiated = labelMap.ContainsKey(targetLabel);
        // instantiate the current if needed
        if(current == null) InstantiateCurrent();
        if(nodeInstantiated) {
          CFGNode targetNode = nodeMap[labelMap[targetLabel]-1]; 
          current?.Edges.Add(targetNode);
        } else {
          if(current == null) InstantiateCurrent();
          CreateTargetNode();
        }
        current = null;
        break;
      // if not jump, just add to CFG and continue
      default:
        // if current has been nullified, then make a new one
        if(current == null) InstantiateCurrent();
        current?.ParseNodes.Add(node);
        break;
    }
  }

  public void visitOperand(OperandNode node) {

  }

  public void visitSegment(SegmentNode node) {
    // handle depending on what type of segment (section) it is
    switch(node.SegmentIdentifier) {
      // if data segment, just make it its own node
      case SEGMENT_IDENTIFIER_TOKEN.DATA_SEGMENT_IDENTIFIER:
        // one node with all ASTNodes of the data segment
        InstantiateCurrent();
        foreach(ASTNode child in node.Children) {
          current?.ParseNodes.Add(child);
        }
        break;
      // if text segment, just let the children handle it
      case SEGMENT_IDENTIFIER_TOKEN.TEXT_SEGMENT_IDENTIFIER:
        foreach(ASTNode child in node.Children) {
          child.accept(this);
        }
        break;
      // unsupported section
      default:
        throw new Exception("FATAL: Unhandled section in CFGBuilder.");
    }
  }
}
