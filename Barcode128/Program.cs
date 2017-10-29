using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
            string barcode = Barcode.Code128.StringToBarcode(code);
            Console.Write(barcode);
        }
    }
}
