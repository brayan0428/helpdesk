@Code
    Dim CodTicket As String = ViewBag.CodTicket,
    Necesidad As String = ViewBag.Necesidad,
    Justificacion As String = ViewBag.Justificacion,
    Adjunto As String = ViewBag.Adjunto
    Dim DtInfo As System.Data.DataTable = Nothing,
        oHome As New HelpDesk.HomeController,
        Iniciado As Boolean = False,
        DtResponsables As System.Data.DataTable = Nothing,
        ResponsableAct As String = String.Empty,
        UsuSesion As String = Session("UserCod").ToString.Trim,
        DtUsuarios As System.Data.DataTable = Nothing,
        DtTipoTicket As System.Data.DataTable = Nothing,
        DtProveedores As System.Data.DataTable = Nothing
    DtInfo = oHome.Consultar_InfoTicket(CodTicket)
    DtResponsables = oHome.Consulta_Responsables_Soporte(DtInfo.Rows(0)("TipoSoporte").ToString.Trim)
    DtUsuarios = oHome.Consultar_Usuarios
    DtProveedores = oHome.Consultar_Proveedores
    DtTipoTicket = oHome.Consultar_TiposTicketPadre
    ResponsableAct = DtInfo.Rows(0)("Responsable").ToString.Trim
    If CDate(DtInfo.Rows(0)("Fecha_Inicio").ToString.Trim) <> CDate("01/01/1900") Then
        Iniciado = True
    End If
