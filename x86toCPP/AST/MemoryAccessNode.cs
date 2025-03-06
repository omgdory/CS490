namespace x86toCPP;

public class MemoryAccessNode : ASTNode {
  public Token AccessTypeToken { get; set;} // dword, word, etc.
  public Token Address { get; set; }
  public Token Offset { get; set; }
  public Token OffsetMultiplier { get; set; }

  public MemoryAccessNode(Token accessTypeToken, Token address, Token offset, Token offsetMultiplier) {
    AccessTypeToken = accessTypeToken;
    Address = address;
    Offset = offset;
    OffsetMultiplier = offsetMultiplier;
  }

  public override void Print(int indent = 0) {
    string output = $"{new string(' ', indent)}Memory Access: {AccessTypeToken.Value}\n";
    output += $"{new string(' ', indent+2)}Address: {Address.Value}\n";
    output += $"{new string(' ', indent+2)}Offset: {Offset.Value}\n";
    output += $"{new string(' ', indent+2)}OffsetMultiplier: {OffsetMultiplier.Value}";
    Console.WriteLine(output);
  }
}
