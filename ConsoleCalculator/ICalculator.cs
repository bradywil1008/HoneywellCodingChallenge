using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeywell.CodingChallenge.ConsoleCalculator
{
    public interface ICalculator
    {
        string Equation { get; set ; }

        string[] ValidInputChars { get; }

        double GetCalculatedResults();
    }
}
