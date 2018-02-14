@Code
    Dim oHome As New HelpDesk.HomeController
    Dim DtTipoTicket = oHome.Consultar_TiposTicketPadre
    Dim DtRspTick As System.Data.DataTable = oHome.Consulta_ResponsableTicketsDt()
    Dim OpcionesTick As Boolean = False
    Dim CodUsuario As String = Session("UserCod").ToString.Trim
    Dim TablaEstadistica As System.Data.DataTable = oHome.Consultar_EstTicektNuevo(CodUsuario)
    '
    Dim Nro_Ticket_Calendario As Integer = 0,
        Nro_Ticket_New As Integer = 0,
        Nro_Ticket_PorVerificacion As Integer = 0,
        Nro_Ticket_Viable As Integer = 0
    If IsNothing(TablaEstadistica) = False Then
        Nro_Ticket_New = TablaEstadistica.Rows(0)("TicketNuevos").ToString.Trim
        Nro_Ticket_Calendario = TablaEstadistica.Rows(0)("EnEsperaCalendario").ToString.Trim
        Nro_Ticket_PorVerificacion = TablaEstadistica.Rows(0)("PorVerificacion").ToString.Trim
        Nro_Ticket_Viable = TablaEstadistica.Rows(0)("TicketViable").ToString.Trim
    End If
    '
    For Each item As System.Data.DataRow In DtRspTick.Rows
        If (item("CodUsuario") = Session("UserCod").ToString.Trim) Then
            OpcionesTick = True
        End If
    Next
