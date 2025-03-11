namespace x86toCPP;

public interface Visitor {
  // Default functionality: visit all of the children

  // Visit the root of the syntax tree
  public void visitRoot(RootNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a default node
  public void visitDefault(DefaultNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a data directive node i.e. "ID dw NUMBER"
  public void visitDataDirective(DataDirectiveNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a global declarator node "global LABEL"
  public void visitGlobalDeclarator(GlobalDeclaratorNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a label and its associated code block: "LABEL:"
  public void visitLabel(LabelNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a macro node: "%macro" to "%endmacro"
  public void visitMacro(MacroNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a memory accessor: "dword[eax+edx*4]"
  public void visitMemoryAccess(MemoryAccessNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a mnemonic/instruction: "MNEMONIC [op1, op2, op3]"
  public void visitMnemonic(MnemonicNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit an operand: "MNEMONIC OPERAND"
  public void visitOperand(OperandNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }

  // Visit a segment: "section .data" or "section .text"
  public void visitSegment(SegmentNode node) {
    foreach(ASTNode child in node.Children) {
      child.accept(this);
    }
  }
}
