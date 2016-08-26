using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Barcode
{
    /// <summary>
    /// Code 128
    /// Convert an input string to the equivilant string including start and stop characters.
    /// This object compresses the values to the shortest possible code 128 barcode format 
    /// </summarypublic
    static class Code128
    {
        /// <summary>
        /// Converts an input string to the equivilant Code 128 string (GS1-128)
        /// Application identifiers need to be in parentheses
        /// </summary>
        /// <param name="value">String to be encoded</param>
        /// <returns>Encoded string or empty string if input is invalid</returns>

        public static string StringToBarcode(string value)
        {
            string barcode = string.Empty;
            int num = 0;
            bool tableB = true;
            bool tswCl, tswBl;
            // Remove unnecessary characters
            value = value.Replace(" ", string.Empty);
            // Split string to parts if it contains application identifiers
            string[] parts = value.Split('(');

            // Validate and process string parts and construct barcode
            foreach (string part in parts)
            {
                if (IsValid(part))
                {
                    if (part.Length > 0)
                    {
                        string code = ProcessBarcodePart(part.Replace(")", string.Empty), num, tableB);
                        if (part.Contains(")"))
                        {
                            tswCl = "ÇÍ".Contains(code.Substring(0, 1));
                            tswBl = "Ì" == code.Substring(0, 1);

                            if (tswCl || tswBl)
                            {
                                code = code.Substring(0, 1) + "Ê" + code.Substring(1);
                                if (tswCl) {
                                    tableB = false;
                                }
                                if (tswBl) {
                                    tableB = true;
                                }
                            }
                            else {
                                code = "Ê" + code;
                            }
                        }
                        barcode += code;
                        num++;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            if (barcode.Length > 0)
            {

                // Calculation of the checksum
                int checksum = 0;
                int currentChar;
                for (int loop = 0; loop < barcode.Length; loop++)
                {
                    currentChar = (int)char.Parse(barcode.Substring(loop, 1));
                    currentChar = currentChar < 127 ? currentChar - 32 : currentChar - 100;
                    if (loop == 0)
                        checksum = currentChar;
                    else
                        checksum = (checksum + (loop * currentChar)) % 103;
                }

                // Calculation of the checksum ASCII code
                checksum = checksum < 95 ? checksum + 32 : checksum + 100;
                // Add the checksum and the STOP
                barcode += ((char)checksum).ToString() + ((char)206).ToString();

                return barcode;
            }
            return string.Empty;
        }

        private static bool IsValid(string value)
        {
            int currentChar;
            // Check for valid characters
            for (int charCount = 0; charCount < value.Length; charCount++)
            {
                currentChar = (int)char.Parse(value.Substring(charCount, 1));
                if (!(currentChar >= 32 && currentChar <= 126))
                {
                    return false;
                }
            }
            return true;

        }

        private static string ProcessBarcodePart(string value, int num, bool isTableB)
        {

            int charPos, minCharPos;
            int currentChar;
            string returnValue = string.Empty;

            if (value.Length > 0)
            {

                charPos = 0;
                while (charPos < value.Length)
                {
                    if (isTableB)
                    {
                        // See if interesting to switch to table C
                        // yes for 4 digits at start or end, else if 6 digits
                        if (charPos == 0 || charPos + 4 == value.Length)
                            minCharPos = 4;
                        else
                            minCharPos = 6;


                        minCharPos = IsNumber(value, charPos, minCharPos);

                        if (minCharPos < 0)
                        {
                            // Choice table C
                            if (charPos == 0 && num == 0)
                            {
                                // Starting with table C
                                returnValue = "Í";
                            }
                            else
                            {
                                // Switch to table C
                                returnValue += "Ç";
                            }
                            isTableB = false;
                        }
                        else
                        {
                            if (charPos == 0 && num == 0)
                            {
                                // Starting with table B
                                returnValue = "Ì";
                            }

                        }
                    }

                    if (!isTableB)
                    {
                        // We are on table C, try to process 2 digits
                        minCharPos = 2;
                        minCharPos = Code128.IsNumber(value, charPos, minCharPos);
                        if (minCharPos < 0) // OK for 2 digits, process it
                        {
                            currentChar = int.Parse(value.Substring(charPos, 2));
                            currentChar = currentChar < 95 ? currentChar + 32 : currentChar + 100;
                            returnValue += ((char)currentChar).ToString();
                            charPos += 2;
                        }
                        else
                        {
                            // We haven't 2 digits, switch to table B
                            returnValue += "È";
                            isTableB = true;
                        }
                    }
                    if (isTableB)
                    {
                        // Process 1 digit with table B
                        returnValue += value.Substring(charPos, 1);
                        charPos++;
                    }
                }
            }

            return returnValue;
        }


        private static int IsNumber(string InputValue, int CharPos, int MinCharPos)
        {
            // if the MinCharPos characters from CharPos are numeric, then MinCharPos = -1
            MinCharPos--;
            if (CharPos + MinCharPos < InputValue.Length)
            {
                while (MinCharPos >= 0)
                {
                    if ((int)char.Parse(InputValue.Substring(CharPos + MinCharPos, 1)) < 48
                        || (int)char.Parse(InputValue.Substring(CharPos + MinCharPos, 1)) > 57)
                    {
                        break;
                    }
                    MinCharPos--;
                }
            }
            return MinCharPos;
        }
    }
}