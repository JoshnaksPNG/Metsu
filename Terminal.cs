using NewLangInterpreter.Frontend;
using NewLangInterpreter.Runtime;
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
        string input = Console.ReadLine();

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

Console.WriteLine("Type Repl for Repl, or Run to run test file");//Console.WriteLine("Type Repl for Repl, or Run {path} to run a file");
string choice = Console.ReadLine();

if (choice.ToLower() == "exit")
{
    System.Environment.Exit(0);
} else if(choice.ToLower() == "repl")
{
    repl();
}
else if (choice.ToLower().StartsWith("run"))
{
    //string path = choice.Split(' ')[1];
    //run(path);
    //run("C:\\git\\NewLangInterpreter\\test.txt");
    run("C:\\git\\NewLangInterpreter\\clock.txt");
}