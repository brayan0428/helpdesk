
@Code
    Layout = Nothing
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
End Code
<!--<script type="text/javascript" src="https://www.google.com/jsapi"></script>-->
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script>
    function dibujarGrafico_1() {
        // Tabla de datos: valores y etiquetas de la gráfica
        var Tipo = $("#Tipo").val();
        var FechaIni = $("#FechaIni").val();
        var FechaFin = $("#FechaFin").val();
        if (FechaIni == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar la fecha de inicio del reporte');
            return;
        }
        if (FechaFin == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar la fecha de inicio del reporte');
            return;
        }
        google.charts.load("current", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawChart);
        function drawChart() {
            $.post('/Home/Consultar_DatosGraficas', {
                "Tipo" : Tipo,
                "FechaIni":FechaIni,
                "FechaFin":FechaFin
            },
              function (data) {
                  var tdata = new google.visualization.arrayToDataTable(data);
                  var options = {
                      title: $("#Tipo option:selected").text(),
                      is3D: true,
                  };
                  if (Tipo == '1' || Tipo == '2' || Tipo == '5' || Tipo == '6' || Tipo == '9') {
                      var Chart = new google.visualization.ColumnChart(document.getElementById('GraficoGoogleChart-ejemplo-1'));
                  }
                  if (Tipo == '3' || Tipo == '4' || Tipo == '7' ) {
                      var Chart = new google.visualization.PieChart(document.getElementById('GraficoGoogleChart-ejemplo-1'));
                  }
                  Chart.draw(tdata, options);
              });
        }
    }

    function GenerarExcel() {
        var Tipo        = $("#Tipo").val();
        var FechaIni    = $("#FechaIni").val();
        var FechaFin    = $("#FechaFin").val();
        if (FechaIni == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar la fecha de inicio del reporte');
            return;
        }
        if (FechaFin == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar la fecha de inicio del reporte');
            return;
        }
        $("#bExcel").html('<a id="btnExcel" style="color:white" Class="btn btn-success waves-effect text-left" href="/Home/Consultar_Graficas_Excel?Tipo=' + Tipo + '&FechaIni=' + FechaIni + '&FechaFin=' + FechaFin + '">Descargar Excel</a>');
    }
</script>
<div Class="page-wrapper">
    <div Class="container-fluid">
        <div Class="row page-titles">
            <div Class="col-md-5 col-8 align-self-center">
                <h3 Class="text-themecolor m-b-0 m-t-0">Tickets Nuevos (Aprobados)</h3>
                <ol Class="breadcrumb">
                    <li Class="breadcrumb-item"><a href="Index">Home</a></li>
                    <li Class="breadcrumb-item active">Ticket</li>
                </ol>
            </div>
        </div>
        <div Class="row">
            <div Class="col-12">
                <div Class="card">
                    <div Class="card-block">
                        <h4 Class="card-title">Lista de tickets de soporte</h4>
                        <h6 Class="card-subtitle">Lista de entradas de tickets</h6>
                        <div Class="row m-t-40">
                            <div Class="col-3">
                                <label>Seleccionar Consulta</label> 
                                <select id="Tipo" name="Tipo" class="form-control" onchange="GenerarExcel();">
                                    <option value="1">Ticket Ingresados por usuarios</option>
                                    <option value="2">Ticket Asignados responsables</option>
                                    <option value="3">Estados de tickets</option>
                                    <option value="4">Calificacion de Satisfaccion</option>
                                    <option value="5">Calificacion de Satisfaccion Detallada por Tiempo</option>
                                    <option value="6">Calificacion de Satisfaccion Detallada por Servicio</option>
                                    <option value="7">Consulta por numero de incidentes y Requerimientos</option>
                                    <option value="9">Consulta tickets ingresados por area</option>
                                    <!--<option value="8">Numero de tickets por usuarios</option>-->
                                </select>
                            </div>
                            <!-- Column -->
                            <div Class="col-md-3">
                                <label>Fecha Ini</label>
                                <input type="date" id="FechaIni" name="FechaIni" value="@(Date.Now.ToShortDateString)" class="form-control" onchange="GenerarExcel();"/>
                            </div>
                            <!-- Column -->
                            <div class="col-md-3">
                                <label>Fecha Fin</label>
                                <input type="date" id="FechaFin" name="FechaFin" value="@(Date.Now.ToShortDateString)" class="form-control" onchange="GenerarExcel();"/>
                            </div>
                            <!-- Column -->
                            <div class="col-md-3" id="dBotones">
                                <label>Opciones</label><br />
                                <button id="Consultar" type="button" Class="btn btn-danger waves-effect text-left" onclick="dibujarGrafico_1();">Generar Grafica</button>
                                <span id="bExcel"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div Class="row">
            <div Class="col-12">
                <div id="GraficoGoogleChart-ejemplo-1" style="width:100%;height:600px">
                </div>
            </div>
        </div>
    </div>
</div>