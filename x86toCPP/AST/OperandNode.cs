namespace x86toCPP;

public class OperandNode : ASTNode {
  public string Value { get; }
  public TOKEN_TYPE OperandType { get; }
  public REGISTER_TOKEN RegisterType { get; }

  public OperandNode(string tokenValue, TOKEN_TYPE operandType) {
    Value = tokenValue;
    OperandType = operandType;
    RegisterType = OperandType == TOKEN_TYPE.REGISTER ?
      RegisterToken.StringToType[tokenValue] :
      REGISTER_TOKEN.DEFAULT;
  }

  public override void Print(int indent = 0) {
    switch(OperandType) {
      case TOKEN_TYPE.REGISTER:
        Console.WriteLine($"{new string(' ', indent)}Register: {RegisterType}");
        break;
      case TOKEN_TYPE.HEX_NUMBER:
      case TOKEN_TYPE.NUMBER:
        Console.WriteLine($"{new string(' ', indent)}Numeric Value: {Value}");
        break;
      default:
        Console.WriteLine($"{new string(' ', indent)}Operand: {OperandType}");
        break;
    }
  }
}
