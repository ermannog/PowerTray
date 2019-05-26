Public NotInheritable Class Util
    Private Sub New()
        MyBase.New()
    End Sub

    Public Shared Function SetWaitCursor(ByVal state As Boolean) As Boolean
        Dim waitState As Boolean

        waitState = System.Windows.Forms.Cursor.Current Is System.Windows.Forms.Cursors.WaitCursor

        If state Then
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Else
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End If

        Return waitState
    End Function

    '#Region "Gestione Licenza"
    '    Public Shared Function CheckEulaAccepted(ByVal companyName As String, ByVal productName As String) As Boolean
    '        Const EulaAcceptedValueName As String = "EulaAccepted"
    '        Dim registryKey As String = String.Format("Software\{0}\{1}", _
    '            companyName, productName)

    '        Dim key = My.Computer.Registry.CurrentUser.OpenSubKey(registryKey, True)
    '        Dim value As Object = Nothing

    '        If key IsNot Nothing Then
    '            value = key.GetValue(EulaAcceptedValueName)
    '        End If

    '        If key Is Nothing OrElse _
    '            value Is Nothing OrElse _
    '            String.IsNullOrEmpty(value.ToString()) OrElse _
    '            value.ToString <> "1" Then

    '            'Visualizzazione Dialog
    '            Using frm As New LicenseForm
    '                If frm.ShowDialog() <> DialogResult.OK Then
    '                    Return False
    '                End If
    '            End Using

    '            'Creazione Key
    '            If key Is Nothing Then
    '                key = My.Computer.Registry.CurrentUser.CreateSubKey(registryKey)
    '            End If

    '            'Impostazione Valore
    '            If value Is Nothing OrElse _
    '                String.IsNullOrEmpty(value.ToString()) OrElse _
    '                value.ToString <> "1" Then
    '                key.SetValue(EulaAcceptedValueName, 1, Microsoft.Win32.RegistryValueKind.DWord)
    '            End If
    '        End If

    '        Return True
    '    End Function
    '#End Region

#Region "Method ShowMessage"
    Public Overloads Shared Function ShowMessage(ByVal text As String) As System.Windows.Forms.DialogResult
        Return ShowMessage(text, String.Empty)
    End Function

    Public Overloads Shared Function ShowMessage(ByVal text As String, ByVal title As String) As System.Windows.Forms.DialogResult
        Return ShowMessage(text, title, System.Windows.Forms.MessageBoxIcon.None)
    End Function

    Public Overloads Shared Function ShowMessage(ByVal text As String,
                ByVal icon As System.Windows.Forms.MessageBoxIcon) As System.Windows.Forms.DialogResult
        Return ShowMessage(text, String.Empty, icon)
    End Function

    Public Overloads Shared Function ShowMessage(ByVal text As String,
                                  ByVal title As String,
                                  ByVal icon As System.Windows.Forms.MessageBoxIcon) As System.Windows.Forms.DialogResult
        Return ShowMessage(text, title, System.Windows.Forms.MessageBoxButtons.OK, icon, System.Windows.Forms.MessageBoxDefaultButton.Button1)
    End Function

    Public Overloads Shared Function ShowMessage(ByVal text As String,
                                  ByVal title As String,
                                  ByVal buttons As System.Windows.Forms.MessageBoxButtons,
                                  ByVal icon As System.Windows.Forms.MessageBoxIcon,
                                  ByVal defaultButton As System.Windows.Forms.MessageBoxDefaultButton) As System.Windows.Forms.DialogResult
        ShowMessage = System.Windows.Forms.MessageBox.Show(text,
            My.Application.Info.Title & " " & title,
            buttons, icon, defaultButton)
    End Function
#End Region

