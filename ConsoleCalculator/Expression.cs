using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeywell.CodingChallenge
{
    public class Expression : IExpression
    {
        public string Name { get; set; }

        public double Operand1 { get; set; }

        public double Operand2 { get; set; }

        public ExpressionType Operation { get; set; }

        public string Operand1Variable { get; set; }

        public string Operand2Variable { get; set; }
    }
}
