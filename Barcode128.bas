Attribute VB_Name = "Barcode"
Sub Barcode()
Attribute Barcode.VB_ProcData.VB_Invoke_Func = "Normal.NewMacros.Test"
    Dim result As String
    Dim path As String
    Dim text As String
    path = "C:\Admin\Barcode128"
    text = Selection.text
    text = Replace(text, vbCrLf, "")
    text = Replace(text, Chr(13), "")
    text = Replace(text, Chr(10), "")
    result = CreateObject("WScript.Shell").Exec(path & " " & text).stdout.readall()
    result = Replace(result, Chr(0), "")
    Selection.text = result
    Selection.Font.Name = "Code 128"
    Selection.Font.Size = 48
    Selection.MoveRight Unit:=wdCharacter, Count:=1
    Selection.Font.Name = "Calibri"
End Sub
