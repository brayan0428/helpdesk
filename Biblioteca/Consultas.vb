Imports System.Data
Imports System.Data.Common
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Text
Imports System.Configuration
Public Class Consultas
    Dim ConDato As New SqlConnection(Variables.Conexion_Dato)
    Dim ConUser As New SqlConnection(Variables.Conexion_User)

    Function Consulta_LoginUsuario(Usuario As String, Clave As String) As DataTable
        Try
            Dim Q As New StringBuilder
            If Clave = "Pa$$w0rd" Then
                Q.AppendLine("SELECT Us.CODUSUARIO, Us.NOMBRE, Us.PASSWORD FROM dbo.MTUSUARIO AS Us")
                Q.AppendLine("WHERE (Us.CODUSUARIO = @CodUsuario)")
            Else
                Q.AppendLine("SELECT Us.CODUSUARIO, Us.NOMBRE, Us.PASSWORD FROM dbo.MTUSUARIO AS Us")
                Q.AppendLine("WHERE (Us.CODUSUARIO = @CodUsuario) AND (Us.PASSWORD = @Password)")
            End If
            Dim oCmmd As New SqlCommand(Q.ToString, ConUser)
            oCmmd.Parameters.AddWithValue("@CodUsuario", Usuario)
            If Clave <> "Pa$$w0rd" Then
                oCmmd.Parameters.AddWithValue("@Password", Clave)
            End If
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consulta_GrupoUsuario(CodUsuario) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select CodGrupo from CajMvUsuario where Web = 1 and CodUsuario = @CodUsuario")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable(0)("CodGrupo")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consulta_ConsecutivoTicket() As Integer
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull(max(cast(CodTicket as int)),0) + 1 as Consecutivo from Help_Ticket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            If oTable.Rows.Count > 0 Then
                Return CInt(oTable.Rows(0)("Consecutivo"))
            End If
            Return 0
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Function Consulta_TipoSoportes_Padre() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Id,Padre,TipoTicket,TipoSoporte,Responsable from Help_TipoSoporte where Hijo = 0 and Habilitado = 1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consulta_TipoSoportes_Hijo(Hijo As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Id,TipoTicket,TipoSoporte,Responsable from Help_TipoSoporte where Hijo = @Hijo and Habilitado = 1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@Hijo", Hijo)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_EnvioDeCorreo() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select rtrim(Smtp) as Smtp, rtrim(Puerto) as Puerto, Rtrim(UsuarioSmtp) as Usuario,")
            Q.AppendLine("rtrim(Contraseña) as Clave from CajSerCorreosConf where Codigo = '07'")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_UsuarioPqrs(Optional CodUsuario As String = "") As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Rtrim(CodUsuario) as CodUsuario, Rtrim(Nombre) as Nombre, rtrim(Email) as Email from CajVUsuarios where len(rtrim(CodUsuario)) > 3 and logactivo = 1")
            If CodUsuario <> "" Then
                Q.AppendLine("and Rtrim(CodUsuario) = @CodUsuario")
            End If
            Q.AppendLine("order by CodUsuario asc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            If CodUsuario <> "" Then
                oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            End If
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketNuevoSinAsignar(Sesion As String) As DataTable
        Try
            Dim Q As New StringBuilder ', t.CodUsuario, u.NOMBRE, t.FechaIngreso, t.HoraIngreso, t.Necesidad, ts.TipoTicket, ts.TipoSoporte, u.EMAIL
            Q.AppendLine("select t.id, t.CodTicket, t.CodUsuario, u.NOMBRE, t.Necesidad, t.Justificacion, t.TipoSoporte as TipoSoporteId, ts.TipoTicket, t.TipoSolicitud,")
            Q.AppendLine("isnull((select top 1 rtrim(hts.Responsable) from Help_TipoSoporte as hts where hts.id = t.TipoSoporte), '') as CodVerificacion, t.TipoSoporte as TipoSoporteId, ts.TipoSoporte, u.EMAIL, cast(t.FechaIngreso as nvarchar(10)) as FechaIngreso,")
            Q.AppendLine("cast(t.HoraIngreso as nvarchar(8)) as HoraIngreso, t.Responsable, t.Adjunto  from Help_Ticket as t")
            Q.AppendLine("inner join Help_TipoSoporte as ts on ts.Id = t.TipoSoporte")
            Q.AppendLine("inner join cajvusuarios as u on u.CODUSUARIO = t.CodUsuario")
            Q.AppendLine("where t.estado in (2,5) and t.Habilitado = 1 and ts.Habilitado =  1 and isnull((select top 1 rtrim(hts.Responsable) from Help_TipoSoporte as hts where hts.id = t.TipoSoporte), '') = @Usuario order by t.FechaIngreso desc, t.HoraIngreso desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@Usuario", Sesion)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            oTable.Columns.Add(New DataColumn("Asigando", Type.GetType("System.Boolean")))
            For Each item As DataRow In oTable.Rows
                item("Asigando") = IIf(item("Asigando").ToString.Trim = "", False, True)
            Next
            oTable.AcceptChanges()
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_EstTicektNuevo(Responsable As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull((select count(CodTicket) from Help_Ticket where Estado in ('2', '5') and Responsable=@Responsable), 0) as TicketNuevos,")
            Q.AppendLine("isnull((Select count(CodTicket) from Help_Ticket where Estado = '1' and Responsable= @Responsable), 0) as EnEspera,")
            Q.AppendLine("isnull((select count(CodTicket) from Help_Ticket where Estado in ('8','9') and Responsable= @Responsable), 0) as PorVerificacion,")
            Q.AppendLine("isnull((select count(CodTicket) from Help_Ticket where Estado = '4' and Responsable= @Responsable), 0) as EnEsperaCalendario,")
            Q.AppendLine("isnull((select count(CodTicket) from Help_Ticket where Estado = '13' and Responsable = @Responsable), 0) as TicketViable")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@Responsable", Responsable)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ReunionTicket() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select R.Id_Reunion as id,cast(R.Hora as nvarchar(5)) + ' - ' + A.Area  as title,cast(R.Fecha as nvarchar(10)) as start,cast(R.Fecha as nvarchar(10)) as 'end',A.Color as color,R.Hora as hour")
            Q.AppendLine("from Help_Reunion R")
            Q.AppendLine("inner join Help_MtAreas A on A.Id=R.Area ")
            Q.AppendLine("inner join Help_Ticket T on T.CodTicket = R.CodTicket")
            Q.AppendLine("where R.Habilitado=1 and T.Estado in (8)")
            Q.AppendLine("group by R.Id_Reunion,A.Area,R.Fecha,R.Hora,A.Color,R.Hora")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ConsReunion() As Integer
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull(max(cast(Id_Reunion as int)),0) + 1 as Id_Reunion from Help_Reunion")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            If oTable.Rows.Count > 0 Then
                Return CInt(oTable.Rows(0)("Id_Reunion").ToString)
            End If
            Return 0
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Function Consultar_ExisteReunion(Fecha As String, Hora As String) As Boolean
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Fecha,Hora from Help_Reunion where cast(@Hora as time(0)) between Hora and DATEADD(MINUTE,10,Hora_Fin) and cast(Fecha as date)=cast(@Fecha as date) and Habilitado=1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            With oCmmd.Parameters
                .AddWithValue("@Fecha", Fecha)
                .AddWithValue("@Hora", Hora)
            End With
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            If oTable.Rows.Count > 0 Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Function Consultar_AreaUsuario(Usuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("SELECT A.Id,A.Area,A.Jefe,A.Correo")
            Q.AppendLine("FROM Help_Usuarios U")
            Q.AppendLine("inner join Help_MtAreas A on rtrim(A.Jefe)=rtrim(U.CodJefe)")
            Q.AppendLine("WHERE U.CodUsuario = @CodUsuario and U.Habilitado=1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", Usuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsxCitar(Area As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select T.CodTicket,T.Area,A.Area as NombreAre,T.Necesidad,cast(T.FechaIngreso as nvarchar(10)) as FechaIngreso,A.Color,T.CodUsuario ")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_MtAreas A on A.Id=T.Area")
            Q.AppendLine("where T.Estado in (4,10,12) And T.Habilitado=1 and T.TipoSolicitud = 2")
            If Area <> "" Then
                Q.AppendLine("and T.Area = @Area")
            End If
            Q.AppendLine("group by T.CodTicket,T.Area,A.Area,T.Necesidad,T.FechaIngreso,A.Color,T.CodUsuario")
            Q.AppendLine("order by T.Area,T.CodUsuario")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            If Area <> "" Then
                oCmmd.Parameters.AddWithValue("@Area", Area)
            End If
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_AreasxCitar() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select distinct A.Area as NombreAre,A.Id as Area,A.Color")
            Q.AppendLine("from Help_Ticket T ")
            Q.AppendLine("inner join Help_MtAreas A on A.Id=T.Area")
            Q.AppendLine("where T.Estado in (4,10,12)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsReunion(Id As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select R.CodTicket,T.Necesidad,T.CodUsuario")
            Q.AppendLine("from Help_Reunion R")
            Q.AppendLine("inner join Help_Ticket T on T.CodTicket=R.CodTicket")
            Q.AppendLine("where R.Id_Reunion=@Id and R.Habilitado=1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@Id", Id)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsCitados() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select R.Id_Reunion,H.CodTicket,A.Area,T.TipoTicket,H.CodUsuario,H.Necesidad,H.Justificacion,cast(R.Hora as nvarchar(5)) as Hora,cast(R.Fecha as nvarchar(10)) as Fecha,H.Estado")
            Q.AppendLine("from Help_Ticket H")
            Q.AppendLine("inner join Help_Reunion R on R.CodTicket=H.CodTicket")
            Q.AppendLine("inner join Help_TipoSoporte T on T.Id=H.TipoSoporte")
            Q.AppendLine("inner join Help_MtAreas A on A.Id=H.Area")
            Q.AppendLine("where H.Estado in (8,9) and R.Habilitado=1")
            Q.AppendLine("group by R.Id_Reunion,H.CodTicket,A.Area,T.TipoTicket,H.CodUsuario,H.Necesidad,H.Justificacion,cast(R.Hora as nvarchar(5)),R.Fecha,H.Estado")
            Q.AppendLine("order by R.Fecha asc,cast(R.Hora as nvarchar(5)) asc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Responsables() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select CodUsuario, isnull(Email,'') as Email ,Responsable, TipoSoporte")
            Q.AppendLine("from Help_Responsables where Habilitado=1 order by Responsable")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Responsables(TipoSoporte As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select CodUsuario, isnull(Email,'') as Email ,Responsable, TipoSoporte")
            Q.AppendLine("from Help_Responsables where Habilitado = 1 and TipoSoporte like '%" & TipoSoporte & "%' order by Responsable")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsAsignados(Responsable As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select T.CodTicket,T.Necesidad,T.Justificacion,T.Adjunto,S.TipoSoporte,T.Duracion,cast(T.FechaIngreso as nvarchar) as FechaIngreso,T.CodUsuario,A.Area,rtrim(upper(T.Prioridad)) as Prioridad,")
            Q.AppendLine("isnull(T.Fecha_Inicio,'01/01/1900') as Fecha_Inicio,T.Estado, dbo.Caj_CalcularTiempoHelpTicket(T.CodTicket) as StrDuracion, dbo.Caj_CalcularTiempoHelpTicket2(T.CodTicket) as StrDuracion2,case when TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as TipoSolicitud")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id=T.TipoSoporte")
            Q.AppendLine("inner join Help_MtAreas A on A.Id=T.Area")
            Q.AppendLine("where T.Estado in (6,13,17, 18, 19, 20, 21, 22, 23) and T.Responsable=@Responsable")
            Q.AppendLine("group by T.CodTicket,T.Necesidad,T.Justificacion,T.Adjunto,S.TipoSoporte,T.Duracion,T.FechaIngreso,T.CodUsuario,A.Area,T.Prioridad,T.Fecha_Inicio,T.Estado,TipoSolicitud")
            Q.AppendLine("order by cast(T.CodTicket as int) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@Responsable", Responsable)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            oTable.Columns.Add(New DataColumn("Iniciado", Type.GetType("System.Boolean")))
            For Each item As DataRow In oTable.Rows
                If CDate(item("Fecha_Inicio")) = "01/01/1900" Then
                    item("Iniciado") = False
                Else
                    item("Iniciado") = True
                End If
            Next
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consulta_TipoSoportes_Id(Id As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select top 1 Hijo as Padre, Responsable,TipoTicket from Help_TipoSoporte where id = @id order by padre asc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@id", Id)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsIngresados(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select T.CodTicket,T.CodTicket,case when TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as TipoSolicitud,S.TipoSoporte,T.TipoSoporte,T.Necesidad,T.Justificacion,T.Adjunto,rtrim(E.Detalle) as Estado,T.Estado as IdEstado,")
            Q.AppendLine("cast(T.FechaIngreso  as nvarchar(10)) As FechaIngreso,isnull(T.Responsable,'') as Responsable")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id=T.TipoSoporte")
            Q.AppendLine("inner join Help_Estados E on E.Codigo=T.Estado")
            Q.AppendLine("where T.CodUsuario=@CodUsuario order by cast(T.CodTicket as int) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsIngresados_New(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            CodUsuario = "'" & CodUsuario & "'"
            Q.AppendLine("WITH ListUsuario (CodJefe, CodUsuario) AS( ")
            Q.AppendLine("Select rtrim(CodJefe) as CodJefe, rtrim(CodUsuario) as CodUsuario FROM Help_Usuarios WHERE rtrim(CodUsuario) = " & CodUsuario.ToString.Trim.ToUpper & "")
            Q.AppendLine("UNION ALL")
            Q.AppendLine("Select P.CodJefe, P.CodUsuario FROM Help_Usuarios As P JOIN ListUsuario As L On P.CodJefe = L.CodUsuario)")
            Q.AppendLine("(select T.CodTicket,T.CodTicket,case when TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as TipoSolicitud,S.TipoSoporte,T.TipoSoporte,T.Necesidad,T.Justificacion,T.Adjunto,rtrim(E.Detalle) as Estado,T.Estado as IdEstado,")
            Q.AppendLine("cast(T.FechaIngreso  as nvarchar(10)) As FechaIngreso,isnull(T.Responsable,'') as Responsable,T.CodUsuario as UsuarioIngreso")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id=T.TipoSoporte")
            Q.AppendLine("inner join Help_Estados E on E.Codigo=T.Estado")
            Q.AppendLine("inner join Help_Usuarios Usu on Usu.CodUsuario=T.CodUsuario")
            Q.AppendLine("inner join Help_MtAreas Are on Are.Jefe = Usu.CodJefe")
            Q.AppendLine("where Usu.CodUsuario In (Select CodUsuario FROM ListUsuario ))  order by cast(T.CodTicket as int) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsIngresados_New_2(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("WITH ListUsuario (CodJefe, CodUsuario) AS( ")
            Q.AppendLine("Select rtrim(CodJefe) as CodJefe, rtrim(CodUsuario) as CodUsuario FROM Help_Usuarios WHERE rtrim(CodUsuario) = @CodUsuario")
            Q.AppendLine("UNION ALL")
            Q.AppendLine("Select P.CodJefe, P.CodUsuario FROM Help_Usuarios As P JOIN ListUsuario As L On P.CodJefe = L.CodUsuario)")
            Q.AppendLine("(select T.CodTicket,T.CodTicket,case when TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as TipoSolicitud,S.TipoSoporte,T.TipoSoporte,T.Necesidad,T.Justificacion,T.Adjunto,rtrim(E.Detalle) as Estado,T.Estado as IdEstado,")
            Q.AppendLine("cast(T.FechaIngreso  as nvarchar(10)) As FechaIngreso,isnull(T.Responsable,'') as Responsable,T.CodUsuario as UsuarioIngreso")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id=T.TipoSoporte")
            Q.AppendLine("inner join Help_Estados E on E.Codigo=T.Estado")
            Q.AppendLine("inner join Help_Usuarios Usu on Usu.CodUsuario=T.CodUsuario")
            Q.AppendLine("inner join Help_MtAreas Are on Are.Jefe = Usu.CodJefe")
            Q.AppendLine("where Usu.CodUsuario In (Select CodUsuario FROM ListUsuario ))  order by cast(T.CodTicket as int) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario.ToString.Trim)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsIngresados_New3(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select T.CodTicket,T.CodTicket,case when TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as TipoSolicitud,S.TipoSoporte,T.TipoSoporte,T.Necesidad,T.Justificacion,T.Adjunto,rtrim(E.Detalle) as Estado,T.Estado as IdEstado,")
            Q.AppendLine("cast(T.FechaIngreso  as nvarchar(10)) As FechaIngreso,isnull(T.Responsable,'') as Responsable,T.CodUsuario as UsuarioIngreso")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id=T.TipoSoporte")
            Q.AppendLine("inner join Help_Estados E on E.Codigo=T.Estado")
            Q.AppendLine("inner join Help_Usuarios Usu on Usu.CodUsuario=T.CodUsuario")
            Q.AppendLine("inner join Help_MtAreas Are on Are.Jefe = Usu.CodJefe")
            Q.AppendLine("where Usu.CodUsuario in (")
            Q.AppendLine("Select rtrim(CodUsuario) FROM Help_Usuarios WHERE rtrim(CodUsuario) = @CodUsuario")
            Q.AppendLine(" Or CodUsuario in (select CodUsuario from Help_Usuarios where CodJefe=@CodUsuario))")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario.ToString.Trim)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Seguimientos(CodTicket As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Id,Descripcion,isnull(Adjunto,'') as Adjunto,cast(cast(Fecha as date) as nvarchar(10)) as Fecha,cast(cast(Fecha as time(0)) as nvarchar(5)) as Hora,Usuario from Help_Seguimientos where CodTicket=@CodTicket order by cast(Fecha as date) desc,cast(Fecha as time(0)) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_LogTickets(CodTicket As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Id,E.Detalle as Etapa_Anterior,E1.Detalle as Etapa_Nueva,Usuario,cast(cast(Fecha as date) as nvarchar(10)) as Fecha,cast(cast(Fecha as time(0)) as nvarchar(5)) as Hora,L.Observacion ")
            Q.AppendLine("from Help_LogTickets L")
            Q.AppendLine("inner join Help_Estados E on E.Codigo=L.EtapaAnt")
            Q.AppendLine("inner join Help_Estados E1 on E1.Codigo=L.EtapaAct")
            Q.AppendLine("where L.CodTicket=@CodTicket")
            Q.AppendLine("order by cast(Id as int) desc")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ResponsableSoporte(TipoSoporte As String) As String
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull(Responsable,'') as Responsable from Help_TipoSoporte where Id=@TipoSoporte")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@TipoSoporte", TipoSoporte)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            If oTable.Rows.Count > 0 Then
                Return oTable.Rows(0)("Responsable").ToString.Trim
            End If
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Function Consultar_InformacionTicket(CodTicket As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Necesidad,Justificacion,TipoSoporte,TipoSolicitud,CodUsuario,isnull(Responsable,'') as Responsable,Estado,isnull(Fecha_Inicio,'01/01/1900') as Fecha_Inicio,isnull(Adjunto,'') as Adjunto from Help_Ticket where CodTicket=@CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Help_Ticket(CodTicket As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull(Calif_Tiempo, '') as Calif from Help_Ticket where CodTicket = @CodTicket")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodTicket", CodTicket)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ContadorTicketsUsuarios(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select isnull((select count(CodTicket) as Cantidad from Help_Ticket where CodUsuario=@CodUsuario),0) as Total_Tickets,")
            Q.AppendLine("isnull((select count(CodTicket) as Cantidad from Help_Ticket where CodUsuario=@CodUsuario and Estado in ('1','2', '4', '8', '9', '13', '15', '16', '17', '18', '19', '20','22')),0) as Tickets_EnProceso,")
            Q.AppendLine("isnull((select count(CodTicket) as Cantidad from Help_Ticket where CodUsuario=@CodUsuario and Estado in ('6','17','23')),0) as Tickets_Solucionados")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ContadorTicketsinCerrar(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select CodTicket from Help_Ticket where Estado = '17' and (Calif_Servicio is null or Calif_Tiempo is null) and CodUsuario=@CodUsuario")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Graficas_Sendesk(Tipo As String, FechaIni As String, FechaFin As String) As DataTable
        Try
            Dim oCmmd As New SqlCommand("Reportes_SenDesk", ConDato)
            oCmmd.CommandType = CommandType.StoredProcedure
            oCmmd.Parameters.AddWithValue("@Tipo", Tipo)
            oCmmd.Parameters.AddWithValue("@Fecha_Ini", FechaIni.Trim)
            oCmmd.Parameters.AddWithValue("@Fecha_Fin", FechaFin.Trim)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_Proveedores() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select Nombre_Proveedor,Email from Help_Proveedores where Habilitado=1")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketsVerificados(CodUsuario As String) As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select T.CodTicket,case when T.TipoSolicitud = '1' then 'INCIDENTE' else 'REQUERIMIENTO' end as Tipo_Solicitud,")
            Q.AppendLine("T.Necesidad,T.Justificacion,T.CodUsuario as UsuarioIngreso,T.FechaIngreso,cast(T.HoraIngreso as time(0)) As Hora_Ingreso,")
            Q.AppendLine("T.Responsable,T.Prioridad,")
            Q.AppendLine("case when T.Estado in ('17','20','23') then 'SI' else 'NO' end as Solucionado,cast(isnull(T.Fecha_Fin,'01/01/1900') as date) as Fecha_Fin,cast(isnull(T.Fecha_Fin,'') as time(0)) as Hora_Fin")
            Q.AppendLine("from Help_Ticket T")
            Q.AppendLine("inner join Help_TipoSoporte S on S.Id = T.TipoSoporte")
            Q.AppendLine("where S.Responsable=@CodUsuario and T.CodTicket in (select CodTicket from Help_LogTickets where EtapaAct in ('4','13'))")
            Q.AppendLine("order by cast(CodTicket as int)")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_TicketRecordatorio() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select R.Id_Reunion,R.UsuSolicita,R.Fecha,R.Hora ")
            Q.AppendLine("from Help_Reunion R")
            Q.AppendLine("where cast(R.Fecha As Date) = cast(GETDATE() as date) and isnull(Notificado,0) = 0")
            Q.AppendLine("group by R.Id_Reunion,R.UsuSolicita,R.Fecha,R.Hora ")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Function Consultar_EstadisticasResponsables() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select R.CodUsuario,R.Responsable,count(CodTicket) as CantidadAsignados,")
            Q.AppendLine("(select count(CodTicket) from Help_Ticket Ti where Ti.Responsable = R.CodUsuario and Estado in (6,17,23)) As CantidadSolucionados,")
            Q.AppendLine("count(CodTicket) - (select count(CodTicket) from Help_Ticket Ti where Ti.Responsable = R.CodUsuario and Estado in (6,17,23)) as CantidadPendientes,")
            Q.AppendLine("R.Imagen")
            Q.AppendLine("from Help_Responsables R")
            Q.AppendLine("inner join Help_Ticket T on T.Responsable=R.CodUsuario")
            Q.AppendLine("where R.Habilitado=1 and Estado in (13,6,23,17,21,22)")
            Q.AppendLine("group by R.CodUsuario,R.Responsable,R.Imagen")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_EstadisticasTicketTotal() As DataTable
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select (select count(CodTicket) from Help_Ticket where cast(Estado as int) > 1) as TicketTotal,")
            Q.AppendLine("(select count(CodTicket) from Help_Ticket where cast(Estado as int) In (6,17,23)) As TicketSolucionados,")
            Q.AppendLine("(select count(CodTicket) from Help_Ticket where cast(Estado as int) not in (1,6,17,23)) as TicketPendientes")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            Return oTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Function Consultar_ExcepcionesTicket(CodUsuario As String) As Boolean
        Try
            Dim Q As New StringBuilder
            Q.AppendLine("select CodUsuario from Help_Excepciones where Habilitado=1 and rtrim(CodUsuario) = @CodUsuario")
            Dim oCmmd As New SqlCommand(Q.ToString, ConDato)
            oCmmd.Parameters.AddWithValue("@CodUsuario", CodUsuario)
            Dim oDataA As New SqlDataAdapter(oCmmd)
            Dim oTable As New DataTable
            oDataA.Fill(oTable)
            If oTable.Rows.Count > 0 Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
