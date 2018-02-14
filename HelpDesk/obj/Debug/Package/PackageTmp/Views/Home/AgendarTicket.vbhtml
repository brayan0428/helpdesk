@Code
    ViewData("Title") = "AgendarTicket"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
End Code
<link href="~/assets/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css" rel="stylesheet">
<!-- Page plugins css -->
<link href="~/assets/plugins/clockpicker/dist/jquery-clockpicker.min.css" rel="stylesheet">
<!-- Color picker plugins css -->
<link href="~/assets/plugins/jquery-asColorPicker-master/css/asColorPicker.css" rel="stylesheet">
<!-- Date picker plugins css -->
<link href="~/assets/plugins/bootstrap-datepicker/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
<!-- Daterange picker plugins css -->
<link href="~/assets/plugins/timepicker/bootstrap-timepicker.min.css" rel="stylesheet">
<link href="~/assets/plugins/bootstrap-daterangepicker/daterangepicker.css" rel="stylesheet">
<div class="page-wrapper" style="min-height: 840px;" ng-controller="Agenda_Tickets" >
    <!-- ============================================================== -->
    <!-- Container fluid  -->
    <!-- ============================================================== -->
    <div class="container-fluid">
        <!-- ============================================================== -->
        <!-- Bread crumb and right sidebar toggle -->
        <!-- ============================================================== -->
        <div class="row page-titles">
            <div class="col-md-5 col-8 align-self-center">
                <h3 class="text-themecolor m-b-0 m-t-0">Agendar Tickets</h3>
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="Index">Home</a></li>
                    <li class="breadcrumb-item active">Icon</li>
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
        <!-- ============================================================== -->
        <!-- End Bread crumb and right sidebar toggle -->
        <!-- ============================================================== -->
        <!-- ============================================================== -->
        <!-- Start Page Content -->
        <!-- ============================================================== -->
        <div class="row">
            <div class="col-md-3">
                <div class="card">
                    <div class="card-block">
                        <h4 class="card-title">Aréas</h4>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div id="calendar-events" class="m-t-20">
                                    <div ng-repeat="dato in Datos" class="calendar-events ui-draggable ui-draggable-handle" data-class="bg-info" style="position: relative;cursor:pointer" ng-click="MostrarTickets(dato.Area,dato.NombreAre);"><i class="fa fa-circle" style="color:{{dato.Color}}"></i> {{dato.NombreAre | limitTo : 20}}...</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-9">
                <div class="card">
                    <div class="card-block">
                        <div class="container">
                            <div class="row">
                                <div class="col-lg-12 text-center">
                                    <div id="calendar" class="col-centered">
                                    </div>
                                </div>
                            </div>

                            <!-- Modal Agregar -->
                            <a id="VerTicket" data-toggle="modal" data-target=".bs-example-modal-lg" style="visibility:hidden;">VerTicket</a>
                            <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="modal-title-add"></h4>
                                            <input type="hidden" id="modal-id"/>
                                            <button id="CerrarInfoTicket" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        </div>
                                        <div class="modal-body">
                                            <div ng-show="CargandoTicket" style="text-align:center;height:450px;">
                                                <img src="~/assets/images/cargando.gif" />
                                            </div>
                                            <div ng-show="!CargandoTicket"> 
                                                <table class="table color-bordered-table info-bordered-table" data-tablesaw-mode="swipe" data-tablesaw-sortable="" data-tablesaw-sortable-switch="" data-tablesaw-minimap="" data-tablesaw-mode-switch="" id="tTickets">
                                                    <thead>
                                                        <tr>
                                                            <th width="20">Acción</th>
                                                            <th># Ticket</th>
                                                            <th>Necesidad</th>
                                                            <th>Usuario</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr ng-repeat="dato in Tickets">
                                                            <td class="title tablesaw-cell-persist" style="text-align:center">
                                                                <div class="md-checkbox">
                                                                    <input type="checkbox" id="Check_{{dato.CodTicket}}" class="md-check">
                                                                    <label for="Check_{{dato.CodTicket}}">
                                                                    </label>
                                                                </div>
                                                            </td>
                                                            <td>{{dato.CodTicket}}</td>
                                                            <td>{{dato.Necesidad | limitTo : 150}}...</td>
                                                            <td>{{dato.CodUsuario}}</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="control-label col-md-3">Fecha</label>
                                                            <div class="col-md-9">
                                                                <input type="text" class="form-control" placeholder="@(Date.Now.ToString("yyyy-MM-dd"))" id="FechaCita">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="control-label col-md-3">Hora</label>
                                                            <div class="col-md-9">
                                                                <input class="form-control" id="HoraCita" placeholder="14:00" data-dtp="dtp_mhjoh">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="TicketSelecc" name="TicketSelecc" value="" />
                                                    <button type="button" class="btn btn-success waves-effect text-left" ng-click="GuardarCita();">Guardar Cita</button>
                                                    <button type="button" class="btn btn-danger waves-effect text-left" data-dismiss="modal" id="xCloseAdd">Cerrar</button>
                                                </div>
                                            </div>
                                            </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Modal Editar -->
                            <div class="modal fade" id="ModalEdit" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title" id="myModalLabel">Editar Cita</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div ng-show="CargandoTicket" style="text-align:center;height:450px;">
                                                <img src="~/assets/images/cargando.gif" />
                                            </div>
                                            <div ng-show="!CargandoTicket">
                                                <div class="form-group">
                                                    <label for="title" class="col-sm-2 control-label">Aréa</label>
                                                    <div class="col-sm-10">
                                                        <input type="text" name="title" class="form-control" id="title" placeholder="Title" disabled="disabled">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="control-label col-md-3">Fecha</label>
                                                            <div class="col-md-9">
                                                                <input type="text" class="form-control" id="FechaCita_Edit">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="control-label col-md-3">Hora</label>
                                                            <div class="col-md-9">
                                                                <input class="form-control" id="HoraCita_Edit" data-dtp="dtp_mhjoh">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <input type="hidden" name="id" class="form-control" id="Id_Editar">
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default" data-dismiss="modal" id="xCloseEdit" >Close</button>
                                            <button type="submit" class="btn btn-primary" id="Edit_Cita" ng-click="EditarCita();">Actualizar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- BEGIN MODAL -->
        <div class="modal none-border" id="my-event">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title"><strong>Add Event</strong></h4>
                    </div>
                    <div class="modal-body"></div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-white waves-effect" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-success save-event waves-effect waves-light">Create event</button>
                        <button type="button" class="btn btn-danger delete-event waves-effect waves-light" data-dismiss="modal">Delete</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal Add Category -->
        <div class="modal fade none-border" id="add-new-event">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title"><strong>Add</strong> a category</h4>
                    </div>
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="control-label">Category Name</label>
                                    <input class="form-control form-white" placeholder="Enter name" type="text" name="category-name">
                                </div>
                                <div class="col-md-6">
                                    <label class="control-label">Choose Category Color</label>
                                    <select class="form-control form-white" data-placeholder="Choose a color..." name="category-color">
                                        <option value="success">Success</option>
                                        <option value="danger">Danger</option>
                                        <option value="info">Info</option>
                                        <option value="primary">Primary</option>
                                        <option value="warning">Warning</option>
                                        <option value="inverse">Inverse</option>
                                    </select>
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect waves-light save-category" data-dismiss="modal">Save</button>
                        <button type="button" class="btn btn-white waves-effect" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- END MODAL -->
        <!-- ============================================================== -->
        <!-- End PAge Content -->
        <!-- ============================================================== -->
    </div>
    <!-- ============================================================== -->
    <!-- End Container fluid  -->
    <!-- ============================================================== -->
    <!-- ============================================================== -->
    <!-- footer -->
    <!-- ============================================================== -->
    <footer class="footer">
        © 2017 Material Pro Admin by wrappixel.com
    </footer>
    <!-- ============================================================== -->
    <!-- End footer -->
    <!-- ============================================================== -->
