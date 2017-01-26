# Cygni
A lightweight, object-oriented script interpreter implemented in C#.

The Cygni language is inspired by many script languages, such as Lua, Python, etc. 

## Screen shots
![image](https://github.com/JasonHe0727/Cygni/blob/master/Screenshots/Cygni_Screen_shot1.png)

## Installation 
Here are two options. 
The first option is to download the compiled interpreter.
- Download the program from 

[Cygni_CLI](https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Mono.Test/Cygni_CLI/Cygni_CLI/bin/Release)
- If you are using windows, just double click the Cygni_CLI.exe. 

- If you are using Linux, you need to install mono environment and input "mono Cygni_CLI.exe" in the terminal.


The second option is to compile one by yourself. 
- Download the Cygni Core from 

[Cygni](https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Mono/Cygni/bin/Release)

- Import the Cygni.dll to your project.

The API is very easy. Have a look at this example.
``` csharp
using System;
using Cygni;

namespace Cygni_CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Engine engine = Engine.CreateInstance ();
			engine.ExecuteInConsole ();
		}
	}
}
```

## Some tutorial examples
For more information, see the short reference in [Cygni-short-ref](https://github.com/JasonHe0727/Cygni/tree/master/Cygni.Doc/Cygni-short-ref.md)

### Factorial
``` cygni
def fact(n) {
	if n == 0 {
		return 1
	} else {
		return n * fact(n - 1)
	}
}
print(fact(10)) # 3628800
```
### Calculate the sum of a list
``` cygni
def sum(myList) {
	var s = 0
	for v in myList {
		s = s + v
	}
	return s
}
print(sum([1,2,3,4,5])) # 15
```

### Define a class and a derived class
``` cygni
class Person {

	var name, age

	def __init__(name, age) {
		this.name = name
		this.age = age
	}

	def say() {
		printf("{0} is a person.", this.name)
	}

}

class Employee: Person {

	var salary = 3000

	def say() {
		printf("{0} is an employee.", this.name)
	}

}

alice = Person("Alice", 35)
john = Employee("John", 40)

alice.say()
john.say()

print("John's salary : " & john.salary)
```
