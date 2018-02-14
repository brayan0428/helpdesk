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
        DtUsuarios As System.Data.DataTable = Nothing
    DtInfo = oHome.Consultar_InfoTicket(CodTicket)
    DtResponsables = oHome.Consulta_Responsables_Soporte(DtInfo.Rows(0)("TipoSoporte").ToString.Trim)
    DtUsuarios = oHome.Consultar_Usuarios
    ResponsableAct = DtInfo.Rows(0)("Responsable").ToString.Trim
    If CDate(DtInfo.Rows(0)("Fecha_Inicio").ToString.Trim) <> CDate("01/01/1900") Then
        Iniciado = True
    End If
End Code
<div class="container">
    <input type="hidden" id="xNroTicket" value="@(CodTicket)" />
    <!-- Nav tabs -->
    <ul class="nav nav-tabs" role="tablist">
        <li class="nav-item"> <a class="nav-link active" data-toggle="tab" href="#info" role="tab" aria-expanded="true"><span class="hidden-sm-up"><i class="ti-home"></i></span> <span class="hidden-xs-down">Información</span></a> </li>
        <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#seguimientos" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Seguimientos</span></a> </li>
        <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#cambios" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Historial de Cambio</span></a> </li>
        @If Iniciado = False And ResponsableAct = UsuSesion Then
            @<li Class="nav-item"> <a class="nav-link" data-toggle="tab" href="#reasignar" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Reasignar</span></a> </li>
        End If
    </ul>
    <!-- Tab panes -->
    <div Class="tab-content tabcontent-border">
        <div Class="tab-pane active" id="info" role="tabpanel" aria-expanded="true">
            <div Class="p-20">
                <div style="text-align:center;height:450px;display:none" id="dCargando">
                    <img src="/assets/images/cargando.gif">
                </div>
                <div id="dInformacion">
                    <div class="row">
                        <div class="col-md-12">
                            <label><b>Necesidad</b></label>
                            <div style="text-align:justify;padding-left:15px;padding-right:15px" id="xNecesidad">
                                @Necesidad
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <label><b>Justificación</b></label>
                            <div style="text-align:justify;padding-left:15px;padding-right:15px" id="xJustificacion">
                                @Justificacion
                            </div>
                        </div>
                    </div>
                    <br />
                    <div id="xAdjunto">
                        @If Adjunto <> String.Empty Then
                            @<a href="/Home/DescargarAdjunto?Url=@Adjunto" target="_blank" Class="btn btn-success waves-effect text-left" download>Descargar Adjunto</a>
                        End If
                    </div>
                    <br />
                    <div Class="row">
                        <div Class="col-md-12">
                            <div Class="form-group">
                                <Label> <b> Seguimiento</b></Label>
                                <textarea Class="form-control" rows="3" id="mSeguimiento"></textarea>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div>
                        <Label> Subir Archivo</Label>
                        <form id="FormArchivo_m" action="@Url.Action("Index", "Files")" method="post" enctype="multipart/form-data">
                            <input type="hidden" name="RutaArchivo" id="RutaArchivo_m" value="" />
                            <input style="margin-bottom:3px;" type="file" name="archivo" id="archivo" value="" Class="btn waves-effect waves-light btn-rounded btn-success" />
                            <Button type="submit" name="subir" Class="btn waves-effect waves-light btn-rounded btn-success">Subir Archivo</Button>
                            <div id="RespuestaFile_m" style="font-style: italic;font-size:10px;margin-top:3px;"></div>
                        </form>
                    </div>
                    @If ResponsableAct = UsuSesion Then
                    @<div Class="row">
                        <div Class="col-md-5">
                            <div Class="form-group">
                                <Label> <b> Usuarios Copia</b></Label>
                                <select Class="form-control" id="xUsuCopia">
                                    @For Each item As System.Data.DataRow In DtUsuarios.Rows
                                            @<option value="@(item("Email"))">@(item("Nombre"))</option>
                                    Next
                                </select>
                            </div>
                        </div>
                        <div Class="col-md-1">
                            <div Class="form-group">
                                <Label style = "visibility:hidden"><b> Usuarios</b></label>
                                <Button type = "button" Class="btn btn-warning waves-effect text-left" onclick="AgregarUsuario();">Agregar</Button>
                            </div>
                        </div>
                        <div Class="col-md-5">
                            <div Class="form-group">
                                <Label style = "visibility:hidden"><b> Usuarios</b></label>
                                <select multiple = "multiple" Class="form-control" id="UsuariosCopia" style="margin-left:50px;" ondblclick="EliminarUsuario();"></Select>
                            </div>
                        </div>
                    </div>
                    End If
                    
                    <br /><br />
                    <center> <Button type="button" Class="btn btn-info waves-effect text-left" onclick="GuardarSeguimiento();">Guardar</Button></center>
                </div>
            </div>
        </div>
        <div Class="tab-pane p-20" id="seguimientos" role="tabpanel" aria-expanded="false">
            <div Class="table-responsive">
                <Table Class="table color-table muted-table">
                    <thead>
                        <tr>
                            <th> Id</th>
                            <th> Descripción</th>
                            <th> Fecha</th>
                            <th> Hora</th>
                            <th> Usuario</th>
                        </tr>
                    </thead>
                    <tbody id="tTabla"></tbody>
                </Table>
            </div>
        </div>
        <div Class="tab-pane p-20" id="cambios" role="tabpanel" aria-expanded="false">
            <div Class="table-responsive">
                <Table Class="table color-table muted-table">
                    <thead>
                        <tr>
                            <th> Id</th>
                            <th> Etapa Anterior</th>
                            <th> Etapa Nueva</th>
                            <th> Fecha</th>
                            <th> Hora</th>
                            <th> Usuario</th>
                            <th> Observación</th>
                        </tr>
                    </thead>
                    <tbody id="lTabla"></tbody>
                </Table>
            </div>
        </div>
        <div Class="tab-pane p-20" id="reasignar" role="tabpanel" aria-expanded="false">
            <div style="text-align:center;height:450px;display:none" id="rCargando">
                <img src="/assets/images/cargando.gif">
            </div>
            <div Class="table-responsive" id="rInformacion">
                <div Class="col-md-6">
                    <div Class="form-group">
                        <Label> <b> Nuevo Recurso</b></Label>
                        <select Class="form-control" id="RResponsable">
                            @For Each item As System.Data.DataRow In DtResponsables.Rows
                            @<option value="@(item("CodUsuario").ToString.Trim)*@(item("Email").ToString.Trim) ">@(item("Responsable").ToString.Trim)</option>
                            Next
                        </select>
                    </div>
                </div>
                <div Class="col-md-12">
                    <div Class="form-group">
                        <Label> <b> Motivo </b></Label>
                        <textarea Class="form-control" rows="3" id="mObservacion"></textarea>
                    </div>
                </div>
                <center> <Button type="button" Class="btn btn-success waves-effect text-left" onclick="ReasignarRecurso();">Reasignar</Button></center>
            </div>
        </div>
    </div>