</div>
<script>

    function CargarCalendario() {
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar({
            locale: 'es',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            defaultDate: FechaHoraAct(),
            editable: true,
            eventLimit: true, // allow "more" link when too many events
            selectable: true,
            selectHelper: true,
            eventRender: function(event, element) {
                element.bind('dblclick', function() {
                    $('#ModalEdit #Id_Editar').val(event.id);
                    $('#ModalEdit #title').val(event.title);
                    var Hora = String(event.hour.Hours);
                    var Min = String(event.hour.Minutes);
                    if (Hora.length == 1) {
                        Hora = '0' + Hora;
                    }
                    if (Min.length == 1) {
                        Min = '0' + Min;
                    }
                    console.log(Hora + ':' + Min);
                    $('#ModalEdit #FechaCita_Edit').val(event.start._i);
                    $('#ModalEdit #HoraCita_Edit').val(Hora + ':' + Min);
                    $('#ModalEdit').modal('show');
                });
            },
            events: '/Home/Consulta_Reuniones'
        });
    }

    function FechaHoraAct() {
        var FechaAct = new Date();
        var dd = FechaAct.getDate();
        var mm = FechaAct.getMonth() + 1;
        var yyyy = FechaAct.getFullYear();
        var hh = FechaAct.getHours();
        var mi = FechaAct.getMinutes();
        var ss = FechaAct.getSeconds();
        if (dd < 10) { dd = '0' + dd; }
        if (mm < 10) { mm = '0' + mm; }
        if (hh < 10) { hh = '0' + hh; }
        if (mi < 10) { mi = '0' + mi; }
        if (ss < 10) { ss = '0' + ss; }
        var FechaActFomato = yyyy + '-' + mm + '-' + dd;
        return FechaActFomato;
    }

    setInterval(function () { CargarCalendario() }, 5000);

    $(document).ready(function () {
        CargarCalendario();
	});

