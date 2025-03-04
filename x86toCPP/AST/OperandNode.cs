namespace x86toCPP;

public class OperandNode : ASTNode {
  public Token Token  { get; set; }
  public string Value { get; set;}
  public TOKEN_TYPE OperandType { get; set; }
  public REGISTER_TOKEN RegisterType { get; }

  public OperandNode(Token token) {
    Token = token;
    Value = token.Value;
    if(!Enum.IsDefined(typeof(TOKEN_TYPE), token.TokenType)) {
      string exceptionData = $"({token.Line}) Token type invalid when creating operand node:\n";
      exceptionData += $"\tToken value: {Value}";
      throw new Exception(exceptionData);
    }
    OperandType = (TOKEN_TYPE)token.TokenType;
    RegisterType = token.TokenType == (int)TOKEN_TYPE.REGISTER ?
      RegisterToken.StringToType[token.Value] :
      REGISTER_TOKEN.DEFAULT;
  }

  public override void Print(int indent = 0) {
    string output = $"{new string(' ', indent)}Operand: {Value}";
    switch(OperandType) {
      case TOKEN_TYPE.REGISTER:
        Console.WriteLine(output + $" (Register {RegisterType})");
        break;
      case TOKEN_TYPE.HEX_NUMBER:
      case TOKEN_TYPE.NUMBER:
        Console.WriteLine(output + $" (Numeric Value)");
        break;
      case TOKEN_TYPE.IDENTIFIER:
        Console.WriteLine(output + $" (Identifier)");
        break;
      default:
        Console.WriteLine(output);
        break;
    }
  }
}
