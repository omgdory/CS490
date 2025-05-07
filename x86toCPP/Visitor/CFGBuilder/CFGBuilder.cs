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
  private CFGNode current;

	// for CFG analyzer reference
	public Dictionary<int, CFGNode> NodeMap { get; private set; }
  
  public CFGBuilder() {
    // public attributes
    Type = VisitorType.CFGBuilder;
    // private attributes
    labelMap = new Dictionary<string, int>();
    nodeMap = new Dictionary<int, CFGNode>();
		NodeMap = nodeMap;
    id = 1;
    current = new CFGNode(id);
    nodeMap[id] = current;
    id++;
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
  private CFGNode InstantiateCurrent() {
    if(current != null) throw new Exception("Current instantiation while still extant.");
    current = new CFGNode(id++);
    nodeMap[id-1] = current;
    return current;
  }

  /// <summary>
  /// Instantiates a new <see cref="CFGNode"/> and creates an edge from the current node.
  /// Sets the newly created node to <c>current</c> if <c>newCurrent</c> is <c>true</c>.
  /// Adds the newly created node to <c>current.Edges</c> if <c>newEdge</c> is <c>true</c>.
  /// </summary>
  /// <remarks>
  /// Exception thrown if <c>current</c> is <c>null</c>
  /// </remarks>
  private CFGNode NewNode(string? label = null, bool newCurrent = true, bool addEdge = true) {
    if(current == null) throw new Exception("New edge creation while current null.");
    CFGNode target = new CFGNode(id);
    nodeMap[id] = target;
    if(label != null) {
      target.Label = label;
      labelMap[label] = id;
    }
    id++;
    if(addEdge) AddEdge(current, target);
    if(newCurrent) current = target;
    return target;
  }

  /// <summary>
  /// Adds an <c>ASTNode</c> to <c>current</c>'s associated parse tree nodes.
  /// </summary>
  /// <remarks>
  /// Exception thrown if <c>current</c> is <c>null</c>.
  /// </remarks>
  private void AddNode(ASTNode node) {
    if(current == null) throw new Exception("Attempted handling of current CFG variable while null.");
    current.ParseNodes.Add(node);
  }

  /// <summary>
  /// Adds a <c>targetNode</c> as a <c>baseNode</c> edge. Does not add if the node has already been added.
  /// </summary>
  /// <remarks>
  /// Exception thrown if either argument is <c>null</c>.
  /// </remarks>
  private void AddEdge(CFGNode baseNode, CFGNode targetNode) {
    if(baseNode == null || targetNode == null) throw new Exception("Attempted handling of current CFG variable while null.");
    if(baseNode.Edges.Any(a => a.Id == targetNode.Id)) return;
    baseNode.Edges.Add(targetNode);
  }

  /// <summary>
  /// Adds the <c>current</c> node as an edge to the most recently parsed CFGNode.
  /// </summary>
  /// <remarks>
  /// Exception thrown if <c>current</c> is <c>null</c>.
  /// </remarks>
  private void ConnectPrevious() {
    if(current == null) throw new Exception("Attempted handling of current CFG variable while null.");
    nodeMap[id-1].Edges.Add(current);
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
    // has label already been found earlier?
    bool nodeInstantiated = labelMap.ContainsKey(node.Identifier);
    // if yes, just set current to this one and connect previous
    if(nodeInstantiated) {
      CFGNode tmp = current;
      current = nodeMap[labelMap[node.Identifier]];
      AddEdge(tmp, current);
    } 
    // otherwise, we'll make a new one if needed
    else {
      if(current == null || current.ParseNodes.Count != 0)
        NewNode(node.Identifier);
      else
        current.SetLabel(node.Identifier);
    }
    // now, just visit the children
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
        // if unconditional, then make sure the next node isn't connected
        bool unconditionalJump = node.Opcode == MNEMONIC_TOKEN.JMP;
        // get the label
        string targetLabel = ((OperandNode)node.Children[0]).Value;
        // does the target already exist?
        bool nodeInstantiated = labelMap.ContainsKey(targetLabel);
        AddNode(node);
        if(nodeInstantiated) {
          CFGNode targetNode = nodeMap[labelMap[targetLabel]]; 
          AddEdge(current, targetNode);
        } else {
          // node not instantiated -> create but DON'T set to current
          NewNode(targetLabel, newCurrent: false);
        }
        // re-instantiate current
        NewNode(addEdge: !unconditionalJump);
        break;
      // if not jump, just add to CFG and continue
      default:
        // add current ASTNode to "current"
        AddNode(node);
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
        // InstantiateCurrent();
        foreach(ASTNode child in node.Children) {
          AddNode(child);
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
