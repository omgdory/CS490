# make FILENAME=("<filename>")

OBJ	= obj_file.o
ASM	= yasm -g dwarf2 -f elf64
LD = ld -g

all: main

obj_file.o: $(FILENAME)
	$(ASM) $(FILENAME) -l lst_file.lst -o $(OBJ)

main: $(OBJ)
	$(LD) -o main $(OBJ)

# -----
# clean by removing object files.

clean:
	rm  $(OBJ)
	rm  lst_file.lst