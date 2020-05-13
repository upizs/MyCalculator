using System;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace BetterCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            returnFocus();
        }

        #region Default constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Clearing buttons
        private void backspaceButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteValue();
            returnFocus();
        }

        
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            equationBox.Text = String.Empty;
            comunicationBox.Text = String.Empty;
            returnFocus();
        }

        #endregion

        #region Number buttons
        private void sevenButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("7");
            returnFocus();
        }

        private void fourButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("4");
            returnFocus();
        }

        private void oneButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("1");
            returnFocus();
        }

        private void eightButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("8");
            returnFocus();
        }

        private void fifeButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("5");
            returnFocus();
        }

        private void twoButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("2");
            returnFocus();
        }

        private void zeroButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("0");
            returnFocus();
        }

        private void nineButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("9");
            returnFocus();
        }

        private void sixButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("6");
            returnFocus();
        }

        private void threeButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("3");
            returnFocus();
        }

        private void dotButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue(".");
            returnFocus();

        }

        #endregion

        #region Operative Buttons

        private void closeBracketButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue(")");
            returnFocus();
        }

        private void openBracketButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("(");
            returnFocus();
        }

        private void divideButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("/");
            returnFocus();
        }

        private void multiplyButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("*");
            returnFocus(); 
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("-");
            returnFocus();
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue("+");
            returnFocus();
        }
        #endregion

        #region Equation Button
        private void equalsButton_Click(object sender, RoutedEventArgs e)
        {

            //set up priorities
            //set close button as cancel button
            comunicationBox.Text = GetResult(equationBox.Text);

            returnFocus();
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Checks for brackets and then calls the Calculate Input method
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        private string GetResult(string userInput)
        {

            try
            {
                userInput = CheckBrackets(userInput);

                userInput = ParseInput(userInput);
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return userInput;
        }

        /// <summary>
        /// Check and finds brackets and selects the inner operations. 
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        private string CheckBrackets(string userInput)
        {
            try
            {
                //Check if equation has no brackets, if not, then return string untouched.
                if (!userInput.Contains("(") && !userInput.Contains(")"))
                {
                    return userInput;
                }

                //set up while loop control
                var bracketsFound = true;

                //save equation 
                var equation = userInput;

                //gonna go through the equation and look for the brackets until there wont be any left. 
                while (bracketsFound)
                {
                    //Here both brackets should be in equation, however it is possible that one of the brackets is missing, or not equalt amount
                    //so I check the if both brackets are here. 
                    if (!equation.Contains(")"))
                        throw new InvalidOperationException("You are missing an closed bracket");

                    if (!equation.Contains("("))
                        throw new InvalidOperationException("You are missing an open bracket");

                    //looking for the last index to start with the most inner brackets
                    var openBracketIndex = equation.LastIndexOf('(');

                    //looking for the first index to start with the most inner brackets
                    var closeBracketIndex = equation.IndexOf(')');

                    //save the most inner operation.
                    var innerOperation = ParseInput(equation.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1));

                    //Check if before or after brackets exists number, if yes, add multiply operator to the innerOperation
                    //Also check if equation doesnt start with brackets (2+1)*2 becasue the index will be out of range
                    if (openBracketIndex > 0 && "1234567890".Any(c => equation[openBracketIndex-1] == c))
                        innerOperation = innerOperation.Insert(0, "*");

                    //Also check if equation deosnt end with brackets 2*(2+1) so index is not out of range
                    if (closeBracketIndex +1 < equation.Length && "1234567890".Any(c => equation[closeBracketIndex + 1] == c))
                        innerOperation += "*";

                    //check if there is no dot before or after brackets 
                    //also check that there is no errors with insuficiant index.
                    if ((openBracketIndex > 0 && equation[openBracketIndex - 1] == '.') || (closeBracketIndex + 1 < equation.Length && equation[closeBracketIndex + 1] == '.'))
                        throw new InvalidOperationException("Cannot use the period before or after brackets");

                    //remove the most inner operation and insert the result
                    equation = equation.Remove(openBracketIndex, closeBracketIndex - openBracketIndex + 1).Insert(openBracketIndex, innerOperation);

                    //check if more brackets exist
                    if (!equation.Contains("(") && !equation.Contains(")"))
                    {
                        bracketsFound = false;
                    }


                }
                return equation;
            }
            catch (Exception)
            {
                throw;
            }


        }


        /// <summary>
        /// Returns the focus to the equation Box
        /// </summary>
        private void returnFocus()
        {
            equationBox.Focus();
        }



        /// <summary>
        /// Inputs the value into the equation box and keeps the selector at the right
        /// place and lets to inout value in the middle of a statement.
        /// </summary>
        private void InputValue(string value)
        {
            //Save the selection start
            var selectionStart = equationBox.SelectionStart;

            //if selection lenght => 1 then remove all the selected
            if (equationBox.SelectionLength >= 1)
                equationBox.Text = equationBox.Text.Remove(equationBox.SelectionStart, equationBox.SelectionLength);

            //Insert the User value into the equation box incase 
            //the selecation has been in middle of the statement
            equationBox.Text = equationBox.Text.Insert(selectionStart, value);

            //Set the selectionstart to fallow the lenght of input
            equationBox.SelectionStart = selectionStart + value.Length;

            //reset the selection lenght
            equationBox.SelectionLength = 0;

        }



        /// <summary>
        /// Deletes a character right before the selection start
        /// </summary>
        private void DeleteValue()
        {
            var selectionStart = equationBox.SelectionStart;

            //check if there is something to delete
            if (equationBox.SelectionStart == 0)
            {
                return;
            }

            //check selection lenght in case of multiple character selection
            if (equationBox.SelectionLength >= 1)
            {
                //Here Everything happens at the same index
                equationBox.Text = equationBox.Text.Remove(equationBox.SelectionStart, equationBox.SelectionLength);

                equationBox.SelectionStart = selectionStart;
            }
            //otherwise just remove one character
            else
            {
                //Here the action happens at the index before selection start.
                //remove the character before selection start
                equationBox.Text = equationBox.Text.Remove(equationBox.SelectionStart - 1, 1);

                //keep the selection start at the same location, 
                //because the selection start all the time jumps back to the start
                //because the focus is jumping when buttons are being pressed.
                equationBox.SelectionStart = selectionStart - 1;
            }
        }



        /// <summary>
        /// Takes the user input and makes calculations by splitting string into left side and right side
        /// </summary>
        private string ParseInput(string userInput)
        {

             userInput = userInput.Replace(" ", "").ToLower();
            try
            {
                //check if equation box has input
                if (userInput.Length <= 0)
                    return String.Empty;


                var operation = new Operations();

                var leftSide = true;

                //look for priority operations '*/'
                var newInput = PriorityOperation(userInput, operation);

                for (int i = 0; i < newInput.Length; i++)
                {
                    var numbers = "0123456789.";

                    if (numbers.Any(c => newInput[i] == c))
                    {
                        //check if input is just one number in case of 4 or 2(4)3, then return input untouched
                        if (newInput.Length == 1)
                            return newInput;

                        //if its number or dot then build leftside if true or right side if false
                        if (leftSide)
                        {
                            operation.LeftSide = AddNumberPart(operation.LeftSide, newInput[i]);
                        }
                        else
                        {
                            operation.RightSide = AddNumberPart(operation.RightSide, newInput[i]);
                        }
                    }
                    //must be an operator or mistake
                    else
                    {

                        //check if the leftside has no value
                        if (operation.LeftSide.Length == 0)
                        {
                            //check if the first character is not minus, if it is add, to the left side
                            if (newInput[i] == '-')
                                operation.LeftSide += newInput[i];
                            else
                                throw new InvalidOperationException($"Cannot start equation with {newInput[i]}");
                        }

                        else
                        {
                            //if leftSide is false and another operator is being used, means the right side 
                            //needs to be caldulated with leftside and moved
                            if (!leftSide)
                            {
                                //check if the rightside has no value
                                if (operation.RightSide.Length == 0)
                                {
                                    //check if the character is not minus, if it is add, to the right side
                                    if (newInput[i] == '-')
                                        operation.RightSide += newInput[i];
                                    else
                                        throw new InvalidOperationException($"(+/*) Cannot fallow another operator");
                                }
                                else
                                {
                                    //calculate and put the result on the left side
                                    operation.LeftSide = CalculateOperation(operation);

                                    //Do this after leftside has been calculated with the old operator
                                    operation.OperationType = GetOperator(newInput[i]);

                                    //clean the right side so it can be built again.
                                    operation.RightSide = string.Empty;

                                }
                            }
                            else
                            {
                                ////If only leftside is available in case of 7*(-4)/2 then return just left side
                                //if (i == newInput.Length - 1)
                                //    return newInput;
                                ////set the operator and turn leftside false
                                operation.OperationType = GetOperator(newInput[i]);

                                leftSide = false;
                            }
                        }
                    }
                }
                return CalculateOperation(operation);
            }
            catch (Exception)
            {
                throw;
            }
            
        }



        /// <summary>
        /// Gets a character and sets up an operator type
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private OperationType GetOperator(char character)
        {
            switch (character)
            {
                case '+':
                    return OperationType.Plus;
                case '-':
                    return OperationType.Minus;
                case '/':
                    return OperationType.Divide;
                case '*':
                    return OperationType.Multiply;
                case 'x':
                    return OperationType.Multiply;
                default:
                    throw new InvalidOperationException($"Unknow operator type {character}");


            }


        }



        /// <summary>
        /// Adds the new number or dot and checks if the current number already is not a decimal
        /// </summary>
        /// <param name="currentNumber"></param>
        /// <param name="newChar"></param>
        /// <returns></returns>
        private string AddNumberPart(string currentNumber, char newChar)
        {
            //check if the new caracter is . and if currrent number already is a decimal
            if (newChar == '.' && currentNumber.Contains("."))
                throw new InvalidOperationException($"{currentNumber} already is decimal");

            //add the new character to the current number
            return currentNumber + newChar;
        }



        /// <summary>
        /// Converts string into double numbers, makes the calculation and returns the result as a string
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        private string CalculateOperation(Operations operation)
        {
            //check if leftside has any numbers
            if (operation.LeftSide.Length == 1 && operation.LeftSide == "-")
                throw new InvalidOperationException("Cannot calculate just - ");
            
            //check if righttside has any numbers
            if (operation.RightSide.Length == 1 && operation.RightSide == "-")
                throw new InvalidOperationException("Cannot calculate just - ");

            //check if only left side is available in case of -7 or 4*(-7) if so return just leftside
            if (operation.LeftSide.Length > 1 && operation.RightSide.Length < 1)
            {
                return operation.LeftSide;
            }

            //Convert the strings into actual numbers for calculation
            var leftNumber = Convert.ToDecimal(operation.LeftSide);
            var rightNumber = Convert.ToDecimal(operation.RightSide);

            switch (operation.OperationType)
            {
                case OperationType.Plus:
                    return (leftNumber + rightNumber).ToString();
                case OperationType.Minus:
                    return (leftNumber - rightNumber).ToString();
                case OperationType.Multiply:
                    return (leftNumber * rightNumber).ToString();
                case OperationType.Divide:
                    return (leftNumber / rightNumber).ToString();
                default:
                    throw new InvalidOperationException($"Operation unknown");
            }

        }


        /// <summary>
        /// Finds and solves priority operations
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="operation"></param>
        /// <returns>New string with pririties solved</returns>
        private string PriorityOperation(string userInput, Operations operation)
        {
            string newCalculation = string.Empty;

            //see if user input doestn contain priority operator
            if (!userInput.Contains('*') || !userInput.Contains('/') )
                return userInput;

            var leftSide = true;

            //search for priority operator
            for (int i = 0; i < userInput.Length; i++)
            {

                var numbers = "0123456789.";

                if (numbers.Any(c => userInput[i] == c))
                {
                    //build the left and right side
                    if (leftSide)
                    {
                        operation.LeftSide = AddNumberPart(operation.LeftSide, userInput[i]);
                    }
                    else
                    {
                        operation.RightSide = AddNumberPart(operation.RightSide, userInput[i]);
                    }
                }
                else if ("-+".Any(c => userInput[i] == c))
                {
                    //check if left side has any numbers and if it doesnt if the op is minus(checking for negative number)
                    if (operation.LeftSide.Length < 1 && userInput[i] == '-')
                        operation.LeftSide += userInput[i];
                    //check if the same thing about right side
                    //TODO: if we are still working on leftside and op is minus this will save minus on the right side,
                    //Could be solved by moving the if*/ before this one.
                    else if (leftSide == false && operation.RightSide.Length < 1 && userInput[i] == '-')
                        operation.RightSide += userInput[i];
                    else
                    {

                        if (operation.RightSide.Length >= 1)
                        {
                            operation.LeftSide = CalculateOperation(operation);

                            operation.RightSide = string.Empty;
                        }

                        //add the operator to the left side
                        operation.LeftSide += userInput[i];

                        //save the leftside to the new string
                        newCalculation += operation.LeftSide;

                        //clean the left side
                        operation.LeftSide = string.Empty;

                        //set the left side true in case there has been an operation before. (4*5+7/2)
                        leftSide = true;
                    }

                }
                else if ("*/".Any(c => userInput[i] == c))
                {
                    //check if Right side has a number in case this is second or third op (5*4/2) if so calculate both and reset the right side

                    if (!leftSide)
                    {
                        //if left side is false and there is another operator, check if right side has a number,
                        //if not, two priotiry operators has been used in a row
                        if (operation.RightSide.Length < 1)
                        {
                            throw new Exception($"Cannot use two priority operators in a row");
                        }

                        //otherwise calculate both sides, move the result to the left side and reset Right side
                        operation.LeftSide = CalculateOperation(operation);

                        operation.RightSide = string.Empty;
                    }

                    //check if left side has a number in case the equation starts with an operator
                    if (operation.LeftSide.Length < 1) 
                    {
                        //if newEquation is empthy, then equation has only started
                        if (newCalculation.Length < 1)
                            throw new Exception("Cannot start equation with an operator");

                        //otherwise there has been two operators in a row like 2+/9
                        throw new InvalidOperationException("Cannot use two operators at the same time");
                    }

                    operation.OperationType = GetOperator(userInput[i]);

                    leftSide = false;
                }
                }

            //save the last calculation to the new string
            newCalculation += CalculateOperation(operation);

            //reset all the operation values
            operation.LeftSide = string.Empty;
            operation.RightSide = string.Empty;

            //return the new string
            return newCalculation;
        }

        /// <summary>
        /// Changes the opacity to a needed level
        /// </summary>
        /// <param name="value"></param>
        private void OpacityChanger(double value)
        {
            Background.Opacity = value;
            buttonsGrid.Opacity = value;
            equationBox.Opacity = value;
            comunicationBox.Opacity = value;
        }

        #endregion

        #region Menu buttons

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            OpacityChanger(1);
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            OpacityChanger(0.3);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }


        
        #endregion

        //TODO: Find out how to change the mouseover

    }
}
