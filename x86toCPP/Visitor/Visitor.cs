namespace x86toCPP;

public interface Visitor {
  public abstract void visitRoot(RootNode node);
  public abstract void visitDefault(DefaultNode node);
  public abstract void visitDataDirective(DataDirectiveNode node);
  public abstract void visitGlobalDeclarator(GlobalDeclaratorNode node);
  public abstract void visitLabel(LabelNode node);
  public abstract void visitMacro(MacroNode node);
  public abstract void visitMemoryAccess(MemoryAccessNode node);
  public abstract void visitMnemonic(MnemonicNode node);
  public abstract void visitOperand(OperandNode node);
  public abstract void visitSegment(SegmentNode node);
}