#Region "Metodo Question"
    Public Overloads Shared Function ShowQuestion(ByVal text As String) As System.Windows.Forms.DialogResult
        Return ShowQuestion(text, System.Windows.Forms.MessageBoxButtons.YesNo)
    End Function

    Public Overloads Shared Function ShowQuestion(ByVal text As String, ByVal defaultButton As System.Windows.Forms.MessageBoxDefaultButton) As System.Windows.Forms.DialogResult
        Return ShowQuestion(text, System.Windows.Forms.MessageBoxButtons.YesNo, defaultButton)
    End Function

    Public Overloads Shared Function ShowQuestion(ByVal text As String,
                                    ByVal buttons As System.Windows.Forms.MessageBoxButtons) As System.Windows.Forms.DialogResult
        Return ShowQuestion(text, buttons, System.Windows.Forms.MessageBoxDefaultButton.Button2)
    End Function

    Public Overloads Shared Function ShowQuestion(ByVal text As String,
                                              ByVal buttons As System.Windows.Forms.MessageBoxButtons,
                                              ByVal defaultButton As System.Windows.Forms.MessageBoxDefaultButton) As System.Windows.Forms.DialogResult
        Return ShowMessage(text, String.Empty, buttons, System.Windows.Forms.MessageBoxIcon.Question, defaultButton)
    End Function
#End Region

    Public Shared Sub ShowError(ByVal message As String, ByVal abort As Boolean)
        Util.ShowMessage(message, "Error", System.Windows.Forms.MessageBoxIcon.Stop)

        If abort Then
            System.Environment.Exit(0)
        End If
    End Sub

#Region "Method ShowErrorException"
    Public Overloads Shared Sub ShowErrorException(ByVal exception As System.Exception, ByVal abort As Boolean)
        Util.ShowError(GetExceptionMessage(String.Empty, exception), abort)
    End Sub

    Public Overloads Shared Sub ShowErrorException(ByVal message As String, ByVal exception As System.Exception, ByVal abort As Boolean)
        Util.ShowError(GetExceptionMessage(message, exception), abort)
    End Sub
#End Region

    Public Shared Function GetExceptionMessage(ByVal message As String, ByVal exception As System.Exception) As String
        Dim text As String = message

        Dim ex As System.Exception = exception

        While ex IsNot Nothing
            'Aggiunta Message
            If Not String.IsNullOrEmpty(ex.Message) Then
                If Not String.IsNullOrEmpty(text) Then _
                    text &= ControlChars.NewLine & ControlChars.NewLine
                text &= ex.Message
            End If

            'If showDetailsException Then
            'Aggiunta Source
            If Not String.IsNullOrEmpty(ex.Source) Then
                If Not String.IsNullOrEmpty(text) Then _
                    text &= ControlChars.NewLine
                text &= ex.GetType().ToString()
            End If

            'Aggiunta Error code
            Dim lastWin32Error = GetLastWin32Error(String.Empty)
            If Not String.IsNullOrEmpty(lastWin32Error) Then
                text &= ControlChars.NewLine & ControlChars.NewLine
                text &= "Last Win32 Error: " & lastWin32Error
            End If

            'Aggiunta StackTrace
            If Not String.IsNullOrEmpty(ex.StackTrace) Then
                If Not String.IsNullOrEmpty(text) Then _
                    text &= ControlChars.NewLine & ControlChars.NewLine
                text &= ex.StackTrace.Trim()
            End If

            ex = ex.InnerException
        End While

        Return text
    End Function

    Public Shared Function GetLastWin32Error(ByVal message As String) As String
        Dim text As String = message

        Dim ex As New System.ComponentModel.Win32Exception(
            System.Runtime.InteropServices.Marshal.GetLastWin32Error())

        'Aggiunta Error code
        If ex.NativeErrorCode <> 0 Then
            If Not String.IsNullOrEmpty(text) Then _
                text &= ControlChars.NewLine & ControlChars.NewLine
            text &= "Error code: " & Hex(ex.NativeErrorCode) & " (" & ex.Message & ")"
        End If

        Return text
    End Function

