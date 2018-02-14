Imports System.Net.Mail
Imports System.Web.Script.Serialization
Public Class Funciones

    Dim oConsultas As New Consultas

    'Remplazamos los Caracteres $ por comillas dobles.
    Public Function ComillasDoble(cadena As String) As String
        Try
            Dim quot As String = """"
            Return Replace(cadena, "§", quot) & vbCrLf
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function DatatableToJson(Dt As System.Data.DataTable) As String
        Dim serializer As New JavaScriptSerializer()
        'maxivo valor 
        serializer.MaxJsonLength = Int32.MaxValue
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In Dt.Rows
            row = New Dictionary(Of String, Object)()
            For Each col As DataColumn In Dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function

    Public Function EnviarCorreo(ByVal destinatarios As String, ByVal Asunto As String, ByVal Cuerpo As String) As Boolean
        Try
            Dim DtCorreo As System.Data.DataTable = oConsultas.Consultar_EnvioDeCorreo()
            If DtCorreo.Rows.Count > 0 Then
                Dim Correo As New System.Net.Mail.MailMessage(New System.Net.Mail.MailAddress(DtCorreo(0)("Usuario"), "Cajacopi"), New System.Net.Mail.MailAddress(destinatarios))
                Dim mailSender As SmtpClient = New SmtpClient(DtCorreo(0)("Smtp").ToString.Trim, DtCorreo(0)("Puerto").ToString.Trim)
                mailSender.Credentials = New Net.NetworkCredential(DtCorreo(0)("Usuario").ToString.Trim, DtCorreo(0)("Clave").ToString.Trim)
                mailSender.EnableSsl = True
                Correo.Subject = Asunto
                Correo.IsBodyHtml = True
                Correo.Body = Cuerpo
                Correo.Priority = System.Net.Mail.MailPriority.High
                mailSender.Send(Correo)
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function EnviarCorreo(ByVal destinatarios As String, ByVal Asunto As String, ByVal Cuerpo As String, Adjunto As String, ByVal Optional CC As List(Of String) = Nothing, ByVal Optional Adjunto1 As String = "", ByVal Optional Adjunto2 As String = "", ByVal Optional Adjunto3 As String = "", ByVal Optional Adjunto4 As String = "") As Boolean
        Try
            Dim DtCorreo As System.Data.DataTable = oConsultas.Consultar_EnvioDeCorreo()
            If DtCorreo.Rows.Count > 0 Then
                Dim Correo As New System.Net.Mail.MailMessage(New System.Net.Mail.MailAddress(DtCorreo(0)("Usuario"), "Cajacopi"), New System.Net.Mail.MailAddress(destinatarios))
                If IsNothing(Adjunto) = False Then
                    If Adjunto.ToString.Trim <> String.Empty Then
                        Dim NuevoArchivo As New System.Net.Mail.Attachment(Adjunto)
                        Correo.Attachments.Add(NuevoArchivo)
                    End If
                End If
                If IsNothing(Adjunto2) = False Then
                    If Adjunto2.ToString.Trim <> String.Empty Then
                        Dim NuevoArchivo As New System.Net.Mail.Attachment(Adjunto2)
                        Correo.Attachments.Add(NuevoArchivo)
                    End If
                End If
                If IsNothing(Adjunto3) = False Then
                    If Adjunto3.ToString.Trim <> String.Empty Then
                        Dim NuevoArchivo As New System.Net.Mail.Attachment(Adjunto3)
                        Correo.Attachments.Add(NuevoArchivo)
                    End If
                End If
                If IsNothing(Adjunto4) = False Then
                    If Adjunto4.ToString.Trim <> String.Empty Then
                        Dim NuevoArchivo As New System.Net.Mail.Attachment(Adjunto4)
                        Correo.Attachments.Add(NuevoArchivo)
                    End If
                End If
                Dim mailSender As SmtpClient = New SmtpClient(DtCorreo(0)("Smtp").ToString.Trim, DtCorreo(0)("Puerto").ToString.Trim)
                mailSender.Credentials = New Net.NetworkCredential(DtCorreo(0)("Usuario").ToString.Trim, DtCorreo(0)("Clave").ToString.Trim)
                mailSender.EnableSsl = False
                Correo.Subject = Asunto
                Correo.IsBodyHtml = True
                Correo.Body = Cuerpo
                Correo.Priority = System.Net.Mail.MailPriority.High
                If IsNothing(CC) = False Then
                    For Each item In CC
                        If item <> "" Then
                            Correo.CC.Add(item)
                        End If
                    Next
                End If
                mailSender.Send(Correo)
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
