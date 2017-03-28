using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrigerV2._0
{
    class Program:ProcessActions
    {
        static void Main(string[] args)
        {
            FileInitialization();
            Add(); Add(); Add();
            FLAG = "h";
            Console.WriteLine(JSON_GETDATA());
            Console.ReadLine();
        }
    }
}