</div>
<script src="~/Scripts/jquery.form.js"></script>
<script>
    ConsultarSeguimientos();
    ConsultarLogs();

    function GuardarSeguimiento() {
        var CodTicket = $("#xNroTicket").val();
        var Observacion = $("#mSeguimiento").val();
        var UsuCopias = '';
        var Adjunto = $("#RutaArchivo_m").val();;
        if (Observacion.trim() == '') {
            NotificacionPopup('error', 'Error de Validación', 'Debe ingresar una observación');
            return;
        }
        var id = document.getElementById("UsuariosCopia");
        if (id != null) {
            for (i = 0; ele = id.options[i]; i++) {
                UsuCopias += ele.value + ',';
            }
        }
        
        $("#dCargando").css('display', '');
        $("#dInformacion").css('display', 'none');
        $.getJSON('@Url.Action("Guardar_Seguimiento", "Home")', {
            "CodTicket": CodTicket,
            "Observacion": Observacion,
            "UsuCopia": UsuCopias,
            "Adjunto":Adjunto
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                if (Data.Result == 'True') {
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                    $("#mSeguimiento").val('');
                    $("#UsuariosCopia").empty();
                    $('#RespuestaFile_m').html('');
                    $('#RutaArchivo_m').val('');
                    ConsultarSeguimientos();
                } else {
                    NotificacionPopup('error', 'Error de Validación', Data.Msn);
                }
            });
            $("#dCargando").css('display', 'none');
            $("#dInformacion").css('display', '');
        });
    }

    function ConsultarSeguimientos() {
        $("#tTabla").empty();
        var Html = '';
        $.getJSON('@Url.Action("Consultar_Seguimientos", "Home")', {
            "CodTicket": '@(CodTicket)'
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                Html += '<tr>';
                Html += '<td>' + Data.Id + '</td>';
                Html += '<td>' + Data.Descripcion;
                if (Data.Adjunto != '') {
                    Html += '. <a href="/Home/DescargarAdjunto?Url=' + Data.Adjunto + '" target="_blank">Ver Adjunto</a>';
                }
                Html += '</td>';
                Html += '<td>' + Data.Fecha + '</td>';
                Html += '<td>' + Data.Hora + '</td>';
                Html += '<td>' + Data.Usuario + '</td>';
                Html += '</tr>';
            });
            $("#tTabla").append(Html);
        });
    }

    function ConsultarLogs() {
        $("#lTabla").empty();
        var Html = '';
        $.getJSON('@Url.Action("Consultar_LogTickets", "Home")', {
            "CodTicket": '@(CodTicket)'
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                Html += '<tr>';
                Html += '<td>' + Data.Id + '</td>';
                Html += '<td>' + Data.Etapa_Anterior + '</td>';
                Html += '<td>' + Data.Etapa_Nueva + '</td>';
                Html += '<td>' + Data.Fecha + '</td>';
                Html += '<td>' + Data.Hora + '</td>';
                Html += '<td>' + Data.Usuario + '</td>';
                Html += '<td>' + Data.Observacion + '</td>';
                Html += '</tr>';
            });
            $("#lTabla").append(Html);
        });
    }

    function ReasignarRecurso() {
        var CodTicket = $("#xNroTicket").val();
        var Recurso = $("#RResponsable").val();
        var Observacion = $("#mObservacion").val();
        if (Observacion.trim() == '') {
            NotificacionPopup('error', 'Error de Validación', 'Debe ingresar una observación');
            return;
        }
        $("#rCargando").css('display', '');
        $("#rInformacion").css('display', 'none');
        $.getJSON('@Url.Action("Reasignar_Recurso", "Home")', {
            "CodTicket": CodTicket,
            "Recurso": Recurso,
            "Observacion": Observacion
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                if (Data.Result == 'True') {
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                    $("#mObservacion").val('');
                    ConsultarSeguimientos();
                    ConsultarLogs();
                } else {
                    NotificacionPopup('error', 'Error de Validación', Data.Msn);
                }
            });
            $("#rCargando").css('display', 'none');
            $("#rInformacion").css('display', '');
        });
    }

    function AgregarUsuario() {
        $('#xUsuCopia option:selected').appendTo("#UsuariosCopia");
    }

    function EliminarUsuario() {
        $('#UsuariosCopia option:selected').remove();
    }

    $('#FormArchivo_m').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: new FormData(this),
            cache: false,
            contentType: false,
            processData: false,
            success: function (res) {
                resp1 = res.split(",", 1);
                resp2 = res.substring(27, 1000);
                $('#RespuestaFile_m').html('<img src="../assets/images/pack/Valid.png" width="20px" height="20px" /> ' + resp1);
                $('#RutaArchivo_m').val(resp2)
            },
            error: function (ex) {
                alert(Json.stringify(ex.responseText))
                $('#RespuestaFile_m').html('<img src="../assets/images/pack/Error.png" width="20px" height="20px"/> ' + " Error al subir el archivo");
            }
        });
    });
</script>