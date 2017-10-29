# Barcode128 Library
Generate Code128 barcodes from GS1-128 barcode numbers

Based on http://www.jtbarton.com/Barcodes/Code128.aspx

Code 128 font http://www.dafont.com/code-128.font

Executable https://github.com/mak3e/Barcode128/raw/master/Barcode128/bin/Release/Barcode128.exe

Executable Wrapper Usage
```
C:\path> Barcode128 "(application identifier) data ..."
```

Barcode.cs Library Usage
```C#
Code128.StringToBarcode("(application identifier) data ...")
```

Non-Alphanumeric characters are ignored. Supports multiple application identifiers.

More info https://en.wikipedia.org/wiki/GS1-128

List of application identifiers http://www.gs1-128.info/application-identifiers/

This program doesn't produce the most optimal results or the following reasons:

FUNC1 is added in front of all applications (this is not required if size of previous application is known)
Applications are not reordered to the optimal order


