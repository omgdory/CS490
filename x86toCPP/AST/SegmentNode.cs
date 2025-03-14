namespace x86toCPP;

public class SegmentNode : ASTNode {
  public SEGMENT_IDENTIFIER_TOKEN SegmentIdentifier { get; }

  public SegmentNode(SEGMENT_IDENTIFIER_TOKEN segmentIdentifier, List<ASTNode> instructionNodes) {
    SegmentIdentifier = segmentIdentifier;
    Children.AddRange(instructionNodes);
  }

  public override void accept(Visitor visitor) {
    visitor.visitSegment(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Segment: {SegmentIdentifier}");
    PrintChildren(indent);
  }
}