End Code
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
                    <ul class="navbar-nav mr-auto mt-md-0">
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
                                                <div class="alert alert-info" style="font-size:14px;margin-top:10px;text-align:justify">
                                                    <b>Nota Importante!</b> Al momento de subir un archivo, debe seleccionarlo y luego presionar el el boton Subir Archivo, de lo contrario este no sera adjuntado al ticket.
                                                </div>
                                                <div id="RespuestaFile" style="font-style: italic;font-size:10px;margin-top:3px;"></div>
                                            </form>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                        </li>
                    </ul>
                    <ul Class="navbar-nav my-lg-0">
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
                    </ul>
                </div>
            </nav>
        </header>
        <aside class="left-sidebar">
            <div class="scroll-sidebar">
                <nav class="sidebar-nav">
                    <ul id="sidebarnav">
                        <li class="nav-small-cap">PERSONAL</li>
                        <li>
                            <a Class="has-arrow" href="http://sendesk.cajacopi.com/Home/Dashboard" aria-expanded="false">
                                <i class="mdi mdi-gauge"></i>
                                <span class="hide-menu"> Inicio Sendesk </span>
                            </a>
                        </li>
                        @if (OpcionesTick = True) Then
                            @<li>
                                <a Class="has-arrow" href="#" aria-expanded="false"><i class="mdi mdi-gauge"></i>
                                <span class="hide-menu">Procesos</span></a>
                                <ul aria-expanded="false" Class="collapse">                                
                                    <li><a href="/Home/TicketNuevo"><span class="label label-info">@(Nro_Ticket_New.ToString.Trim)</span> Ticket Nuevos</a></li>
                                    <li><a href="/Home/AgendarTicket"><span class="label label-info">@(Nro_Ticket_Calendario.ToString.Trim)</span> Agendar Tickets</a></li>
                                    <li><a href="/Home/TicketCitados"><span class="label label-info">@(Nro_Ticket_PorVerificacion.ToString.Trim)</span> Ticket Citados</a></li>
                                    <li><a href="/Home/TicketAsignados"><span class="label label-info">@(Nro_Ticket_Viable.ToString.Trim)</span> Ticket Asignados</a></li>
                                    <li><a href="/Home/Graficas"><span class="label label-info">0</span>Gráficas</a></li>                                    
                                </ul>
                            </li>
                        End If                        
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
    <script>
        function EnviarDatos(Funcion, Controlador, Titulo, Div, Datos) {
            $("#" + Div + '_Content').html("");
            var t_url = "/" + Controlador + "/" + Funcion
            $('#' + Div + '_title').html(Titulo);
            $('#' + Div + '_Content').html('<div style="width:100%;height:100%;margin-top:20px;"><img src="../assets/images/cargando.gif" height="200px" style="display:block;margin-left:auto;margin-right:auto;border:none;"/></div>');
            $.ajax({
                type: 'POST', data: Datos, dataType: 'html', url: t_url,
                success: function (result) { $("#" + Div + '_Content').html(result); },
                error: function (error) { alert('Lo sentimos, se presento un error al cargar la ventana.'); }
            });
        }

        function NotificacionPush(titulo, mensaje, url) {
            Push.create(titulo, {
                body: mensaje,
                icon: "https://lh3.googleusercontent.com/-U2-QPR5evu8/AAAAAAAAAAI/AAAAAAAAACA/HL7nT-jWBwE/s640/photo.jpg",
                timeout: 1200000,
                onClick: function () { if (url != '') { window.location.href = url } }
            });
        }
        function NotificacionPopup(shortCutFunction, titulo, mensaje) {
            //shortCutFunction: success, error
            var i = -1,
                toastCount = 0,
                $toastlast
            var msg = mensaje;
            var title = titulo;
            var $showDuration = 1000;
            var $hideDuration = 1000;
            var $timeOut = 5000;
            var $extendedTimeOut = 1000;
            var $showEasing = 'swing';
            var $hideEasing = 'linear';
            var $showMethod = 'fadeIn';
            var $hideMethod = 'fadeOut';
            var toastIndex = toastCount++;
            toastr.options = {
                closeButton: $('#closeButton').prop('checked'),
                debug: $('#debugInfo').prop('checked'),
                positionClass: $('#positionGroup input:checked').val() || 'toast-top-right',
                onclick: null
            };
            toastr.options.showDuration = $showDuration;
            toastr.options.hideDuration = $hideDuration;
            toastr.options.timeOut = $timeOut;
            toastr.options.extendedTimeOut = $extendedTimeOut;
            toastr.options.showEasing = $showEasing;
            toastr.options.hideEasing = $hideEasing;
            toastr.options.showMethod = $showMethod;
            toastr.options.hideMethod = $hideMethod;
            var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
            $toastlast = $toast;
            if ($toast.find('#okBtn').length) {
                $toast.delegate('#okBtn', 'click', function () {
                    alert('you clicked me. i was toast #' + toastIndex + '. goodbye!');
                    $toast.remove();
                });
            }
            if ($toast.find('#surpriseBtn').length) {
                $toast.delegate('#surpriseBtn', 'click', function () {
                    alert('Surprise! you clicked me. i was toast #' + toastIndex + '. You could perform an action here.');
                });
            }
        };

        $(document).ready(function () {
            $('.dropify').dropify();
            $('.dropify-fr').dropify({
                messages: {
                    default: 'Glissez-déposez un fichier ici ou cliquez',
                    replace: 'Glissez-déposez un fichier ou cliquez pour remplacer',
                    remove: 'Supprimer',
                    error: 'Désolé, le fichier trop volumineux'
                }
            });
            var drEvent = $('#input-file-events').dropify();
            drEvent.on('dropify.beforeClear', function (event, element) {
                return confirm("Do you really want to delete \"" + element.file.name + "\" ?");
            });
            drEvent.on('dropify.afterClear', function (event, element) {
                alert('File deleted');
            });
            drEvent.on('dropify.errors', function (event, element) {
                console.log('Has Errors');
            });
            var drDestroy = $('#input-file-to-destroy').dropify();
            drDestroy = drDestroy.data('dropify')
            $('#toggleDropify').on('click', function (e) {
                e.preventDefault();
                if (drDestroy.isDropified()) {
                    drDestroy.destroy();
                } else {
                    drDestroy.init();
                }
            })
        });
    </script>
    <script>
        $('#FormArchivo').submit(function (e) {
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
                    $('#RespuestaFile').html('<img src="../assets/images/pack/Valid.png" width="20px" height="20px" /> ' + resp1);
                    $('#RutaArchivo').val(resp2)
                },
                error: function (ex) {
                    $('#RespuestaFile').html('<img src="../assets/images/pack/Error.png" width="20px" height="20px"/> ' + " Error al subir el archivo");
                }
            });
        });
        function ConsultarSoporte() {
            var TipoTicket = $("#TipoTicket").val();
            var Html = '';
            $.getJSON('@Url.Action("Consultar_TiposSoporte", "Home")', {
                'TipoTicket': TipoTicket
            }, function (resultado) {
                $.each(JSON.parse(resultado), function (index, Data) {
                    Html = Html + '<option value="' + Data.Id + '">' + Data.TipoSoporte + '</option>';
                });
                $("#TipoSoporte").html(Html);
            });
        }
        function LimpiarNuevoTicket() {
            $("#TipoTicket").val('');
            $("#TipoSoporte").empty();
            $("#Necesidad").val('');
            $("#Justificacion").val('');
            $("#RutaArchivo").val('');
            $('#RespuestaFile').empty();
        }
    </script>
    <div id="mDefinicion" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Definiciones</h4>
                </div>
                <div class="modal-body">
                    <p style="text-align:justify">
                        <b>Incidente:</b>  situación que se presenta en el desarrollo de una actividad que interrumpe u obstaculiza su ejecución de manera satisfactoria. Los incidentes
                        obedecen a fallas o problemas sobre herramientas ya existentes que venian operando sin problemas y que por factores internos o externos alteran el resultado
                        esperado. <br /><br />
                        <b>Requerimiento:</b> Este tipo de solicitudes aplica cuando existe la necesidad de una nueva herramienta o efectuar cambios, mejoras o actualizaciones sobres las ya
                        existentes por cambios normativos, mejoras en procesos internos o implementación de políticas o nuevos enfoques de la organización.
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>

        </div>
    </div>
</body>
</html>
<div id="modal_long" class="modal fade bs-modal-lg" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                <h4 id="modal_long_title" class="modal-title"></h4>
            </div>
            <div class="modal-body">
                <div id="modal_long_scroller" class="scroller" style="height:100%;" data-always-visible="1" data-rail-visible1="1">
                    <div id="modal_long_Content"></div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn default" data-dismiss="modal"> Cerrar Ventana </button>
            </div>
        </div>
    </div>
</div>
