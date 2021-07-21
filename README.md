# uni-vscode

This is a "universal language" vscode extension based on Antlr
and Language Server Protocol. It is useful for quick parsing checks
using VSCode. The only real requirement is that the grammar should be
[target agnostic](https://github.com/antlr/antlr4/blob/master/doc/python-target.md#target-agnostic-grammars).
Then, using [trgen](https://github.com/kaby76/Domemtech.Trash/tree/main/trgen),
a parse can be generated for the grammar and plugged into
this extension. Semantic highlighting is the only major component implemented
because static semantics computations (aka attributes) are not implemented
for Antlr4 grammars.

The code is divided into two parts:
Server and Client:

* The server (with Logger, LspHelpers, and Workspaces) is C# code that
implements an LSP server.
* The client is Typescript code that implemements the client VSCode
extension.

## How to use this extension

1) You will need prerequisites:

* [.NET SDK](https://dotnet.microsoft.com/)
* [Trash](https://github.com/kaby76/Domemtech.Trash#install)

2) Clone the repo. Run dotnet to build the language server, and run the "install.sh" script
to create the extesion for VSCode.

The server is a
C# program that reads stdin and writes to stdout. VSCode will redirect the input and output
in order to communicate with the server.

The client is a thin layer of code in Typescript. The "install.sh" script builds the
.vsix file which you can install.

    git clone https://github.com/kaby76/uni-vscode.git
    cd uni-vscode
    dotnet build
    cd VsCode
    bash clean.sh; bash install.sh

3) Create (or copy) an Antlr4 grammar that you want to test.
The grammar must be processed by the
[trgen](https://github.com/kaby76/Domemtech.Trash/tree/main/trgen) application of Trash.
`trgen` creates a standardized parser application from templates.

The Java grammar from grammars-g4 works out of the box with the extension, so you may want to
start there:

    # Clone the grammars-v4 repo, pick a grammar, and generate a parser for the extension.
    git clone https://github.com/antlr/grammars-v4.git
    cd grammars-v4/java/java
    trgen; cd Generated; dotnet build

4) Create a `~/.uni-vscode.rc` file. For the Java example, the file should contain this
configuration in JSON:

	[{
	    "Suffix":".java",
	    "ParserLocation":"c:/Users/kenne/Documents/GitHub/i2248/java/java/Generated/bin/Debug/net5.0/Test.dll",
	    "ClassesAndClassifiers":[
	       {"Item1":"class","Item2":"//classDeclaration/IDENTIFIER"},
	       {"Item1":"property","Item2":"//fieldDeclaration/variableDeclarators/variableDeclarator/variableDeclaratorId/IDENTIFIER"},
	       {"Item1":"variable","Item2":"//variableDeclarator/variableDeclaratorId/IDENTIFIER"},
	       {"Item1":"method","Item2":"//methodDeclaration/IDENTIFIER"},
	       {"Item1":"keyword","Item2":"//(ABSTRACT | ASSERT | BOOLEAN | BREAK | BYTE | CASE | CHAR | CLASS | CONST | CONTINUE | DEFAULT | DO | DOUBLE | ELSE | ENUM | EXTENDS | FINAL | FINALLY | FLOAT | FOR | IF | GOTO | IMPLEMENTS | IMPORT | INSTANCEOF | INT | INTERFACE | LONG | NATIVE | NEW | PACKAGE | PRIVATE | PROTECTED | PUBLIC | SHORT | STATIC | STRICTFP | SUPER | SWITCH | SYNCHRONIZED | THIS | THROW | THROWS | TRANSIENT | TRY | VOID | VOLATILE | WHILE)"},
	       {"Item1":"string","Item2":"//(DECIMAL_LITERAL | HEX_LITERAL | OCT_LITERAL | BINARY_LITERAL | HEX_FLOAT_LITERAL | BOOL_LITERAL | CHAR_LITERAL | STRING_LITERAL | NULL_LITERAL)"}
	       ]
	}]

The `Suffix` field is just the name of the suffix for the grammars
recognized and applied.

The `ParserLocation` field is the full path for the Test.dll of the parser.
You must use `trgen` to create a parser and then build it with .NET.

The `ClassesAndClassifiers` is a list of tuples that identify the
leaf node in the parse tree that you want to classify and color.
The first item in the tuple identifies the class, chosen from the list of classifications
in Language Server Protocol 3.16.,
[SemanticTokenTypes](https://microsoft.github.io/language-server-protocol/specifications/specification-current/#textDocument_semanticTokens). Note, the classes defined here
can be in any order. The second item in the tuple is
the XPath expression used to find parse tree nodes and label with the class.


5) Run VSCode, and install the .vsix 

    code .
    
In VSCode, open a file (e.g., a Java source file). In the lower right corner, there is a type. Change
the type of the file to "any". It takes a little while, but it should colorize the source file.

## Implementation

* LSP server in C#.
* VSCode client code in Typescript.
* Grammars are implemented in Antlr4. The parser driver is implemented
using [trgen](https://github.com/kaby76/Domemtech.Trash/tree/main/trgen).
* The LSP server reads ~/.uni-vscode.rc
(in Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) which
are used to specify the location of the standardized Antlr4 parser and
classes of symbols in XPath notation.
