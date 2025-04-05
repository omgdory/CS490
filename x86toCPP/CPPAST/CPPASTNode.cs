namespace x86toCPP;

// node for C++ translation
public abstract class CPPASTNode {
  public List<CPPASTNode> Children { get; }

  public CPPASTNode() {
    Children = new List<CPPASTNode>();
  }
}

public class CPPRootNode : CPPASTNode {

}
