using NewLangInterpreter.Frontend;
using NewLangInterpreter.Runtime;
using NewLangInterpreter.src.Frontend;
using NewLangInterpreter.src.Compiler;
using System.IO;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


//Lexer lexer = new Lexer();
//List<Token> tokens;
//tokens = lexer.tokenize("mut int name = 9.8 + 0x1; mut bool is_true = false;");

//foreach (Token token in tokens)
//{
//    Console.WriteLine(token.ToString());
//}



//foreach(AST.Statement statement in program.body) 
//{
//    Console.WriteLine(statement.ToString());
//}

//Console.Write(program.body);


//Console.WriteLine(result.ToString());

void repl()
{
    Parser p = new Parser();
    Console.WriteLine("Repl v0.0.1");

    NewLangInterpreter.Runtime.Environment env = new NewLangInterpreter.Runtime.Environment();

    while (true)
    {
        Console.Write("> ");
        string? input = Console.ReadLine();

        if (input == null)
        {
            Console.WriteLine("Failed to read data from the console.");
            System.Environment.Exit(-1);
        }

        if (input == "exit")
        {
            System.Environment.Exit(0);
        }

        AST.Program program = p.produceAST(input);

        Values.RuntimeVal result = Interpreter.evaluate(program, env);

        Console.WriteLine(result.ToString());
    }

    
}

void run(string path)
{
    Parser p = new Parser();

    NewLangInterpreter.Runtime.Environment env = new NewLangInterpreter.Runtime.Environment();

    string input = File.ReadAllText(path);

    AST.Program program = p.produceAST(input);

    Values.RuntimeVal result = Interpreter.evaluate(program, env);

    //Console.WriteLine(result.ToString());
}

void comp(string path)
{
    Parser p = new Parser();

    NewLangInterpreter.Runtime.Environment env = new NewLangInterpreter.Runtime.Environment();

    string input = File.ReadAllText(path);

    AST.Program program = p.produceAST(input);

    Console.WriteLine(CIL_Emitter.get_node_instruction(program).body);

    //Values.RuntimeVal result = Interpreter.evaluate(program, env);

    //Console.WriteLine(result.ToString());
}

if (args.Length > 1)
{
    string appName = System.AppDomain.CurrentDomain.FriendlyName;
    Console.WriteLine("Usage:");
    Console.WriteLine("  " + appName + "             : Enter interpreter/REPL mode");
    Console.WriteLine("  " + appName + " script_file : Run script_file");
    System.Environment.Exit(-1);
}
else if (args.Length == 1)
{
    run(args.First());
    System.Environment.Exit(0);
}

Console.WriteLine("Type Repl for Repl, or Run {path} to run a file");
string? choice = Console.ReadLine();

if (choice == null)
{
    Console.WriteLine("Failed to read data from the console.");
    System.Environment.Exit(-1);
}
if (choice.ToLower() == "exit")
{
    System.Environment.Exit(0);
}
else if(choice.ToLower() == "repl")
{
    repl();
}
else if (choice.ToLower().StartsWith("run"))
{
    string[] cmdWords = choice.Split(' ');
    if (cmdWords.Length != 2)
    {
        Console.WriteLine("Only one argument may be passed to the run command.");
        System.Environment.Exit(-1);
    }
    string path = cmdWords[1];
    run(path);
}
else if (choice.ToLower().StartsWith("comp"))
{
    string[] cmdWords = choice.Split(' ');
    if (cmdWords.Length != 2)
    {
        Console.WriteLine("Only one argument may be passed to the comp command.");
        System.Environment.Exit(-1);
    }
    string path = cmdWords[1];
    comp(path);
}