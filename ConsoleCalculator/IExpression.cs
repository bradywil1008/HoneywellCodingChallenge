using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeywell.CodingChallenge.ConsoleCalculator
{
    public interface IExpression
    {
        string Name { get; set; }

        double Operand1 { get; set; }

        string Operand1Variable { get; set; }

        double Operand2 { get; set; }

        string Operand2Variable { get; set; }

        ExpressionType Operation { get; set; }
    }
}
