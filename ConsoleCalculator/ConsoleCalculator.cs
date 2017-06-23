using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using NCalc;
using System.Diagnostics;

namespace Honeywell.CodingChallenge.ConsoleCalculator
{
    public class ConsoleCalculator : ICalculator
    {
        static readonly double Pi = 3.1415927;
        static readonly double E = 2.71828;

        #region ctor(s)
        /// <summary>
        /// Creates a new instance of <see cref="Honeywell.CodingChallenge.ConsoleCalculator"/>.
        /// </summary>
        public ConsoleCalculator()
        {
            ValidInputChars = new string[] { "+", "-", "*", "/", "PI", "E" };
        }

        /// <param name="equation">Single line simple math equation.</param>
        public ConsoleCalculator(string equation) : this()
        {
            Equation = equation;
        }
        #endregion

        #region prop(s)
        /// <summary>
        /// Gets or sets(protected) the set of valid input characters. 
        /// </summary>
        public virtual string[] ValidInputChars { get; protected set; }
        /// <summary>
        /// Gets/sets the input for calculation.
        /// </summary>
        public virtual string Equation { get; set; }
        /// <summary>
        /// Gets/sets the sub-equations in order of operation.
        /// </summary>
        protected virtual Queue<IExpression> Expressions { get; set; }
        /// <summary>
        /// Gets/sets the hashtable that stores calculated expression variable results.
        /// </summary>
        protected virtual IDictionary<string, double> VariableResults { get; set; }
        #endregion