End Code
<div style="width:100%;height:100%;margin-top:20px;display:none;" id="xCargando"><img src="../assets/images/cargando.gif" height="200px" style="display:block;margin-left:auto;margin-right:auto;border:none;" /></div>
<div class="container" id="xContenido">
    <input type="hidden" id="xNroTicket" value="@(CodTicket)" />
    <!-- Nav tabs -->
    <div Class="">
        <div style="text-align:center;height:450px;display:none" id="dCargando">
            <img src="/assets/images/cargando.gif">
        </div>
        <div id="dInformacion">
            <div class="row">
                <div class="col-md-12">
                    <label>Necesidad</label>
                    <textarea Class="form-control" rows="3" id="Necesidad" readonly style="color:black;resize:vertical">@(Necesidad)</textarea>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <label>Justificación</label>
                    <textarea Class="form-control" rows="3" id="Necesidad" readonly style="color:black;resize:vertical">@(Justificacion)</textarea>
                </div>
            </div>
            <br />
            <div id="xAdjunto">
                @If Adjunto <> String.Empty Then
                    @<a href="/Home/DescargarAdjunto?Url=@Adjunto" target="_blank" Class="btn btn-success waves-effect text-left" download>Descargar Adjunto</a>
                End If
            </div>
            <div class="row">
                <div class="col-md-6">
                    <label>Tipo Solicitud</label>
                    <select class="form-control" id="TipoSolicitud_N">
                        <option value="">SELECCIONE</option>
                        <option value="1">INCIDENTE</option>
                        <option value="2">REQUERIMIENTO</option>
                    </select>
                </div>
                <div class="col-md-6">
                    <label>Tipo Ticket</label>
                    <select class="form-control" id="TipoTicket_N" onchange="ConsultarTipoSoporte();">
                        
                    </select>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <label>Tipo Soporte</label>
                    <select class="form-control" id="TipoSoporte_N">
                       
                    </select>
                </div>
                <div class="col-md-6">
                    <label>Prioridad</label>
                    <select class="form-control" id="Prioridad_N">
                        <option value="URGENTE"> URGENTE </option>
                        <option value="ALTA"> ALTA </option>
                        <option value="MEDIA"> MEDIA </option>
                        <option value="BAJA"> BAJA </option>
                    </select>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <label>Recurso</label>
                    <select class="form-control" id="Recurso_N">

                    </select>
                </div>
                <div class="col-md-6">
                    <label>Observación</label>
                    <textarea Class="form-control" rows="3" id="Observacion_N"></textarea>
                </div>
            </div>
            <div Class="row">
                <div Class="col-md-5">
                    <div Class="form-group">
                        <Label> <b>Enviar Copia Proveedores</b></Label>
                        <select Class="form-control" id="xUsuCopia">
                            @For Each item As System.Data.DataRow In DtProveedores.Rows
                                @<option value="@(item("Email"))">@(item("Nombre_Proveedor"))</option>
                            Next
                        </select>
                    </div>
                </div>
                <div Class="col-md-1">
                    <div Class="form-group">
                        <Label style="visibility:hidden"><b> Usuarios</b></Label>
                        <Button type="button" Class="btn btn-warning waves-effect text-left" onclick="AgregarUsuario();">Agregar</Button>
                    </div>
                </div>
                <div Class="col-md-5">
                    <div Class="form-group">
                        <Label style="visibility:hidden"><b> Usuarios</b></Label>
                        <select multiple="multiple" Class="form-control" id="UsuariosCopia" style="margin-left:50px;" ondblclick="EliminarUsuario();"></select>
                    </div>
                </div>
            </div>

            <br />
            <div class="row">
                <div class="col-md-12">
                    <Button id="btnSolicitarSolucion" type="button" Class="btn btn-success waves-effect text-left" onclick="GuardarDatos(6);">Actualizar Ticket</Button>
                    <Button id="btnSolicitarSolucion" type="button" Class="btn btn-success waves-effect text-left" onclick="GuardarDatos(1);">Verificar Ticket</Button>
                    @If DtInfo.Rows(0)("TipoSolicitud").ToString.Trim = "2" Then
                        @<Button id = "btnSolicitarSolucion" type="button" Class="btn btn-warning waves-effect text-left" onclick="GuardarDatos(7);">Requerimiento Especial</Button>
                    End If
                    <Button id = "btnSolicitarSolucion" type="button" Class="btn btn-info waves-effect text-left" onclick="GuardarDatos(3);">Solucionado</Button>
                    <Button id = "btnSolicitarSolucion" type="button" Class="btn btn-success waves-effect text-left" onclick="GuardarDatos(4);">Reasignar</Button>
                    <Button id = "btnSolicitarSolucion" type="button" Class="btn btn-danger waves-effect text-left" onclick="GuardarDatos(5);">Anular</Button>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
    setTimeout(function () {
        ConsultarSoporte();
    }, 1000);

    function ConsultarSoporte() {
        var IdPa = '';
        $.post('@Url.Action("Consulta_TipoSoportes_Padre", "Home")', {
        }, function (resultado) {
            var Html = '';
            $.each(JSON.parse(resultado), function (index, Data){
                //console.log(resultado);
                Html += '<option value="' + Data.Padre + '">' + Data.TipoTicket + '</option>';
            });
            $("#TipoTicket_N").html(Html);
            $('#TipoSolicitud_N > option[value="' + '@(DtInfo.Rows(0)("TipoSolicitud"))' + '"]').attr('selected', 'selected');
        });

        $.post('@Url.Action("Consulta_TipoSoportes_Id", "Home")', {
            'idTipoSoporte': '@(DtInfo.Rows(0)("TipoSoporte"))'
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                IdPa = Data.Padre;
            });
            $('#TipoTicket_N > option[value="' + IdPa + '"]').attr('selected', 'selected');

            //Consulto los TipoSoporte
            $.post('@Url.Action("Consultar_TiposSoporte", "Home")', {
                "TipoTicket": IdPa
            }, function (resultado) {
                var Html = '';
                $.each(JSON.parse(resultado), function (index, Data) {
                    //console.log(resultado);
                    Html += '<option value="' + Data.Id + '">' + Data.TipoSoporte + '</option>';
                });
                $("#TipoSoporte_N").html(Html);
                $('#TipoSoporte_N > option[value="' + '@(DtInfo.Rows(0)("TipoSoporte"))' + '"]').attr('selected', 'selected');
                ResponsableTickets();
            });
        });
    }

    function ResponsableTickets() {
        var SelTipoTicket = $("#TipoTicket_N option:selected").text();
        $("#Recurso_N").empty();
        $.getJSON('Consulta_ResponsableTickets2', {
            'TipoSoporte': SelTipoTicket
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (id, value) {
                $("#Recurso_N").append('<option value="' + value.CodUsuario + '">' + value.Responsable + '</option>');
            });
        });
    };

    function ConsultarTipoSoporte() {
        var TipoTicket = $("#TipoTicket_N").val();
        $("#TipoSoporte_N").empty();
        $.getJSON('Consultar_TiposSoporte', {
            'TipoTicket': TipoTicket
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                $("#TipoSoporte_N").append('<option value="' + Data.Id + '">' + Data.TipoSoporte + '</option>');
            });
            ResponsableTickets();
        });
    }

    function GuardarDatos(Accion) {
        var CodTicket = '@(CodTicket)';
        var TipoSolicitud = $("#TipoSolicitud_N").val();
        var TipoSoporte = $("#TipoSoporte_N").val();
        var Prioridad = $("#Prioridad_N").val();
        var Observacion = $("#Observacion_N").val();
        var Recurso = $("#Recurso_N").val();
        var UsuCopias = '';
        if (Accion == 4 || Accion == 5) {
            if (Observacion.trim() == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe ingresar una observación');
                return;
            }
        }
        MostrarCargando(true);
        var id = document.getElementById("UsuariosCopia");
        if (id != null) {
            for (i = 0; ele = id.options[i]; i++) {
                UsuCopias += ele.value + ',';
            }
        }

        $.post('VerificacionDeTicket_New', {
            'CodTicket': CodTicket,
            'Accion': Accion,
            'TipoSolicitud':TipoSolicitud,
            'TipoSoporte': TipoSoporte,
            'Prioridad':Prioridad,
            'Observacion' : Observacion,
            'Recurso':Recurso,
            'UsuCopia': UsuCopias
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                if (Data.Result == 'True') {
                    NotificacionPopup('success', 'Confirmación', 'Guardado Exitosamente');
                    setTimeout(function () { window.location.reload(); }, 3000);
                } else {
                    NotificacionPopup('error', 'Error de Validación', Data.Msn);
                }
                MostrarCargando(false);
            });
        });
    }

    function AgregarUsuario() {
        $('#xUsuCopia option:selected').appendTo("#UsuariosCopia");
    }

    function EliminarUsuario() {
        $('#UsuariosCopia option:selected').remove();
    }

    function MostrarCargando(Op) {
        if (Op == true) {
            $("#xCargando").css("display", "");
            $("#xContenido").css("display", "none");
        } else {
            $("#xCargando").css("display", "none");
            $("#xContenido").css("display", "");
        }
    }
</script>