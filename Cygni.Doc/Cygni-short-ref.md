# A Short Reference for Cygni 1.0
Author: He Yanjie

# What is Cygni?
Cygni is a script programming language implemented in C\#. It is easy to use, has neat grammar and can interacts with C\#. It is convenient to wrap C\# classes as Cygni class, namely the Cygni libraries are based on the C\# class libraries.

Cygni is designed by me. I spent a lot of spare time on it, and I love it very much. I hope you will like it too!

# Core Language
## Reserved words
- and
- or
- not
- true
- false
- nil
- if
- else
- elif
- while
- for
- in
- def
- lambda
- class
- var
- break
- continue
- return	

## Reserved Symbols
- Add:+
- Subtract:- 
- Multiply:*
- Divide:/
- Integer Divide: //
- Modulo:\%
- Power: \^{}
- RightArrow: ->
- Concatenate: \&
- assign: =
- Equals:==
- Not Equals: !=
- Greater than: $>$
- Less than: $<$
- Greater than or Equals: $>=$
- Less than or Equals: $<=$
- Goes to: $=>$
- Parentheses: (\ )
- Brackets: [ \  ]
- Braces: \{ \ \}
- Colon: :
- Comma: ,

## Identifiers
The first character of identifiers should be underline or letters, the rest can be underlines, letters or numbers.
Note that the identifiers should not be the same as the reserved words.

## Comments
Line comment start with \#.

## Strings
String should be enclosed by '' or "". If there is symbol \@ at the start of string, then the escaped characters in the string will be ignored, and the ' or " in the string shoule be written twice.

## Types
Variables in Cygni don't have type. Only the values have.
- integer
- number
- boolean: true or false
- string
- list: lists contain elements from various types.
- dictionary: key-value pairs
- function
- native function: wrapper for C\# native functions
- tuple
- key value pair
- range
- struct
- class
- userdata: wrapper for C\# native classes.
- nil

## Control Statements
### If
'if' statement can have one or more braches.

```cygni
if condition {
	# Do something
} else {
	# Do something
}

if condition1 {
	# Branch 1
} elif condition2 {
	# Branch 2
} else {
	# Branch 3
}
```

### For
'for' statement iterates a collection. You may use range syntax to declare a iterable collection. The iterator shouldn't be changed during the iteration.

```cygni
for i = start: end {
	# Do something
}

for i = start: end: step {
	# Do something
}

for item in collection {
	# Do something
}
```

### While
'while' statement will keep running unless the condition is false.

```cygni
while condition {
	# Do something
}
```

### Break, Continue, Return

break, return exit the loop. continue stays in the loop.

## Range Constructor

```cygni
start: end
start: end: step
```

## List Constructor
```cygni
[item1, item2, ...]
```
## Dictionary Constructor
Note that dictionary only takes values of integer, boolean, string as keys.
```cygni
{key1 -> value1, key2 -> value2, ...}
```

## Function Definition
'def' statement declares a function with a name.
'lambda' statement declares an anonymous function, which contains a statement or a block.

```cygni
def FunctionName (arg1, arg2, ...) {
	# Do something	
}

a = lambda(arg1, arg2, ...) => # Expression

a = lambda(arg1, arg2,...) => {
	# Do something
}
```


## Invoke Function
```cygni
f(arg1, arg2, ...)
```

## Tuple Constructor
```cygni
a = tuple(10, 20)
```

## KeyValuePair Constructor
```cygni
a = key -> value
```
## Struct Constructor
The types of keys of structure can only be string.
```cygni
a = struct('key1' -> value1, 'key2' -> value2, ...)
print(a.key1)
```

## Class Definition
```cygni
class MyClass {
	# body
}

class DerivedClass: MyClass {
	# body
}
```

### Reserved Fields
- \_\_init\_\_
- \_\_add\_\_
- \_\_sub\_\_
- \_\_mul\_\_
- \_\_div\_\_
- \_\_mod\_\_
- \_\_pow\_\_
- \_\_unp\_\_
- \_\_unm\_\_
- \_\_cmp\_\_
- \_\_eq\_\_
- \_\_getItem\_\_
- \_\_setItem\_\_
- \_\_toStr\_\_


# Basic Library
## Executing
- source(fileName [,encoding]) 	description: execute a script file.
- require(fileName [,encoding]) description: execute a script file and return it as a module.
- import(fileName [,encoding]) 	description: execute a script file in the current global scope.

## Console output and input
- print(args) print arguments in the console, separated by tab.
- printf(content, args) print format string in the console. The arguments can be indexed by \{0\},\{1\}... in the string.
- input([content]) write the content in the console and waiting for user to input. The content can be omitted.


### Conversion
- int(a) convert certain value into integer.
- number(a) convert certain value into number.
- str(a) convert certain value into string.
- list(a) convert an iterable object into list.

## The Math Library
To simplify, all the arguments for the functions in math library only takes number as parameters. If the argument is an integer, it will be converted into number.

- math.sqrt(x)
- math.abs(x)
- math.log(x [,base])
- math.log10(x)
- math.max(args)
- math.min(args)
- math.exp(x)
- math.sign(x)
- math.sin(x)
- math.cos(x)
- math.tan(x)
- math.asin(x)
- math.acos(x)
- math.atan(x)
- math.sinh(x)
- math.cosh(x)
- math.tanh(x)
- math.ceiling(x)
- math.floor(x)
- math.round(x)
- math.truncate(x)

## The IO Library
To use the io library, you should use "import" or "require" function to load the library.

```cygni
import('IO')
IO = require('IO')
```
