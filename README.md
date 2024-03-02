# NewLangInterpreter
Interpreter for an as yet unnamed language

This language and its documentation is very much a work in progress.

## Interesting Features

- Integral constants of different bases:
	- Binary: 1294 = `0b10100001110`
	- Quaternary (base-4): 1294 = `0q110032`
	- Octal: 1294 = `0o2416`
	- Decimal: 1294 = `1294`
	- Hexadecimal: 1294 = `0x50E`
	- Duotridecimal (base-32): 1294 = `0v18E`
	- Hexatridecimal (base-36): 1294 = `0zZY`
- First Class Functions
	- Functions can be assigned to variables.
	- Functions can be passed as parameters to other functions.
	- Functions can be returned from other functions. 
- Variable Mutablility:
	- Variables are immutable by default
	- One can make variables mutable by default by typing `#mutable` at the top of the file

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
| Type Name  | Description | Special Values|
| - | - | - |
| `Null`  | Null value  | `null` |
| `bool`  | Boolean value  | `true`, `false` |
| `int` | Integer value | |
| `float` | Single-precision floating-point value | |
| `char` | Character value | |
| `string` | String value | |
| `obj` | Object value | |
| `void` | Absence of a value.  Used exclusively for the return type of functions that do not return any value. | |

## Operators

### bool
| Operator | Name | Description |
| - | - | - |
| `!` | Logical NOT | Returns the inverse boolean value to the value right of the operator. Returns boolean. |
| `&&` | Logical AND | Returns `true` if both values to right and left of operator are `true`, returns `false` otherwise. Returns boolean. |
| `||` | Logical OR | Returns `false` if both values to right and left of operator are `false`, returns `true` otherwise. Returns boolean. |

### int
| Operator | Name | Description |
| - | - | - |
| `+` | Integer Add | Adds two integers together. Returns an integer |
| `-` | Integer Subtract | Subtracts integer right of the operator from integer left of the operator. Returns an integer. |
| `*` | Integer Multiply | Multiplies two integers together. Returns an integer. |
| `\` | Integer Divide | Divides integer left of the operator by integer right of the operator. Returns an integer. |
| `%` | Integer Modulo | Returns the remainder of an integer division. Returns an integer. |
| `>` | Greater Than | Returns `true` if value on the left is greater than value on the right of operator. Returns a boolean. |
| `<` | Less Than | Returns `true` if value on the left is less than value on the right of operator. Returns a boolean. |
| `>=` | Greater Than Equal To | Returns `true` if value on the left is greater than or equal to value on the right of operator. Returns a boolean. |
| `<=` | Less Than Equal To | Returns `true` if value on the left is less than or equal to value on the right of operator. Returns a boolean. |
| `==` | Equal To | Returns `true` if value on the left is equal to value on the right of operator. Returns a boolean. |
| `!=` | Not Equal | Returns `true` if value on the left is not equal to value on the right of operator. Returns a boolean. |

### float
| Operator | Name | Description |
| - | - | - |
| `+` | Floating Point Add | Adds two floats together. Returns an float. |
| `-` | Floating Point Subtract | Subtracts float right of the operator from float left of the operator. Returns an float. |
| `*` | Floating Point Multiply | Multiplies two floats together. Returns an float. |
| `/` | Floating Point Divide | Divides float left of the operator by integer right of the operator. Returns an float. |
| `%` | Floating Point Modulo | Returns the remainder of an float division. Returns an float. |
| `>` | Greater Than | Returns `true` if value on the left is greater than value on the right of operator. Returns a boolean. |
| `<` | Less Than | Returns `true` if value on the left is less than value on the right of operator. Returns a boolean. |
| `>=` | Greater Than Equal To | Returns `true` if value on the left is greater than or equal to value on the right of operator. Returns a boolean. |
| `<=` | Less Than Equal To | Returns `true` if value on the left is less than or equal to value on the right of operator. Returns a boolean. |
| `==` | Equal To | Returns `true` if value on the left is equal to value on the right of operator. Returns a boolean. |
| `!=` | Not Equal | Returns `true` if value on the left is not equal to value on the right of operator. Returns a boolean. |

### char
| Operator | Name | Description |
| - | - | - |
| `+` | Character Concat | Concatenates two characters together into a string. Returns a string. |

### string
| Operator | Name | Description |
| - | - | - |
| `+` | String Concat | Concatenates two strings together. Returns a string. |

### obj
| Operator | Name | Description |
| - | - | - |
| `.` | Object Accessor | Accesses the property to the right of the operator of the object to the left of the operator. Returns any type. |

## Standard Modules

### chrono
| Function | Description |
| - | - |
| `int second()` | Returns the second field of the present time.  The range of the returned value is [0, 60]. (A value of 60 only appears during a [leap second](https://en.wikipedia.org/wiki/Leap_second).) |
| `int minute()` | Returns the minute field of the present time.  The range of the returned value is [0, 59]. |
| `int hour()` | Returns the hour field of the present time.  The range of the returned value is [0, 23]. |
| `int day()` | Returns the day field of the present date.  The range of the returned value is [1, 31]. |
| `int month()` | Returns the month field of the present date.  The range of the returned value is [1, 12]. |
| `int year()` | Returns the year field of the present date. |
| `void sleep(int ms)` | Puts the program to sleep for **ms** milliseconds. |
| `void sleep_second()` | Puts the program to sleep for 1 second. |

### io
| Function | Description |
| - | - |
| `void print(val)` | Converts **val** to a string and prints that string to the console. |
| `void println(val)` | Converts **val** to a string and prints that string to the console. |
| `string readln()` | Reads line of characters from the console and returns them as a string. |
| `void clearConsole()` | Clears the console. |
| `void consoleBackgroundColor(int color)` | Changes the background color of the console to **color**. |
| `void consoleTextColor(int color)` | Changes the foreground color of the console to **color**. |
| `string readTextFile(string path)` | Opens the file located at **path** as a text file and returns the contents as a string. |
| `void writeTextFile(string path, string contents)` | Creates a text file at **path** and writes **contents** to that file. |

### math
| Function | Description |
| - | - |
| `int randomInt(int a, int b)` | Returns an integer uniform deviate in the range of [**a**,**b**).  **a** may be returned, but **b** cannot be returned.|
| `int randomInt()` | Synonymous with `randomInt(0, 2147483648)`.|
| `float randomFloat()` | Returns a floating point uniform deviate in the range of [0, 1.0) |
