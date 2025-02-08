using Lexer_x86;
namespace TestingProject;

public class LexerTest {
    [Fact]
    public void GenerateLexerCoverage() {
      // CS490\TestingProject\bin\Debug\net8.0\TestFiles
      string testFilesDirectoryPath = @"..\..\..\..\TestFiles";
      string testFilesExtension = "*.asm";
      IEnumerable<string> files = Directory.EnumerateFiles(testFilesDirectoryPath, testFilesExtension);
      foreach(string file in files) {
        List<Token> tmp = Lexer.LexTokens(file);
      }
      // get the HTML file with "reportgenerator" on the XML report to view the results
    }
}