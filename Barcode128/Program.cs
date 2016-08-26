using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = string.Empty;
            if (args.Length > 0)
            {
                code = string.Join("", args);

            }
            else {
                code = Console.ReadLine();
            }
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write(Barcode.Code128.StringToBarcode(code));
        }
    }
}