        #region method(s)
        /// <summary>
        /// Calculates the input line.
        /// </summary>
        /// <returns>Calculated expression result</returns>
        public virtual double GetCalculatedResults()
        {
            try
            {                
                string[] expressionBlocks = GetEquationBlocks();
                BuildExpressionQueue(expressionBlocks);
                double results = GetExpressionResult();
                return results;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.GetCalculatedRestuls(): {0}", ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Validates the input and creates the expression blocks from the equation string.
        /// </summary>
        /// <param name="equation">Equation string</param>
        /// <returns>string[]</returns>
        protected virtual string[] GetEquationBlocks()
        {
            if(String.IsNullOrEmpty(Equation))
                throw new ArgumentException("Please enter a valid equation.  Equation can't be null or empty.");
                            
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] asciiCodes = ascii.GetBytes(this.Equation);
            StringBuilder builder = new StringBuilder();
                        
            try
            {
                foreach (byte asciiCode in asciiCodes)
                {
                    bool isOperator = false;

                    switch (asciiCode)
                    {
                        // operators (*,/,+,-)
                        case 42:  // *
                        case 43:  // +
                        case 45:  // -
                        case 47:  // /
                            isOperator = true;
                            Debug.WriteLine(String.Format("op:{0}", Char.ConvertFromUtf32(asciiCode)));
                            break;
                        // integers (0-9)
                        case 48:  // 0
                        case 49:  // 1
                        case 50:  // 2
                        case 51:  // 3
                        case 52:  // 4
                        case 53:  // 5
                        case 54:  // 6
                        case 55:  // 7
                        case 56:  // 8
                        case 57:  // 9
                            Debug.WriteLine(String.Format("int:{0}", Char.ConvertFromUtf32(asciiCode)));
                            break;
                        case 69:  // E
                            Debug.WriteLine(String.Format("char:{0}", Char.ConvertFromUtf32(asciiCode)));
                            break;
                        case 80:  // P
                            Debug.WriteLine(String.Format("char:{0}", Char.ConvertFromUtf32(asciiCode)));
                            break;
                        case 73:  // I
                            Debug.WriteLine(String.Format("char:{0}", Char.ConvertFromUtf32(asciiCode)));
                            break;
                        default:
                            throw new ArgumentException(String.Format("Invalid input provided. {0}", Char.ConvertFromUtf32(asciiCode)));
                    }

                    // add pipe delimiter around operators
                    if (isOperator)
                        builder.Append(String.Format("|{0}|", Char.ConvertFromUtf32(asciiCode)));
                    else
                        builder.Append(Char.ConvertFromUtf32(asciiCode));
                }

                return builder.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.GetEquationBlocks(): {0}", ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Builds the expression queue by order of operations.
        /// </summary>
        /// <param name="equationBlocks"></param>
        protected virtual void BuildExpressionQueue(string[] equationBlocks)
        {
            const string Multiplication = "*";
            const string Division = "/";
            const string Addition = "+";
            const string Subtraction = "-";
            int expressionCount = 0;
            int indexCount = 0;
            object operand1;
            object operand2;
            string operandString;
            Expressions = new Queue<IExpression>();
            
            try
            {
                // Get multiplication or division expressions, left to right
                foreach (string eb1 in equationBlocks)
                {
                    if(eb1 != null) // ignore emptied equation blocks
                    {
                        if (eb1 == Multiplication || eb1 == Division)
                        {
                            ++expressionCount;
                            IExpression expression = new Expression();
                            expression.Name = String.Format("e{0}", expressionCount);
                            expression.Operation = (eb1 == Multiplication) ? ExpressionType.Multipllication : ExpressionType.Division;
                            #region left operand
                            operandString = equationBlocks[indexCount - 1];

                            // if the preceding equation block is null, walk back until you
                            // find an expression variable
                            if (String.IsNullOrEmpty(operandString))
                            {
                                for (int x = (indexCount - 2); x >= 0; x--)
                                {
                                    if (equationBlocks[x] != null)
                                    {
                                        if (equationBlocks[x].Contains("e"))
                                        {
                                            operandString = equationBlocks[x];
                                            break;
                                        }
                                    }
                                }
                            }

                            operand1 = GetOperand(operandString);

                            if (operand1 is String)
                                expression.Operand1Variable = (String)operand1;
                            else
                                expression.Operand1 = (Int32)operand1;
                            #endregion

                            #region right operand
                            operandString = equationBlocks[indexCount + 1];

                            // if the preceding equation block is null, walk forward until you
                            // find an expression variable
                            if (String.IsNullOrEmpty(operandString))
                            {
                                for (int x = (indexCount + 2); x < equationBlocks.Length; x++)
                                {
                                    if (equationBlocks[x] != null)
                                    {
                                        if (equationBlocks[x].Contains("e"))
                                        {
                                            operandString = equationBlocks[x];
                                            break;
                                        }
                                    }
                                }
                            }

                            operand2 = GetOperand(operandString);

                            if (operand2 is String)
                                expression.Operand2Variable = (String)operand2;
                            else
                                expression.Operand2 = (Int32)operand2;
                            #endregion

                            // add expression to queue
                            Expressions.Enqueue(expression);

                            // replace sub-equation with variable; places variable in left operand and removes operator and right operand items
                            equationBlocks[indexCount - 1] = expression.Name;
                            equationBlocks[indexCount + 1] = null;
                            equationBlocks[indexCount] = null;
                        }
                    }

                    indexCount++;
                }

                // reset index counter for reprocessing
                indexCount = 0;

                // Get addition and subtraction expressions, left to right
                foreach (string eb2 in equationBlocks)
                {
                    if (eb2 != null) // ignore emptied equation blocks
                    {
                        if (eb2 == Addition || eb2 == Subtraction)
                        {
                            ++expressionCount;
                            IExpression expression = new Expression();
                            expression.Name = String.Format("e{0}", expressionCount);
                            expression.Operation = (eb2 == Addition) ? ExpressionType.Addition : ExpressionType.Subtraction;

                            #region left operand
                            operandString = equationBlocks[indexCount - 1];

                            // if the preceding equation block is null, walk back until you
                            // find an expression variable
                            if(String.IsNullOrEmpty(operandString))
                            {
                                for(int x=(indexCount-2);x>=0;x--)
                                {
                                    if(equationBlocks[x] != null)
                                    {
                                        if (equationBlocks[x].Contains("e"))
                                        {
                                            operandString = equationBlocks[x];
                                            break;
                                        }
                                    }
                                }
                            }                            
                            
                            operand1 = GetOperand(operandString);

                            if (operand1 is String)
                                expression.Operand1Variable = (String)operand1;
                            else
                                expression.Operand1 = (Int32)operand1;
                            #endregion

                            #region right operand
                            operandString = equationBlocks[indexCount + 1];

                            // if the preceding equation block is null, walk forward until you
                            // find an expression variable
                            if (String.IsNullOrEmpty(operandString))
                            {
                                for (int x = (indexCount + 2); x < equationBlocks.Length; x++)
                                {
                                    if (equationBlocks[x] != null)
                                    {
                                        if (equationBlocks[x].Contains("e"))
                                        {
                                            operandString = equationBlocks[x];
                                            break;
                                        }
                                    }
                                }
                            }

                            operand2 = GetOperand(operandString);

                            if (operand2 is String)
                                expression.Operand2Variable = (String)operand2;
                            else
                                expression.Operand2 = (Int32)operand2;
                            #endregion

                            // add expression to queue
                            Expressions.Enqueue(expression);

                            // replace sub-equation with variable; places variable in left operand and removes operator and right operand items
                            equationBlocks[indexCount - 1] = expression.Name;
                            equationBlocks[indexCount + 1] = null;
                            equationBlocks[indexCount] = null;
                        }
                    }

                    indexCount++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.BuildExpressionQueue(): {0}", ex.Message);
                throw ex;
            }
        }

        protected virtual double GetExpressionResult()
        {
            try
            {
                double result = 0.00;
                VariableResults = new Dictionary<string, double>();

                while (Expressions.Count > 0)
                {
                    IExpression expression = Expressions.Dequeue();

                    // operand1 variable lookup
                    if (!String.IsNullOrEmpty(expression.Operand1Variable))
                        expression.Operand1 = GetExpressionVariableValue(expression.Operand1Variable);

                    // operand2 variable lookup
                    if (!String.IsNullOrEmpty(expression.Operand2Variable))
                        expression.Operand2 = GetExpressionVariableValue(expression.Operand2Variable);

                    // calculate expression result
                    switch (expression.Operation)
                    {
                        case ExpressionType.Division:
                            result = expression.Operand1 / expression.Operand2;
                            break;
                        case ExpressionType.Multipllication:
                            result = expression.Operand1 * expression.Operand2;
                            break;
                        case ExpressionType.Subtraction:
                            result = expression.Operand1 - expression.Operand2;
                            break;
                        case ExpressionType.Addition:
                            result = expression.Operand1 + expression.Operand2;
                            break;
                        default:
                            throw new NotSupportedException(String.Format("Currently, {0} is not a supported operation.", Enum.GetName(typeof(ExpressionType), expression.Operation)));
                    }

                    // add results to expression variable hashtable
                    VariableResults.Add(expression.Name, result);
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.GetExpressionResult(): {0}", ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the resolved variable value.
        /// </summary>
        /// <param name="expression"><see cref="Honeywell.CodingChallenge.ConsoleCalculator.IExpression"/></param>
        protected virtual double GetExpressionVariableValue(string variableName)
        {
            try
            {
                double variableValue = 0.00;

                if (variableName == "PI")
                    variableValue = Pi;
                else if (variableName == "E")
                    variableValue = E;
                else
                    variableValue = VariableResults[variableName];

                return variableValue;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.GetExpressionVariableValue(): {0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the operand from the provided string.
        /// </summary>
        /// <param name="operandString">Contains the operand value.</param>
        /// <returns>Formatted operand as string or int</returns>
        protected virtual object GetOperand(string operandString)
        {
            try
            {
                object operand = null;
                int op = -1;

                // gets the left operand, if integer conversion fails, it's PI or E
                if (!Int32.TryParse(operandString, out op))
                {
                    // assign strings directly if E, PI or expression variable
                    if (operandString.StartsWith("e") ||
                        operandString == "E" ||
                        operandString == "PI")
                        operand = operandString;
                    else
                        throw new ArgumentException(String.Format("Invalid argument provided. {0}", operandString));
                }
                else
                    operand = op;

                return operand;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ConsoleCalculator.GetOperand(): {0}", ex.Message);
                throw ex;
            }
        }
        #endregion
    }
}