#Region "Encrypt / Decrypt"
    Public Shared Function EncryptByRijndael(ByVal value As String) As String
        Dim returnValue As String = String.Empty

        If String.IsNullOrEmpty(value) Then Return returnValue

        Dim rjm As New System.Security.Cryptography.RijndaelManaged

        'Generazione automatica key a 256 caratteri
        Dim key As String = GetKey(rjm.KeySize)
        Dim iv As String = GetKey(rjm.BlockSize)

        rjm.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key)
        rjm.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv)

        Dim input As Byte() = System.Text.Encoding.UTF8.GetBytes(value)

        Dim encriptor As System.Security.Cryptography.ICryptoTransform =
             rjm.CreateEncryptor()

        Dim output As Byte() = encriptor.TransformFinalBlock(
            input, 0, input.Length)

        'Rilascio risorse 
        encriptor.Dispose() : encriptor = Nothing
        rjm = Nothing

        Return Convert.ToBase64String(output)
    End Function

    Public Shared Function DecryptByRijndael(ByVal value As String) As String
        Dim returnValue As String = String.Empty

        If String.IsNullOrEmpty(value) Then Return returnValue

        Dim rjm As New System.Security.Cryptography.RijndaelManaged

        'Generazione automatica key a 256 caratteri
        Dim key As String = GetKey(rjm.KeySize)
        Dim iv As String = GetKey(rjm.BlockSize)

        rjm.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key)
        rjm.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv)

        Dim input As Byte() = Convert.FromBase64String(value)

        Dim decriptor As System.Security.Cryptography.ICryptoTransform =
             rjm.CreateDecryptor()

        Dim output As Byte() = decriptor.TransformFinalBlock(
            input, 0, input.Length)

        'Rilascio risorse 
        decriptor.Dispose() : decriptor = Nothing
        rjm = Nothing

        Return System.Text.Encoding.UTF8.GetString(output)
    End Function

    Private Shared Function GetKey(ByVal keySize As Integer) As String
        'Generazione automatica key a 256 caratteri
        Dim keyLenght As Integer = CInt(keySize / 8)
        Dim key As String = String.Empty

        Dim type As System.Type = GetType(Util)

        While type IsNot Nothing
            If String.IsNullOrEmpty(key) Then
                key = type.FullName
            Else
                key = MergeStrings(key, type.FullName, False)
            End If
            key = MergeStrings(key, type.FullName, True)

            For Each m In type.GetMembers
                key = MergeStrings(key, type.FullName, False)

                key = MergeStrings(key, type.FullName, True)

                If key.Length >= keyLenght Then
                    key = key.Substring((key.Length - keyLenght) \ 2, keyLenght)
                    Exit While
                End If
            Next

            type = type.BaseType
        End While

        'Occorre evitare l'uso del metodo GetHashCode in quanto da risultati diversi
        ' a seconda della piattaforma 32 bit o 64 bit
        'http://msdn.microsoft.com/en-us/library/system.string.gethashcode(VS.100).aspx

        Return key
    End Function

    Private Shared Function MergeStrings(ByVal str1 As String, ByVal str2 As String, ByVal reverse As Boolean) As String
        Dim str As String = String.Empty
        Dim index As Integer = 0

        Dim sourceString As String
        If reverse Then
            Dim a As Char()
            a = str1.ToCharArray()
            Array.Reverse(a)
            sourceString = New String(a)
        Else
            sourceString = str1
        End If

        For Each c In sourceString
            str &= c
            If index <= str2.Length - 1 Then
                str &= str2.Chars(index)
                index += 1
            End If
        Next

        Return str
    End Function

#End Region

