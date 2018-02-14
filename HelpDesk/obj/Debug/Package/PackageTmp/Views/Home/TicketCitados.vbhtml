@Code
    ViewData("Title") = "TicketCitados"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
End Code
<div Class="page-wrapper" ng-controller="TicketsCitados">
    <div class="container-fluid">
        <div class="row page-titles">
            <div class="col-md-5 col-8 align-self-center">
                <h3 class="text-themecolor m-b-0 m-t-0">Tickets Citados para Reunion de Viabilidad</h3>
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="Index">Home</a></li>
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
                            <table class="table color-table success-table">
                                <thead>
                                    <tr>
                                        <th># Ticket</th>
                                        <th>Area</th>
                                        <th>Tipo Soporte</th>
                                        <th>Necesidad</th>
                                        <th>Usuario</th>
                                        <th>Fecha</th>
                                        <th>Hora</th>
                                        <th>Acepto</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="dato in Tickets_Citados">
                                        <td>
                                            <button type="button" alt="default" data-toggle="modal" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="AsignarValTicket(dato.Id_Reunion,dato.CodTicket,dato.Necesidad,dato.Justificacion)">{{dato.CodTicket}}</button>
                                        </td>
                                        <td>{{dato.Area}}</td>
                                        <td>{{dato.TipoTicket}}</td>
                                        <td>{{dato.Necesidad | limitTo : 150}}...</td>
                                        <td>{{dato.CodUsuario}}</td>
                                        <td>{{dato.Fecha}}</td>
                                        <td>{{dato.Hora}}</td>
                                        <td ng-if="dato.Estado=='8'">NO</td>
                                        <td ng-if="dato.Estado=='9'">SI</td>
                                    </tr>
                                </tbody>
                            </table>
                            <a id="VerTicket" data-toggle="modal" data-target=".bs-example-modal-lg" style="visibility:hidden;">VerTicket</a>
                            <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myLargeModalLabel">Ticket #<span id="xNroTicket"></span></h4>
                                            <button id="CerrarInfoTicket" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        </div>
                                        <div class="modal-body">
                                            <div ng-show="CargandoTicket" style="text-align:center;height:450px;" class="ng-hide">
                                                <img src="/assets/images/cargando.gif">
                                            </div>
                                            <div ng-show="!CargandoTicket">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label><b>Necesidad</b></label>
                                                        <div style="text-align:justify;padding-left:15px;padding-right:15px" id="xNecesidad">
                                                            Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas "Letraset", las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label><b>Justificación</b></label>
                                                        <div style="text-align:justify;padding-left:15px;padding-right:15px" id="xJustificacion">
                                                            Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas "Letraset", las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label><b>Viabilidad</b></label>
                                                            <select class="custom-select col-12" id="Viabilidad">
                                                                <option value="1">Si</option>
                                                                <option value="0">No</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label><b>Recurso</b></label>
                                                            <select class="custom-select col-12" id="Recurso">
                                                                <option ng-repeat="res in Responsables" value="{{res.CodUsuario}}*{{res.Email}}">{{res.Responsable}}</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label><b>Prioridad</b></label>
                                                            <select class="custom-select col-12" id="Prioridad">
                                                                <option value="URGENTE">URGENTE</option>
                                                                <option value="ALTA">ALTA</option>
                                                                <option value="MEDIA">MEDIA</option>
                                                                <option value="BAJA">BAJA</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label><b>Tiempo de Solución</b></label>
                                                            <input type="number" class="form-control form-control-line" value="" id="TiempoSolucion">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label><b>Observación</b></label>
                                                            <textarea class="form-control" rows="3" id="mObservacion" name="mObservacion"></textarea>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div>
                                        <div class="modal-footer">
                                            <input type="hidden" id="TicketSelecc" name="TicketSelecc" value="">
                                            <button type="button" class="btn btn-success waves-effect text-left" ng-click="ActualizarEvaluacion();">Guardar</button>
                                            <button type="button" class="btn btn-danger waves-effect text-left" data-dismiss="modal">Cerrar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Modal Confirmación-->
    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="mySmallModalLabel">Confirmar Asistencia</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="xCloseM">×</button>
                </div>
                <div class="modal-body">
                    <select class="custom-select col-12" id="ConfAsistencia" onchange="showObservation();">
                        <option selected="" value="1">Si</option>
                        <option value="0">No</option>
                    </select>
                    <br /><br />
                    <div class="form-group" id="xObservacion" style="display:none">
                        <label style="color:black">Observación</label>
                        <textarea class="form-control" rows="5" id="xObservacionT"></textarea>
                    </div>
                    <div Class="form-group" style="text-align:center;margin-top:10px;">
                        <button type="button" class="btn waves-effect waves-light btn-rounded btn-info" ng-click="ValidarAsistencia()">Aceptar</button>
                    </div>
                    <input type="hidden" id="mc_CodTicket" />
                    <input type="hidden" id="mc_IdReunion"/>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
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