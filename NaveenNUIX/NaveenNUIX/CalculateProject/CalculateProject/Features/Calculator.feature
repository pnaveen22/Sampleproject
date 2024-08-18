Feature: Calculator


Background:
	Given Launch Calculator Application

Scenario: Add two numbers
	And Enter first number as 50
	When the firstnumber is added with second number 70
	Then the result should be 120

Scenario: Add multiple numbers
	When I perform addition on "70,60,50,40,30,20,10" Number
	Then the result should be 280

Scenario: Substract two numbers
	And Enter first number as 60
	When the firstnumber is subtracted with second number 40
	Then the result should be 20

Scenario: Substract multiple numbers
	When I perform subtraction on "900,100,200,300,200,50,30" Number
	Then the result should be 20

Scenario: Multiply two numbers
	And Enter first number as 600
	When the second number 700 is multiplied
	Then the result should be 420000

Scenario: multiply multiple numbers
	When I perform multiplication on "9,6,2,4,3,7,8" Number
	Then the result should be 72576

Scenario: divided multiple numbers
	When I perform division on "1000,5,10,2" Number
	Then the result should be 10

Scenario: Division of two numbers
	And Enter first number as 3600
	When the firstnumber is divided by second number 60
	Then the result should be 60

Scenario Outline: ArithmeticOperation operations with different data
	When Enter "<FirstNumber>" and "<SecondNumber>" with "<arithmeticOperation>"
	Then the result should be <Result>

Examples:
	| FirstNumber | SecondNumber | Result | arithmeticOperation |
	| 40          | 50           | 90     | addition            |
	| 200         | 100          | 100    | subtraction         |
	| 25          | 25           | 625    | multiplication      |
	| 900         | 30           | 30     | division            |

