# uni-vscode

This is a "universal language" vscode extension based on Antlr
and Language Server Protocol. The code is divided into two parts:
Server and VsCode:

* Server (with Logger, LspHelpers, and Workspaces) is C# code that
implements a LSP server.
* VsCode is Typescript code that implemements the client VSCode
extension.

## How to use

You will need prerequisites:

* .NET SDK
* Trash

Then, follow this script to create an Antlr4 parser for a grammar.

    # Clone the grammars-v4 repo, pick a grammar, and generate a parser for the extension.
    git clone https://github.com/antlr/grammars-v4.git
    cd grammars-v4/java/java
    trgen; cd Generated; make

Then, clone this repo, set your ~/.grammar-location and ~/.grammar-classes, and run VSCode.

    echo "...../grammars-v4/java/java" > ~/.grammar-location
    cat << EOF > ~/.grammar-classes
    //classDeclaration/IDENTIFIER
    //methodDeclaration/IDENTIFIER
    //fieldDeclaration/variableDeclarators/variableDeclarator/variableDeclaratorId/IDENTIFIER
    EOF

    git clone https://github.com/kaby76/uni-vscode.git
    cd uni-vscode
    dotnet build
    cd VsCode
    bash clean.sh; bash install.sh
    code .
    # In VSCode, type F5.


## Implementation

* LSP server in C#.
* VSCode client code in Typescript.
* Grammars are implemented in Antlr4. The parser driver is implemented
using [trgen](https://github.com/kaby76/Domemtech.Trash/tree/main/trgen).
* The LSP server reads two files (.grammar-location and .grammar-classes)
in Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) which
are used to specify the location of the standardized Antlr4 parser and
classes of symbols in XPath notation.
