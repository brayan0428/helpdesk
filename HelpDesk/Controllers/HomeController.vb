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

    Function TicketCalificacion(CodTicket As String) As ActionResult
        ViewBag.CodTicket = CodTicket
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


    Function Graficas() As ActionResult
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

    Function ReenviarCorreo() As ActionResult
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

    Function Modal_ProcesarTicket(CodTicket As String, Necesidad As String, Justificacion As String, Adjunto As String) As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
        ViewBag.CodTicket = CodTicket
        ViewBag.Necesidad = Necesidad
        ViewBag.Justificacion = Justificacion
        ViewBag.Adjunto = Adjunto
        Return View()
    End Function

    Function AutorizacionTicket() As ActionResult
        Return View()
    End Function

    Function Estadisticas() As ActionResult
        If IsNothing(Session("UserCod")) Then
            Return RedirectToAction("Index", "Home")
        End If
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
        Dim TransaccionActiva As Boolean = False
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
            Dim Excepcion As Boolean = oConsultas.Consultar_ExcepcionesTicket(CodUsuario.Trim)
            If TipoSolicitud.Trim = "1" Then
                Estado = "2"
            Else
                If Excepcion = True Then
                    Estado = "2"
                End If
            End If
            Dim DtArea As DataTable = oConsultas.Consultar_AreaUsuario(CodUsuario)
            If DtArea.Rows.Count > 0 Then
                CodArea = DtArea.Rows(0)("Id").ToString.Trim
                CodJefe = DtArea.Rows(0)("Jefe").ToString.Trim
            End If
            If CodArea.ToString.Trim = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se ha configurado Aréa para el usuario§}]"), JsonRequestBehavior.AllowGet)
            End If

            Dim DtSinCerrar As DataTable = oConsultas.Consultar_ContadorTicketsinCerrar(CodUsuario)
            If DtSinCerrar.Rows.Count > 0 Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Usted tiene " & DtSinCerrar.Rows.Count & " ticket solucionado que estan pendientes por calificar, favor verifique su correo§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Consultamos el usuario responsable del tipo de soporte
            Dim UsuarioResponsable As String = oConsultas.Consultar_ResponsableSoporte(TipoSoporte)
            If UsuarioResponsable = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se encontro usuario responsable§}]"), JsonRequestBehavior.AllowGet)
            End If
            'If (UsuarioResponsable = "JCARDALES" Or UsuarioResponsable = "ARODRIGUEZ") And (TipoSolicitud = 2) Then
            '    Estado = "2"
            'End If
            oProcesos.BeginTransaction()
            TransaccionActiva = True
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
                .Replace("{Necesidad}", Necesidad.ToLower) _
                .Replace("{Justificacion}", Justificacion.ToLower)
            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodUsuario.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el comunicado interno§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se valida si es un incidente y se envia el correo al responsable. Para el caso de los requerimientos este correo se envia cuando
            'el jefe autorice el ticket.
            If TipoSolicitud.Trim = "1" Or (TipoSolicitud.Trim = "2" And Excepcion = True) Then
                Asunto = "Cajacopi - Ingreso Nuevo Ticket"
                Cuerpo = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(UsuarioResponsable)) _
                    .Replace("{Necesidad}", Necesidad.ToLower) _
                    .Replace("{Justificacion}", Justificacion.ToLower)
                Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(UsuarioResponsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo al responsable§}]"), JsonRequestBehavior.AllowGet)
                End If

                'Si es incidente se ingresa el log de etapa 2 automaticamente
                Msn = oProcesos.Ingresar_LogTicket(CodTicket, "1", "2", "", CodUsuario)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al ingresar el log del ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If

            If TipoSolicitud.Trim = "2" And Excepcion = False Then
                'Mensaje que se le envia al jefe
                Dim link1 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "2", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)
                Dim link2 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "3", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)

                Asunto = "Cajacopi - Autorización Ticket"
                Cuerpo = My.Resources.Email_AutorizacionJefe.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(CodJefe)) _
                    .Replace("{CodUsuario}", NombreUsuario(CodUsuario)) _
                    .Replace("{Necesidad}", Necesidad.ToLower) _
                    .Replace("{Justificacion}", Justificacion.ToLower) _
                    .Replace("{link1}", link1) _
                    .Replace("{link2}", link2)
                Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodJefe.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, Archivo).ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el comunicado interno§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§" & CodTicket & "§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            If TransaccionActiva = True Then
                oProcesos.RollBackTransaction()
            End If
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
            Dim Autorizado As Boolean = IIf(Estado = "2", True, False),
                DtTicket As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            If DtTicket.Rows.Count > 0 Then
                If DtTicket.Rows(0)("Estado").ToString.Trim <> "1" Then
                    Return RedirectToAction("Index", "Home")
                End If
            End If
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
                        .Replace("{Necesidad}", InfoTicket.Rows(0)("Necesidad").ToString.Trim.ToLower) _
                        .Replace("{Justificacion}", InfoTicket.Rows(0)("Justificacion").ToString.Trim.ToLower)
                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Responsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                End If
            End If
            Return RedirectToAction("AutorizacionTicket", "Home")
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
            If Estado = "10" Then
                Msn = oProcesos.Actualizar_Reunion(Id)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return RedirectToAction("Error404", "Home")
                End If
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

    Public Function Consultar_InfoTicket(CodTicket As String) As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_InfoTicket_Json(CodTicket As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
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

    Public Function Consultar_Proveedores() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Proveedores()
            Return Dt
        Catch ex As Exception
            Return Nothing
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

    Public Function Consultar_EstTicektNuevo(CodUsuario) As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_EstTicektNuevo(CodUsuario)
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_ContadorTicketsUsuarios(CodUsuario) As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_ContadorTicketsUsuarios(CodUsuario)
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
            Dim Dt As DataTable = oConsultas.Consultar_TicketsIngresados_New3(CodUsuario)
            'If Dt.Rows.Count <= 0 Then
            '    Dt.Rows.Clear()
            '    Dt = oConsultas.Consultar_TicketsIngresados_New_2(CodUsuario)
            'End If
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

                Msn = oProcesos.ActualizarEstadoTicket("8", item("NroTicket").ToString.Trim)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            Next

            'Mensaje que se envia al usuario
            Dim link1 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "9", .Id = Consecutivo}, Request.Url.Scheme)
            Dim link2 As String = Url.Action("ActualizarTicket_Reunion", "Home", New With {.Estado = "10", .Id = Consecutivo}, Request.Url.Scheme)

            Dim Asunto As String = "Cajacopi - Citación Ticket"
            Dim Cuerpo As String = My.Resources.Email_CitacionTickets.Replace("{Nombre}", NombreUsuario(UsuTicket)) _
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

    Public Function Consulta_Responsables_Soporte(TipoSoporte) As DataTable
        Try
            Dim NombreSoporte As String = oConsultas.Consulta_TipoSoportes_Id(TipoSoporte).Rows(0)("TipoTicket").ToString
            Dim Dt As DataTable = oConsultas.Consultar_Responsables(NombreSoporte)
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_Usuarios() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_UsuarioPqrs
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_TicketRecordatorio() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_TicketRecordatorio
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_EstadisticasResponsables() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_EstadisticasResponsables
            Return Dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Consultar_EstadisticasTicketTotal() As DataTable
        Try
            Dim Dt As DataTable = oConsultas.Consultar_EstadisticasTicketTotal
            Return Dt
        Catch ex As Exception
            Return Nothing
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

    Public Function Guardar_NoAsistenciaTickets(IdReunion As String, CodTicket As String, Observacion As String) As ActionResult
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
            Msn = oProcesos.Actualizar_NoAsistenciaCita(IdReunion, CodTicket)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al actualizar estado de reunion§}]"), JsonRequestBehavior.AllowGet)
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
            Responsable = Responsable.Split("*")(0).ToString
            Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)

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

            Msn = oProcesos.Ingresar_LogTicket(CodTicket, "9", Estado, Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar log del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If

            If Responsable <> "" Then
                Dim Asunto As String = "Cajacopi - Asignación Ticket"
                Dim Cuerpo As String = My.Resources.Email_TicketAsignado.Replace("{Nombre}", NombreUsuario(Responsable)) _
                    .Replace("{UsuarioIngreso}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString.Trim)) _
                    .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                    .Replace("{Tiempo}", Duracion) _
                    .Replace("{Necesidad}", Necesidad) _
                    .Replace("{Justificacion}", "") _
                    .Replace("{Observacion}", "")
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

    Public Function Guardar_Seguimiento(CodTicket As String, Observacion As String, UsuCopia As String, Adjunto As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            If Observacion.Trim = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe ingresar una observación§}]"), JsonRequestBehavior.AllowGet)
            End If
            Dim CC As New List(Of String)(UsuCopia.Split(","))
            oProcesos.BeginTransaction()
            Msn = oProcesos.Ingresar_SeguimientoTicket(CodTicket, Observacion, CodUsuario, Adjunto)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el estado del ticket§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se envia correo al usuario que ingreso el ticket
            If EnviarCorreoSeguimiento(DtInfo.Rows(0)("CodUsuario").ToString, CodTicket, Observacion, DtInfo.Rows(0)("Necesidad").ToString.Trim, DtInfo.Rows(0)("Justificacion").ToString.Trim, Adjunto) <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al notificar al usuario§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Se envia correo al responsable del ticket si lo hay
            If DtInfo.Rows(0)("Responsable").ToString <> String.Empty Then
                If EnviarCorreoSeguimiento(DtInfo.Rows(0)("Responsable").ToString, CodTicket, Observacion, DtInfo.Rows(0)("Necesidad").ToString.Trim, DtInfo.Rows(0)("Justificacion").ToString.Trim, Adjunto, CC) <> "" Then
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
                If Estado.Trim = "17" Then
                    Dim link As String = Url.Action("SatisfaccionTicket", "Home", New With {.CodTicket = CodTicket}, Request.Url.Scheme)
                    Dim Asunto As String = "Cajacopi - Finalización Ticket"
                    Dim Cuerpo As String = My.Resources.Email_Satisfaccion.Replace("{Nombre}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString)) _
                        .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                        .Replace("{link1}", link & "?Observacion") _
                        .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString)

                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(DtInfo.Rows(0)("CodUsuario").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                    If Val_Email = False Then
                        oProcesos.RollBackTransaction()
                        Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                    End If
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

    Function EnviarCorreoSeguimiento(Usuario As String, CodTicket As String, Seguimiento As String, Necesidad As String, Justificacion As String, Optional Adjunto As String = "", Optional CC As List(Of String) = Nothing) As String
        Try
            Dim Asunto As String = "Cajacopi - Seguimiento Ticket"
            Dim Cuerpo As String = My.Resources.Email_NuevoSeguimiento.Replace("{NroTicket}", CodTicket) _
                .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                .Replace("{Seguimiento}", Seguimiento) _
                .Replace("{Necesidad}", Necesidad) _
                .Replace("{Justificacion}", Justificacion)

            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Usuario).Rows(0)("Email").ToString.Trim, Asunto, Cuerpo, Adjunto, CC).ToString)
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

    Public Function CalificarTicket(CodTicket As String, TRespuesta As String, TAtencion As String, Observacion As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                Procesar As Boolean = False
            Dim DtTicket As DataTable = oConsultas.Consultar_Help_Ticket(CodTicket)
            If IsNothing(DtTicket) = False Then
                If DtTicket.Rows.Count > 0 Then
                    If DtTicket(0)("Calif").ToString.Trim = "" Then
                        Procesar = True
                    Else
                        Procesar = False
                    End If
                Else
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
                End If
            Else
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
            End If
            If Procesar = True Then
                Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket),
                    CodUsuario As String = Session("UserCod").ToString.Trim
                If IsNothing(DtInfo) = True Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
                Else
                    If DtInfo.Rows.Count <= 0 Then
                        Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
                    End If
                End If
                oProcesos.BeginTransaction()
                Msn = oProcesos.ActualizarEstadoTicket("23", CodTicket)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al actualizar estado§}]"), JsonRequestBehavior.AllowGet)
                End If
                Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtInfo.Rows(0)("Estado").ToString, "23", Observacion, CodUsuario)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al ingresar el log§}]"), JsonRequestBehavior.AllowGet)
                End If
                Msn = oProcesos.Ingresar_Help_Ticket(CodTicket, TRespuesta, TAtencion, Observacion)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar el estado del ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
                oProcesos.CommitTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
            Else
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Ya el ticket fue solucionado.§}]"), JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function CancelarCalificarTicket(CodTicket As String, Observacion As String) As ActionResult
        Try
            Dim Msn As String = String.Empty,
                DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket),
                CodUsuario As String = Session("UserCod").ToString.Trim
            If IsNothing(DtInfo) = True Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
            Else
                If DtInfo.Rows.Count <= 0 Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.BeginTransaction()
            Dim Asunto As String = "Cajacopi - Reasignacion de Ticket"
            Dim Cuerpo As String = My.Resources.Email_NoSatisfecha.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(DtInfo.Rows(0)("Responsable").ToString.Trim))
            Dim Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(DtInfo.Rows(0)("Responsable").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo al responsable§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.ActualizarEstadoTicket("21", CodTicket)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al actualizar estado§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtInfo.Rows(0)("Estado").ToString, "21", Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se presento un error al ingresar el log§}]"), JsonRequestBehavior.AllowGet)
            End If
            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Reasignar_Recurso(CodTicket As String, Recurso As String, Observacion As String) As ActionResult
        Try
            Dim DtInfoTicket As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket),
                Estado As String = String.Empty,
                Responsable_Old As String = String.Empty,
                Msn As String = String.Empty,
                CodUsuario As String = Session("UserCod").ToString.Trim,
                Email_RecursoNew As String = String.Empty

            Responsable_Old = DtInfoTicket.Rows(0)("Responsable").ToString.Trim
            Email_RecursoNew = Recurso.Split("*")(1).ToString.Trim
            Recurso = Recurso.Split("*")(0).ToString.Trim

            If Responsable_Old = Recurso Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§El nuevo recurso debe ser diferente al que esta actualmente§}]"), JsonRequestBehavior.AllowGet)
            End If

            If Observacion.Trim = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe ingresar una observación§}]"), JsonRequestBehavior.AllowGet)
            End If

            oProcesos.BeginTransaction()
            Msn = oProcesos.ActualizarColumnaTicket_2("Responsable", Recurso, CodTicket)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al actualizar responsable§}]"), JsonRequestBehavior.AllowGet)
            End If

            Msn = oProcesos.Ingresar_SeguimientoTicket(CodTicket, "Ticket Reasignado a " & Recurso.ToUpper & ". Motivo: " & Observacion, CodUsuario, "")
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar seguimiento§}]"), JsonRequestBehavior.AllowGet)
            End If

            Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtInfoTicket.Rows(0)("Estado").ToString, "24", Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al ingresar log§}]"), JsonRequestBehavior.AllowGet)
            End If

            Dim Asunto As String = "Cajacopi - Reasignacion de Ticket"
            Dim Cuerpo As String = My.Resources.Email_Reasignar.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(Recurso))
            Dim Val_Email = CBool(oFunciones.EnviarCorreo(Email_RecursoNew, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo al nuevo responsable§}]"), JsonRequestBehavior.AllowGet)
            End If

            oProcesos.CommitTransaction()
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§Guardado Exitosamente§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_Graficas(Tipo As String, FechaIni As String, FechaFin As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Graficas_Sendesk(Tipo, FechaIni, FechaFin)
            Return Json(oFunciones.DatatableToJson(Dt), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_Graficas_Excel(Tipo As String, FechaIni As String, FechaFin As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Graficas_Sendesk(Tipo, FechaIni, FechaFin)
            Dim grid = New GridView()
            grid.DataSource = Dt
            grid.DataBind()
            Response.ClearContent()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment; filename=Reporte_Sendesk_" & Date.Now.ToString("dd-MM-yy_hh-mm") & ".xls")
            Response.ContentType = "application/ms-excel"
            Response.Charset = ""
            Dim sw As New StringWriter()
            Dim htw As New HtmlTextWriter(sw)
            grid.RenderControl(htw)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.End()
            Return View()
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function Consultar_DatosGraficas(Tipo As String, FechaIni As String, FechaFin As String) As ActionResult
        Try
            Dim Dt As DataTable = oConsultas.Consultar_Graficas_Sendesk(Tipo, FechaIni, FechaFin)
            Dim chartData As New List(Of Object)()
            chartData.Add(New Object() {"Usuario", "Cantidad"})
            For Each item As DataRow In Dt.Rows
                chartData.Add(New Object() {item("Nombre").ToString.Trim, CInt(item("NroTicket").ToString)})
            Next
            Return Json(chartData, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de aplicacion§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function VerificacionDeTicket(Estado As String, CodTicket As String, Op As String, TipoSolicitud As String, TipoAccion As Integer, Observacion As String, Responsable As String, NivelResponsable As String) As ActionResult
        Try
            Dim Msn As String = ""
            Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            If (TipoAccion = 2 Or TipoAccion = 4) And Observacion.ToString.Trim = "" Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe colocar una observacion§}]"), JsonRequestBehavior.AllowGet)
            End If
            If (Op.ToString.Trim = "" Or Op.ToString.Trim = "0") Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Debe seleccionar el tipo de soporte.§}]"), JsonRequestBehavior.AllowGet)
            End If
            Dim CodUsuario As String = Session("UserCod").ToString.Trim
            If Estado = 5 Then
                Dim CodResp As DataTable = oConsultas.Consulta_TipoSoportes_Id(Op)
                Responsable = ""
                Dim Asunto As String = "Cajacopi - Reasignacion de Ticket"
                Dim Cuerpo As String = My.Resources.Email_Reasignar.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(CodResp.Rows(0)("Responsable").ToString.Trim))
                Dim Val_Email = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodResp.Rows(0)("Responsable").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Motivo§:§Error al enviar el correo al responsable§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            If Estado = 6 Then
                Dim link As String = Url.Action("SatisfaccionTicket", "Home", New With {.CodTicket = CodTicket}, Request.Url.Scheme)
                Dim Asunto As String = "Cajacopi - Finalización Ticket"
                Dim Cuerpo As String = My.Resources.Email_Satisfaccion.Replace("{Nombre}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString)) _
                    .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                    .Replace("{link1}", link & "?Observacion") _
                    .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString)
                Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(DtInfo.Rows(0)("CodUsuario").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            Dim DtinfoTicket As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            If IsNothing(DtinfoTicket) = True Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de Aplicacion.§}]"), JsonRequestBehavior.AllowGet)
            Else
                If DtinfoTicket.Rows.Count <= 0 Then
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de Aplicacion.§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            oProcesos.BeginTransaction()
            Msn = oProcesos.ActualizarTipoSoporteTicket(Op, CodTicket, TipoSolicitud, TipoAccion, Observacion, Responsable, NivelResponsable)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            End If
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtinfoTicket(0)("Estado"), Estado, Observacion, CodUsuario)
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

    Function VerificacionDeTicket_New(CodTicket As String, Accion As String, TipoSolicitud As String, TipoSoporte As String, Prioridad As String,
                                      Observacion As String, Recurso As String, UsuCopia As String) As ActionResult
        Dim TransaccionActiva As Boolean = False
        Try
            Dim Msn As String = ""
            Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket),
                Estado As String = String.Empty,
                CC As New List(Of String)(UsuCopia.Split(",")),
                CodUsuario As String = Session("UserCod").ToString.Trim

            Dim UsuarioResponsable As String = oConsultas.Consultar_ResponsableSoporte(TipoSoporte)
            If UsuarioResponsable = String.Empty Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§No se encontro usuario responsable§}]"), JsonRequestBehavior.AllowGet)
            End If
            Dim DtResponsables As DataTable = Consulta_Responsables_Soporte(TipoSoporte),
                ExisteResp As Boolean = False
            If DtResponsables.Rows.Count > 0 Then
                For Each item As DataRow In DtResponsables.Rows
                    If item("CodUsuario").ToString.Trim = Recurso.ToString.Trim Then
                        ExisteResp = True
                    End If
                Next
            End If
            oProcesos.BeginTransaction()
            TransaccionActiva = True
            If Accion = "1" Or Accion = "7" Then
                'Ticket Verificado o Requerimiento Especial
                If ExisteResp = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§El recurso seleccionado no pertenece a este tipo de aplicación§}]"), JsonRequestBehavior.AllowGet)
                End If
                If Accion = "1" Then
                    If TipoSolicitud = "1" Then
                        Estado = "13"
                    Else
                        If Recurso.Trim = "ARODRIGUEZ" Or Recurso.Trim = "JCARDALES" Then
                            Estado = "13"
                        Else
                            Estado = "4"
                            Recurso = ""
                        End If
                    End If
                Else
                    Estado = "13"
                End If
                If Estado = "13" Then
                    Dim Asunto As String = "Cajacopi - Asignación Ticket"
                    Dim Cuerpo As String = My.Resources.Email_TicketAsignado.Replace("{Nombre}", NombreUsuario(Recurso)) _
                            .Replace("{UsuarioIngreso}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString)) _
                            .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                            .Replace("{Tiempo}", "24") _
                            .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString) _
                            .Replace("{Justificacion}", DtInfo.Rows(0)("Justificacion").ToString) _
                            .Replace("{Observacion}", Observacion)
                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(Recurso)(0)("Email").ToString.Trim, Asunto, Cuerpo, DtInfo.Rows(0)("Adjunto").ToString, CC).ToString)
                    If Val_Email = False Then
                        oProcesos.RollBackTransaction()
                        Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo al recurso§}]"), JsonRequestBehavior.AllowGet)
                    End If
                End If
            End If
            If Accion = "3" Then
                'Ticket Solucionado
                Estado = "6"
                Dim link As String = Url.Action("SatisfaccionTicket", "Home", New With {.CodTicket = CodTicket}, Request.Url.Scheme)
                Dim Asunto As String = "Cajacopi - Finalización Ticket"
                Dim Cuerpo As String = My.Resources.Email_Satisfaccion.Replace("{Nombre}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString)) _
                    .Replace("{NroTicket}", StrConv(CodTicket, VbStrConv.ProperCase)) _
                    .Replace("{link1}", link & "?Observacion") _
                    .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString)

                Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(DtInfo.Rows(0)("CodUsuario").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            If Accion = "4" Then
                'Ticket Reasignado
                Estado = "5"
                Dim Asunto As String = "Cajacopi - Ingreso Nuevo Ticket"
                Dim Cuerpo As String = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(UsuarioResponsable)) _
                    .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString.Trim.ToLower) _
                    .Replace("{Justificacion}", DtInfo.Rows(0)("Justificacion").ToString.Trim.ToLower)
                Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(UsuarioResponsable.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            If Accion = "5" Then
                'Ticket Rechazado
                Estado = "7"
                Dim Asunto As String = "Cajacopi - Rechazo Ticket"
                Dim Cuerpo As String = My.Resources.Email_NuevoTicket.Replace("{NroTicket}", CodTicket) _
                    .Replace("{Nombre}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString.Trim)) _
                    .Replace("{Motivo}", Observacion)
                Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(DtInfo.Rows(0)("CodUsuario").ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                If Val_Email = False Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            If Accion = "6" Then
                Estado = DtInfo.Rows(0)("Estado").ToString.Trim
                'Si el ticket pasa de incidente a requerimiento, se debe enviar al jefe el correo para que realice la respectiva aprobación
                If DtInfo.Rows(0)("TipoSolicitud").ToString.Trim = "1" And TipoSolicitud = "2" Then
                    Estado = "1"
                    'Consulto al Jefe
                    Dim DtArea As DataTable = oConsultas.Consultar_AreaUsuario(DtInfo.Rows(0)("CodUsuario").ToString.Trim),
                        CodJefe As String = String.Empty
                    If DtArea.Rows.Count > 0 Then
                        CodJefe = DtArea.Rows(0)("Jefe").ToString.Trim
                    End If

                    'Mensaje que se le envia al jefe
                    Dim link1 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "2", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)
                    Dim link2 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "3", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)

                    Dim Asunto As String = "Cajacopi - Autorización Ticket"
                    Dim Cuerpo As String = My.Resources.Email_AutorizacionJefe.Replace("{NroTicket}", CodTicket) _
                        .Replace("{Nombre}", NombreUsuario(CodJefe)) _
                        .Replace("{CodUsuario}", NombreUsuario(DtInfo.Rows(0)("CodUsuario").ToString.Trim)) _
                        .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString.Trim.ToLower) _
                        .Replace("{Justificacion}", DtInfo.Rows(0)("Justificacion").ToString.Trim.ToLower) _
                        .Replace("{link1}", link1) _
                        .Replace("{link2}", link2)
                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodJefe.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, DtInfo.Rows(0)("Adjunto").ToString.Trim).ToString)
                    If Val_Email = False Then
                        oProcesos.RollBackTransaction()
                        Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el correo al jefe§}]"), JsonRequestBehavior.AllowGet)
                    End If
                End If
            End If
            If Accion <> 5 Then
                'Actualizar Tickets
                Msn = oProcesos.ActualizarTipoSoporteTicket(TipoSoporte, CodTicket, TipoSolicitud, Accion, Observacion, Recurso, Prioridad)
                If Msn <> "" Then
                    oProcesos.RollBackTransaction()
                    Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Se produjo un error al actualizar el ticket§}]"), JsonRequestBehavior.AllowGet)
                End If
            End If
            'Ingresar LogTickets
            Msn = oProcesos.Ingresar_LogTicket(CodTicket, DtInfo(0)("Estado"), Estado, Observacion, CodUsuario)
            If Msn <> "" Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            End If

            'Cambio de Etapa del Ticket
            If (CambioEtapa(oProcesos, Estado, CodTicket, Msn) = False) Then
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§" & Msn & "§}]"), JsonRequestBehavior.AllowGet)
            End If
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            If TransaccionActiva = True Then
                oProcesos.RollBackTransaction()
            End If
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de la aplicación§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function

    Public Function ExportarTicketsVerificados() As ActionResult
        Dim DtConsulta As DataTable = Nothing,
            CodUsuario As String = Session("UserCod").ToString.Trim
        DtConsulta = oConsultas.Consultar_TicketsVerificados(CodUsuario)
        Dim grid = New GridView()
        grid.DataSource = DtConsulta
        grid.DataBind()
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment; filename=Listado_TicketVerificados.xls")
        Response.ContentType = "application/ms-excel"
        Response.Charset = ""
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        grid.RenderControl(htw)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
        Return View()
    End Function

    Public Function RecordatorioCitacion() As Boolean
        Try
            Dim DtCitar As DataTable = oConsultas.Consultar_TicketRecordatorio
            If DtCitar.Rows.Count > 0 Then
                For Each item As DataRow In DtCitar.Rows
                    Dim Asunto As String = "Cajacopi - Recordatorio Citación Ticket"
                    Dim Cuerpo As String = My.Resources.Email_RecordatorioCita.Replace("{Nombre}", NombreUsuario(item("UsuSolicita").ToString)) _
                        .Replace("{Fecha}", item("Fecha")) _
                        .Replace("{Hora}", item("Hora").ToString.Substring(0, item("Hora").ToString.Length - 3))
                    Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(item("UsuSolicita").ToString)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
                    If Val_Email = True Then
                        oProcesos.BeginTransaction()
                        oProcesos.Actualizar_TicketCitado(item("Id_Reunion"))
                        oProcesos.CommitTransaction()
                    End If
                Next
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ReenviarCorreoAutorizacion(CodTicket As String, CodJefe As String, CodUsuario As String) As ActionResult
        Try
            Dim DtInfo As DataTable = oConsultas.Consultar_InformacionTicket(CodTicket)
            Dim UsuarioResponsable As String = oConsultas.Consultar_ResponsableSoporte(DtInfo.Rows(0)("TipoSoporte").ToString.Trim)
            'Mensaje que se le envia al jefe
            Dim link1 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "2", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)
            Dim link2 As String = Url.Action("Autorizar_Ticket", "Home", New With {.Estado = "3", .CodTicket = CodTicket, .Usuario = CodJefe, .Responsable = UsuarioResponsable}, Request.Url.Scheme)

            Dim Asunto As String = "Cajacopi - Autorización Ticket"
            Dim Cuerpo As String = My.Resources.Email_AutorizacionJefe.Replace("{NroTicket}", CodTicket) _
                .Replace("{Nombre}", NombreUsuario(CodJefe)) _
                .Replace("{CodUsuario}", NombreUsuario(CodUsuario)) _
                .Replace("{Necesidad}", DtInfo.Rows(0)("Necesidad").ToString.Trim.ToLower) _
                .Replace("{Justificacion}", DtInfo.Rows(0)("Justificacion").ToString.Trim.ToLower) _
                .Replace("{link1}", link1) _
                .Replace("{link2}", link2)
            Dim Val_Email As Boolean = CBool(oFunciones.EnviarCorreo(oConsultas.Consultar_UsuarioPqrs(CodJefe.ToString.Trim)(0)("Email").ToString.Trim, Asunto, Cuerpo, "").ToString)
            If Val_Email = False Then
                oProcesos.RollBackTransaction()
                Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error al enviar el comunicado interno§}]"), JsonRequestBehavior.AllowGet)
            End If
            Return Json(oFunciones.ComillasDoble("[{§Result§:§True§, §Msn§:§Correo Enviado Exitosamente§}]"), JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(oFunciones.ComillasDoble("[{§Result§:§False§, §Msn§:§Error de la aplicación§}]"), JsonRequestBehavior.AllowGet)
        End Try
    End Function
End Class
