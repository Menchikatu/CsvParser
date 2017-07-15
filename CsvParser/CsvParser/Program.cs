using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

using Parser.CSV;
using Parser.Converter;

namespace CsvParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CsvParser");
            FileStream fs = new FileStream("CSVTest1.csv",
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            CsvParser parser = new CsvParser();
            parser.parse(new StreamReader(fs));
            Console.WriteLine(parser.ToString());
            //IntegerConverter iconv = new IntegerConverter();
            DecimalConverter dconv = new DecimalConverter();
            parser.get(0,0,dconv);
            Console.WriteLine(dconv.get());
            Console.ReadKey();
        }
    }
}
