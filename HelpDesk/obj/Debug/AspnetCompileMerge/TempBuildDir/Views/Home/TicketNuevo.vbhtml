@Code
    ViewData("Title") = "TicketNuevo"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
    Dim oHome As New HomeController,
        TicketNuevos As Integer = 0,
        EnEspera As Integer = 0,
        PorVerificacion As Integer = 0,
        EnEsperaCalendario As Integer = 0
    Dim TablaEstadistica As System.Data.DataTable = oHome.Consultar_EstTicektNuevo()
    Dim DtResponsables As System.Data.DataTable = oHome.Consulta_ResponsableTicketsDt()
    If (IsNothing(TablaEstadistica) = False) Then
        If TablaEstadistica.Rows.Count > 0 Then
            For Each item As System.Data.DataRow In TablaEstadistica.Rows
                TicketNuevos = item("TicketNuevos")
                EnEspera = item("EnEspera")
                PorVerificacion = item("PorVerificacion")
                EnEsperaCalendario = item("EnEsperaCalendario")
            Next
        End If
    End If
End Code
<script>
    sessionStorage.setItem('SessionUsuario', '@(Session("UserCod"))');
    function ConsultarRecurso(){
        var TipoSolicitud2 = $("#TipoSolicitud2").val();
        if(TipoSolicitud2 == 1){
            document.getElementById("IdResponsable").style.display = 'block';
            document.getElementById("NivelResponsable").style.display = 'block';
        } else {
            document.getElementById("IdResponsable").style.display = 'none';
            document.getElementById("NivelResponsable").style.display = 'none';
        }
    }
    function RechazarTicked(Tipo) {
        $("#TipoAccion").val(Tipo);
        if (Tipo == 1) {
            //Verificado
            $('#TextoRechazo').html('');
            $('#TextoCancelacion').html('');
            $('#NombreBtnAceptar').html('Ticket Verificado');
            document.getElementById("InfoTicket_1").style.display = 'block';
            document.getElementById("InfoTicket_2").style.display = 'none';
            document.getElementById("InfoTicket_3").style.display = 'none';
            //
            document.getElementById("btnSolicitarReasignacion").style.display = 'block';
            document.getElementById("btnSolicitarCancelacion").style.display = 'block';
            document.getElementById("btnSolicitarSolucion").style.display = 'block';
        } else {
            if (Tipo == 2) {
                //Rechazado
                $('#TextoCancelacion').html('');
                $('#NombreBtnAceptar').html('Confirmar Rechazo');
                document.getElementById("InfoTicket_1").style.display = 'none';
                document.getElementById("InfoTicket_2").style.display = 'block';
                document.getElementById("InfoTicket_3").style.display = 'none';
                //
                document.getElementById("btnSolicitarReasignacion").style.display = 'none';
                document.getElementById("btnSolicitarCancelacion").style.display = 'none';
                document.getElementById("btnSolicitarSolucion").style.display = 'none';
            } else {
                if (Tipo == 3) {
                    //Solucionado

                } else {
                    if (Tipo == 4) {
                        //Cancelado
                        $('#TextoRechazo').html('');
                        $('#NombreBtnAceptar').html('Confirmar Cancelacion');
                        document.getElementById("InfoTicket_1").style.display = 'none';
                        document.getElementById("InfoTicket_2").style.display = 'none';
                        document.getElementById("InfoTicket_3").style.display = 'block';
                        //
                        document.getElementById("btnSolicitarReasignacion").style.display = 'none';
                        document.getElementById("btnSolicitarCancelacion").style.display = 'none';
                        document.getElementById("btnSolicitarSolucion").style.display = 'none';
                    }                    
                }                
            }            
        }
    }
