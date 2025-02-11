using x86toCPP;
using Token_x86;
using System.IO;

namespace x86toCPP;

public class Program {
  public static void Main(string[] args) {
    if(args.Length != 1) {
      Console.WriteLine("Usage: x86toCPP <file.asm>");
      return;
    }
    List<Token> tokens = Lexer.LexTokens(args[0]);
    // foreach(Token t in tokens) {
    //   Console.WriteLine(t.ToString());
    // }
    Parser parser = new Parser();
    ASTNode syntaxTree = parser.ParseTokens(tokens);
    syntaxTree.Print();
  }
}