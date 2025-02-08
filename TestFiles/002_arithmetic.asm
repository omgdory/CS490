; 002_arithmetic.asm - basic arithmetic operations

section .data

SYS_exit equ 60
EXIT_SUCCESS equ 0

section .text

global _start
_start:
  add al, bl    ; byte
  add cx, dx    ; word
  add rsp, edi  ; dword
  add rbp, r11  ; qword

  sub cl, al    ; byte
  sub dx, ax    ; word
  sub ecx, esi  ; dword
  sub r12, rdi  ; qword

  mul r8b       ; byte
  mul di        ; word
  mul esi       ; dword
  mul r13       ; qword

  div dil       ; byte
  div cx        ; word
  div esi       ; dword
  div r14       ; qword

  imul r8b      ; byte
  imul di       ; word
  imul esi      ; dword
  imul r13       ; qword

  idiv dil      ; byte
  idiv cx       ; word
  idiv esi      ; dword
  idiv r14      ; qword

  inc al        ; byte
  inc ax        ; word
  inc eax       ; dword
  inc rax       ; qword

  dec al        ; byte
  dec ax        ; word
  dec eax       ; dword
  dec rax       ; qword

; exit
last:
  mov rax, SYS_exit
  mov rdi, EXIT_SUCCESS
  syscall