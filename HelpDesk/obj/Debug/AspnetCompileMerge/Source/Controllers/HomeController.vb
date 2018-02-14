Imports System.IO
Imports Biblioteca

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Dim oConsultas As New Consultas,
        oProcesos As New Procesos,
        oFunciones As New Funciones

    Function Index() As ActionResult
        Return View()
    End Function

    Function TicketNuevo() As ActionResult
        Return View()
    End Function

    Function TicketCitados() As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        Return View()
    End Function

    Function Dashboard() As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        Return View()
    End Function


    Function AgendarTicket() As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        Return View()
    End Function

    Function TicketAsignados() As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        Return View()
    End Function

    Function Modal_VerTicket(CodTicket As String, Necesidad As String, Justificacion As String, Adjunto As String) As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        ViewBag.CodTicket = CodTicket
        ViewBag.Necesidad = Necesidad
        ViewBag.Justificacion = Justificacion
        ViewBag.Adjunto = Adjunto
        Return View()
    End Function

    Public Function LoguearUsuario(username As String, password As String) As ActionResult
        Try
            If IsNothing(Session("UserCod")) = True Then
                Session.Add("UserCod", Nothing)
                Session.Add("UserNom", Nothing)
                Session.Add("UserModulo", Nothing)
                Session.Add("UserGrupo", Nothing)
                Session.Timeout = 60
            End If
            Dim DtUsuario As DataTable = oConsultas.Consulta_LoginUsuario(username.Trim, password.Trim)
            If DtUsuario.Rows.Count > 0 Then
                Session("UserCod") = DtUsuario(0)("CODUSUARIO").ToString.Trim.ToUpper
                Session("UserNom") = DtUsuario(0)("NOMBRE").ToString.Trim.ToUpper
                Session("UserModulo") = "Home"
                Session("UserGrupo") = oConsultas.Consulta_GrupoUsuario(Session("UserCod").Trim)
                'Variables.sCodUsuario = DtUsuario(0)("CODUSUARIO").ToString.Trim
                FormsAuthentication.SetAuthCookie(DtUsuario(0)("CODUSUARIO").ToString.Trim.ToUpper, False)
                Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Error§:§§}]"), JsonRequestBehavior.AllowGet)
            Else
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Error§:§§}]"), JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json("[ERROR]:" + ex.Message)
        End Try
    End Function

    Function CerrarSesion() As ActionResult
        If IsNothing(Session) = False Then
            Session("UserCod") = Nothing
            Session("UserNom") = Nothing
            Session("UserModulo") = Nothing
            Session.Abandon()
        End If
        FormsAuthentication.SignOut()
        Return RedirectToAction("Index", "Home")
    End Function

    <HttpPost>
    Public Function GuardarTicket(TipoSolicitud As String, TipoSoporte As String, Necesidad As String, Justificacion As String, Archivo As String) As ActionResult
        Try
            Dim CodTicket As Integer = oConsultas.Consulta_ConsecutivoTicket,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                Msn As String = String.Empty,
                CodArea As String = String.Empty,
                CodJefe As String = String.Empty,
                Estado As String = "1"
            If CodTicket = 0 Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se pudo establecer el numero del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            If TipoSolicitud.Trim = "1" Then
                Estado = "2"
            End If
            Dim DtArea As DataTable = oConsultas.Consultar_AreaUsuario(CodUsuario)
            If DtArea.Rows.Count > 0 Then
                CodArea = DtArea.Rows(0)("Id").ToString.Trim
                CodJefe = DtArea.Rows(0)("Jefe").ToString.Trim
            End If
            If CodArea.ToString.Trim = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se ha configurado Aréa para el usuario§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Consultamos el usuario responsable del tipo de soporte
            Dim UsuarioResponsable As String = oConsultas.Consultar_ResponsableSoporte(TipoSoporte)
            If UsuarioResponsable = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se encontro usuario responsable§}]"), JsonRequestBehavior.AllowGet)
            End If

            oProcesos.BeginTransaction()
            Msn = oProcesos.IngresarTicket(CodTicket, CodUsuario, TipoSolicitud, TipoSoporte, Necesidad, Justificacion, Estado, Archivo, CodArea)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al ingresar el ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "0", "1", "", CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al ingresar el log del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            'Mensaje que se envia al usuario
            Dim Asunto As String = "Cajacopi - Ingreso Nuevo Ticket"
            Dim Cuerpo As String = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                .Replace("{Nombre}", NombreUsuario(CodUsuario)) _
                .Replace("{Necesidad}", StrConv(Necesidad, VbStrConv.ProperCase)) _
                .Replace("{Justificacion}", StrConv(Justificacion, VbStrConv.ProperCase))
            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodUsuario.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el comunicado interno§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se valida si es un incidente y se envia el correo al responsable. Para el caso de los requerimientos este correo se envia cuando
            'el jefe autorice el ticket.
            If TipoSolicitud.Trim = "1" Then
                Asunto = "Cajacopi - Ingreso Nuevo Ticket"
                Cuerpo = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(UsuarioResponsable)) _
                    .Replace("{Necesidad}", StrConv(Necesidad, VbStrConv.ProperCase)) _
                    .Replace("{Justificacion}", StrConv(Justificacion, VbStrConv.ProperCase))
                Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(UsuarioResponsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo al responsable§}]"), JsonRequestBehavior.AllowGet)
                End If

                'Si es incidente se ingresa el log de etapa 2 automaticamente
                Msn = oProcesos.Ingresar_LogTicket(CodTicket, "1", "2", "", CodUsuario)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al ingresar el log del ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If

            If TipoSolicitud.Trim = "2" Then
                'Mensaje que se le envia al jefe
                Dim link1 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "2", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)
                Dim link2 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "3", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)

                Asunto = "Cajacopi - Autorización Ticket"
                Cuerpo = My.Resources.Email_AutorizacionJefe.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(CodUsuario)) _
                    .Replace("{Necesidad}", StrConv(Necesidad, VbStrConv.ProperCase)) _
                    .Replace("{Justificacion}", StrConv(Justificacion, VbStrConv.ProperCase)) _
                    .Replace("{link1}", link1) _
                    .Replace("{link2}", link2)
                Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodUsuario.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el comunicado interno§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§" & CodTicket & "§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar el ticket§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function ActualizarTicket(Estado As String, CodTicket As String) As ActionResult
        Try
            Dim Msn As String = ""
            oProcesos.BeginTransaction()
            CambioEtapa(oProcesos, Estado, CodTicket, Msn)

            Return RedirectToAction("Index", "Home")
        Catch ex As Exception
            Return RedirectToAction("Error404", "Home")
        End Try
    End Function

    Public Function VerificacionDeTicket(Estado As String, CodTicket As String, Op As String, TipoSolicitud As String, TipoAccion As Integer, Observacion As String, Responsable As String, NivelResponsable As String) As ActionResult
        Try
            Dim Msn As String = ""
            If (TipoAccion = 2 Or TipoAccion = 4) And Observacion.ToString.Trim = "" Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe colocar una observacion§}]"), JsonRequestBehavior.AllowGet)
            End If
            If (Op.ToString.Trim = "" Or Op.ToString.Trim = "0") Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe seleccionar el tipo de soporte.§}]"), JsonRequestBehavior.AllowGet)
            End If
            Dim CodUsuario As String = Session("UserCod").ToString.Trim
            If Estado = 5 Then
                Dim Asunto As String = "Cajacopi - Reasignacion de Ticket"
                Dim Cuerpo As String = My.Resources.Email_Reasignar.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(Responsable))
                Dim Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Responsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo al responsable§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.BeginTransaction()
            Msn = oProcesos.ActualizarTipoSoporteTicket(Op, CodTicket, TipoSolicitud, TipoAccion, Observacion, Responsable, NivelResponsable)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "2", Estado, Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            End If
            If (CambioEtapa(oProcesos, Estado, CodTicket, Msn) = False) Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            Else
                Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar la etapa§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function


    'Public Function VerificacionDeTicket2(Estado As String, CodTicket As String, Op As String, TipoSolicitud As String) As ActionResult
    '    Try
    '        Dim Msn As String = ""
    '        oProcesos.BeginTransaction()
    '        Msn = oProcesos.ActualizarTipoSoporteTicket(Op, CodTicket, TipoSolicitud)
    '        If Msn <> "" Then
    '            oProcesos.RollBackTransaction()
    '            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
    '        End If
    '        If (CambioEtapa(oProcesos, Estado, CodTicket, Msn) = False) Then
    '            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
    '        Else
    '            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
    '        End If
    '    Catch ex As Exception
    '        Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar la etapa§}]"), JsonRequestBehavior.AllowGet)
    '    End Try
    'End Function

    Public Function CambioEtapa(xProceso As Procesos, Estado As String, CodTicket As String, ByRef Msn As String) As Boolean
        Try
            Msn = xProceso.ActualizarEstadoTicket(Estado, CodTicket)
            If Msn <> "" Then
                xProceso.RollBackTransaction()
                Return False
            End If
            xProceso.CommitTransaction()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Autorizar_Ticket(Estado As String, CodTicket As String, Usuario As String, Responsable As String) As ActionResult
        Try
            Dim Msn As String = String.Empty
            Dim Autorizado As Boolean = IIf(Estado = "2", True, False)
            oProcesos.BeginTransaction()
            Msn = oProcesos.Actualizar_TicketAutorizacion(CodTicket, Usuario, Autorizado, Estado)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return RedirectToAction("Error404", "Home")
            End If
            'Se ingresa registro de cambio de estado en el log
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "0", Estado, "", Usuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al ingresar el log del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.CommitTransaction()

            If Autorizado = True Then
                Dim InfoTicket As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
                If InfoTicket.Rows.Count > 0 Then
                    Dim Asunto As String = "Cajacopi - Ingreso Nuevo Ticket"
                    Dim Cuerpo As String = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(Responsable)) _
                        .Replace("{Necesidad}", StrConv(InfoTicket.Rows(0)("Necesidad").ToString.Trim, VbStrConv.ProperCase)) _
                        .Replace("{Justificacion}", StrConv(InfoTicket.Rows(0)("Justificacion").ToString.Trim, VbStrConv.ProperCase))
                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Responsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                End If
            End If
            Return RedirectToAction("Index", "Home")
        Catch ex As Exception
            Return RedirectToAction("Error404", "Home")
        End Try
    End Function
    Public Function ActualizarTicket_Reunion(Estado As String, Id As String) As ActionResult
        Try
            Dim Msn As String = String.Empty
            oProcesos.BeginTransaction()
            Msn = oProcesos.Actualizar_EstadoTicketReunion(Estado, Id)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return RedirectToAction("Error404", "Home")
            End If
            oProcesos.CommitTransaction()
            Return RedirectToAction("Index", "Home")
        Catch ex As Exception
            Return RedirectToAction("Error404", "Home")
        End Try
    End Function

    Public Function Consultar_TiposTicketPadre() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consulta_TipoSoportes_Padre
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consulta_TipoSoportes_Padre() As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consulta_TipoSoportes_Padre
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_TiposSoporte(TipoTicket As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consulta_TipoSoportes_Hijo(TipoTicket)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Consultar_TicketNuevoSinAsignar(Sesion As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_TicketNuevoSinAsignar(Sesion)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_EstTicektNuevo() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_EstTicektNuevo
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consulta_Reuniones() As String
        Try
            Dim Dt As DataTable = oConsultas.Consultar_ReunionTicket
            Return oFunciones.DatatableToJson(Dt)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function Consultar_MtAreas() As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_AreasxCitar()
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_TicketsxCitar(Area As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_TicketsxCitar(Area)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consulta_TicketsIngresados() As ActionResult
        Try
            Dim CodUsuario As String = Session("UserCod").ToString.Trim
            Dim Dt As DataTable = oConsultas.Consultar_TicketsIngresados(CodUsuario)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_Seguimientos(CodTicket As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Seguimientos(CodTicket)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_LogTickets(CodTicket As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_LogTickets(CodTicket)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Agendar_Cita(FechaCita As String, HoraCita As String, Cadena As String, Area As String) As ActionResult
        Dim TransaccionActiva As Boolean = False
        Try
            Dim DtCita As New DataTable,
                Cont As Integer = 0
            DtCita.Columns.Add(New DataColumn("Citado", Type.GetType("System.String")))
            DtCita.Columns.Add(New DataColumn("NroTicket", Type.GetType("System.String")))
            DtCita.Columns.Add(New DataColumn("Necesidad", Type.GetType("System.String")))
            DtCita.Columns.Add(New DataColumn("Usuario", Type.GetType("System.String")))

            'Recorremos Array
            Dim Array As Array = Cadena.Split("*")
            For Each item In Array
                Dim Array2 As Array = item.ToString.Split("~")
                If Array2(0).ToString = "true" Then
                    DtCita.Rows.Add()
                    DtCita.Rows(Cont)("Citado") = Array2(0).ToString
                    DtCita.Rows(Cont)("NroTicket") = Array2(1).ToString
                    DtCita.Rows(Cont)("Necesidad") = Array2(2).ToString
                    DtCita.Rows(Cont)("Usuario") = Array2(3).ToString
                    Cont += 1
                End If
            Next

            'Valido que la cita no sea para un dia inferior al actual
            If CDate(FechaCita) < CDate(Date.Now.ToString("yyyy-MM-dd")) Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No puede agendar una cita para una fecha inferior al dia actual§}]"), JsonRequestBehavior.AllowGet)
            End If

            If DtCita.Rows.Count <= 0 Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al procesar los tickets§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Valido que los tickets x citar pertenezcan a un solo usuario
            Dim UsuTicket As String = DtCita.Rows(0)("Usuario").ToString,
                TablaTicket As String = String.Empty
            For Each item As DataRow In DtCita.Rows
                If item("Usuario").ToString <> UsuTicket.Trim Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Los tickets a citar deben pertenecer a un solo usuario§}]"), JsonRequestBehavior.AllowGet)
                End If
                UsuTicket = item("Usuario").ToString.Trim
                TablaTicket = TablaTicket & "<tr><td>" & item("NroTicket") & "</td><td>" & item("Necesidad") & "</td></tr>"
            Next
            Dim Consecutivo As Integer = oConsultas.Consultar_ConsReunion,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                CantMinutos As Integer = DtCita.Rows.Count * 2
            If Consecutivo = 0 Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al generar consecutivo de reunión§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Validamos que no exista una reunion para la misma hora y fecha
            Dim ExisteReunion As Boolean = False
            ExisteReunion = oConsultas.Consultar_ExisteReunion(FechaCita, HoraCita)
            If ExisteReunion = True Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se puede agendar cita porque ya hay una en ese horario§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Procesamos Tickets
            Dim Msn As String = String.Empty,
                EmailUsu As String = oConsultas.Consultar_UsuarioPqrs(UsuTicket).Rows(0)("Email").ToString.Trim
            oProcesos.BeginTransaction()
            TransaccionActiva = True
            For Each item As DataRow In DtCita.Rows
                Msn = oProcesos.IngresarReunion(Consecutivo, item("NroTicket").ToString.Trim, FechaCita, HoraCita, CantMinutos, Area, item("Usuario").ToString.Trim, CodUsuario)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar registro de reunión§}]"), JsonRequestBehavior.AllowGet)
                End If

                Msn = oProcesos.ActualizarEstadoTicket("5", item("NroTicket").ToString.Trim)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            Next

            'Mensaje que se envia al usuario
            Dim link1 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "9", .Id = Consecutivo}, Request.Url.Scheme)
            Dim link2 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "10", .Id = Consecutivo}, Request.Url.Scheme)

            Dim Asunto As String = "Cajacopi - Citación Ticket"
            Dim Cuerpo As String = My.Resources.Email_CitacionTickets.Replace("{Nombre}", NombreUsuario(CodUsuario)) _
                .Replace("{Fecha}", StrConv(FechaCita, VbStrConv.ProperCase)) _
                .Replace("{Hora}", StrConv(HoraCita, VbStrConv.ProperCase)) _
                .Replace("{Tabla}", TablaTicket) _
                .Replace("{link1}", link1) _
                .Replace("{link2}", link2)
            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(EmailUsu, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
            End If

            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§Reunión agendada exitosamente§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            If TransaccionActiva = True Then
                oProcesos.RollBackTransaction()
            End If
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error insesperado.§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Actualizar_Reunion(Fecha As String, Hora As String, Id As String) As ActionResult
        Try
            'Validamos que no exista una reunion para la misma hora y fecha
            Dim ExisteReunion As Boolean = False
            ExisteReunion = oConsultas.Consultar_ExisteReunion(Fecha, Hora)
            If ExisteReunion = True Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se puede agendar cita porque ya hay una en ese horario§}]"), JsonRequestBehavior.AllowGet)
            End If

            Dim Msn As String = String.Empty,
                DtTickets As DataTable = oConsultas.Consultar_TicketsReunion(Id),
                TablaTicket As String = String.Empty, CodUsuario As String = String.Empty

            For Each item As DataRow In DtTickets.Rows
                TablaTicket = TablaTicket & "<tr><td>" & item("CodTicket") & "</td><td>" & item("Necesidad") & "</td></tr>"
                CodUsuario = item("CodUsuario").ToString.Trim
            Next

            oProcesos.BeginTransaction()
            Msn = oProcesos.Actualizar_Reunion(Fecha, Hora, Id)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al actualizar la reunión§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Mensaje que se le envia al jefe
            Dim link1 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "9", .Id = Id}, Request.Url.Scheme)
            Dim link2 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "10", .Id = Id}, Request.Url.Scheme)

            Dim Asunto As String = "Cajacopi - Citación Ticket"
            Dim Cuerpo As String = My.Resources.Email_CitacionTickets.Replace("{Nombre}", NombreUsuario(CodUsuario)) _
                .Replace("{Fecha}", StrConv(Fecha, VbStrConv.ProperCase)) _
                .Replace("{Hora}", StrConv(Hora, VbStrConv.ProperCase)) _
                .Replace("{Tabla}", TablaTicket) _
                .Replace("{link1}", link1) _
                .Replace("{link2}", link2)
            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodUsuario).Rows(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§Reunión actualizada exitosamente§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error insesperado.§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consulta_TicketsCitados() As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_TicketsCitados
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consulta_ResponsableTickets() As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Responsables
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consulta_ResponsableTicketsDt() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Responsables()
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consulta_ResponsableTickets2(TipoSoporte) As ActionResult
        Try
            Dim pSoporte As String = TipoSoporte(0).ToString.Trim.ToUpper
            Dim Dt As DataTable = oConsultas.Consultar_Responsables(pSoporte)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consulta_TicketsAsignados() As ActionResult
        Try
            Dim Responsable As String = Session("UserCod").ToString.Trim
            Dim Dt As DataTable = oConsultas.Consultar_TicketsAsignados(Responsable)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Guardar_NoAsistenciaTickets(CodTicket As String, Observacion As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                CodUsuario As String = Session("UserCod").ToString.Trim
            oProcesos.BeginTransaction()
            Msn = oProcesos.ActualizarEstadoTicket("12", CodTicket)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al actualizar el estado del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "8", "12", Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al ingresar log del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Motivo§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Actualizar_ColumnaTicket(Columna As String, Valor As String, CodTicket As String) As ActionResult
        Try
            Dim Msn As String = String.Empty
            oProcesos.BeginTransaction()
            Msn = oProcesos.ActualizarColumnaTicket(Columna, Valor, CodTicket)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el estado del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Actualizar_EvaluacionTicket(CodTicket As String, Responsable As String, Prioridad As String, Viabilidad As String, Duracion As String, Observacion As String, Necesidad As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                Estado As String = "13",
                EmailResp As String = String.Empty
            EmailResp = Responsable.Split("*")(1).ToString.Trim
            Responsable = Responsable.Split("*")(0).ToString.Trim

            If CBool(Viabilidad) = False Then
                Estado = "14"
                Prioridad = ""
                Duracion = ""
                Responsable = ""

                If Observacion.Trim = "" Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe ingresar una observación§}]"), JsonRequestBehavior.AllowGet)
                End If
            Else
                If Duracion = "0" Or Duracion.Trim = "" Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe ingresar una duración valida§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.BeginTransaction()
            Msn = oProcesos.Actualizar_TicketEvaluacion(CodTicket, Responsable, Prioridad, Viabilidad, Duracion, Estado)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el ticket§}]"), JsonRequestBehavior.AllowGet)
            End If

            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "6", Estado, Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar log del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If

            If Responsable <> "" Then
                Dim Asunto As String = "Cajacopi - Asignación Ticket"
                Dim Cuerpo As String = My.Resources.Email_TicketAsignado.Replace("{Nombre}", NombreUsuario(Responsable)) _
                    .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                    .Replace("{Tiempo}", Duracion) _
                    .Replace("{Necesidad}", Necesidad)

                Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(EmailResp, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If

            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Guardar_Seguimiento(CodTicket As String, Observacion As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            If Observacion.Trim = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe ingresar una observación§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.BeginTransaction()
            Msn = oProcesos.Ingresar_SeguimientoTicket(CodTicket, Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el estado del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se envia correo al usuario que ingreso el ticket
            If EnviarCorreoSeguimiento(DtInfo.Rows(0)("CodUsuario").ToString, CodTicket, Observacion) <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al notificar al usuario§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se envia correo al responsable del ticket si lo hay
            If DtInfo.Rows(0)("Responsable").ToString <> String.Empty Then
                If EnviarCorreoSeguimiento(DtInfo.Rows(0)("CodUsuario").ToString, CodTicket, Observacion) <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al notificar al responsable del ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Actualizar_TicketsAsignados(CodTicket As String, Estado As String) As ActionResult
        Try
            Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket),
                CodUsuario As String = Session("UserCod").ToString.Trim,
                Msn As String = String.Empty
            If DtInfo.Rows.Count > 0 Then
                oProcesos.BeginTransaction()
                Msn = oProcesos.ActualizarEstadoTicket(Estado, CodTicket)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al actualizar estado§}]"), JsonRequestBehavior.AllowGet)
                End If
                Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtInfo.Rows(0)("Estado").ToString, Estado, "", CodUsuario)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al ingresar el log§}]"), JsonRequestBehavior.AllowGet)
                End If
                oProcesos.CommitTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§Información Guardada Exitosamente§}]"), JsonRequestBehavior.AllowGet)
            Else
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se encontro información del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error Inesperado: Catch§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function
    Function EnviarCorreoSeguimiento(Usuario As String, CodTicket As String, Seguimiento As String) As String
        Try
            Dim Asunto As String = "Cajacopi - Seguimiento Ticket"
            Dim Cuerpo As String = My.Resources.Email_NuevoSeguimiento.Replace("{NroTicket}", CodTicket) _
                .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                .Replace("{Seguimiento}", Seguimiento)

            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Usuario).Rows(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                Return "Error al Enviar Correo"
            End If
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function Consulta_TipoSoportes_Id(idTipoSoporte As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consulta_TipoSoportes_Id(idTipoSoporte)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function
    Public Function DescargarAdjunto(Url As String) As ActionResult
        Try
            Url = Url.ToString.Trim
            Dim NombreArchivo As String = Path.GetFileName(Url)
            Return File(Url, "application/pdf", Server.UrlEncode(NombreArchivo))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function NombreUsuario(CodUsuario As String) As String
        Try
            Dim Dt As DataTable = oConsultas.Consultar_UsuarioPqrs(CodUsuario)
            If Dt.Rows.Count > 0 Then
                Return Dt.Rows(0)("Nombre").ToString.Trim
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
