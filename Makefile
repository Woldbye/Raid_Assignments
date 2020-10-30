#change this to the name of the Main class file, without file extension
MAIN_FILE = raid_assign

#change this to the depth of the project folders
#if needed, add a prefix for a common project folder
CSHARP_SOURCE_FILES = $(wildcard */*/*/*/*/*/*.cs */*/*/*/*/*.cs */*/*/*/*.cs */*/*/*.cs */*/*.cs */*.cs *.cs)

#add needed flags to the compilerCSHARP_FLAGS = -out:$(EXECUTABLE)
CSHARP_FLAGS = -out:$(EXECUTABLE)

#Will hold the debug flag
DEBUG_FLAG = ""

#change to the environment compiler
CSHARP_COMPILER = mcs

#if needed, change the executable file
EXECUTABLE = $(MAIN_FILE).exe

#if needed, change the remove command according to your system
RM_CMD = -rm -f $(EXECUTABLE)

run: all
	./$(EXECUTABLE)

debug: DEBUG_FLAG = -define:DEBUG
debug: all
	./$(EXECUTABLE)

all: $(EXECUTABLE)

$(EXECUTABLE): $(CSHARP_SOURCE_FILES)
	@ $(CSHARP_COMPILER) $(CSHARP_SOURCE_FILES) $(DEBUG_FLAG) $(CSHARP_FLAGS)
	@ echo compiling $(CSHARP_COMPILER) $(CSHARP_SOURCE_FILES) $(DEBUG_FLAG) $(CSHARP_FLAGS)

clean:
	@ $(RM_CMD)

remake:
	@ $(MAKE) clean
	@ $(MAKE)