</script>
<div Class="page-wrapper" ng-controller="Consultar_TicketsNuevos">
    <div Class="container-fluid">
        <div Class="row page-titles">
            <div Class="col-md-5 col-8 align-self-center">
                <h3 Class="text-themecolor m-b-0 m-t-0">Support Ticket</h3>
                <ol Class="breadcrumb">
                    <li Class="breadcrumb-item"><a href="javascript:void(0)">Home</a></li>
                    <li Class="breadcrumb-item active">Ticket</li>
                </ol>
            </div>
            <div Class="col-md-7 col-4 align-self-center">
                <div Class="d-flex m-t-10 justify-content-end">
                    <div Class="d-flex m-r-20 m-l-10 hidden-md-down">
                        <div Class="chart-text m-r-10">
                            <h6 Class="m-b-0"><small>THIS MONTH</small></h6>
                            <h4 Class="m-t-0 text-info">$58,356</h4>
                        </div>
                        <div Class="spark-chart">
                            <div id="monthchart"></div>
                        </div>
                    </div>
                    <div Class="d-flex m-r-20 m-l-10 hidden-md-down">
                        <div Class="chart-text m-r-10">
                            <h6 Class="m-b-0"><small>LAST MONTH</small></h6>
                            <h4 Class="m-t-0 text-primary">$48,356</h4>
                        </div>
                        <div Class="spark-chart">
                            <div id="lastmonthchart"></div>
                        </div>
                    </div>
                    <div Class="">
                        <Button Class="right-side-toggle waves-effect waves-light btn-success btn btn-circle btn-sm pull-right m-l-10"><i class="ti-settings text-white"></i></Button>
                    </div>
                </div>
            </div>
        </div>
        <div Class="row">
            <div Class="col-12">
                <div Class="card">
                    <div Class="card-block">
                        <h4 Class="card-title">Lista de tickets de soporte</h4>
                        <h6 Class="card-subtitle">Lista de entradas de tickets</h6>
                        <div Class="row m-t-40">
                            <!-- Column -->
                            <div Class="col-md-6 col-lg-3 col-xlg-3">
                                <div Class="card card-inverse card-info">
                                    <div Class="box bg-info text-center">
                                        <h1 Class="font-light text-white">@(TicketNuevos)</h1>
                                        <h6 class="text-white">Total Tickets</h6>
                                    </div>
                                </div>
                            </div>
                            <!-- Column -->
                            <div class="col-md-6 col-lg-3 col-xlg-3">
                                <div class="card card-primary card-inverse">
                                    <div class="box text-center">
                                        <h1 class="font-light text-white">@(EnEspera)</h1>
                                        <h6 class="text-white">Espera de Aprobacion</h6>
                                    </div>
                                </div>
                            </div>
                            <!-- Column -->
                            <div class="col-md-6 col-lg-3 col-xlg-3">
                                <div class="card card-inverse card-success">
                                    <div class="box text-center">
                                        <h1 class="font-light text-white">@(PorVerificacion)</h1>
                                        <h6 class="text-white">Por Verificacion</h6>
                                    </div>
                                </div>
                            </div>
                            <!-- Column -->
                            <div class="col-md-6 col-lg-3 col-xlg-3">
                                <div class="card card-inverse card-dark">
                                    <div class="box text-center">
                                        <h1 class="font-light text-white">@(EnEsperaCalendario)</h1>
                                        <h6 class="text-white">Pendiente</h6>
                                    </div>
                                </div>
                            </div>
                            <!-- Column -->
                        </div>
                        <div class="table-responsive">
                            <table class="tablesaw table-striped table-hover table-bordered table tablesaw-columntoggle" data-tablesaw-mode="columntoggle" id="table-2933"><!--ng-controller="TicketNuevo"-->
                                <thead>
                                    <tr>
                                        <th>Ticket</th>
                                        <th>Tipo Soporte</th>
                                        <th>Usuario</th>
                                        <th width="150px;">Fecha Ing</th>
                                        <th width="150px;">Hora Ing</th>
                                        <th>Necesidad</th>
                                        <th>Asigando</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="Fila in DataTable"> <!-- CodVerificacion -->
                                        <td><a class="btn btn-success" style="color:#ffffff;" onclick="RechazarTicked(1);" ng-click="asignarticket(Fila.CodTicket, Fila.Necesidad, Fila.Justificacion, Fila.TipoSoporteId, Fila.TipoSolicitud, Fila.Adjunto, Fila.Responsable, 1);"><b>{{Fila.CodTicket}}</b></a></td>
                                        <td>{{Fila.TipoTicket}}</td>
                                        <td title="{{Fila.NOMBRE}}"><center><a href="javascript:void(0)"><img src="~/assets/images/users/1.jpg" style="width:32px;height:32px;" alt="{{Fila.CodUsuario}}" class="img-circle"> {{Fila.CodUsuario}}</a></center></td>
                                        <td>{{Fila.FechaIngreso}}</td>
                                        <td>{{Fila.HoraIngreso}}</td>
                                        <td style="text-align:justify;">{{Fila.Necesidad | limitTo : 200}}...</td>
                                        <td>
                                            <span ng-show="Fila.Asigando" class="label label-success">Si</span>
                                            <span ng-show="!Fila.Asigando" class="label label-warning">No</span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <a id="VerTicket" data-toggle="modal" data-target=".bs-example-modal-lg" style="visibility:hidden;">VerTicket</a>
                            <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myLargeModalLabel">Ticket #<span id="NroTicket"></span></h4>
                                            <button id="CerrarInfoTicket" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        </div>
                                        <div class="modal-body">
                                            <div ng-show="CargandoTicket" style="text-align:center;height:450px;">
                                                <img src="~/assets/images/cargando.gif" />
                                            </div>
                                            <div id="InfoTicket_1" ng-show="!CargandoTicket">
                                                <table class="tablesaw table-striped table-hover table-bordered table tablesaw-columntoggle" data-tablesaw-mode="columntoggle">
                                                    <tr>
                                                        <td><h4>Necesidad</h4></td>
                                                        <td><p style="text-align:justify;font-size:14px;" id="NecesidadTicket" name="NecesidadTicket"></p></td>
                                                    </tr>
                                                    <tr>
                                                        <td><h4>Justificacion</h4></td>
                                                        <td><p style="text-align:justify;font-size:14px;" id="JustificacionTicket" name="JustificacionTicket"></p></td>
                                                    </tr>
                                                    <tr>
                                                        <td><h4>Adjunto </h4></td>
                                                        <td><p style="text-align:justify;font-size:14px;" id="DescAdjunto" name="DescAdjunto"></p></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td><h4>Tipo Ticket <a class="mytooltip" href="javascript:void(0)" data-toggle="modal" data-target="#mDefinicion"><i class="fa fa-fw fa-question"></i></a></h4></td>
                                                                    <td><h4>Tipo Soporte</h4></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <select class="form-control" id="TipoSolicitud2" onchange="ConsultarRecurso();">
                                                                            <option value="">SELECCIONE</option>
                                                                            <option value="1">INCIDENTE</option>
                                                                            <option value="2">REQUERIMIENTO</option>
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                <select class="form-control" id="IdResponsable">
                                                                                    @For Each item As System.Data.DataRow In DtResponsables.Rows
                                                                                        @<option value="@item("CodUsuario").ToString.Trim">@item("Responsable").ToString.Trim</option>
                                                                                    Next
                                                                                </select>
                                                                                </td>
                                                                                <td>
                                                                                <select class="form-control" id="NivelResponsable">
                                                                                    <option value="URGENTE"> URGENTE </option>
                                                                                    <option value="ALTA"> ALTA </option>
                                                                                    <option value="MEDIA"> MEDIA </option>
                                                                                    <option value="BAJA"> BAJA </option>
                                                                                </select></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <select id="SelTipoTicket" Class="form-control" onchange="ConsultarTipoSoporte();"> </Select>
                                                                    </td>
                                                                    <td>
                                                                        <select id = "SelTipoTicket_2" Class="form-control"></Select>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id = "InfoTicket_2" ng-show="!CargandoTicket">
                                                <h4>Motivos por lo cual rechaza el ticket</h4>
                                                <textarea Class="form-control" style="width:100%" rows="6" id="TextoRechazo"></textarea>
                                            </div>
                                            <div id = "InfoTicket_3" ng-show="!CargandoTicket">
                                                <h4> Motivos por lo cual cancela el ticket</h4>
                                                <textarea Class="form-control" style="width:100%" rows="6" id="TextoCancelacion"></textarea>
                                            </div>      
                                            <input id = "TipoAccion" name="TipoAccion" type="hidden" value="" />                                
                                        </div>
                                        <div Class="modal-footer">
                                            <center>
                                                <Table width = "100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                                                        <td style = "padding-left:5px;padding-right:5px;" >
                                                            <input type="hidden" id="TicketSelecc" name="TicketSelecc" value=""/>
                                                                                        <Button type = "button" Class="btn btn-success waves-effect text-left" ng-click="ActualizarEtapa();"><div id="NombreBtnAceptar">Ticket Verificado</div></button>
                                                        </td>
                                                        <td style = "padding-left:5px;padding-right:5px;"><Button id="btnSolicitarSolucion" type="button" Class="btn btn-success waves-effect text-left" ng-click="ActualizarEtapa2();">Marcar Como Solucionado</button></td>
                                                        <td style = "padding-left:5px;padding-right:5px;"><Button id="btnSolicitarReasignacion" type="button" Class="btn btn-danger waves-effect text-left" onclick="RechazarTicked(2);">Solicitar Reasignacion</button></td>
                                                        <td style = "padding-left:5px;padding-right:5px;"><button id="btnSolicitarCancelacion" type="button" Class="btn btn-danger waves-effect text-left" onclick="RechazarTicked(4);">Cancelar Ticket</button></td>
                                                    </tr>
                                                </table>
                                                <!--<button type="button" class="btn btn-danger waves-effect text-left" data-dismiss="modal">Close</button>-->
                                            </center>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div Class="right-sidebar">
            <div Class="slimscrollright">
                <div Class="rpanel-title"> Service Panel <span><i class="ti-close right-side-toggle"></i></span> </div>
                <div Class="r-panel-body">
                    <ul id = "themecolors" Class="m-t-20">
                        <li> <b>With Light sidebar</b></li>
                        <li> <a href = "javascript:void(0)" data-theme="default" Class="default-theme">1</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="green" Class="green-theme">2</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="red" Class="red-theme">3</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="blue" Class="blue-theme working">4</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="purple" Class="purple-theme">5</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="megna" Class="megna-theme">6</a></li>
                        <li Class="d-block m-t-30"><b>With Dark sidebar</b></li>
                        <li> <a href = "javascript:void(0)" data-theme="default-dark" Class="default-dark-theme">7</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="green-dark" Class="green-dark-theme">8</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="red-dark" Class="red-dark-theme">9</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="blue-dark" Class="blue-dark-theme">10</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="purple-dark" Class="purple-dark-theme">11</a></li>
                        <li> <a href = "javascript:void(0)" data-theme="megna-dark" Class="megna-dark-theme ">12</a></li>
                    </ul>
                    <ul Class="m-t-20 chatonline">
                        <li> <b> Chat Option</b></li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/1.jpg" alt="user-img" Class="img-circle"> <span>Varun Dhavan <small Class="text-success">online</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/2.jpg" alt="user-img" Class="img-circle"> <span>Genelia Deshmukh <small Class="text-warning">Away</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/3.jpg" alt="user-img" Class="img-circle"> <span>Ritesh Deshmukh <small Class="text-danger">Busy</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/4.jpg" alt="user-img" Class="img-circle"> <span>Arijit Sinh <small Class="text-muted">Offline</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/5.jpg" alt="user-img" Class="img-circle"> <span>Govinda Star <small Class="text-success">online</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/6.jpg" alt="user-img" Class="img-circle"> <span>John Abraham<small Class="text-success">online</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/7.jpg" alt="user-img" Class="img-circle"> <span>Hritik Roshan<small Class="text-success">online</small></span></a>
                        </li>
                        <li>
                                                                                                                <a href = "javascript:void(0)" <> img src="../assets/images/users/8.jpg" alt="user-img" Class="img-circle"> <span>Pwandeep rajan <small Class="text-success">online</small></span></a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <footer Class="footer"> © 2017 Material Pro Admin by wrappixel.com </footer>
</div>
<script>
    function ConsultarTipoSoporte() {
        var TipoTicket = $("#SelTipoTicket").val();
        $("#SelTipoTicket_2").empty();
        $.getJSON('Consultar_TiposSoporte', {
            'TipoTicket': TipoTicket
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (index, Data) {
                $("#SelTipoTicket_2").append('<option value="' + Data.Id + '">' + Data.TipoSoporte + '</option>');
            });
            ResponsableTickets2();
        });
    }

    function ResponsableTickets2(Responsable) {
        var SelTipoTicket = $("#SelTipoTicket option:selected").text();
        $("#IdResponsable").empty();
        $.getJSON('Consulta_ResponsableTickets2', {
            'TipoSoporte': SelTipoTicket
        }, function (resultado) {
            $.each(JSON.parse(resultado), function (id, value) {
                if (Responsable == value.CodUsuario) {
                    $("#IdResponsable").append('<option value="' + value.CodUsuario + '" selected>' + value.Responsable + '</option>');
                }else{
                    $("#IdResponsable").append('<option value="' + value.CodUsuario + '">' + value.Responsable + '</option>');
                }                
            });
        });
    };
</script>