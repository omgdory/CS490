using x86toCPP;
using System.IO;

namespace x86toCPP;

public class Program {
  public static void Main(string[] args) {
    // check usage
    if(args.Length != 1) {
      Console.WriteLine("Usage: x86toCPP <file.asm>");
      return;
    }
    // ----- lexer -----
    List<Token> tokens = Lexer.LexTokens(args[0]);
    int i=0;
    // output lexer
    foreach(Token t in tokens) {
      Console.WriteLine(i + ": " + t.ToString());
      i++;
    }
    // ----- parser -----
    Parser parser = new Parser(tokens);
    // output parser
    parser.SyntaxTreeRoot.Print();
    // ----- namechecker -----
    RootNode rootNode = parser.SyntaxTreeRoot;
    NameChecker nameChecker = new NameChecker();
    nameChecker.Execute(rootNode);
    // // basic declaration translation
    // DeclarationTranslator declarationTranslator = new DeclarationTranslator();
    // declarationTranslator.Execute(rootNode);
    // // output basic declaration translation
    // declarationTranslator.PrintTranslation();
  }
}