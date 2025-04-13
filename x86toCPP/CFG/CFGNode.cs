namespace x86toCPP;

public class CFGNode {
// CFG node
// - list of pointers to other nodes (self-point allowed)
// - list of ASTNodes
  public int Id { get; }
  public List<CFGNode> Paths { get; }
  public List<ASTNode> ParseNodes { get; }

  public CFGNode(int id) {
    Id = id;
    Paths = new List<CFGNode>();
    ParseNodes = new List<ASTNode>();
  }

  public void Print() {
    Console.Write($"Node {Id} to ");
    bool first = true;
    // where the node goes to
    foreach(CFGNode n in Paths) {
      if(!first) {
        Console.Write(", ");
      }
      first = false;
      Console.Write(n.Id);
    }
    Console.Write(".\n");
  }
}