#Region "Xml Serialize/Deserialize"
    Public Overloads Shared Sub XmlSerialize(ByVal filePath As String, ByVal obj As System.Object, ByVal encryptByRijndael As Boolean)
        'http://support.microsoft.com/kb/316730


        'Creazione directory se inesistente
        If Not System.IO.Directory.Exists(
            System.IO.Path.GetDirectoryName(filePath)) Then
            System.IO.Directory.CreateDirectory(
                System.IO.Path.GetDirectoryName(filePath))
        End If

        'Creazione/Sovrascrittura File
        Dim serializer As System.Xml.Serialization.XmlSerializer
        serializer = New System.Xml.Serialization.XmlSerializer(
            obj.GetType())

        If encryptByRijndael Then
            Dim serializeText = String.Empty
            Using stream As New System.IO.StringWriter
                serializer.Serialize(stream, obj)
                serializeText = stream.ToString()
            End Using

            serializeText = Util.EncryptByRijndael(serializeText)

            My.Computer.FileSystem.WriteAllText(filePath, serializeText, False)
            serializeText = Nothing
        Else
            Using stream = New System.IO.FileStream(filePath, System.IO.FileMode.Create)
                serializer = New System.Xml.Serialization.XmlSerializer(
                    obj.GetType())
                serializer.Serialize(stream, obj)
            End Using
        End If

        serializer = Nothing
    End Sub

    Public Overloads Shared Sub XmlSerialize(ByVal filePath As String, ByVal type As System.Type, ByVal encryptByRijndael As Boolean)
        Dim obj As System.Object = System.Activator.CreateInstance(type)
        Util.XmlSerialize(filePath, obj, encryptByRijndael)

        Dim iDisposable As System.Type = obj.GetType.GetInterface(
            "IDisposable", True)
        If iDisposable IsNot Nothing Then
            DirectCast(obj, System.IDisposable).Dispose()
            iDisposable = Nothing
        End If

        obj = Nothing
    End Sub

    Public Overloads Shared Function XmlDeserialize(ByVal filePath As String, ByVal obj As System.Object, ByVal decryptByRijndael As Boolean) As Boolean
        If Not System.IO.File.Exists(filePath) Then Return False

        Dim deserializedObj As Object = Nothing
        Dim serializer As System.Xml.Serialization.XmlSerializer
        serializer = New System.Xml.Serialization.XmlSerializer(obj.GetType())

        If decryptByRijndael Then
            Dim serializeText = My.Computer.FileSystem.ReadAllText(filePath)
            serializeText = Util.DecryptByRijndael(serializeText)

            Using stream As New IO.StringReader(serializeText)
                deserializedObj = serializer.Deserialize(stream)
            End Using

            serializeText = Nothing
        Else
            Using stream As New System.IO.FileStream(filePath, System.IO.FileMode.Open, IO.FileAccess.Read)
                deserializedObj = serializer.Deserialize(stream)
            End Using
        End If


        'Using stream As New System.IO.FileStream(filePath, System.IO.FileMode.Open, IO.FileAccess.Read)
        '    serializer = New System.Xml.Serialization.XmlSerializer(obj.GetType())

        '    If decryptByRijndael Then
        '        Dim serializeText = String.Empty

        '        Using sw As New System.IO.StringReader(stream)
        '            serializeText = sw.ToString()
        '            serializeText = Util.DecryptByRijndael(serializeText)
        '        End Using

        '        Using sw As New System.IO.StreamWriter(stream)
        '            sw.Write(serializeText)
        '        End Using

        '        deserializedObj = serializer.Deserialize(sw)
        '    Else
        '        deserializedObj = serializer.Deserialize(stream)
        '    End If

        '    deserializedObj = serializer.Deserialize(stream)
        '    serializer = Nothing
        'End Using

        serializer = Nothing

        Util.Copy(deserializedObj, obj)

        Return True
    End Function

    Public Shared Sub Copy(ByVal sourceObject As Object, ByVal destinationObject As Object)

        For Each p As System.Reflection.PropertyInfo In destinationObject.GetType().GetProperties()
            Dim setValueFlag As Boolean = p.CanWrite

            'Esclusione proprietà la cui serializzazione
            'è mappata su un'altra proprietà
            If setValueFlag Then
                For Each a As Object In p.GetCustomAttributes(False)
                    If TypeOf a Is System.Xml.Serialization.XmlElementAttribute Then
                        setValueFlag = False
                        Exit For
                    End If
                Next
            End If

            If setValueFlag Then
                Dim sourceValue As Object =
                    sourceObject.GetType.GetProperty(p.Name).GetValue(sourceObject, Nothing)
                p.SetValue(destinationObject, sourceValue, Nothing)
            End If
        Next
    End Sub
