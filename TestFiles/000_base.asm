; 000_base.asm - mov 2 into rax register

section .data
SYS_exit equ 60
EXIT_SUCCESS equ 0

; section .text

; global _start
; _start:
;   mov rax, 2

;   mov rax, SYS_exit
;   mov rdi, EXIT_SUCCESS
;   syscall