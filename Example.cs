using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cw2{
    class Example{
        static void main(){
            Console.WriteLine(Regex.IsMatch("GŁOS 3", "^GŁOS [0-9]+$"));
            Console.WriteLine(Regex.IsMatch("GŁOS 1", "$GŁOS [0-9]*$"));

            string test = Console.ReadLine();
            Console.WriteLine(test);
            Console.WriteLine(Regex.IsMatch(test, "^GŁOS [0-9]+$"));

        }
    }
}