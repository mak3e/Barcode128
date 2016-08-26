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
            if (args.Length > 0){
                string code = string.Join("", args);
                Console.WriteLine(Barcode.Code128.StringToBarcode(code));
            }
        }
    }
}
