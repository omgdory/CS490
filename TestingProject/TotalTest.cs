using x86toCPP;
namespace TestingProject;

public class TotalTest {
  [Fact]
  public void GenerateLexerCoverage() {
    // CS490\TestingProject\bin\Debug\net8.0\TestFiles
    string testFilesDirectoryPath = @"..\..\..\..\TestFiles";
    string testFilesExtension = "*.asm";
    IEnumerable<string> files = Directory.EnumerateFiles(testFilesDirectoryPath, testFilesExtension);
    // test the lexer
    List<Token> currentTokens = Lexer.LexTokens(testFilesDirectoryPath + @"\000_base.asm");
    foreach(Token token in currentTokens) token.ToString();
    // test the parser
    Parser parser = new Parser(currentTokens);
    parser.SyntaxTreeRoot.Print();
    // foreach(string file in files) {
    //   // test the lexer
    //   List<Token> currentTokens = Lexer.LexTokens(file);
    //   foreach(Token token in currentTokens) token.ToString();
    //   // test the parser
    //   Parser parser = new Parser(currentTokens);
    //   parser.SyntaxTreeRoot.Print();
    // }
    // get the HTML file with "reportgenerator" on the XML report to view the results
  }
}