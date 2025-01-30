; hello_world.asm - A simple assembly program that adds two numbers and prints the result

section .data          ; Data section
  num1 db 5            ; Define byte variable num1 with value 5
  num2 db 3            ; Define byte variable num2 with value 3
  result db 0          ; Define byte variable to store the result, initialized to 0

section .text           ; Code section
global _start           ; Make entry point visible

_start:                 ; Entry point
  mov al, [num1]      ; Move the value of num1 into AL register
  add al, [num2]      ; Add the value of num2 to AL (AL = AL + num2)
  add al, 48          ; ASCII form of the value
  mov [result], al    ; Store the (ASCII) result in the memory location of result

  ; Prepare for system call
  mov rax, 1          ; System call number for sys_write (4 is Linux syscall)
  mov rdi, 1          ; File descriptor for stdout (1 is standard output)
  mov rsi, result     ; Address of the string to print
  mov rdx, 1          ; Number of bytes to print
  syscall

  ; Exit gracefully
  mov rax, 60
  mov rdi, 0
  syscall