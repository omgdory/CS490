%macro	int2aSept	2
	mov rbx, %2
	mov rcx, STR_LENGTH
	dec rcx
	mov rdx, 0
	mov r11, 0
	mov r12, 0
	mov r12d, 7 
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
