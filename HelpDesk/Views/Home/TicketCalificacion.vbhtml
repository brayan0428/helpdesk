@Code
    ViewData("Title") = "TicketCitados"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
    Dim CodTicket As String = ViewBag.CodTicket
End Code
<div Class="page-wrapper" ng-controller="Tickets_Calificacion">
    <div class="container-fluid">
        <div class="row page-titles">
            <div class="col-md-5 col-8 align-self-center">
                <h3 class="text-themecolor m-b-0 m-t-0">Support Ticket</h3>
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="javascript:void(0)">Home</a></li>
                    <li class="breadcrumb-item active">Ticket</li>
                </ol>
            </div>
            <!--<div class="col-md-7 col-4 align-self-center">
                <div class="d-flex m-t-10 justify-content-end">
                    <div class="d-flex m-r-20 m-l-10 hidden-md-down">
                        <div class="chart-text m-r-10">
                            <h6 class="m-b-0"><small>THIS MONTH</small></h6>
                            <h4 class="m-t-0 text-info">$58,356</h4>
                        </div>
                        <div class="spark-chart">
                            <div id="monthchart"><canvas width="60" height="35" style="display: inline-block; width: 60px; height: 35px; vertical-align: top;"></canvas></div>
                        </div>
                    </div>
                    <div class="d-flex m-r-20 m-l-10 hidden-md-down">
                        <div class="chart-text m-r-10">
                            <h6 class="m-b-0"><small>LAST MONTH</small></h6>
                            <h4 class="m-t-0 text-primary">$48,356</h4>
                        </div>
                        <div class="spark-chart">
                            <div id="lastmonthchart"><canvas width="60" height="35" style="display: inline-block; width: 60px; height: 35px; vertical-align: top;"></canvas></div>
                        </div>
                    </div>
                    <div class="">
                        <button class="right-side-toggle waves-effect waves-light btn-success btn btn-circle btn-sm pull-right m-l-10"><i class="ti-settings text-white"></i></button>
                    </div>
                </div>
            </div>-->
        </div>
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-block">
                        <h4 class="card-title">Lista de tickets citados</h4>
                        <h6 class="card-subtitle">Tickets Citados</h6>
                        <div class="table-responsive">
                            <table class="table color-table success-table" width="100%">
                                <thead>
                                    <tr>
                                        <th style="text-align:center;">
                                            Tiempo de Atención
                                        </th>
                                        <th style="text-align:center;">
                                            Satisfacción del Servicio
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Considera que el tiempo de respuesta a su solicitud fue</td>
                                        <td>Considera que la solución presentada por TIC en referencia a su solicitud fue</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <select class="form-control" id="TRespuesta">
                                                <option value="5">Excelente</option>
                                                <option value="4">Muy Bueno</option>
                                                <option value="3">Bueno</option>
                                                <option value="2">Regular</option>
                                                <option value="1">Malo</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select class="form-control" id="TAtencion">
                                                <option value="5">Excelente</option>
                                                <option value="4">Muy Buena</option>
                                                <option value="3">Bueno</option>
                                                <option value="2">Regular</option>
                                                <option value="1">Mala</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <h2>Observacion</h2><br /><textarea id="Observacion" name="Observacion" style="width:100%" rows="6" placeholder="Escriba aqui su observacion."></textarea></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align:center">
                                            <div Class="form-group" style="text-align:center">
                                                <input type="hidden" value="@(CodTicket)" id="CodTicket" name="CodTicket" />
                                                <Button type="button" Class="btn waves-effect waves-light btn-rounded btn-success" ng-click="GuardarCalificacion();"> Guardar </Button>
                                                <Button type="button" Class="btn waves-effect waves-light btn-rounded btn-success" ng-click="CancelarCalificacion();"> No estoy satisfecho, Verificar </Button>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
    function showObservation() {
        var Resp = $("#ConfAsistencia").val();
        if (Resp == '0') {
            $("#xObservacion").show('slow');
        } else {
            $("#xObservacion").hide('slow');
        }
    }
</script>