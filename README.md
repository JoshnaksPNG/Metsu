# NewLangInterpreter
Interpreter for an as yet unnamed language

This language and its documentation is very much a work in progress.

## Style
NewLangInterpreter uses a C-style syntax with statements being terminated with a semicolon and code blocks contained within braces.

Control flow is accomplished with the `if`, `else`, `for`, `do`, and `while` keywords.

## Paradigm

NewLangInterpreter is currently a procedural/imperative language.

Unless inside a function definition, code is interpreted as it is encountered.

## Examples

### Hello World
`io.println("Hello, world!");`

Output:
`Hello, world!`

### Pascal's Combination
```
// Calculate the combination function C(n,k) via recursion using recursion ideas of Pascal's Triangle.
int pascals_combination(int n, int k)
{
  if ((k == 0) || (k == n))
  {
    return 1;
  }

  return pascals_combination(n - 1, k - 1) + pascals_combination(n - 1, k);
}

// Print the 4th row of Pascal's Triangle.  (n=0 is the first row)
io.println("The 4th row of Pascal's Triangle:");
io.println(pascals_combination(3,0));
io.println(pascals_combination(3,1));
io.println(pascals_combination(3,2));
io.println(pascals_combination(3,3));
```

Output:
```
The 4th row of Pascal's Triangle:
1
3
3
1
```

## Data Types
TODO

## Operators
TODO

## Functions
TODO

