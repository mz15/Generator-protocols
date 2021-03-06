'autor: Pleshkov Andrey, ITMO University, group 1652. 2015
Const sample1 = "Øàáëîí (çàùèòà ÂÊÐ).doc"   'Èìÿ ôàéëà-øàáëîíà ñ ðàñøèðåíèåì (ïî óìîë÷àíèþ)
Const folder_name1 = "Çàùèòà ÂÊÐ"           'Èìÿ ïàïêè, â êîòîðóþ ñãåíåðèðóþòñÿ ôàéëû (ïî óìîë÷àíèþ)
Const sample2 = "Øàáëîí (ãîñýêçàìåí).doc"
Const folder_name2 = "Ãîñýêçàìåí"
Const file_extensions = ".doc"              'Ðàñøèðåíèå ãåíåðèðóåìûõ ôàéëîâ
Const colomns = 19                          'Ïîñëåäíèé îáðàáàòûâàåìûé ñòîáöåö
Dim folder_location, file_folder, file_location, password, specialty_code, specialty, date_GEK, date_certificate, degree As String
Dim group, number_of_files, last_line_A, last_line_B, last_line_C, last_line_D, last_line_H As Integer
Dim Errors As Boolean

Function AssigningValues()
    password = "mz15"
    last_line_A = Cells(Rows.Count, "A").End(xlUp).Row  'Ïîñëåäíÿÿ çàïîëíåííàÿ ñòðîêà â ñòîëáöå
    last_line_B = Cells(Rows.Count, "B").End(xlUp).Row
    last_line_C = Cells(Rows.Count, "C").End(xlUp).Row
    last_line_D = Cells(Rows.Count, "D").End(xlUp).Row
    last_line_H = Cells(Rows.Count, "H").End(xlUp).Row
    number_of_files = last_line_B - 5   'Êîëè÷åñòâî ãåíåðèðóåìûõ ôàéëîâ (çàïîëíåííûõ ñòðîê)
    specialty_code = Range("B3")
    specialty = Range("C3")
    group = Range("D3")
    degree = Range("E3")
    date_GEK = Range("F3")
    date_certificate = Range("G3")
    Errors = False
End Function

Sub Auto_Open()
ActiveSheet.Unprotect (password)
Cells.Locked = True
Range("B3:G3").Locked = False
ActiveSheet.Protect (password)
End Sub

Sub Generation1()
    file_folder = Replace(ThisWorkbook.FullName, ThisWorkbook.name, folder_name1 & " " & group) 'Íàçâàíèå ïàïêè, â êîòîðóþ ñãåíåðèðóþòñÿ ôàéëû
    file_location = Replace(ThisWorkbook.FullName, ThisWorkbook.name, sample1)                  'Ðàñïîëîæåíèå øàáëîíà äëÿ ãåíåðèðóåìûõ ôàéëîâ
    Call Generation
End Sub

Sub Generation2()
    file_folder = Replace(ThisWorkbook.FullName, ThisWorkbook.name, folder_name2 & " " & group)
    file_location = Replace(ThisWorkbook.FullName, ThisWorkbook.name, sample2)
    Call Generation
End Sub

