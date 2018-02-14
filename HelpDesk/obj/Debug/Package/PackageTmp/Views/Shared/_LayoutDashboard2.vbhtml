<!DOCTYPE html>
<html lang="en">
<!-- Mirrored from wrappixel.com/demos/admin-templates/material-pro/horizontal/index4.html by HTTrack Website Copier/3.x [XR&CO'2014], Wed, 05 Jul 2017 22:07:03 GMT -->
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" type="image/png" sizes="16x16" href="~/assets/images/favicon.png">
    <title>Sendesk - Cajacopi</title>
    <link href="~/assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="~/assets/plugins/chartist-js/dist/chartist.min.css" rel="stylesheet">
    <link href="~/assets/plugins/chartist-js/dist/chartist-init.css" rel="stylesheet">
    <link href="~/assets/plugins/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.css" rel="stylesheet">
    <link href="~/assets/plugins/css-chart/css-chart.css" rel="stylesheet">
    <link rel="stylesheet" href="~/assets/plugins/dropify/dist/css/dropify.min.css">
    <link href="~/assets/plugins/vectormap/jquery-jvectormap-2.0.2.css" rel="stylesheet" />
    <link href="~/assets/plugins/calendar/dist/fullcalendar.css" rel="stylesheet" />
    <link href="~/Content/css/style.css" rel="stylesheet">
    <link href="~/Content/css/colors/blue.css" id="theme" rel="stylesheet">
    <link href="~/Content/css/toastr.min.css" rel="stylesheet" />
    <script src="~/assets/plugins/jquery/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.5.6/angular.min.js"></script>
    <!--<script src="~/AngularJS/angular.js"></script>-->
    <script src="~/AngularJS/angular-route.js"></script>
    <script src="~/AngularJS/HomeController.js"></script>
    <link href="~/assets/plugins/tablesaw-master/dist/tablesaw.css" rel="stylesheet">
    <link href="~/assets/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css" rel="stylesheet">
</head>
<body class="fix-header fix-sidebar card-no-border logo-center" ng-app="HelpDesk">
    <div class="preloader">
        <svg class="circular" viewBox="25 25 50 50">
            <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10" />
        </svg>
    </div>
    <div id="main-wrapper">
        <header class="topbar">
            <nav class="navbar top-navbar navbar-toggleable-sm navbar-light">
                <div class="navbar-header">
                    <a class="navbar-brand" href="/Home/Dashboard">
                        <b>
                            <img src="~/assets/images/logo-icon.png" alt="homepage" class="dark-logo" />
                            <img src="~/assets/images/logo.png" alt="homepage" class="light-logo" width="200px" height="60px" />
                        </b>
                    </a>
                </div>
                <div class="navbar-collapse">
                    @*<ul class="navbar-nav mr-auto mt-md-0">
                        <li class="nav-item"> <a class="nav-link nav-toggler hidden-md-up text-muted waves-effect waves-dark" href="javascript:void(0)"><i class="mdi mdi-menu"></i></a> </li>
                        <li class="nav-item dropdown mega-dropdown" ng-controller="TicketNuevo">
                            <a class="nav-link dropdown-toggle text-muted waves-effect waves-dark" href="#" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="NuevoTicket"><i class="mdi mdi-library-plus" title="Nuevo Ticket"></i><span style=""> Nuevo Ticket</span></a>
                            <div class="dropdown-menu scale-up-left">
                                <div id="cargarTicket" style="text-align:center;height:450px;" ng-show="Cargando"><img src="~/assets/images/cargando.gif" /></div>
                                <div id="formTicket" ng-show="!Cargando">
                                    <h4 class="m-b-20">Nuevo Ticket</h4>
                                    <ul class="mega-dropdown-menu row">
                                        <li class="col-lg-4  m-b-30">
                                            <div class="form-group">
                                                <label>Tipo Solicitud</label>
                                                <a class="mytooltip" href="javascript:void(0)" data-toggle="modal" data-target="#mDefinicion"><i class="fa fa-fw fa-question"></i></a>
                                                <select class="form-control" id="TipoSolicitud">
                                                   <option value="">SELECCIONE</option>
                                                   <option value="1">INCIDENTE</option>
                                                   <option value="2">REQUERIMIENTO</option>
                                                </select>
                                            </div>
                                            <div Class="form-group">
                                                <Label> Necesidad</Label>
                                                <textarea Class="form-control" rows="5" id="Necesidad"></textarea>
                                            </div>
                                        </li>
                                        <li Class="col-lg-3 col-xlg-4 m-b-30">
                                            <div class="form-group">
                                                <label>Tipo Ticket</label>
                                                <select class="form-control" id="TipoTicket" onchange="ConsultarSoporte();">
                                                    <option value="">SELECCIONE</option>
                                                    @For Each item As System.Data.DataRow In DtTipoTicket.Rows
                                                        @<option value="@(item("Padre").ToString.Trim)">@(item("TipoTicket").ToString.Trim)</option>
                                                    Next
                                                </select>
                                            </div>
                                            <div Class="form-group">
                                                <Label> Justificación</Label>
                                                <textarea Class="form-control" rows="5" id="Justificacion" ></textarea>
                                            </div>
                                            <div Class="form-group" style="text-align:center">
                                                <Button type="button" Class="btn waves-effect waves-light btn-rounded btn-success" ng-click="GuardarTicket();">Guardar</Button>
                                                <Button type="button" Class="btn waves-effect waves-light btn-rounded btn-danger">Limpiar</Button>
                                            </div>
                                        </li>
                                        <li Class="col-lg-3 col-xlg-4 m-b-30">
                                            <div Class="form-group">
                                                <Label> Tipo Soporte</Label>
                                                <select Class="form-control" id="TipoSoporte"></select>
                                            </div>
                                            <Label> Subir Archivo</Label>
                                            <form id="FormArchivo" action="@Url.Action("Index", "Files")" method="post" enctype="multipart/form-data">
                                                <input type="hidden" name="RutaArchivo" id="RutaArchivo" value=""/>
                                                <input style="margin-bottom:3px;" type="file" name="archivo" id="archivo" value="" Class="btn waves-effect waves-light btn-rounded btn-success" />
                                                <Button type="submit" name="subir" Class="btn waves-effect waves-light btn-rounded btn-success">Subir Archivo</Button>
                                                <div id="RespuestaFile" style="font-style: italic;font-size:10px;margin-top:3px;"></div>
                                            </form>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                        </li>
                    </ul>*@
                    @*<ul Class="navbar-nav my-lg-0">
                        <li Class="nav-item dropdown">
                            <a Class="nav-link dropdown-toggle text-muted waves-effect waves-dark" href="#" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img src="~/assets/images/users/1.jpg" alt="user" class="profile-pic" /></a>
                            <div Class="dropdown-menu dropdown-menu-right scale-up">
                                <ul Class="dropdown-user">
                                    <li>
                                        <div Class="dw-user-box">
                                            <div class="u-img"><img src="~/assets/images/users/1.jpg" alt="user"></div>
                                            <div class="u-text">
                                                <h4>@(Session("UserNom"))</h4>
                                                <p class="text-muted">@(Session("UserCod"))</p>
                                            </div>
                                        </div>
                                    </li>
                                    <li><a href="@Url.Action("CerrarSesion", "Home")"><i class="fa fa-power-off"></i> Cerrar Sesión</a></li>
                                </ul>
                            </div>
                        </li>
                    </ul>*@
                </div>
            </nav>
        </header>
        <aside class="left-sidebar">
            <div class="scroll-sidebar">
                <nav class="sidebar-nav">
                    <ul id="sidebarnav">
                        <li class="nav-small-cap">PERSONAL</li>
                        <li>
                            <a Class="has-arrow" href="http://sendesk.cajacopi.com/" aria-expanded="false">
                                <i class="mdi mdi-gauge"></i>
                                <span class="hide-menu">Sendesk </span>
                            </a>
                        </li>
                        <!--<li>
                            <a Class="has-arrow " href="#" aria-expanded="false"><i class="mdi mdi-arrange-send-backward"></i><span class="hide-menu">Multi level dd</span></a>
                            <ul aria-expanded="false" Class="collapse">
                                <li> <a href = "#" > item 1.1</a></li>
                                <li> <a href = "#" > item 1.2</a></li>
                                <li>
                                    <a Class="has-arrow" href="#" aria-expanded="false">Menu 1.3</a>
                                    <ul aria-expanded="false" Class="collapse">
                                        <li> <a href = "#" > item 1.3.1</a></li>
                                        <li> <a href = "#" > item 1.3.2</a></li>
                                        <li> <a href = "#" > item 1.3.3</a></li>
                                        <li> <a href = "#" > item 1.3.4</a></li>
                                    </ul>
                                </li>
                                <li> <a href = "#" > item 1.4</a></li>
                            </ul>
                        </li>-->
                    </ul>
                </nav>
            </div>
        </aside>
        @RenderBody
        <footer class="footer">
            @(Biblioteca.Variables.tFooter)
        </footer>
    </div>
    <script src="~/assets/plugins/bootstrap/js/tether.min.js"></script>
    <script src="~/assets/plugins/bootstrap/js/bootstrap.min.js"></script>
    <script src="~/Content/js/jquery.slimscroll.js"></script>
    <script src="~/Content/js/waves.js"></script>
    <script src="~/Content/js/sidebarmenu.js"></script>
    <script src="~/assets/plugins/sticky-kit-master/dist/sticky-kit.min.js"></script>
    <script src="~/assets/plugins/sparkline/jquery.sparkline.min.js"></script>
    <script src="~/Content/js/custom.min.js"></script>
    <script src="~/assets/plugins/chartist-js/dist/chartist.min.js"></script>
    <script src="~/assets/plugins/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js"></script>
    <script src="~/assets/plugins/echarts/echarts-all.js"></script>
    <script src="~/assets/plugins/vectormap/jquery-jvectormap-2.0.2.min.js"></script>
    <script src="~/assets/plugins/vectormap/jquery-jvectormap-world-mill-en.js"></script>
    <script src="~/assets/plugins/moment/moment.js"></script>
    <script src='~/assets/plugins/calendar/dist/fullcalendar.min.js'></script>
    <script src="~/assets/plugins/calendar/dist/jquery.fullcalendar.js"></script>
    <script src="~/assets/plugins/calendar/dist/locale-all.js"></script>
    <script src="~/assets/plugins/sparkline/jquery.sparkline.min.js"></script>
    <script src="~/Content/js/dashboard4.js"></script>
    <script src="~/assets/plugins/styleswitcher/jQuery.style.switcher.js"></script>
    <script src="~/assets/plugins/dropify/dist/js/dropify.min.js"></script>
    <script src="~/Content/js/toastr.min.js"></script>
    <script src="~/Content/js/ui-toastr.min.js"></script>
    <script src="~/AngularJS/push.min.js"></script>
    