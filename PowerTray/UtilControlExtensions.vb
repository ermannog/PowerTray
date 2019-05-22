Public Module UtilControlExtensions

#Region "Metodo UtilInvokeRefresh"
    ''' <summary>
    ''' This method is  thread safe.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension()>
    Public Sub UtilInvokeRefresh(ByVal control As System.Windows.Forms.Control)
        If control.InvokeRequired Then
            control.Invoke(New UtilRefresh(AddressOf UtilInvokeRefreshMethod), New Object() {control})
        Else
            UtilControlExtensions.UtilInvokeRefreshMethod(control)
        End If
    End Sub

    Private Delegate Sub UtilRefresh(ByVal control As System.Windows.Forms.Control)

    Private Sub UtilInvokeRefreshMethod(ByVal control As System.Windows.Forms.Control)
        control.Refresh()
    End Sub
#End Region

    '#Region "Metodo E2InvokeGetText"
    '    ''' <summary>
    '    ''' This method is  thread safe.
    '    ''' </summary>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2InvokeGetText(ByVal control As System.Windows.Forms.Control) As String
    '        If control.InvokeRequired Then
    '            Return control.Invoke(New E2GetText(AddressOf E2GetTextMethod), New Object() {control}).ToString()
    '        Else
    '            Return E2ControlExtensions.E2GetTextMethod(control)
    '        End If
    '    End Function

    '    Private Delegate Function E2GetText(ByVal control As System.Windows.Forms.Control) As String

    '    Private Function E2GetTextMethod(ByVal control As System.Windows.Forms.Control) As String
    '        Return control.Text
    '    End Function
    '#End Region

#Region "Metodo UtilInvokeGetValue"
    ''' <summary>
    ''' Get Value/SelectedValue or Text if Value/SelectedValue property not exist.
    ''' This method is  thread safe.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension()>
    Public Function UtilInvokeGetValue(ByVal control As System.Windows.Forms.Control) As Object
        Dim value As Object = Nothing
        If control.InvokeRequired Then
            value = control.Invoke(
                New UtilGetValueDelegate(AddressOf UtilGetValue), New Object() {control})
        Else
            value = UtilGetValue(control)
        End If

        Return value
    End Function

    Private Delegate Function UtilGetValueDelegate(ByVal control As System.Windows.Forms.Control) As Object

    Private Function UtilGetValue(ByVal control As System.Windows.Forms.Control) As Object
        Dim value As Object = Nothing

        If TypeOf control Is System.Windows.Forms.DateTimePicker Then
            With DirectCast(control, System.Windows.Forms.DateTimePicker)
                If .ShowCheckBox AndAlso Not .Checked Then
                    value = Nothing
                Else
                    If .Format = System.Windows.Forms.DateTimePickerFormat.Short Then
                        value = New DateTime(.Value.Year, .Value.Month, .Value.Day, 0, 0, 0, 0)
                    Else
                        value = .Value
                    End If
                End If
            End With
        ElseIf TypeOf control Is System.Windows.Forms.NumericUpDown Then
            With DirectCast(control, System.Windows.Forms.NumericUpDown)
                If String.IsNullOrEmpty(.Text) Then
                    value = Nothing
                Else
                    value = .Value
                End If
            End With
        ElseIf TypeOf control Is System.Windows.Forms.ComboBox Then
            With DirectCast(control, System.Windows.Forms.ComboBox)
                If .DataSource IsNot Nothing AndAlso
                    Not String.IsNullOrEmpty(.ValueMember) Then
                    value = .SelectedValue
                Else
                    If String.IsNullOrEmpty(.Text) Then
                        value = Nothing
                    Else
                        value = .Text
                    End If
                End If
            End With
        Else
            If String.IsNullOrEmpty(control.Text) Then
                value = Nothing
            Else
                value = control.Text
            End If
        End If

        Return value
    End Function
#End Region

#Region "Metodo UtilInvokeSetValue"
    ''' <summary>
    ''' Set Value/SelectedValue or Text if Value/SelectedValue property not exist.
    ''' This method is  thread safe.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension()>
    Public Sub UtilInvokeSetValue(ByVal control As System.Windows.Forms.Control, ByVal value As Object)
        If control.InvokeRequired Then
            control.Invoke(
                New UtilSetValueDelegate(AddressOf UtilSetValue), New Object() {control, value})
        Else
            UtilSetValue(control, value)
        End If
    End Sub

    Private Delegate Sub UtilSetValueDelegate(ByVal control As System.Windows.Forms.Control, ByVal value As Object)

    ''' <summary>
    ''' Set Value/SelectedValue or Text if Value/SelectedValue property not exist
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()>
    Private Sub UtilSetValue(ByVal control As System.Windows.Forms.Control, ByVal value As Object)
        If TypeOf control Is System.Windows.Forms.DateTimePicker Then
            With DirectCast(control, System.Windows.Forms.DateTimePicker)
                If .ShowCheckBox Then
                    .Checked = True
                    .Value = DirectCast(value, Date)
                Else
                    .Value = DirectCast(value, Date)
                End If
            End With
        ElseIf TypeOf control Is System.Windows.Forms.ComboBox Then
            Dim comboBox As System.Windows.Forms.ComboBox = DirectCast(control, System.Windows.Forms.ComboBox)
            If Not comboBox.DataSource Is Nothing AndAlso
                Not comboBox.ValueMember = String.Empty Then
                comboBox.SelectedValue = value
            Else
                comboBox.Text = System.Convert.ToString(value)
            End If
        Else
            control.Text = System.Convert.ToString(value)
        End If
    End Sub
