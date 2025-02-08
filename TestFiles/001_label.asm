; 001_label.asm - have labels

section .data
SYS_exit equ 60
EXIT_SUCCESS equ 0

section .text

global _start
_start:

move_me:
  mov rax, 2

last:
  mov rax, SYS_exit
  mov rdi, EXIT_SUCCESS
  syscall