﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lexer_x86;

public class Lexer {
  private static Dictionary<string, TOKEN_TYPE> stringToTypeDict = new Dictionary<string, TOKEN_TYPE>() {
    {"section", TOKEN_TYPE.SECTION},
    {"equ", TOKEN_TYPE.EQU},
    {"db", TOKEN_TYPE.DB},
    {"dw", TOKEN_TYPE.DW},
    {"dd", TOKEN_TYPE.DD},
    {"dq", TOKEN_TYPE.DQ},
    {"ddq", TOKEN_TYPE.DDQ},
    {"dt", TOKEN_TYPE.DT},
    {"resb", TOKEN_TYPE.RESB},
    {"resw", TOKEN_TYPE.RESW},
    {"resd", TOKEN_TYPE.RESD},
    {"resq", TOKEN_TYPE.RESQ},
    {".code", TOKEN_TYPE.CODE_SEGMENT_IDENTIFIER},
    {".data", TOKEN_TYPE.DATA_SEGMENT_IDENTIFIER},
    {".bss", TOKEN_TYPE.BSS_SEGMENT_IDENTIFIER},
    {".text", TOKEN_TYPE.TEXT_SEGMENT_IDENTIFIER},
    {".stack", TOKEN_TYPE.STACK_SEGMENT_IDENTIFIER},
    {"global", TOKEN_TYPE.GLOBAL},
  };

  /// <summary>
  /// Get tokens in a given assembly file
  /// </summary>
  /// <param name="filepath">Path of the specified file (with extension)</param>
  /// <returns>List of Token objects in the file</returns>
  public static List<Token> LexTokens(string filepath) {
    List<Token> res = new List<Token>();
    
    try {
      using (StreamReader reader = new StreamReader(filepath)) {
        int character;
        bool inComment = false;
        string currentValue = "";
        int lineTracker = 1;
        // one character at a time until can't read
        while ((character = reader.Read()) != -1) {
          char currChar = (char)character;
          HandleChar(currChar, ref inComment, ref currentValue, ref res, lineTracker);
          if(currChar=='\n') lineTracker++;
        }
        // deal with leftover currentValue if necessary
        if(!String.IsNullOrWhiteSpace(currentValue)) res.Add(CreateTokenFromString(currentValue, lineTracker));
      }
    }
    catch (Exception ex) {
      Console.WriteLine("Error: " + ex.Message);
    }
    return res;
  }
  private static void HandleChar(char currChar, ref bool inComment, ref string currentValue, ref List<Token> result, int lineTracker) {
    // if in comment mode, then just append to currentValue (comment contents)
    if(inComment) {
      if(currChar == '\r' || currChar == '\n') {
        result.Add(new Token((int)TOKEN_TYPE.COMMENT, (int)CHANNEL_TYPE.COMMENT, currentValue, lineTracker));
        currentValue = "";
        inComment = false;
        return;
      }
      currentValue += currChar;
      return;
    }
    // semicolon -> enter comment
    if(currChar == ';') {
      inComment = true;
      // handle previous token if necessary
      if(currentValue.Length > 0) {
        // ignore whitespace
        // guaranteed not to be in comment, because it would have been caught earlier
        if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      }
      return;
    }
    // comma -> operand separator
    if(currChar == ',') {
      if(currentValue.Length == 0) return;
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      result.Add(new Token((int)TOKEN_TYPE.SPECIAL_CHAR, (int)CHANNEL_TYPE.DEFAULT, currChar.ToString(), lineTracker));
      return;
    }
    // colon
    if(currChar == ':') {
      if(currentValue.Length == 0) return;
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      result.Add(new Token((int)TOKEN_TYPE.SPECIAL_CHAR, (int)CHANNEL_TYPE.DEFAULT, currChar.ToString(), lineTracker));
      return;
    }
    // whitespace -> handle current token, then add a newline token if necessary
    if(currChar == ' ' || currChar == '\r' || currChar == '\t' || currChar == '\n') {
      if(currentValue.Length == 0) return;
      // ignore whitespace
      if(!String.IsNullOrWhiteSpace(currentValue)) result.Add(CreateTokenFromString(currentValue, lineTracker));
      currentValue = "";
      return;
    }
    // anything else
    // Console.WriteLine("Current char: " + currChar);
    currentValue += currChar;
  }

  private static Token CreateTokenFromString(string value, int lineTracker) {
    List<char> special_chars = new List<char>
      {'!','@','#','$','%','^','&','*','(',')'
      ,',','-','+','=','{','}','[',']',';'
      ,':','\'','.','/','?','\\','|'};
    // if just whitespace, then add to whitespace channel
    if(String.IsNullOrWhiteSpace(value)) return new Token((int)TOKEN_TYPE.WHITESPACE, (int)CHANNEL_TYPE.WHITESPACE, value, lineTracker);
    // first, check if it is a register
    else if(RegisterToken.StringToType.ContainsKey(value)) {
      return new RegisterToken(value, lineTracker);
    }
    // then, check if it is a mnemonic
    else if(MnemonicToken.StringToType.ContainsKey(value)) {
      return new MnemonicToken(value, lineTracker);
    }
    // any other keyword
    else if(stringToTypeDict.ContainsKey(value)) {
      return new Token((int)stringToTypeDict[value], (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // special: single character
    else if(value.Length == 1) {
      if(Char.IsDigit(value[0])) return new Token((int)TOKEN_TYPE.NUMBER,(int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      else if(Char.IsAsciiLetter(value[0])) return new Token((int)TOKEN_TYPE.IDENTIFIER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      else if(special_chars.Contains(value[0])) return new Token((int)TOKEN_TYPE.SPECIAL_CHAR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      // error otherwise
      return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // any more than one char
    else if(value.Length > 1) {
      // check hex
      if(value[0] == '0' && value[1]=='x') {
        // if not a power of 2 plus 2, error
        if(value.Length != 4 && value.Length != 6 && value.Length != 10 && value.Length != 18) {
          return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
        // if just "0x", error
        if(value.Length == 2) {
          return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
        }
        List<char> valid_hex_char = new List<char>
          {'A','B','C','D','E','F','a','b','c','d','e','f'};
        for(int i=2; i<value.Length; i++) {
          if(!valid_hex_char.Contains(value[i]) && !Char.IsDigit(value[i])) {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.HEX_NUMBER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // check valid number
      else if(Char.IsDigit(value[0])) {
        for(int i=1; i<value.Length; i++) {
          if(!Char.IsDigit(value[i])) {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.NUMBER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // check valid identifier
      else if(Char.IsAsciiLetter(value[0]) || value[0] == '_') {
        for(int i=1; i<value.Length; i++) {
          if(!Char.IsAsciiLetter(value[0]) && !Char.IsDigit(value[0]) && value[0] != '_') {
            return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
          }
        }
        return new Token((int)TOKEN_TYPE.IDENTIFIER, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
      }
      // error otherwise
      return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
    }
    // anything else should be an error (will test continuously)
    return new Token((int)TOKEN_TYPE.ERROR, (int)CHANNEL_TYPE.DEFAULT, value, lineTracker);
  }
  public static void Test() {
    Console.WriteLine("Hello world");
  }
}