#End Region

    '#Region "Metodo E2CheckTextNullOrEmpty"
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextNullOrEmpty(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If String.IsNullOrEmpty(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckNullValue)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextNullOrEmpty(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If String.IsNullOrEmpty(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckNullValue)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '#Region "Metodo E2CheckTextIsNumeric"
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsNumeric(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If Not IsNumeric(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckNumeric)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsNumeric(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If Not IsNumeric(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckNumeric)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '#Region "Metodo E2CheckTextIsEmailAddress"
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsEmailAddress(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If Not E2Util.IsEmailValid(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckEmailAddress)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsEmailAddress(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If Not E2Util.IsEmailValid(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckEmailAddress)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '#Region "Metodo E2CheckTextIsAlfaNumeric"
    '    ''' <summary>
    '    ''' Check Text in A-Z or 0-9
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="errorProvider"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsAlfaNumeric(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If Not E2Util.IsAlfaNumeric(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckAlfaNumeric)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    ''' <summary>
    '    ''' Check Text in A-Z or 0-9
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="errorProvider"></param>
    '    ''' <param name="align"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsAlfaNumeric(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If Not E2Util.IsAlfaNumeric(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckAlfaNumeric)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '#Region "Metodo E2CheckTextIsLetterOrDigit"
    '    ''' <summary>
    '    ''' Check Text in a-z or A-Z or 0-9
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="errorProvider"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsLetterOrDigit(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If Not E2Util.IsLetterOrDigit(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckLetterOrDigit)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    ''' <summary>
    '    ''' Check Text in a-z or A-Z or 0-9
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="errorProvider"></param>
    '    ''' <param name="align"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsLetterOrDigit(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If Not E2Util.IsLetterOrDigit(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckLetterOrDigit)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '#Region "Metodo E2CheckTextIsCAP"
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsCAP(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider) As Boolean
    '        If Not E2Util.IsCAPValid(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckCAP)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CheckTextIsCAP(ByVal control As System.Windows.Forms.Control, ByVal errorProvider As ErrorProvider, ByVal align As ErrorIconAlignment) As Boolean
    '        If Not E2Util.IsCAPValid(control.Text) Then
    '            errorProvider.SetError(control, My.Resources.UserMessages.CheckCAP)
    '            errorProvider.SetIconAlignment(control, align)
    '            Return False
    '        End If

    '        Return True
    '    End Function
    '#End Region

    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2IsTextNullOrEmpty(ByVal control As System.Windows.Forms.Control) As Boolean
    '        Return String.IsNullOrEmpty(control.Text)
    '    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function UtilGetScreenshot(ByVal control As System.Windows.Forms.Control) As System.Drawing.Bitmap
        Dim bitmap As New System.Drawing.Bitmap(control.Width, control.Height)
        control.DrawToBitmap(bitmap,
            System.Drawing.Rectangle.FromLTRB(0, 0, control.Width, control.Height))
        Return bitmap
    End Function

    '''' <summary>
    '''' Invoke OnValidating method
    '''' </summary>
    '''' <param name="control"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.Runtime.CompilerServices.Extension()>
    'Public Function E2PerformValidating(ByVal control As System.Windows.Forms.Control) As Boolean
    '    Dim method As System.Reflection.MethodInfo =
    '        control.GetType.GetMethod("OnValidating", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)

    '    Dim e As New System.ComponentModel.CancelEventArgs
    '    method.Invoke(control, New Object() {e})

    '    Return Not e.Cancel
    'End Function

    '<System.Runtime.CompilerServices.Extension()>
    'Public Function E2IsEditable(ByVal control As System.Windows.Forms.Control) As Boolean
    '    Dim returnValue As Boolean = True

    '    If Not TypeOf control Is System.Windows.Forms.DateTimePicker AndAlso
    '        Not TypeOf control Is System.Windows.Forms.ListControl AndAlso
    '        Not TypeOf control Is System.Windows.Forms.MonthCalendar AndAlso
    '        Not TypeOf control Is System.Windows.Forms.TextBoxBase AndAlso
    '        Not TypeOf control Is System.Windows.Forms.DataGrid AndAlso
    '        Not TypeOf control Is System.Windows.Forms.DataGridView Then
    '        'Esclusione dei controlli non editabili
    '        returnValue = False
    '    ElseIf Not control.Enabled Then
    '        'Se il controllo è disabilitato non è editabile
    '        returnValue = False
    '    ElseIf TypeOf control Is System.Windows.Forms.TextBoxBase Then
    '        'Gestione non editabilità TextBoxBase
    '        With DirectCast(control, System.Windows.Forms.TextBoxBase)
    '            If .ReadOnly Then
    '                returnValue = False
    '            End If
    '        End With
    '    ElseIf TypeOf control Is System.Windows.Forms.DataGrid Then
    '        'Gestione non editabilità DataGrid
    '        With DirectCast(control, System.Windows.Forms.DataGrid)
    '            If .ReadOnly Then
    '                returnValue = False
    '            End If
    '        End With
    '    ElseIf TypeOf control Is System.Windows.Forms.DataGrid Then
    '        'Gestione non editabilità DataGridView
    '        With DirectCast(control, System.Windows.Forms.DataGridView)
    '            If .ReadOnly Then
    '                Return False
    '            End If
    '        End With
    '    End If

    '    Return returnValue
    'End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Sub UtilSetStyle(ByVal control As System.Windows.Forms.Control, ByVal style As System.Windows.Forms.ControlStyles, ByVal value As Boolean)
        Dim setStyleMethodInfo = control.GetType().GetMethod("SetStyle", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
        setStyleMethodInfo.Invoke(control, New System.Object() {style, value})
        setStyleMethodInfo = Nothing
    End Sub

    <System.Runtime.CompilerServices.Extension()>
    Public Sub UtilSetTransparentBackColor(ByVal control As System.Windows.Forms.Control)
        control.UtilSetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
        control.UtilSetStyle(System.Windows.Forms.ControlStyles.UserPaint, True)
        control.BackColor = System.Drawing.Color.Transparent
    End Sub

    <System.Runtime.CompilerServices.Extension()>
    Public Sub UtilSetDoubleBuffered(ByVal control As System.Windows.Forms.Control, ByVal value As Boolean)
        Dim doubleBufferedPropertyInfo = control.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
        doubleBufferedPropertyInfo.SetValue(control, value, Nothing)
        doubleBufferedPropertyInfo = Nothing
    End Sub

    <System.Runtime.CompilerServices.Extension()>
    Private Sub UtilRefreshControlAndChilds(ByVal control As System.Windows.Forms.Control)
        control.Refresh()
        For Each c As System.Windows.Forms.Control In control.Controls
            UtilRefreshControlAndChilds(c)
        Next
    End Sub

    <System.Runtime.CompilerServices.Extension()>
    Public Function UtilGetCursorPointToClient(ByVal control As System.Windows.Forms.Control) As System.Drawing.Point
        Return control.PointToClient(System.Windows.Forms.Cursor.Position)
    End Function


    '#Region "Metodo UtilCreateChildProgressBar"
    '    ''' <summary>
    '    ''' Create a child ProgressBar
    '    ''' Minimum will set to 0
    '    ''' Maximum will set to 100
    '    ''' Step will set to 1
    '    ''' Release the PregressBar with the method E2CreateChildProgressBar
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CreateChildProgressBar(ByVal control As System.Windows.Forms.Control) As System.Windows.Forms.ProgressBar
    '        Return UtilControlExtensions.E2CreateChildProgressBar(control, 0, 100, 1)
    '    End Function

    '    ''' <summary>
    '    ''' Create a child ProgressBar
    '    ''' Minimum will set to 0
    '    ''' Release the ProgressBar with the method E2CreateChildProgressBar
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="maximum"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CreateChildProgressBar(ByVal control As System.Windows.Forms.Control, maximum As Integer) As System.Windows.Forms.ProgressBar
    '        Return UtilControlExtensions.E2CreateChildProgressBar(control, 0, maximum, 1)
    '    End Function

    '    ''' <summary>
    '    ''' Create a child ProgressBar
    '    ''' Release the ProgressBar with the method E2CreateChildProgressBar
    '    ''' </summary>
    '    ''' <param name="control"></param>
    '    ''' <param name="minimum"></param>
    '    ''' <param name="maximum"></param>
    '    ''' <param name="step"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Function E2CreateChildProgressBar(ByVal control As System.Windows.Forms.Control, minimum As Integer, maximum As Integer, [step] As Integer) As System.Windows.Forms.ProgressBar
    '        Dim pb As New System.Windows.Forms.ProgressBar
    '        pb.Parent = control

    '        pb.Width = control.ClientSize.Width
    '        pb.Left = 0
    '        pb.Top = CInt((control.ClientSize.Height - pb.Height) / 2)

    '        pb.Minimum = minimum
    '        pb.Maximum = maximum
    '        pb.Step = [step]

    '        control.Update()

    '        Return pb
    '    End Function
    '#End Region

    '''' <summary>
    '''' Release a ProgressBar create the method E2CreateChildProgressBar
    '''' </summary>
    '''' <param name="control"></param>
    '''' <param name="progressbar"></param>
    '''' <remarks></remarks>
    '<System.Runtime.CompilerServices.Extension()>
    'Public Sub UtilDisposeChildProgressBar(ByVal control As System.Windows.Forms.Control, progressbar As System.Windows.Forms.ProgressBar)
    '    progressbar.Dispose()
    '    control.Update()
    'End Sub

    '#Region "Metodo E2SetTextFormat"
    '    ''' <summary>
    '    ''' Set the formatted text of a control using String.Format
    '    ''' </summary>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Sub E2SetTextFormat(control As System.Windows.Forms.Control, arg0 As Object)
    '        control.Text = String.Format(control.Text, arg0)
    '    End Sub

    '    ''' <summary>
    '    ''' Set the formatted text of a control using String.Format
    '    ''' </summary>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Sub E2SetTextFormat(control As System.Windows.Forms.Control, ParamArray args() As Object)
    '        control.Text = String.Format(control.Text, args)
    '    End Sub

    '    ''' <summary>
    '    ''' Set the formatted text of a control using String.Format
    '    ''' </summary>
    '    <System.Runtime.CompilerServices.Extension()>
    '    Public Sub E2SetTextFormat(control As System.Windows.Forms.Control, arg0 As Object, arg1 As Object)
    '        control.Text = String.Format(control.Text, arg0, arg1)
    '    End Sub
    '#End Region
End Module
