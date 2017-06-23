using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Honeywell.CodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // console instructions
                Console.WriteLine("Please enter a single line math equation.  Only addition, \n" +
                                    "subtraction, multiplication and division are supported. Operands \n" +
                                    "can be integers, PI or E.\n\nPress the Esc key to exit.\n");
                InitializeAppLoop();
            }
            catch (Exception ex)
            {
                string message = String.Format("Error: {0}", ex.Message);
                Console.WriteLine(message);
                InitializeAppLoop();
            }
        }

        private static void InitializeAppLoop()
        {
            string equation;
            ICalculator calculator;
            int count = 0;
            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                Console.Write(">");
                equation = Console.ReadLine();
                calculator = new ConsoleCalculator(equation);
                double results = calculator.GetCalculatedResults();
                Console.WriteLine(String.Format("{0:0.####}", results));
                count++;
                consoleKeyInfo = Console.ReadKey(true);
            } while (consoleKeyInfo.Key != ConsoleKey.Escape);
        }
    }
}