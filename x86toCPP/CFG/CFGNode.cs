using System.Reflection.Emit;

namespace x86toCPP;

public class CFGNode {
// CFG node
// - list of pointers to other nodes (self-point allowed)
// - list of ASTNodes
  public int Id { get; }
  public List<CFGNode> Edges { get; }
  public List<ASTNode> ParseNodes { get; }
  public string? Label { get; set; }

  public CFGNode(int id) {
    Id = id;
    Edges = new List<CFGNode>();
    ParseNodes = new List<ASTNode>();
    Label = null;
  }

  // so the compiler stops whining
  public void SetLabel(string label) {
    Label = label;
  }

  public void Print() {
    Console.Write($"Node {Id} to ");
    bool first = true;
    // where the node goes to
    foreach(CFGNode n in Edges) {
      if(!first) {
        Console.Write(", ");
      }
      first = false;
      Console.Write(n.Id);
    }
    Console.Write(".\n");
  }
}