#End Region

#Region "Execute PowerShell Script"
    'Public Shared Function RunPowerShellScript(scriptText As String) As String
    '    'https://www.codeproject.com/Articles/18229/How-to-run-PowerShell-scripts-from-C

    '    Dim results As ObjectModel.Collection(Of Management.Automation.PSObject) = Nothing

    '    Using runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace()
    '        runspace.Open()

    '        Using pipeline = runspace.CreatePipeline()
    '            pipeline.Commands.AddScript(scriptText)
    '            pipeline.Commands.Add("Out-String")
    '            results = pipeline.Invoke()
    '            runspace.Close()
    '        End Using
    '    End Using

    '    Dim result = String.Empty
    '    If results IsNot Nothing Then
    '        Dim stringBuilder = New System.Text.StringBuilder()
    '        For Each psObject In results
    '            stringBuilder.AppendLine(psObject.ToString())
    '        Next
    '        result = stringBuilder.ToString()
    '    End If

    '    'MsgBox(result)

    '    Return result
    'End Function


    'Public Enum PowerShellOutputFormats
    '    List
    '    Table
    'End Enum

    Public Shared Function RunPowerShellScript(scriptText As String, timeout As Integer) As String
        'https://www.emoreau.com/Entries/Articles/2018/06/Running-a-PowerShell-script-from-a-Net-application.aspx

        Dim output = String.Empty
        Dim errors = String.Empty

        If String.IsNullOrWhiteSpace(scriptText) Then
            Throw New System.ArgumentNullException()
        End If

        Using process As New System.Diagnostics.Process()
            process.StartInfo.FileName = "powershell.exe"
            process.StartInfo.Arguments = scriptText
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.RedirectStandardError = True
            process.StartInfo.UseShellExecute = False
            process.StartInfo.CreateNoWindow = True
            If process.Start() Then
                If Not process.WaitForExit(timeout) Then
                    process.Kill()
                End If
            End If

            output = process.StandardOutput.ReadToEnd().Trim()
            errors = process.StandardError.ReadToEnd()

            process.Close()
        End Using

        If Not String.IsNullOrEmpty(errors) Then
            Throw New System.Exception(errors)
        End If

        Return output
    End Function
#End Region

    Public Shared Sub SetReadOnlyAttribute(type As System.Type, propertyName As String, value As Boolean)
        Dim propertyDescriptor = System.ComponentModel.TypeDescriptor.GetProperties(type)(propertyName)
        Dim readOnlyAttribute = DirectCast(propertyDescriptor.Attributes(GetType(System.ComponentModel.ReadOnlyAttribute)), System.ComponentModel.ReadOnlyAttribute)

        Dim isReadOnlyField = readOnlyAttribute.GetType().GetField("isReadOnly", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)

        isReadOnlyField.SetValue(readOnlyAttribute, value)
    End Sub

    Public Shared Sub SetBrowsableAttribute(type As System.Type, propertyName As String, value As Boolean)
        Dim propertyDescriptor = System.ComponentModel.TypeDescriptor.GetProperties(type)(propertyName)
        Dim browsableAttribute = DirectCast(propertyDescriptor.Attributes(GetType(System.ComponentModel.BrowsableAttribute)), System.ComponentModel.BrowsableAttribute)

        Dim isBrowsableField = browsableAttribute.GetType().GetField("browsable", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)

        isBrowsableField.SetValue(browsableAttribute, value)
    End Sub
End Class
