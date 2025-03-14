namespace x86toCPP;

public class MnemonicNode : ASTNode {
  public Token Token { get; set; }
  public MNEMONIC_TOKEN Opcode { get; }

  public MnemonicNode(Token token, MNEMONIC_TOKEN opcode, List<ASTNode> operands) {
    Token = token;
    Opcode = opcode;
    Children.AddRange(operands);
  }

  public override void accept(Visitor visitor) {
    visitor.visitMnemonic(this);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Instruction: {Opcode}, line {Token.Line}");
    PrintChildren(indent);
  }
}
