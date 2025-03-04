namespace x86toCPP;

public class MnemonicNode : ASTNode {
  public MNEMONIC_TOKEN Opcode { get; }

  public MnemonicNode(MNEMONIC_TOKEN opcode, List<ASTNode> operands) {
    Opcode = opcode;
    Children.AddRange(operands);
  }

  // print to screen
  public override void Print(int indent = 0) {
    Console.WriteLine($"{new string(' ', indent)}Instruction: {Opcode}");
    PrintChildren(indent);
  }
}
