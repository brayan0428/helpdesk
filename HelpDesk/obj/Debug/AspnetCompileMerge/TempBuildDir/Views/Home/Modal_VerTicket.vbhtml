@Code
    Dim CodTicket As String = ViewBag.CodTicket,
Necesidad As String = ViewBag.Necesidad,
Justificacion As String = ViewBag.Justificacion,
Adjunto As String = ViewBag.Adjunto
End Code
<div class="container">
    <input type="hidden" id="xNroTicket" value="@(CodTicket)"/>
    <!-- Nav tabs -->
    <ul class="nav nav-tabs" role="tablist">
        <li class="nav-item"> <a class="nav-link active" data-toggle="tab" href="#info" role="tab" aria-expanded="true"><span class="hidden-sm-up"><i class="ti-home"></i></span> <span class="hidden-xs-down">Información</span></a> </li>
        <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#seguimientos" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Seguimientos</span></a> </li>
        <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#cambios" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Historial de Cambio</span></a> </li>
    </ul>
    <!-- Tab panes -->
    <div class="tab-content tabcontent-border">
        <div class="tab-pane active" id="info" role="tabpanel" aria-expanded="true">
            <div class="p-20">
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
                    <center> <Button type="button" Class="btn btn-success waves-effect text-left" onclick="GuardarSeguimiento();">Guardar</Button></center>
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
                    <tbody id="tTabla">
                        
                    </tbody>
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
                        </tr>
                    </thead>
                    <tbody id="lTabla">

                    </tbody>
                </Table>
            </div>
        </div>
    </div>
</div>
<script>
    ConsultarSeguimientos();
    ConsultarLogs();

    function GuardarSeguimiento() {
        var CodTicket = $("#xNroTicket").val();
        var Observacion = $("#mSeguimiento").val();
        if (Observacion.trim() == '') {
            NotificacionPopup('error', 'Error de Validación', 'Debe ingresar una observación');
            return;
        }
        $("#dCargando").css('display', '');
        $("#dInformacion").css('display', 'none');
        $.getJSON('@Url.Action("Guardar_Seguimiento", "Home")', {
            "CodTicket": CodTicket,
            "Observacion": Observacion
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                if (Data.Result == 'True') {
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                    $("#mSeguimiento").val('');
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
                Html += '<td>' + Data.Descripcion + '</td>';
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
                Html += '</tr>';
            });
            $("#lTabla").append(Html);
        });
    }
</script>