Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class FilesController
        Inherits Controller

        Public Function Index(archivo As HttpPostedFileBase) As ActionResult
            Try
                Dim bytes As Byte() = New Byte(archivo.ContentLength - 1) {}
                archivo.InputStream.Read(bytes, 0, bytes.Length)
                Dim Ruta As String = "\\192.168.100.23\helpdesk\" & archivo.FileName.Replace(" ", "_").Replace("  ", "_").Replace("__", "_")
                IO.File.WriteAllBytes(Ruta, bytes)
                Dim serializer As New JavaScriptSerializer()
                serializer.MaxJsonLength = Int32.MaxValue

                Dim resultado = New With {
                    Key .nombre = archivo.FileName,
                    Key .tipo = archivo.ContentType,
                    Key .tamano = archivo.ContentLength,
                    Key .contenido = Convert.ToBase64String(bytes)
                }

                Dim contentType As String
                If Request.Form("ie") = "1" Then
                    contentType = "text/html"
                Else
                    contentType = "application/json"
                End If

                Return Json("Archivo cargado con exito, " & Ruta, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json("Error al subir el Archivo", JsonRequestBehavior.AllowGet)
            End Try
        End Function

    End Class
End Namespace