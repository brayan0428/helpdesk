@Code
    ViewData("Title") = "TicketAsignados"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
End Code
<div Class="page-wrapper" ng-controller="TicketsAsignados">
    <div style="width:100%;height:150%; background-color:#808080; text-align:center; position:fixed; z-index:100; opacity:.3;" ng-show="Cargando"><img src="../assets/images/cargando.gif" height="200px" style="display:block;margin-left:auto;margin-right:auto;border:none;" /></div>
    <div class="container-fluid">
        <div class="row page-titles">
            <div class="col-md-5 col-8 align-self-center">
                <h3 class="text-themecolor m-b-0 m-t-0">Ticket Asignados</h3>
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
            <div class="col-md-12">
                <div class="card">
                    <div class="card-block">
                        <h4 class="card-title">Tickets Asignados</h4>
                        <h6 class="card-subtitle">Prioridad</h6>
                        <br />
                        <!-- Nav tabs -->
                        <div class="nav-tabs">
                            <ul class="nav nav-tabs tabcontent-border" role="tablist">
                                <li class="nav-item"> <a class="nav-link active" data-toggle="tab" href="#urgente" role="tab" aria-expanded="true"><span class="hidden-sm-up"><i class="ti-home"></i></span> <span class="hidden-xs-down">Urgentes <span class="label label-info">{{CantUrgente}}</span></span> </a> </li>
                                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#alta" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Alta <span class="label label-info">{{CantAlta}}</span></span></a> </li>
                                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#media" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-email"></i></span> <span class="hidden-xs-down">Media <span class="label label-info">{{CantMedia}}</span></span></a> </li>
                                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#baja" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-email"></i></span> <span class="hidden-xs-down">Baja <span class="label label-info">{{CantBaja}}</span></span></a> </li>
                                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#pendiente" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-email"></i></span> <span class="hidden-xs-down">Pendientes <span class="label label-info">{{CantPendientes}}</span></span></a> </li>
                                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#finalizados" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-email"></i></span> <span class="hidden-xs-down">Finalizados <span class="label label-info">{{CantSolucionados}}</span></span></a> </li>
                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content tabcontent-border">
                                <div class="tab-pane active" id="urgente" role="tabpanel" aria-expanded="true">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th>Tiempo Transc</th>
                                                    <th>Tiempo Proc</th>
                                                    <th>Estado</th>
                                                    <th>Opcion</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados | filter: { Prioridad: 'URGENTE'}" ng-if="dato.Estado === '13' || dato.Estado==='21'">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>{{dato.StrDuracion}}</td>
                                                    <td>{{dato.StrDuracion2}}</td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Inicio');" ng-show="!dato.Iniciado">Iniciar</button>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-danger model_img" ng-show="dato.Iniciado" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Fin');Actualizar_TicketAsignado(dato.CodTicket,17);">Finalizar</button>
                                                    </td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-warning model_img" ng-click="Actualizar_TicketAsignado(dato.CodTicket,22);" ng-show="dato.Iniciado">Pendiente</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane" id="alta" role="tabpanel" aria-expanded="false">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th>Tiempo Transc</th>
                                                    <th>Tiempo Proc</th>
                                                    <th>Estado</th>
                                                    <th>Opcion</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados | filter: { Prioridad: 'ALTA'}" ng-if="dato.Estado === '13' || dato.Estado === '21'">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>{{dato.StrDuracion}}</td>
                                                    <td>{{dato.StrDuracion2}}</td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Inicio');" ng-show="!dato.Iniciado">Iniciar</button>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-danger model_img" ng-show="dato.Iniciado" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Fin');Actualizar_TicketAsignado(dato.CodTicket,17);">Finalizar</button>
                                                    </td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-warning model_img" ng-click="Actualizar_TicketAsignado(dato.CodTicket,22);" ng-show="dato.Iniciado">Pendiente</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane" id="media" role="tabpanel" aria-expanded="false">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th>Tiempo Transc</th>
                                                    <th>Tiempo Proc</th>
                                                    <th>Estado</th>
                                                    <th>Opcion</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados | filter: { Prioridad: 'MEDIA' }" ng-if="dato.Estado === '13' || dato.Estado==='21'">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>{{dato.StrDuracion}}</td>
                                                    <td>{{dato.StrDuracion2}}</td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Inicio');" ng-show="!dato.Iniciado">Iniciar</button>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-danger model_img" ng-show="dato.Iniciado" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Fin');Actualizar_TicketAsignado(dato.CodTicket,17);">Finalizar</button>
                                                    </td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-warning model_img" ng-click="Actualizar_TicketAsignado(dato.CodTicket,22);" ng-show="dato.Iniciado">Pendiente</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane" id="baja" role="tabpanel" aria-expanded="false">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th>Tiempo Transc</th>
                                                    <th>Tiempo Proc</th>
                                                    <th>Estado</th>
                                                    <th>Opcion</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados | filter: { Prioridad: 'BAJA'}" ng-if="dato.Estado === '13' || dato.Estado==='21'">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>{{dato.StrDuracion}}</td>
                                                    <td>{{dato.StrDuracion2}}</td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Inicio');" ng-show="!dato.Iniciado">Iniciar</button>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-danger model_img" ng-show="dato.Iniciado" ng-click="IniciarTicket(dato.CodTicket,'Fecha_Fin');Actualizar_TicketAsignado(dato.CodTicket,17);">Finalizar</button>
                                                    </td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-warning model_img" ng-click="Actualizar_TicketAsignado(dato.CodTicket,22);" ng-show="dato.Iniciado">Pendiente</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane" id="pendiente" role="tabpanel" aria-expanded="false">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados | filter: { Estado : '22' }">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>
                                                        <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-success model_img" ng-click="Actualizar_TicketAsignado(dato.CodTicket,13);">Reanudar</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane" id="finalizados" role="tabpanel" aria-expanded="false">
                                    <div class="p-l-20">
                                        <br />
                                        <table class="table color-table info-table">
                                            <thead>
                                                <tr>
                                                    <th># Ticket</th>
                                                    <th>Area</th>
                                                    <th>Tipo Solicitud</th>
                                                    <th>Tipo Soporte</th>
                                                    <th>Usuario Ing</th>
                                                    <th>Duración (Horas)</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="dato in Tickets_Asignados" 
                                                    ng-if="dato.Estado === '6' || dato.Estado === '17' || dato.Estado === '18' || dato.Estado === '19' || dato.Estado === '20' || dato.Estado === '23'">
                                                    <td>
                                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                                    </td>
                                                    <td>{{dato.Area}} </td>
                                                    <td>{{dato.TipoSolicitud}}</td>
                                                    <td>{{dato.TipoSoporte}}</td>
                                                    <td>{{dato.CodUsuario}}</td>
                                                    <td>{{dato.Duracion}}</td>
                                                    <td>{{dato.FechaIngreso}}</td>
                                                    <td>
                                                        <div ng-if="dato.Estado === '23'">
                                                            <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-info model_img" disabled="disabled"> Calificado </button>
                                                        </div>
                                                        <div ng-if="dato.Estado === '17' || dato.Estado === '18' || dato.Estado === '19' || dato.Estado === '20' || dato.Estado === '6' ">
                                                            <button type="button" data-target=".bs-example-modal-sm" class="btn waves-effect waves-light btn-info model_img" disabled="disabled"> Solucionado </button>
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
        </div>
    </div>
</div>
