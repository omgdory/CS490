; 003_logic.asm - basic logical operations

section .data

test dq 35

SYS_exit equ 60
EXIT_SUCCESS equ 0

section .text

global _start
_start:
  shl ax, 8
  shl eax, cl
  shl rcx, 32
  shl qword[test], cl

  shr ax, 8
  shr eax, cl
  shr rcx, 32
  shr qword[test], cl

; exit
last:
  mov rax, SYS_exit
  mov rdi, EXIT_SUCCESS
  syscall