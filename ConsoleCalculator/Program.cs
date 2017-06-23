using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Honeywell.CodingChallenge.ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string equation;
            ICalculator calculator;
            int count = 0;
            ConsoleKeyInfo consoleKeyInfo;

            // console instructions
            Console.WriteLine("Please enter a single line math equation.  Only addition, \n" +
                                "subtraction, multiplication and division are supported. Operands \n" +
                                "can be integers, PI or E.\n\nPress the Esc key to exit.\n");

            // app loop
            do
            {
                Console.Write(">");
                equation = Console.ReadLine();
                calculator = new ConsoleCalculator(equation);
                double results = calculator.GetCalculatedResults();
                Console.WriteLine(String.Format("{0:#.####}", results));
                count++;
                consoleKeyInfo = Console.ReadKey(true);
            } while (consoleKeyInfo.Key != ConsoleKey.Escape);
        }
    }
}