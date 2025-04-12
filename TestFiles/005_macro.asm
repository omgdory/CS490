%macro	testMacro	2
	mov rbx, %2
	mov rcx, STR_LENGTH
	dec rcx
	mov r13, 0 
	mov byte[rbx + rcx], NULL
	dec rcx
	mov edx, 0
	mov eax, %1
	mov r13d, eax
	cmp eax, 0 
	jge %%convertNumberInt2Sept
	neg eax
%%convertNumberInt2Sept:
	idiv r12d 
	add edx, 48 
	mov byte[rbx + rcx], dl 
  mov byte[rbx+rcx], dl   ; duplicated for testing...
	mov edx, 0
	dec rcx
	cmp eax, 0 
	jne %%convertNumberInt2Sept

	cmp r13d, r11d 
	jl %%addNegativeInt2Sept
	mov byte[rbx + rcx], 43 
	dec rcx
	jmp %%addWhitespacesInt2Sept
%%addNegativeInt2Sept:
	mov byte[rbx + rcx], 45 
	dec rcx
%%addWhitespacesInt2Sept:
	mov byte[rbx + rcx], 32 
	dec rcx
	cmp rcx, r11
	jge %%addWhitespacesInt2Sept
%endmacro

section .data
SYS_exit      equ 60
EXIT_SUCCESS  equ 0
NULL          equ 0
test          equ 0x31

section .text
global _start
_start:
  mov rax, 2
  testMacro dword[array+rsi*4], tempString
  mov rax, SYS_exit
  mov rdi, EXIT_SUCCESS
  syscall