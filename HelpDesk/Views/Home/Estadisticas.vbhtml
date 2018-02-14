@Code
    ViewData("Title") = "Estadisticas"
    Layout = "~/Views/Shared/_LayoutDashboard2.vbhtml"
    Dim oHome As New HelpDesk.HomeController,
        DtResponsables As System.Data.DataTable = Nothing,
        DtTotales As System.Data.DataTable = Nothing
    DtResponsables = oHome.Consultar_EstadisticasResponsables
    DtTotales = oHome.Consultar_EstadisticasTicketTotal
End Code
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
        <div class="row">
            <div class="col-4">
                <div class="card card-inverse card-primary">
                    <div class="card-block">
                        <div class="d-flex">
                            <div class="m-r-20 align-self-center">
                                <h1 class="text-white"><i class="ti-folder"></i></h1>
                            </div>
                            <div>
                                <h3 class="card-title">Total Ticket Ingresados</h3>
                                <h6 class="card-subtitle">@(Date.Now.ToString("yyyy-MM"))</h6>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-4 align-self-center">
                                <h2 class="font-light text-white">@DtTotales.Rows(0)("TicketTotal")</h2>
                            </div>
                            <div class="col-8 p-t-10 p-b-20 align-self-center">
                                <div class="usage chartist-chart" style="height:65px"><div class="chartist-tooltip"></div><svg xmlns:ct="http://gionkunz.github.com/chartist-js/ct" width="100%" height="100%" class="ct-chart-line" style="width: 100%; height: 100%;"><g class="ct-grids"></g><g><g class="ct-series ct-series-a"><path d="M30,60L30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5L199.656,60Z" class="ct-area"></path><path d="M30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5" class="ct-line"></path><line x1="30" y1="37.5" x2="30.01" y2="37.5" class="ct-point" value="5"></line><line x1="54.23660714285714" y1="60" x2="54.24660714285714" y2="60" class="ct-point" value="0"></line><line x1="78.47321428571428" y1="6" x2="78.48321428571428" y2="6" class="ct-point" value="12"></line><line x1="102.70982142857143" y1="55.5" x2="102.71982142857144" y2="55.5" class="ct-point" value="1"></line><line x1="126.94642857142857" y1="24" x2="126.95642857142857" y2="24" class="ct-point" value="8"></line><line x1="151.18303571428572" y1="46.5" x2="151.1930357142857" y2="46.5" class="ct-point" value="3"></line><line x1="175.41964285714286" y1="6" x2="175.42964285714285" y2="6" class="ct-point" value="12"></line><line x1="199.65625" y1="-7.5" x2="199.66625" y2="-7.5" class="ct-point" value="15"></line></g></g><g class="ct-labels"></g></svg></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-4">
                <div class="card card-inverse card-success">
                    <div class="card-block">
                        <div class="d-flex">
                            <div class="m-r-20 align-self-center">
                                <h1 class="text-white"><i class="ti-check"></i></h1>
                            </div>
                            <div>
                                <h3 class="card-title">Total Ticket Solucionados</h3>
                                <h6 class="card-subtitle">@(Date.Now.ToString("yyyy-MM"))</h6>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-4 align-self-center">
                                <h2 class="font-light text-white">@DtTotales.Rows(0)("TicketSolucionados")</h2>
                            </div>
                            <div class="col-8 p-t-10 p-b-20 align-self-center">
                                <div class="usage chartist-chart" style="height:65px"><div class="chartist-tooltip"></div><svg xmlns:ct="http://gionkunz.github.com/chartist-js/ct" width="100%" height="100%" class="ct-chart-line" style="width: 100%; height: 100%;"><g class="ct-grids"></g><g><g class="ct-series ct-series-a"><path d="M30,60L30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5L199.656,60Z" class="ct-area"></path><path d="M30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5" class="ct-line"></path><line x1="30" y1="37.5" x2="30.01" y2="37.5" class="ct-point" value="5"></line><line x1="54.23660714285714" y1="60" x2="54.24660714285714" y2="60" class="ct-point" value="0"></line><line x1="78.47321428571428" y1="6" x2="78.48321428571428" y2="6" class="ct-point" value="12"></line><line x1="102.70982142857143" y1="55.5" x2="102.71982142857144" y2="55.5" class="ct-point" value="1"></line><line x1="126.94642857142857" y1="24" x2="126.95642857142857" y2="24" class="ct-point" value="8"></line><line x1="151.18303571428572" y1="46.5" x2="151.1930357142857" y2="46.5" class="ct-point" value="3"></line><line x1="175.41964285714286" y1="6" x2="175.42964285714285" y2="6" class="ct-point" value="12"></line><line x1="199.65625" y1="-7.5" x2="199.66625" y2="-7.5" class="ct-point" value="15"></line></g></g><g class="ct-labels"></g></svg></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-4">
                <div class="card card-inverse card-danger">
                    <div class="card-block">
                        <div class="d-flex">
                            <div class="m-r-20 align-self-center">
                                <h1 class="text-white"><i class="ti-alert"></i></h1>
                            </div>
                            <div>
                                <h3 class="card-title">Total Ticket Pendientes</h3>
                                <h6 class="card-subtitle">@(Date.Now.ToString("yyyy-MM"))</h6>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-4 align-self-center">
                                <h2 class="font-light text-white">@DtTotales.Rows(0)("TicketPendientes")</h2>
                            </div>
                            <div class="col-8 p-t-10 p-b-20 align-self-center">
                                <div class="usage chartist-chart" style="height:65px"><div class="chartist-tooltip"></div><svg xmlns:ct="http://gionkunz.github.com/chartist-js/ct" width="100%" height="100%" class="ct-chart-line" style="width: 100%; height: 100%;"><g class="ct-grids"></g><g><g class="ct-series ct-series-a"><path d="M30,60L30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5L199.656,60Z" class="ct-area"></path><path d="M30,37.5C34.039,41.25,46.158,65.25,54.237,60C62.315,54.75,70.394,6.75,78.473,6C86.552,5.25,94.631,52.5,102.71,55.5C110.789,58.5,118.868,25.5,126.946,24C135.025,22.5,143.104,49.5,151.183,46.5C159.262,43.5,167.341,15,175.42,6C183.499,-3,195.617,-5.25,199.656,-7.5" class="ct-line"></path><line x1="30" y1="37.5" x2="30.01" y2="37.5" class="ct-point" value="5"></line><line x1="54.23660714285714" y1="60" x2="54.24660714285714" y2="60" class="ct-point" value="0"></line><line x1="78.47321428571428" y1="6" x2="78.48321428571428" y2="6" class="ct-point" value="12"></line><line x1="102.70982142857143" y1="55.5" x2="102.71982142857144" y2="55.5" class="ct-point" value="1"></line><line x1="126.94642857142857" y1="24" x2="126.95642857142857" y2="24" class="ct-point" value="8"></line><line x1="151.18303571428572" y1="46.5" x2="151.1930357142857" y2="46.5" class="ct-point" value="3"></line><line x1="175.41964285714286" y1="6" x2="175.42964285714285" y2="6" class="ct-point" value="12"></line><line x1="199.65625" y1="-7.5" x2="199.66625" y2="-7.5" class="ct-point" value="15"></line></g></g><g class="ct-labels"></g></svg></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div Class="row">
            <div Class="col-12">
                <div Class="card">
                    <div Class="card-block">
                        <div class="row el-element-overlay">
                            @For Each item As System.Data.DataRow In DtResponsables.Rows
                                @<div Class="col-lg-3 col-md-6">
                                    <div Class="card">
                                        <div Class="el-card-item">
                                            <div Class="el-card-avatar el-overlay-1">
                                                <img src="@item("Imagen")" alt="user">
                                                @*<div Class="el-overlay">
                                                    <ul Class="el-info">
                                                        <li> <a Class="btn default btn-outline image-popup-vertical-fit" href="../plugins/images/users/1.html"><i class="icon-magnifier"></i></a></li>
                                                        <li> <a Class="btn default btn-outline" href="javascript:void(0);"><i class="icon-link"></i></a></li>
                                                    </ul>
                                                </div>*@
                                            </div>
                                            <div Class="el-card-content">
                                                <h3 Class="box-title">@item("Responsable")</h3> <small>@item("CodUsuario")</small>
                                                <br>
                                                <div class="box bg-light-info" style="color:black">
                                                    Ticket Asignados: @item("CantidadAsignados")
                                                </div>
                                                <div class="box bg-light-success" style="color:black">
                                                    Ticket Solucionados: @item("CantidadSolucionados")
                                                </div>
                                                <div class="box bg-light-danger" style="color:black">
                                                    Ticket Pendientes: @item("CantidadPendientes")
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            Next
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