Function Generation()   'Ôóíêöèÿ ãåíåðàöèè ôàéëîâ

    Call AssigningValues
    If number_of_files < 1 Then MsgBox "Íå çàäàíû ïàðàìåòðû äëÿ ãåíåðàöèè ôàéëîâ!", vbCritical: Exit Function
    If last_line_A <> last_line_B Then MsgBox "Îäíà èëè íåñêîëüêî ñòðîê çàïîëíåíû íå ïîëíîñòüþ!", vbCritical: Exit Function
    If IsNumeric(group) And group <> "" Then Else: MsgBox "Íåêîððåêòíî ââåäåí íîìåð ãðóïïû!": Exit Function
    
    For m = 6 To last_line_B
        With Cells(m, "B")
                If InStr(.Text, "\") > 0 Or InStr(.Text, "/") > 0 Or InStr(.Text, "|") > 0 Or InStr(.Text, "?") > 0 Or InStr(.Text, """") > 0 Or _
                InStr(.Text, ":") > 0 Or InStr(.Text, "*") > 0 Or InStr(.Text, "<") > 0 Or InStr(.Text, ">") > 0 Then
                    Cells(m, "B").Select
                    MsgBox "Âûäåëåííàÿ ÿ÷åéêà ñîäåðæèò íåäîïóñòèìûå ñèìâîëû \ / : "" ? * < > |", vbCritical, "Îøèáêà!"
                    Exit Function
                End If
        End With
    Next
    
    
    Dim default_setting As String
    default_setting = MsgBox("Ïî óìîë÷àíèþ áóäåò èñïîëüçîâàí øàáëîí """ & file_location & """" & _
    vbNewLine & vbNewLine & "Ñãåíåðèðîâàííûå ïðîòîêîëû ÃÝÊ áóäóò ñîõðàíåíû â ïàïêó """ & file_folder & """" & _
    vbNewLine & vbNewLine & "Èñïîëüçîâàòü íàñòðîéêè ïî óìîë÷àíèþ?", vbYesNoCancel, "Èñïîëüçîâàòü íàñòðîéêè ïî óìîë÷àíèþ?")

    If default_setting = vbCancel Then Exit Function
    If default_setting = vbYes Then
        
        If Dir(file_location) = "" Then MsgBox "Ôàéë  """ & file_location & """  íå íàéäåí", vbExclamation, "Ôàéë íå íàéäåí": GoTo select_sample:                                                                                   'Ïðîâåðêà íà ñóùåñòâîàíèå ôàéëà

        On Error GoTo folder_exists
        MkDir file_folder 'Ñîçäàíèå íîâîãî êàòàëîãà (MkDir ðàñïîëîæåíèå)
        folder_location = file_folder & Application.PathSeparator   'Äîáàâëÿåì â êîíåö ïóòè "\"
        GoTo no_error
        Exit Function
        
folder_exists:  MsgBox "Ïàïêà """ & file_folder & """ óæå ñóùåñòâóåò. Âûáåðåòå äðóãóþ ïàïêó.", vbExclamation: GoTo select_folder

    Else:
select_sample:

        With Application.FileDialog(msoFileDialogFilePicker)    'Äèàëîã çàïðîñà âûáîðà ïàïêè
'            .InitialFileName =
            .Title = "Âûáåðåòå ôàéë-øàáëîí"
            .AllowMultiSelect = False
            .ButtonName = "Âûáðàòü"
            .Show
            If .SelectedItems.Count = False Then Exit Function  'Åñëè íè÷åãî íå âûáðàíî, ïðîãðàììà çàâåðøàåòñÿ
            file_location = .SelectedItems(1)                   'Ðàñïîëîæåíèå ôàéëà-øàáëîíà
        End With

select_folder:

        With Application.FileDialog(4)  '(msoFileDialogFolderPicker) - äèàëîã çàïðîñà âûáîðà ôàéëà
'            .InitialFileName =
            .Title = "Âûáåðåòå ïàïêó, êóäà áóäóò ñîõðàíåíû ïðîòîêîëû ÃÝÊ"
            .AllowMultiSelect = False
            .ButtonName = "Âûáðàòü"
            If .Show = False Then Exit Function                         'Åñëè íè÷åãî íå âûáðàíî, ïðîãðàììà çàâåðøàåòñÿ
            file_folder = .SelectedItems(1)                             'Ðàñïîëîæåíèå âûáðàííîé ïàïêè, â êîòîðóþ áóäóò ñîõðàíÿòüñÿ ôàéëû
            folder_location = file_folder & Application.PathSeparator   'Äîáàâëåíèå \
        End With
    End If

no_error:

    Dim line As Range
    Dim WA As Object, WD As Object: Set WA = CreateObject("Word.Application")
    
    For Each line In ActiveSheet.Rows("6:" & last_line_B)               'Öèêë ñ 6 ñòðîêè äî ïîñëåäíåé çàïîëíåííîé
        With line
            name = Trim$(.Cells(2))                                     'Ïðèñâîåíèå èìåí ãåíåðèðóåìûì ôàéëàì
            file_name = folder_location & name & file_extensions        'Ðàñïîëîæåíèå, èìÿ è ðàñøèðåíèå ñãåíåðèðîâàííûõ ôàéëîâ

            Set WD = WA.Documents.Add(file_location): DoEvents
            
            For i = 2 To colomns                                        'Öèêë ñ 1 ñòîëáöà äî ïîñëåäíåãî îáðàáàòûâàåìîãî ñòîëáöà
                FindText = Cells(4, i): ReplaceText = Trim$(.Cells(i))  'Ïîèñê ñîâïàäåíèé ñ ìåòêàìè è çàìåíà íà çíà÷åíèÿ â áàçå äàííûõ

                With WD.Range.Find
                    .Text = FindText                                    'Ìåòêà â øàáëîíå
                    .Replacement.Text = ReplaceText                     'Çàìåíà ìåòêè òåêñòîì èç áàçû äàííûõ
                    .Forward = True                                     'Íàïðàâëåíèå ïîèñêà (âïåðåä/íàçàä)
                    .MatchCase = True                                   'Ó÷èòûâàòü ðåãèñòð
                    .MatchWholeWord = True                              'Èñêàòü ñëîâî öåëèêîì
                    .MatchAllWordForms = False                          'Âñå ñëîâîôîðìû
                    .Execute Replace:=2                                 'Çàìåíèòü âñ¸
                End With
                DoEvents
            Next i
            
            WD.SaveAs file_name: WD.Close False: DoEvents   'Ñîõðàíåíèå è çàêðûòèå ñîçäàííîãî ôàéëà
        End With
    Next line   'Ïåðåõîä ê ñëåäóþùåé èòåðàöèè öèêëà (ñòðîêå áàçû äàííûõ)
    
    WA.Quit False
    MsgBox "Â ïàïêå """ & file_folder & """ ñîçäàíî " & number_of_files & " ôàéëîâ.", vbInformation, "Ãåíåðàöèÿ ïðîòîêîëîâ ÃÝÊ çàâåðøåíà"
End Function

Sub Past()
    Call AssigningValues
    If Range("B6") <> "" Then MsgBox "Ñíà÷àëà íåîáõîäèìî î÷èñòèòü ñîäåðæèìîå ÿ÷ååê", vbCritical: Exit Sub
    ActiveSheet.Unprotect (password)
    Cells.Locked = False
    Range("B6").Select
    On Error GoTo no_text
    ActiveSheet.PasteSpecial Format:="Òåêñò", Link:=False, DisplayAsIcon:=False
    Selection.Rows.AutoFit
    Cells.Locked = True
    Range("B3:G3").Locked = False
    ActiveSheet.Protect (password)
    Exit Sub
no_text:
    MsgBox "Â áóôåðå îáìåíà äîëæíû ñîäåðæàòüñÿ äàííûå èç ôàéëà MSWord ñî ñïèñêîì òåì è ðóêîâîäèòåëåé!", vbCritical, "Îøèáêà!"
End Sub

Sub UnlockCells()
    ActiveSheet.Unprotect (password)
    Cells.Locked = False
    Range("A1:R5").Locked = True
    Range("B3:G3").Locked = False
    ActiveSheet.Protect (password)
End Sub

Sub Clearing()
    Dim last_line As Integer

    ActiveSheet.Unprotect (password)
    Cells.Locked = False
    For m = 1 To colomns
        last_line = Cells(Rows.Count, m).End(xlUp).Row
        If last_line < 6 Then Else Rows("6:" & last_line).ClearContents
    Next m
    Cells.Locked = True
    Range("B3:G3").Locked = False
    ActiveSheet.Protect (password)
    
    If Range("B3") <> "" Or Range("C3") <> "" Then If MsgBox("Óäàëèòü êîä è íàçâàíèå ñïåöèàëüíîñòè?", vbYesNo) = vbYes Then Range("B3, C3").ClearContents
    If Range("D3") <> "" Then If MsgBox("Óäàëèòü íîìåð ãðóïïû?", vbYesNo) = vbYes Then Range("D3").ClearContents

End Sub

Sub FillingColumns()
    Call AssigningValues
    Dim i, m As Integer

    If number_of_files < 1 Then MsgBox "Íå çàäàíû ïàðàìåòðû äëÿ ãåíåðàöèè ôàéëîâ!", vbCritical: Exit Sub
    If last_line_B <> last_line_C Or last_line_B <> last_line_D Then MsgBox "Ïîñëåäíÿÿ ñòðîêà çàïîëíåíà íå ïîëíîñòüþ!", vbCritical: Exit Sub

    For i = 6 To last_line_B
        If Range("C" & i) = "" Or Range("D" & i) = "" Then
        Range("B" & i & ":D" & i).Select
        MsgBox "Âûäåëåííàÿ ñòðîêà çàïîëíåíà íå ïîëíîñòüþ!", vbCritical: Exit Sub
        End If
    Next i
    
    If specialty_code = "" Or specialty = "" Or group = "" Or degree = "" Or date_GEK = "" Or date_certificate = "" Then
        MsgBox "Íå ïîëíîñòüþ çàïîëíåíà ñòðîêà ñ îáùèìè äàííûìè!": Exit Sub
    End If
        
    If IsNumeric(group) Then
    Else: MsgBox "Íåêîððåêòíî ââåäåí íîìåð ãðóïïû!"
        Range("D3").Select
        Exit Sub
    End If
    
    For m = 6 To last_line_B
        With Cells(m, "B")
                If InStr(.Text, "\") > 0 Or InStr(.Text, "/") > 0 Or InStr(.Text, "|") > 0 Or InStr(.Text, "?") > 0 Or InStr(.Text, """") > 0 Or _
                InStr(.Text, ":") > 0 Or InStr(.Text, "*") > 0 Or InStr(.Text, "<") > 0 Or InStr(.Text, ">") > 0 Then
                    Cells(m, "B").Select
                    MsgBox "Âûäåëåííàÿ ÿ÷åéêà ñîäåðæèò íåäîïóñòèìûå ñèìâîëû \ / : "" ? * | < >", vbCritical, "Îøèáêà!"
                    Exit Sub
                End If
        End With
    Next
    
    ActiveSheet.Unprotect (password)
    Cells.Locked = False

    Call GenitiveStudents
    
    If Errors = True Then
        Call Clearing2
        GoTo Error_yes
    End If
    
    Call GenitiveProfessor
    
    If Errors = True Then
        Call Clearing2
        GoTo Error_yes
    End If
    
    
    Call FillingFormulas
    
    If Errors = True Then
        Call Clearing2
        GoTo Error_yes
    End If
    
Error_yes:
    Cells.Locked = True
    Range("B3:G3").Locked = False
    ActiveSheet.Protect (password)
End Sub

Function FillingFormulas()
    Call AssigningValues
    Dim range_degree1, range_degree2, range_degree3, range_degree4, range_degree5 As String
    
    Range("A6:A" & last_line_B).Formula = "=ROW(A1)"                  'Çàïîëíåíèå ñòîëáöà ñ íóìåðàöèåé
    Range("E6:E" & last_line_B).Formula = Range("E1").Formula         'Ïðåîáðàçîâàíèå ïîëíîãî ÔÈÎ ñòóäåíòà â Ôàìèëèÿ È.Î.
    Range("G6:G" & last_line_D).Formula = Range("G1").Formula         'Ïðåîáðàçîâàíèå ïîëíîãî ÔÈÎ ðóêîâîäèòåëÿ â Ôàìèëèÿ È.Î.
    
    If degree = "áàêàëàâð" Or degree = "ñïåöèàëèñò" Or degree = "ìàãèñòð" Then
        If degree = "áàêàëàâð" Then
            range_degree1 = "áàêàëàâðà"
            range_degree2 = "áàêàëàâðèàò"
            range_degree3 = "ÂÊÐ"
            range_degree4 = "ÂÊÐ"
            range_degree5 = "ÂÊÐ"
        End If
        If degree = "ìàãèñòð" Then
            range_degree1 = "ìàãèñòðà"
            range_degree2 = "ìàãèñòðàòóðó"
            range_degree3 = "Ìàãèñòåðñêàÿ äåññåðòàöèÿ"
            range_degree4 = "ìàãèñòåðñêîé äèññåðòàöèè"
            range_degree5 = "ìàãèñòåðñêóþ äèññåðòàöèþ"
        End If
        If degree = "ñïåöèàëèñò" Then
            range_degree1 = "ñïåöèàëèñòà"
            range_degree2 = "ñïåöèàëèòåò"
            range_degree3 = "ÂÊÐ"
            range_degree4 = "ÂÊÐ"
            range_degree5 = "ÂÊÐ"
        End If
    Else:
        MsgBox "Íåâåðíî óêàçàíà ïðèñâàèâàåìàÿ ó÷åíàÿ ñòåïåíü! Èñïðàâüòå îøèáêó è ïîâòîðèòå çàïîëíåíèå ñòîëáöîâ!", vbCritical, "Îøèáêà!"
        Errors = True: Exit Function
    End If
    
    Range("I6:I" & last_line_B) = group
    Range("J6:J" & last_line_B) = specialty
    Range("K6:K" & last_line_B) = specialty_code
    Range("M6:M" & last_line_B) = range_degree1
    Range("N6:N" & last_line_B) = date_GEK
    Range("O6:O" & last_line_B) = date_certificate
    Range("P6:P" & last_line_B) = range_degree2
    Range("Q6:Q" & last_line_B) = range_degree3
    Range("R6:R" & last_line_B) = range_degree4
    Range("S6:S" & last_line_B) = range_degree5
End Function

Function Clearing2()
    Dim last_line As Integer
    For m = 5 To colomns
        last_line = Cells(Rows.Count, m).End(xlUp).Row
        If last_line > 5 Then Range("E6:R" & last_line).ClearContents
    Next m
End Function

Function GenitiveStudents() 'Ôóíêöèÿ ñêëîíåíèÿ ÔÈÎ ñòóäåíòîâ â ðîäèòåëüíûé ïàäåæ
    Call AssigningValues
    Dim surname As String, name As String, patronymic As String, text_msg As String
    
    For i = 6 To last_line_B
        Dim fname$(): fname = Split(Cells(i, "B")) 'Ïðåîáðàçîâàíèå â ìàññèâ ñ òðåìÿ ýëåìåíòàìè
        
        text_msg = "Âûäåëåííàÿ ÿ÷åéêà çàïîëíåíà íåêîððåêòíî." & _
                    " Èñïðàâüòå îøèáêó è ïîâòîðèòå çàïîëíåíèå!"
        If UBound(fname) < 2 Then
            Range("B" & i).Select
            MsgBox text_msg, vbCritical, "Îøèáêà â ñòîëáöå ""ÔÈÎ ñòóäåíòà"""
            Errors = True: Exit Function
        End If
        
        If Len(fname(0)) = 0 Or Len(fname(1)) = 0 Then
            Range("B" & i).Select
            MsgBox text_msg, vbCritical, "Îøèáêà â ñòîëáöå ""ÔÈÎ ñòóäåíòà"""
            Errors = True: Exit Function
        End If
        
        surname = fname(0)
        name = fname(1)
        patronymic = fname(2)
        Cells(i, "F") = genitive(surname, name, patronymic)
        
        If Right(patronymic, 1) <> "÷" Then Cells(i, "L") = "à" 'Îêîí÷àíèå â çàâèñèìîñòè îò ïîëà
    Next
End Function

Function GenitiveProfessor() 'Ôóíêöèÿ ñêëîíåíèÿ ÔÈÎ ðóêîâîäèòåëåé â ðîäèòåëüíûé ïàäåæ
    Call AssigningValues
    Dim fio As Integer
    Dim surname As String, name As String, patronymic As String, text_msg As String
    
    For i = 6 To last_line_D 'Öèêë âûäåëåíèÿ ÔÈÎ ðóêîâîäèòåëåé èç ñòîëáöà D
        With Cells(i, "D")
            On Error GoTo error_full_name
                fio = InStr(.Text, ",") 'Îïðåäåëåíèå ïîçèöèè ïåðâîé çàïÿòîé
                Cells(i, "H") = Left(.Text, fio - 1)    'Îñòàâëÿåì òîëüêî ÔÈÎ
        End With
    Next

    For i = 6 To last_line_D
        Dim fname$(): fname = Split(Cells(i, "H"))  'Ïðåîáðàçîâàíèå â ìàññèâ ñ òðåìÿ ýëåìåíòàìè
        
        If Len(fname(0)) = 0 Or Len(fname(1)) = 0 Then GoTo error_full_name
        surname = fname(0)
        name = fname(1)
        patronymic = fname(2)
        Cells(i, "H") = genitive(surname, name, patronymic)
    Next
    GoTo no_error_full_name
error_full_name:
    Range("D" & i).Select
    text_msg = "Âûäåëåííàÿ ÿ÷åéêà çàïîëíåíà íåêîððåêòíî." & _
                    " Èñïðàâüòå îøèáêó è ïîâòîðèòå çàïîëíåíèå!"
    MsgBox text_msg, vbCritical, "Îøèáêà â ñòîëáöå ""ÔÈÎ, ó÷. ñòåïåíü, ó÷. çíàíèå, äîëæíîñòü"""
    Errors = True
no_error_full_name:
End Function

Function genitive(surname As String, name As String, patronymic As String) As String
    Dim fMan As Boolean
    fMan = (Right(patronymic, 1) = "÷") 'Îïðåäåëåíèå, ìóæñêîå ÔÈÎ èëè æåíñêîå

    If surname <> "" Then
        If fMan Then 'Ñêëîíåíèå ìóæñêîé ôàìèëèè
            Select Case Right(surname, 1)
                Case "î", "è", "ÿ", "õ", "þ", "ó"
                    genitive = surname
                Case "é"
                    genitive = Mid(surname, 1, Len(surname) - 2) & "îãî"
                Case "à"
                    genitive = Mid(surname, 1, Len(surname) - 1) & "û"
            Case Else
                    genitive = surname & "à"
            End Select
        Else 'Ñêëîíåíèå æåíñêîé ôàìèëèè
            Select Case Right(surname, 1)
                Case "î", "è", "á", "â", "ã", "ä", "æ", "ç", "ê", "ë", "ì", "í", "ï", "ð", "ñ", "ò", "ô", "õ", "ö", "÷", "ø", "ù", "ü"
                    genitive = surname
                Case "ÿ"
                    genitive = Mid(surname, 1, Len(surname) - 2) & "îé"
            Case Else
                genitive = Mid(surname, 1, Len(surname) - 1) & "îé"
            End Select
        End If
    End If
    
    genitive = genitive & " " 'Äîáàâëåíèå ïðîáåëà ïîñëå ôàìèëèè
    
    If name <> "" Then
        If fMan Then 'Ñêëîíåíèå ìóæñêîãî èìåíè
            Select Case Right(name, 1)
                Case "é", "ü"
                    genitive = genitive & Mid(name, 1, Len(name) - 1) & "ÿ"
                Case "à"
                    genitive = genitive & Mid(name, 1, Len(name) - 1) & "û"
            Case Else
                genitive = genitive & name & "à"
            End Select
        Else 'Ñêëîíåíèå æåíñêîãî èìåíè
            Select Case Right(name, 1)
                Case "à"
                    Select Case Mid(name, Len(name) - 1, 1)
                        Case "è", "ã"
                            genitive = genitive & Mid(name, 1, Len(name) - 1) & "è"
                    Case Else
                        genitive = genitive & Mid(name, 1, Len(name) - 1) & "û"
                    End Select
                Case "ÿ"
                    If Mid(name, Len(name) - 1, 1) = "è" Then
                        genitive = genitive & Mid(name, 1, Len(name) - 1) & "è"
                    Else
                        genitive = genitive & Mid(name, 1, Len(name) - 1) & "è"
                    End If
                Case "ü"
                    genitive = genitive & Mid(name, 1, Len(name) - 1) & "è"
                Case Else
                    genitive = genitive & name
                End Select
        End If
    End If
    genitive = genitive & " " 'Äîáàâëåíèå ïðîáåëà ïîñëå èìåíè

    If patronymic <> "" Then
        If fMan Then 'Ñêëîíåíèå ìóæñêîãî îò÷åñòâà
            genitive = genitive & patronymic & "à"
        Else 'Ñêëîíåíèå æåíñêîãî îò÷åñòâà
            genitive = genitive & Mid(patronymic, 1, Len(patronymic) - 1) & "û"
        End If
    End If
End Function

Sub Manual()
user_manual = "1. Èç ôàéëà MSWord ñî ñïèñêîì òåì è ðóêîâîäèòåëåé ñêîïèðîâàòü èç òàáëèöû âñå ñòîëáöû, êðîìå  ""¹ ï/ï""." & vbNewLine & _
"2. Â ôàéëå MSExcel ""Ãåíåðàòîð ïðîòîêîëîâ ÃÝÊ"" íàæàòü êíîïêó ""Âñòàâèòü ÿ÷åéêè""." & vbNewLine & _
"3. Çàïîëíèòü ÿ÷åéêè B3-G3 (""Êîä íàïðàâëåíèÿ"" è ò.ä.)" & vbNewLine & _
"4. Íàæàòü ""Çàïîëíèòü ñòîëáöû"". Åñëè íå ïîÿâèëîñü ñîîáùåíèå îá îøèáêå, ïðîâåðèòü ÿ÷åéêè, âûäåëåííûå ãîëóáûì öâåòîì, " & _
    "íà ïðàâèëüíîñòü ñêëîíåíèÿ è ïðåîáðàçîâàíèÿ ïîëíîãî ÔÈÎ â ôàìèëèþ È.Î. Ïðè îáíàðóæåíèè îøèáêè, íàæàòü ""Ðàçáëîêèðîâàòü ÿ÷åéêè"", " & _
    "èñïðàâèòü âðó÷íóþ è ñîîáùèòü îá ýòîì àâòîðó ïðîãðàììû (and-pleshkov-1996@yandex.ru)." & vbNewLine & _
"5. Íàæàòü êïîïêó ""Ñãåíåðèðîâàòü ïðîòîêîëû íà çàùèòó ÂÊÐ"". Ïîñëå ïîÿâëåíèÿ ñîîáùåíèÿ î çàâåðøåíèè ãåíåðàöèè " & _
    "íàæàòü ""Ñãåíåðèðîâàòü ïðîòîêîëû íà ãîñýêçàìåí""." & vbNewLine & _
"6. Ïîñëå çàâåðøåíèÿ ãåíåðàöèè ìîæíî íàæàòü êíîïêó ""Î÷èñòèòü ñîäåðæèìîå"" è âûïîëíèòü òå æå äåéñòâèÿ äëÿ ãåíåðàöèè ïðîòîêîëîâ äëÿ äðóãîé ãðóïïû."
MsgBox user_manual, , "Ðóêîâîäñòâî ïîëüçîâàòåëÿ"
End Sub
