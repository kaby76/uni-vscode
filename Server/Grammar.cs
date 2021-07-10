using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public class Grammar
{
    public static Parser Parser { get; set; }
    public static Lexer Lexer { get; set; }
    public static ITokenStream TokenStream { get; set; }
    public static IParseTree Tree { get; set; }
    private static List<string> _classifiers = null;
    public static List<string> Classifiers
    {
        get
        {
            if (_classifiers == null)
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var grammar_classes = System.IO.File.ReadAllText(home + Path.DirectorySeparatorChar + ".grammar-classifiers");
                var cs = grammar_classes.Split("\n");
                _classifiers = cs.Select(c => c.Trim()).Where(c => c != "").ToList();
            }
            return _classifiers;
        }
    }
    private static List<string> _classes = null;
    public static List<string> Classes
    {
        get
        {
            if (_classes == null)
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var grammar_classes = System.IO.File.ReadAllText(home + Path.DirectorySeparatorChar + ".grammar-classes");
                var cs = grammar_classes.Split("\n");
                _classes = cs.Select(c => c.Trim()).Where(c => c != "").ToList();
            }
            return _classes;
        }
    }
    public static IParseTree Parse(string input)
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var grammar_location = System.IO.File.ReadAllText(home + Path.DirectorySeparatorChar + ".grammar-location");
        var path = grammar_location;
        var full_path = path + "\\Generated\\bin\\Debug\\net5.0\\";
        var exists = File.Exists(full_path + "Test.dll");
        if (!exists) full_path = path + "bin\\Debug\\net5.0\\";
        Assembly asm1 = Assembly.LoadFile(full_path + "Antlr4.Runtime.Standard.dll");
        Assembly asm = Assembly.LoadFile(full_path + "Test.dll");
        var xxxxxx = asm1.GetTypes();
        Type[] types = asm.GetTypes();
        Type type = asm.GetType("Program");
        var methods = type.GetMethods();
        MethodInfo methodInfo = type.GetMethod("Parse");
        object[] parm = new object[] { input };
        DateTime before = DateTime.Now;
        var res = methodInfo.Invoke(null, parm);
        var tree = res as IParseTree;
        var t2 = tree as ParserRuleContext;
        var m2 = type.GetProperty("Parser");
        object[] p2 = new object[0];
        var r2 = m2.GetValue(null, p2);
        Parser = r2 as Parser;
        var m3 = type.GetProperty("Lexer");
        object[] p3 = new object[0];
        var r3 = m3.GetValue(null, p3);
        Lexer = r3 as Lexer;
        var m4 = type.GetProperty("TokenStream");
        object[] p4 = new object[0];
        var r4 = m4.GetValue(null, p4);
        TokenStream = r4 as ITokenStream;
        Tree = tree;
        return Tree;
    }
}
