Imports System.Data
Imports System.Data.Common
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Text
Imports System.Configuration

Public Class Procesos
    Dim ConDato As New SqlConnection(Variables.Conexion_Dato)

#Region "Transaccion"
    Dim oTran As SqlTransaction
    Public Sub BeginTransaction()
        ConDato.Open()
        oTran = ConDato.BeginTransaction()
    End Sub
    Public Sub CommitTransaction()
        oTran.Commit()
        ConDato.Close()
    End Sub
    Public Sub RollBackTransaction()
        oTran.Rollback()
        ConDato.Close()
    End Sub
#End Region

    Public Function IngresarTicket(CodTicket As String, CodUsuario As String, TipoSolicitud As String, TipoSoporte As String, Necesidad As String, Justificacion As String, Estado As String,
                                   Adjunto As String, Area As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("insert into Help_Ticket(CodTicket,Responsable,CodUsuario,TipoSolicitud,TipoSoporte,Necesidad,Justificacion,Estado,Adjunto,Area,Habilitado,Prioridad)")
            Q.AppendLine("values(@CodTicket,'',@CodUsuario,@TipoSolicitud,@TipoSoporte,@Necesidad,@Justificacion,@Estado,@Adjunto,@Area,1,'URGENTE')")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@CodUsuario", CodUsuario)
                .AddWithValue("@TipoSolicitud", TipoSolicitud)
                .AddWithValue("@TipoSoporte", TipoSoporte)
                .AddWithValue("@Necesidad", Necesidad)
                .AddWithValue("@Justificacion", Justificacion)
                .AddWithValue("@Estado", Estado)
                .AddWithValue("@Adjunto", Adjunto)
                .AddWithValue("@Area", Area)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ActualizarEstadoTicket(Estado As String, CodTicket As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set Estado = @Estado where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Estado", Estado)
                .AddWithValue("@CodTicket", CodTicket)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function IngresarReunion(Id_Reunion As String, CodTicket As String, Fecha As String, Hora As String, CantMin As String, Area As String, UsuSolicita As String, CodUsuario As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("insert into Help_Reunion (Id_Reunion,CodTicket,Fecha,Hora,Hora_Fin,Area,UsuSolicita,CodUsuario,Habilitado)")
            Q.AppendLine("values(@Id_Reunion,@CodTicket,@Fecha,@Hora,DATEADD(MINUTE,cast(@CantMin as int),@Hora),@Area,@UsuSolicita,@CodUsuario,1)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Id_Reunion", Id_Reunion)
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@Fecha", Fecha)
                .AddWithValue("@Hora", Hora)
                .AddWithValue("@CantMin", CantMin)
                .AddWithValue("@Area", Area)
                .AddWithValue("@UsuSolicita", UsuSolicita)
                .AddWithValue("@CodUsuario", CodUsuario)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_Reunion(Fecha As String, Hora As String, Id As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Reunion set Fecha=@Fecha, Hora = @Hora where Id_Reunion=@Id")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Fecha", Fecha)
                .AddWithValue("@Hora", Hora)
                .AddWithValue("@Id", Id)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_Reunion(Id As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Reunion set Habilitado=0 where Id_Reunion=@Id")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Id", Id)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_EstadoTicketReunion(Estado As String, Id As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set Estado=@Estado where CodTicket in (select CodTicket from Help_Reunion where Id_Reunion = @Id)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Estado", Estado)
                .AddWithValue("@Id", Id)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Ingresar_LogTicket(CodTicket As String, EtapaAnt As String, EtapaAct As String, Observacion As String, Usuario As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("insert into Help_LogTickets (CodTicket,EtapaAnt,EtapaAct,Observacion,Usuario)")
            Q.AppendLine("values(@CodTicket,@EtapaAnt,@EtapaAct,@Observacion,@Usuario)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@EtapaAnt", EtapaAnt)
                .AddWithValue("@EtapaAct", EtapaAct)
                .AddWithValue("@Observacion", Observacion)
                .AddWithValue("@Usuario", Usuario)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_TicketEvaluacion(CodTicket As String, Responsable As String, Prioridad As String, Viabilidad As String, Duracion As String, Estado As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set Responsable=@Responsable,Prioridad=@Prioridad,Viabilidad=@Viabilidad,Duracion=@Duracion,Estado=@Estado,FechaEvaluacion=GETDATE() where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@Responsable", Responsable)
                .AddWithValue("@Prioridad", Prioridad)
                .AddWithValue("@Viabilidad", Viabilidad)
                .AddWithValue("@Duracion", Duracion)
                .AddWithValue("@Estado", Estado)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_TicketAutorizacion(CodTicket As String, UsuarioAutoriza As String, Autorizado As String, Estado As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set UsuarioAutoriza=@UsuarioAutoriza,FechaAutoriza=GETDATE(),Autorizado=@Autorizado,Estado=@Estado where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@UsuarioAutoriza", UsuarioAutoriza)
                .AddWithValue("@Autorizado", Autorizado)
                .AddWithValue("@Estado", Estado)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function



    Public Function ActualizarTipoSoporteTicket(TipoSoporte As String, CodTicket As String, TipoSolicitud As String, TipoAccion As Integer,
                                                observacion As String, Responsable As String, NivelResponsable As String) As String
        Try
            Dim Q As New StringBuilder
            If TipoAccion = 1 Or TipoAccion = 7 Then
                Q.AppendLine("update Help_Ticket set TipoSoporte = @TipoSoporte, TipoSolicitud = @TipoSolicitud, Responsable = @Responsable, Prioridad = @NivelResponsable where CodTicket = @CodTicket")
            Else
                If TipoAccion = 2 Then
                    Q.AppendLine("update Help_Ticket set TipoSoporte = @TipoSoporte, TipoSolicitud = @TipoSolicitud, FechaRechazo = getdate(), MotivoRechazo = @observacion, Responsable = @Responsable, Prioridad = @NivelResponsable where CodTicket=@CodTicket")
                Else
                    If TipoAccion = 3 Then
                        Q.AppendLine("update Help_Ticket set TipoSoporte = @TipoSoporte, TipoSolicitud = @TipoSolicitud, FechaSolucionado = getdate(), Responsable = @Responsable, Prioridad = @NivelResponsable where CodTicket = @CodTicket")
                    Else
                        If TipoAccion = 4 Then
                            Q.AppendLine("update Help_Ticket set TipoSoporte = @TipoSoporte, TipoSolicitud = @TipoSolicitud, FechaCancelacion = getdate(), MotivoCancelacion = @observacion, Responsable = @Responsable, Prioridad = @NivelResponsable where CodTicket = @CodTicket")
                        Else
                            If TipoAccion = 6 Then
                                Q.AppendLine("update Help_Ticket set TipoSoporte = @TipoSoporte, TipoSolicitud = @TipoSolicitud where CodTicket = @CodTicket")
                            End If
                        End If
                    End If
                End If
            End If
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            If TipoAccion = 1 Or TipoAccion = 3 Or TipoAccion = 7 Then
                With oCmmd.Parameters
                    .AddWithValue("@TipoSoporte", TipoSoporte)
                    .AddWithValue("@CodTicket", CodTicket)
                    .AddWithValue("@TipoSolicitud", TipoSolicitud)
                    .AddWithValue("@Responsable", Responsable)
                    .AddWithValue("@NivelResponsable", NivelResponsable)
                End With
            Else
                If TipoAccion = 2 Then
                    With oCmmd.Parameters
                        .AddWithValue("@observacion", observacion)
                        .AddWithValue("@TipoSoporte", TipoSoporte)
                        .AddWithValue("@TipoSolicitud", TipoSolicitud)
                        .AddWithValue("@CodTicket", CodTicket)
                        .AddWithValue("@Responsable", Responsable)
                        .AddWithValue("@NivelResponsable", NivelResponsable)
                    End With
                Else
                    If TipoAccion = 4 Then
                        With oCmmd.Parameters
                            .AddWithValue("@TipoSoporte", TipoSoporte)
                            .AddWithValue("@CodTicket", CodTicket)
                            .AddWithValue("@TipoSolicitud", TipoSolicitud)
                            .AddWithValue("@observacion", observacion)
                            .AddWithValue("@Responsable", Responsable)
                            .AddWithValue("@NivelResponsable", NivelResponsable)
                        End With
                    Else
                        If TipoAccion = 6 Then
                            With oCmmd.Parameters
                                .AddWithValue("@TipoSoporte", TipoSoporte)
                                .AddWithValue("@CodTicket", CodTicket)
                                .AddWithValue("@TipoSolicitud", TipoSolicitud)
                            End With
                        End If
                    End If
                End If
            End If
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ActualizarColumnaTicket(Columna As String, Valor As String, CodTicket As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set " & Columna & " = " & Valor & " where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ActualizarColumnaTicket_2(Columna As String, Valor As String, CodTicket As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set " & Columna & " = '" & Valor & "' where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Ingresar_SeguimientoTicket(CodTicket As String, Descripcion As String, Usuario As String, Adjunto As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("insert into Help_Seguimientos (CodTicket,Descripcion,Adjunto,Fecha,Usuario) values (@CodTicket,@Descripcion,@Adjunto,GETDATE(),@Usuario)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@CodTicket", CodTicket)
                .AddWithValue("@Descripcion", Descripcion)
                .AddWithValue("@Adjunto", Adjunto)
                .AddWithValue("@Usuario", Usuario)
            End With
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Ingresar_Help_Ticket(CodTicket As String, TRespuesta As String, TAtencion As String, Observacion As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Ticket set Calif_Tiempo = @TRespuesta, Calif_Servicio = @TAtencion, Calif_Fecha = getdate(), Calif_Observacion = @Observacion where CodTicket = @CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            oCmmd.Parameters.AddWithValue("@TRespuesta", TRespuesta)
            oCmmd.Parameters.AddWithValue("@TAtencion", TAtencion)
            oCmmd.Parameters.AddWithValue("@Observacion", Observacion)
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_TicketCitado(IdReunion As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Reunion set Notificado=1 where Id_Reunion=@IdReunion")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@IdReunion", IdReunion)
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Actualizar_NoAsistenciaCita(IdReunion As String, CodTicket As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("update Help_Reunion set Habilitado=0 where CodTicket=@CodTicket and Id_Reunion=@IdReunion")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@IdReunion", IdReunion)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            oCmmd.Transaction = oTran
            oCmmd.ExecuteNonQuery()
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class
