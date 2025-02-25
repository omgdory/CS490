; 000_base.asm - mov 2 into rax register

section .data
SYS_exit equ 60
EXIT_SUCCESS equ 0

adddd db 17
hello dw 0xA034

cool_stuff resb 10

section .text

global _start
_start:
  mov rax, 4

label1:
  mov rbx, 2
  shl rbx
  xor rbx, rbx
label2:
  mov r11, 2
  mov r12, 10
  add r11, r12

exit_label:
  mov rax, SYS_exit
  mov rdi, EXIT_SUCCESS
  syscall

  