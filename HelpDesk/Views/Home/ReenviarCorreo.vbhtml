@Code
    ViewData("Title") = "ReenviarCorreo"
    Layout = "~/Views/Shared/_LayoutDashboard.vbhtml"
    Dim oHome As New HelpDesk.HomeController,
        DtUsuarios As System.Data.DataTable = Nothing
    DtUsuarios = oHome.Consultar_Usuarios
End Code
<div Class="page-wrapper">
    <div Class="container-fluid">
        <div Class="row page-titles">
            <div Class="col-md-5 col-8 align-self-center">
                <h3 Class="text-themecolor m-b-0 m-t-0">Reenviar Correo</h3>
            </div>
        </div>
        <div Class="row">
            <div Class="col-12">
                <div Class="card">
                    <div Class="card-block">
                        <h4 Class="card-title">Reenciar Correo Autorización</h4>
                        <div Class="row m-t-40">
                            <div Class="col-md-3">
                                <label>Nro Ticket</label>
                                <input type="text" id="NTicket" value="" class="form-control" placeholder="Nro Ticket"/>
                            </div>
                            <!-- Column -->
                            <div Class="col-md-3">
                                <label>Usuario Ingreso</label>
                                <select id="UsuIng" class="form-control" disabled>
                                    <option value="">SELECCIONE</option>
                                    @For Each item As System.Data.DataRow In DtUsuarios.Rows
                                        @<option value="@(item("CodUsuario"))">@(item("Nombre"))</option>
                                    Next
                                </select>
                            </div>
                            <!-- Column -->
                            <div Class="col-md-3">
                                <Label> Usuario Jefe</label>
                                <select id = "UsuJefe" Class="form-control" disabled>
                                    <option value="">SELECCIONE</option>
                                    @For Each item As System.Data.DataRow In DtUsuarios.Rows
                                        @<option value="@(item("CodUsuario"))">@(item("Nombre"))</option>
                                    Next
                                </select>
                            </div>
                            <!-- Column -->
                            <div class="col-md-3">
                                <label>Opciones</label><br />
                                <button id="Consultar" type="button" Class="btn btn-success waves-effect text-left">Reenviar Correo</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
    var tTicket = document.getElementById('NTicket'),
        Btn = document.getElementById('Consultar');

    tTicket.addEventListener("change", function () {
        $.getJSON('Consultar_InfoTicket_Json', {
            'CodTicket': tTicket.value
        }, function (resultado) {
            var Data = JSON.parse(resultado);
            document.getElementById('UsuIng').value = Data[0].CodUsuario;
            document.getElementById("UsuJefe").disabled = false;
        });
    });

    Btn.addEventListener('click', function () {
        var CodUsuario = document.getElementById('UsuIng').value,
            CodJefe = document.getElementById('UsuJefe').value;
        if (tTicket.value == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe ingresar un numero de ticket');
            return;
        }
        if (CodUsuario.trim() == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar el usuario');
            return;
        }
        if (CodJefe.trim() == '') {
            NotificacionPopup('error', 'Error de Aplicacion', 'Debe seleccionar el jefe');
            return;
        }
        $.getJSON('ReenviarCorreoAutorizacion', {
            'CodTicket': tTicket.value,
            'CodUsuario': CodUsuario,
            'CodJefe' : CodJefe
        }, function (resultado) {
            var Data = JSON.parse(resultado);
            if (Data[0].Result == 'True') {
                alert('Correo enviado exitosamente');
                window.location.reload();
            } else {
                NotificacionPopup('error', 'Error de Aplicacion', Data[0].Msn);
            }
        });
    });
</script>