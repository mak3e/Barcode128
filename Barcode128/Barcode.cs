using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Barcode
{
    /// <summary>
    /// Code 128
    /// Convert an input string to the equivalent barcode including start and stop characters.
    /// This object compresses the values to the shortest possible code 128 barcode format 
    /// </summarypublic>
    static class Code128
    {

        private static char FNC1 = (char)202;
        private static char STOP = (char)206;
        private static char CODEC = (char)199;
        private static char CODEB = (char)200;
        private static char STARTB = (char)204;
        private static char STARTC = (char)205;
        enum Code { B, C }

        /// <summary>
        /// Converts an input string to the equivalent Code 128 string (GS1-128)
        /// Application identifiers need to be in parentheses
        /// </summary>
        /// <param name="value">String to be encoded</param>
        /// <returns>Encoded string or empty string if input is invalid</returns>
        public static string StringToBarcode(string inputString)
        {
            string barcodeString = StringToBarcodeString(inputString);
            return BarcodeStringToBarcode(barcodeString);
        }

        private static string StringToBarcodeString(string inputString)
        {   
            Regex regex = new Regex("[^a-zA-Z0-9( -]");
            inputString = regex.Replace(inputString, "");
            return inputString.Replace('(', FNC1);
        }

        private static string BarcodeStringToBarcode(string barcodeString)
        {
            string barcode = String.Empty;
            char START = GetStartCode(barcodeString);
            barcode += START;
            barcode += BuildBarcode(GetCode(START), barcodeString);
            barcode += CountCheckSum(barcode);
            barcode += STOP;
            return barcode;
        }

        private static string BuildBarcode(Code code, string barcodeString)
        {
            string barcode = String.Empty;
            if (code == Code.C)
            {
                if (GetLeadingNumberCount(barcodeString) > 1)
                {
                    barcode += ValueToChar(int.Parse(barcodeString.Substring(0, 2)));
                    barcodeString = barcodeString.Substring(2);
                }
                else
                {
                    if (!barcodeString.StartsWith(FNC1.ToString()))
                    {
                        code = Code.B;
                        barcode += CODEB;
                    }
                    barcode += barcodeString.Substring(0, 1);
                    barcodeString = barcodeString.Substring(1);
                }

            }
            else
            {
                if (GetLeadingNumberCount(barcodeString) > 5)
                {
                    code = Code.C;
                    barcode += CODEC;
                }
                else
                {
                    barcode += barcodeString.Substring(0, 1);
                    barcodeString = barcodeString.Substring(1);
                }
            }
            if (barcodeString.Length > 0)
            {
                return barcode + BuildBarcode(code, barcodeString);
            }
            return barcode;
        }

        private static char GetStartCode(string barcodeString) {
            barcodeString = barcodeString.Replace(FNC1.ToString(), String.Empty);
            if (GetLeadingNumberCount(barcodeString)>=4){
                return STARTC;
            }
            return STARTB;
        }
            
        private static char CountCheckSum(string barcode) {
            int weight = 0;
            int sum = CharToValue(barcode.ToCharArray()[0]);
            foreach (char character in barcode) {
                sum += CharToValue(character) * weight;
                weight++;
            }
            return (char)ValueToChar(sum % 103);
        }

        private static Code GetCode(char character)
        {
            if (character == STARTC || character == CODEC)
                return Code.C;
            return Code.B;
        }

        private static char ValueToChar(int value)
        {
            int character = value + 32;
            if (character > 126) {
                character += 68;
            }
            return (char) character;
        }

        private static int GetLeadingNumberCount(string inputString)
        {
            int count = 0;
            foreach (char character in inputString)
            {
                if (char.IsNumber(character))
                {
                    count++;
                }
                else
                {
                    return count;
                }
            }
            return count;
        }


        private static int CharToValue(char character)
        {
            int value = (int)character - 32;
            if (value >= 163)
            {
                value -= 68;
            }
            return value;
        }
    }
}
