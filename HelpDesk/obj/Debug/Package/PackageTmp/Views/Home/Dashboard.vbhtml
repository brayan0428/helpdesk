﻿@Code
    ViewData("Title") = "Dashboard"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
    Dim Total_Tickets As Integer = 0,
        Tickets_EnProceso As Integer = 0,
        Tickets_Solucionados As Integer = 0
    Dim CodUsuario As String = Session("UserCod").ToString.Trim
    Dim oHome As New HelpDesk.HomeController
    Dim TablaEstadistica As System.Data.DataTable = oHome.Consultar_ContadorTicketsUsuarios(CodUsuario)
    If (IsNothing(TablaEstadistica) = False) Then
        If TablaEstadistica.Rows.Count > 0 Then
            For Each item As System.Data.DataRow In TablaEstadistica.Rows
                Total_Tickets = item("Total_Tickets")
                Tickets_EnProceso = item("Tickets_EnProceso")
                Tickets_Solucionados = item("Tickets_Solucionados")
            Next
        End If
    End If
End Code
   <!-- Page wrapper  -->
<!-- ============================================================== -->
<div class="page-wrapper" ng-controller="Tickets_Dashboard">
    <!-- ============================================================== -->
    <!-- Container fluid  -->
    <!-- ============================================================== -->
    <div class="container-fluid">
        <div class="alert alert-success alert-rounded" style="margin-top:20px;display:none;" id="dNTicket">
            Su ticket ha sido ingresado exitosamente. El numero de ticket asignado es <span id="TicketAsignado"></span>
            <button type="button" class="close" onclick="window.location.reload();" aria-label="Close"> <span aria-hidden="true">×</span> </button>
        </div>
        <div class="row page-titles">
            <div class="col-md-5 col-8 align-self-center">
                <h3 class="text-themecolor m-b-0 m-t-0">Lista de Tickets</h3>
            </div>
        </div>
        <div class="card-group">
            <!-- Column -->
            <div class="card">
                <div class="card-block text-center">
                    <h4 class="text-center">Total Tickets</h4>
                    <div id="spark8">
                        <canvas width="99" height="70" style="display: inline-block; width: 99px; height: 70px; vertical-align: top;"></canvas>
                    </div>
                </div>
                <div class="box b-t text-center">
                    <h4 class="font-medium m-b-0"><i class="ti-angle-up text-success"></i> @(Total_Tickets)</h4>
                </div>
            </div>
            <!-- Column -->
            <!-- Column -->
            <div class="card">
                <div class="card-block text-center">
                    <h4 class="text-center">Tickets en Proceso</h4>
                    <div id="spark9">
                        <canvas width="99" height="70" style="display: inline-block; width: 99px; height: 70px; vertical-align: top;"></canvas>
                    </div>
                </div>
                <div class="box b-t text-center">
                    <h4 class="font-medium m-b-0"><i class="ti-angle-up text-success"></i> @(Tickets_EnProceso)</h4>
                </div>
            </div>
            <!-- Column -->
            <!-- Column -->
            <div class="card">
                <div class="card-block text-center">
                    <h4 class="text-center">Tickets Solucionados</h4>
                    <div id="spark10">
                        <canvas width="99" height="70" style="display: inline-block; width: 99px; height: 70px; vertical-align: top;"></canvas>
                    </div>
                </div>
                <div class="box b-t text-center">
                    <h4 class="font-medium m-b-0"><i class="ti-angle-up text-success"></i> @(Tickets_Solucionados)</h4>
                </div>
            </div>
            <!-- Column -->
        </div>
        <div class="nav-tabs">
            <ul class="nav nav-tabs tabcontent-border" role="tablist">
                <li class="nav-item"> <a class="nav-link active" data-toggle="tab" href="#enproceso" role="tab" aria-expanded="true"><span class="hidden-sm-up"><i class="ti-home"></i></span> <span class="hidden-xs-down">En Proceso</span> </a> </li>
                <li class="nav-item"> <a class="nav-link" data-toggle="tab" href="#finalizados" role="tab" aria-expanded="false"><span class="hidden-sm-up"><i class="ti-user"></i></span> <span class="hidden-xs-down">Finalizados</span></a> </li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content tabcontent-border">
                <div class="tab-pane active" id="enproceso" role="tabpanel" aria-expanded="true">
                    <div class="p-l-20">
                        <br />
                        <table class="table color-table info-table">
                            <thead>
                                <tr>
                                    <th># Ticket</th>
                                    <th>Tipo Solicitud</th>
                                    <th>Tipo Soporte</th>
                                    <th>Necesidad</th>
                                    <th>Responsable</th>
                                    <th>Usuario</th>
                                    <th>Estado</th>
                                    <th>Fecha Ingreso</th>
                                    <th>Calificación</th>
                                </tr>
                            </thead>
                            <tbody style="background-color:white">
                                <tr ng-repeat="dato in Tickets_Ingresados" ng-if="dato.IdEstado != '23'">
                                    <td>
                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                    </td>
                                    <td>{{dato.TipoSolicitud}}</td>
                                    <td>{{dato.TipoSoporte}}</td>
                                    <td style="text-align:justify;width:300px;">{{dato.Necesidad | limitTo : 150}}...</td>
                                    <td>{{dato.Responsable}}</td>
                                    <td>{{dato.UsuarioIngreso}}</td>
                                    <td>{{dato.Estado}}</td>
                                    <td>{{dato.FechaIngreso}}</td>
                                    <td ng-if="dato.IdEstado === '17'">
                                        <a href="http://sendesk.cajacopi.com/Home/TicketCalificacion?CodTicket={{dato.CodTicket}}" class="btn btn-info">Calificar</a>
                                    </td>
                                    <td ng-if="dato.IdEstado != '17'"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="tab-pane" id="finalizados" role="tabpanel" aria-expanded="true">
                    <div class="p-l-20">
                        <br />
                        <table class="table color-table info-table">
                            <thead>
                                <tr>
                                    <th># Ticket</th>
                                    <th>Tipo Solicitud</th>
                                    <th>Tipo Soporte</th>
                                    <th>Necesidad</th>
                                    <th>Responsable</th>
                                    <th>Usuario</th>
                                    <th>Estado</th>
                                    <th>Fecha Ingreso</th>
                                    <th>Calificación</th>
                                </tr>
                            </thead>
                            <tbody style="background-color:white">
                                <tr ng-repeat="dato in Tickets_Ingresados" ng-if="dato.IdEstado === '23'">
                                    <td>
                                        <a data-toggle="modal" href="#modal_long" data-ticket="{{dato.CodTicket}}" data-necesidad="{{dato.Necesidad}}" data-justificacion="{{dato.Justificacion}}" data-adjunto="{{dato.Adjunto}}" class="btn btn-info"
                                           onclick="EnviarDatos('Modal_VerTicket', 'Home', 'Ticket Nro ' + $(this).attr('data-ticket'), 'modal_long', { 'CodTicket': $(this).attr('data-ticket'), 'Necesidad': $(this).attr('data-necesidad'), 'Justificacion': $(this).attr('data-justificacion'), 'Adjunto': $(this).attr('data-adjunto') })">{{dato.CodTicket}}</a>
                                    </td>
                                    <td>{{dato.TipoSolicitud}}</td>
                                    <td>{{dato.TipoSoporte}}</td>
                                    <td style="text-align:justify;width:300px;">{{dato.Necesidad | limitTo : 150}}...</td>
                                    <td>{{dato.Responsable}}</td>
                                    <td>{{dato.UsuarioIngreso}}</td>
                                    <td>{{dato.Estado}}</td>
                                    <td>{{dato.FechaIngreso}}</td>
                                    <td ng-if="dato.IdEstado === '17'">
                                        <a href="http://sendesk.cajacopi.com/Home/TicketCalificacion?CodTicket={{dato.CodTicket}}" class="btn btn-info">Calificar</a>
                                    </td>
                                    <td ng-if="dato.IdEstado != '17'"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                </div>
            </div>
                <!-- Row -->
                <!-- ============================================================== -->
                <!-- End PAge Content -->
                <!-- ============================================================== -->
                <!-- ============================================================== -->
                <!-- Right sidebar -->
                <!-- ============================================================== -->
                <!-- .right-sidebar -->
                <div class="right-sidebar">
                    <div class="slimscrollright">
                        <div class="rpanel-title"> Service Panel <span><i class="ti-close right-side-toggle"></i></span> </div>
                        <div class="r-panel-body">
                            <ul id="themecolors" class="m-t-20">
                                <li><b>With Light sidebar</b></li>
                                <li><a href="javascript:void(0)" data-theme="default" class="default-theme">1</a></li>
                                <li><a href="javascript:void(0)" data-theme="green" class="green-theme">2</a></li>
                                <li><a href="javascript:void(0)" data-theme="red" class="red-theme">3</a></li>
                                <li><a href="javascript:void(0)" data-theme="blue" class="blue-theme working">4</a></li>
                                <li><a href="javascript:void(0)" data-theme="purple" class="purple-theme">5</a></li>
                                <li><a href="javascript:void(0)" data-theme="megna" class="megna-theme">6</a></li>
                                <li class="d-block m-t-30"><b>With Dark sidebar</b></li>
                                <li><a href="javascript:void(0)" data-theme="default-dark" class="default-dark-theme">7</a></li>
                                <li><a href="javascript:void(0)" data-theme="green-dark" class="green-dark-theme">8</a></li>
                                <li><a href="javascript:void(0)" data-theme="red-dark" class="red-dark-theme">9</a></li>
                                <li><a href="javascript:void(0)" data-theme="blue-dark" class="blue-dark-theme">10</a></li>
                                <li><a href="javascript:void(0)" data-theme="purple-dark" class="purple-dark-theme">11</a></li>
                                <li><a href="javascript:void(0)" data-theme="megna-dark" class="megna-dark-theme ">12</a></li>
                            </ul>
                            <ul class="m-t-20 chatonline">
                                <li><b>Chat option</b></li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/1.jpg" alt="user-img" class="img-circle"> <span>Varun Dhavan <small class="text-success">online</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/2.jpg" alt="user-img" class="img-circle"> <span>Genelia Deshmukh <small class="text-warning">Away</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/3.jpg" alt="user-img" class="img-circle"> <span>Ritesh Deshmukh <small class="text-danger">Busy</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/4.jpg" alt="user-img" class="img-circle"> <span>Arijit Sinh <small class="text-muted">Offline</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/5.jpg" alt="user-img" class="img-circle"> <span>Govinda Star <small class="text-success">online</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/6.jpg" alt="user-img" class="img-circle"> <span>John Abraham<small class="text-success">online</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/7.jpg" alt="user-img" class="img-circle"> <span>Hritik Roshan<small class="text-success">online</small></span></a>
                                </li>
                                <li>
                                    <a href="javascript:void(0)"><img src="~/assets/images/users/8.jpg" alt="user-img" class="img-circle"> <span>Pwandeep rajan <small class="text-success">online</small></span></a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