</script>
<script src="~/assets/plugins/moment/moment.js"></script>
<script src="~/assets/plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>
<!-- Clock Plugin JavaScript -->
<script src="/assets/plugins/clockpicker/dist/jquery-clockpicker.min.js"></script>
<!-- Color Picker Plugin JavaScript -->
<script src="~/assets/plugins/jquery-asColorPicker-master/libs/jquery-asColor.js"></script>
<script src="/assets/plugins/jquery-asColorPicker-master/libs/jquery-asGradient.js"></script>
<script src="~/assets/plugins/jquery-asColorPicker-master/dist/jquery-asColorPicker.min.js"></script>
<!-- Date Picker Plugin JavaScript -->
<script src="~/assets/plugins/bootstrap-datepicker/bootstrap-datepicker.min.js"></script>
<!-- Date range Plugin JavaScript -->
<script src="~/assets/plugins/timepicker/bootstrap-timepicker.min.js"></script>
<script src="~/assets/plugins/bootstrap-daterangepicker/daterangepicker.js"></script>
<script>
    // MAterial Date picker
    $('#FechaCita').bootstrapMaterialDatePicker({
        weekStart: 0,
        time: false
    });

    $('#FechaCita_Edit').bootstrapMaterialDatePicker({
        weekStart: 0,
        time: false
    });

    $('#HoraCita').bootstrapMaterialDatePicker({
        format: 'HH:mm',
        time: true,
        date: false
    });

    $('#HoraCita_Edit').bootstrapMaterialDatePicker({
        format: 'HH:mm',
        time: true,
        date: false
    });

    $('#date-format').bootstrapMaterialDatePicker({
        format: 'dddd DD MMMM YYYY - HH:mm'
    });

    $('#min-date').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY HH:mm',
        minDate: new Date()
    });
    // Clock pickers
    $('#single-input').clockpicker({
        placement: 'bottom',
        align: 'left',
        autoclose: true,
        'default': 'now'
    });
    $('.clockpicker').clockpicker({
        donetext: 'Done',
    }).find('input').change(function() {
        console.log(this.value);
    });
    $('#check-minutes').click(function(e) {
        // Have to stop propagation here
        e.stopPropagation();
        input.clockpicker('show').clockpicker('toggleView', 'minutes');
    });
    if (/mobile/i.test(navigator.userAgent)) {
        $('input').prop('readOnly', true);
    }
    // Colorpicker
    $(".colorpicker").asColorPicker();
    $(".complex-colorpicker").asColorPicker({
        mode: 'complex'
    });
    $(".gradient-colorpicker").asColorPicker({
        mode: 'gradient'
    });
    // Date Picker
    jQuery('.mydatepicker, #datepicker').datepicker();
    jQuery('#datepicker-autoclose').datepicker({
        autoclose: true,
        todayHighlight: true
    });
    jQuery('#date-range').datepicker({
        toggleActive: true
    });
    jQuery('#datepicker-inline').datepicker({
        todayHighlight: true
    });
    // Daterange picker
    $('.input-daterange-datepicker').daterangepicker({
        buttonClasses: ['btn', 'btn-sm'],
        applyClass: 'btn-danger',
        cancelClass: 'btn-inverse'
    });
    $('.input-daterange-timepicker').daterangepicker({
        timePicker: true,
        format: 'MM/DD/YYYY h:mm A',
        timePickerIncrement: 30,
        timePicker12Hour: true,
        timePickerSeconds: false,
        buttonClasses: ['btn', 'btn-sm'],
        applyClass: 'btn-danger',
        cancelClass: 'btn-inverse'
    });
    $('.input-limit-datepicker').daterangepicker({
        format: 'MM/DD/YYYY',
        minDate: '06/01/2015',
        maxDate: '06/30/2015',
        buttonClasses: ['btn', 'btn-sm'],
        applyClass: 'btn-danger',
        cancelClass: 'btn-inverse',
        dateLimit: {
            days: 6
        }
    });
</script>
<!-- ============================================================== -->
<!-- Style switcher -->
<!-- ============================================================== -->
<script src="~/assets/plugins/styleswitcher/jQuery.style.switcher.js"></script>