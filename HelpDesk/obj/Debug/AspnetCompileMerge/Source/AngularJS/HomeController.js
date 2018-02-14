var HelpDesk = angular.module('HelpDesk', ['ngRoute']);

sessionStorage.setItem('JSonTicketNuevo', '');
HelpDesk.constant('configuracionGlobal', {
    appName: 'HelpDesk',
    appVersion: '2.0'
});
//Inicio de Sesion
HelpDesk.controller('IniciarSesion', function ($scope,$http) {
    try {
        $scope.usuario = 'bllanos';
        $scope.clave = 'Pa$$w0rd';
        $scope.ValidarSesion = function () {
            if ($scope.usuario == undefined) {
                NotificacionPopup('error', 'Error de Validación', 'Debe digitar el usuario');
                return;
            }
            if ($scope.clave == undefined) {
                NotificacionPopup('error', 'Error de Validación', 'Debe digitar la clave');
                return;
            }
            NotificacionPopup('info', 'Validando Usuario', 'Espere por favor...');
            $http.get("Home/LoguearUsuario", {
                params: {
                    'username': $scope.usuario,
                    'password':$scope.clave
                }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    window.location.href = '/Home/Dashboard';
                } else {
                    NotificacionPopup('error', 'Error de Validación', 'Usuario o Clave incorrecta');
                }
            });
        }
    } catch (err) {
        console.log(err);
    }
});

HelpDesk.controller('TicketNuevo', function ($scope, $http, $timeout, $interval) {
    try {
        $scope.Cargando = false;
        //Funcion para guardar Tickets
        $scope.GuardarTicket = function () {
            $scope.Cargando = true;
            var TipoSolicitud = $("#TipoSolicitud").val();
            var TipoTicket = String($("#TipoTicket").val()).trim();
            var TipoSoporte = String($("#TipoSoporte").val()).trim();
            var Necesidad = String($("#Necesidad").val()).trim();
            var Justificacion = String($("#Justificacion").val()).trim();
            var Archivo = String($("#RutaArchivo").val()).trim();
            if (TipoSolicitud == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar un tipo de solicitud');
                $scope.Cargando = false;
                return;
            }
            if (TipoTicket == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar un tipo de ticket');
                $scope.Cargando = false;
                return;
            }
            if (TipoSoporte == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar un tipo de soporte');
                $scope.Cargando = false;
                return;
            }
            if (Necesidad == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe digitar una necesidad');
                $scope.Cargando = false;
                return;
            }
            if (Justificacion == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe digitar una justificación');
                $scope.Cargando = false;
                return;
            }
            $http({
                method: 'POST',
                url: 'GuardarTicket',
                data: $.param({
                    'TipoSolicitud' :TipoSolicitud,
                    'TipoSoporte': TipoSoporte,
                    'Necesidad': Necesidad,
                    'Justificacion':Justificacion,
                    'Archivo':Archivo
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    NotificacionPopup('success', 'Confirmación', 'Guardado Exitosamente');
                    LimpiarNuevoTicket();
                    $("#dNTicket").css('display', '');
                    $("#TicketAsignado").html(data[0].Msn);
                    $("#NuevoTicket").click();
                    $scope.Tickets_Dashboard.Consultar_TicketsIngresados();
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
                
            })
           .finally(function () {
               $scope.Cargando = false;
           });
        }
    } catch (err) {
        console.log(err.message);
    }
});

HelpDesk.controller('Consultar_TicketsNuevos', function ($scope, $http, $timeout, $interval) {
    try {
       //Funcion para Consultar Tickets
        $scope.ServicioConsulta = function () {
            $scope.Cargando = true;
            var SessionUsuario = String(sessionStorage.getItem("SessionUsuario")).trim();
            $http.get("Consultar_TicketNuevoSinAsignar?Sesion=" + SessionUsuario).success(function (RespuestaJson) {
                $scope.DataTable = JSON.parse(RespuestaJson);
                var JSonTicketNuevo = sessionStorage.getItem("JSonTicketNuevo");
                if (JSonTicketNuevo != RespuestaJson) {
                    sessionStorage.setItem('JSonTicketNuevo', RespuestaJson);
                    if (String(RespuestaJson).trim().length > String(JSonTicketNuevo).trim().length) {
                        NotificacionPush('Nuevo Ticket', 'Se ha ingresado un nuevo ticket, porfavor verifiquelo.', '/Home/TicketNuevo');
                    }                    
                }
            })
            .finally(function () {
                $timeout(function () { $scope.Cargando = false; }, 1000);
            });
        };
        $scope.asignarticket = function (CodTicket, NecesidadTicket, JustificacionTicket, TipoSoporte, TipoSolicitud, Adjunto, Responsable, TipoAccion) {
            $('#TicketSelecc').val(CodTicket);
            $('#NroTicket').html(CodTicket);
            $('#NecesidadTicket').html(NecesidadTicket);
            $('#JustificacionTicket').html(JustificacionTicket);
            $('#TipoSolicitud2 > option[value="' + TipoSolicitud + '"]').attr('selected', 'selected');
            if (Adjunto != "") {
                $('#DescAdjunto').html('<a href="/Home/DescargarAdjunto?Url=' + Adjunto + '" target="_blank" Class="btn btn-success waves-effect text-left" download>Descargar Adjunto</a>');
            }else{
                $('#DescAdjunto').html('');
            }
            $scope.ConsultaTipoSoportes2(TipoSoporte, TipoAccion, Responsable);
            document.getElementById('VerTicket').click();
        };
        $scope.ConsultaTipoSoportes2 = function (TipoSoporte, TipoAccion, Responsable) {
            $("#TipoAccion").val(TipoAccion);
            $scope.CargandoTicket = true;
            var IdPadre = "";
            $http({
                method: 'GET',
                url: 'Consulta_TipoSoportes_Id?idTipoSoporte=' + TipoSoporte,
                data: $.param({}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                IdPadre = data[0].Padre
                $("#SelTipoTicket").empty();
                $http.get("Consulta_TipoSoportes_Padre").success(function (RespuestaJson) {
                    $.each(JSON.parse(RespuestaJson), function (id, value) {
                        $("#SelTipoTicket").append('<option value="' + value.Padre + '">' + value.TipoTicket + '</option>');
                        $scope.CargandoTicket = false;
                    });
                    $('#SelTipoTicket > option[value="' + IdPadre + '"]').attr('selected', 'selected');
                    $("#SelTipoTicket_2").empty();
                    ResponsableTickets2(Responsable);
                    $http({
                        method: 'GET',
                        url: 'Consultar_TiposSoporte?TipoTicket=' + IdPadre,
                        data: $.param({}),
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                    }).success(function (data) {
                        $.each(JSON.parse(data), function (index, Data) {
                            $("#SelTipoTicket_2").append('<option value="' + Data.Id + '">' + Data.TipoSoporte + '</option>');
                        });
                        $('#SelTipoTicket_2 > option[value="' + TipoSoporte + '"]').attr('selected', 'selected');
                    });
                });
            });
        };
        $scope.ConsultaTipoSoportes = function () {
            $scope.CargandoTicket = true;
            $("#SelTipoTicket").empty();
            $http.get("Consulta_TipoSoportes_Padre").success(function (RespuestaJson) {              
                $.each(JSON.parse(RespuestaJson), function (id, value) {
                    $("#SelTipoTicket").append('<option value="' + value.Padre + '">' + value.TipoTicket + '</option>');
                    $scope.CargandoTicket = false;
                });
            });
        };
        $scope.ActualizarEtapa2 = function () {
            $("#TipoAccion").val(3);
            $scope.ActualizarEtapa();
        }
        $scope.ActualizarEtapa = function () {
            var TicketSelecc        = $("#TicketSelecc").val();
            var Op                  = $("#SelTipoTicket_2").val();
            var TipoSolicitud       = $("#TipoSolicitud2").val();
            var TipoAccion          = $("#TipoAccion").val();
            var Observacion         = '';
            var estado              = '';
            var Responsable         = '';
            var NivelResponsable    = '';
            if (TipoSolicitud == 1) {
                Responsable         = $("#IdResponsable").val();
                NivelResponsable    = $("#NivelResponsable").val();
            }
            if (TipoAccion == 1) {
                //Verificacion
                if (TipoSolicitud == 1) {
                    estado = '13'
                } else {
                    estado = '4'
                }                
            } else {
                if (TipoAccion == 2) {
                    //Rechazo (Reasignacion)
                    Observacion = $("#TextoRechazo").val();
                    estado = '5'
                } else {                    
                    if (TipoAccion == 3) {
                        //Solucionado
                        estado = '6'
                    } else { 
                        //Cancelacion
                        Observacion = $("#TextoCancelacion").val();
                        estado = '7'
                    }
                }
            }
            if (estado == '') {
                NotificacionPopup('error', 'Error de Validación', 'Error de aplicacion');
                return;
            }
            var pUrl = 'VerificacionDeTicket?Estado=' + estado + '&CodTicket=' + TicketSelecc + '&Op=' + Op + '&TipoSolicitud=' + TipoSolicitud + 
            '&TipoAccion=' + TipoAccion + '&Observacion=' + Observacion + '&Responsable=' + Responsable + '&NivelResponsable=' + NivelResponsable;
            $http({
                method: 'GET',
                url: pUrl,
                data: $.param({}),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    NotificacionPopup('success', 'Confirmación', 'Guardado Exitosamente');
                    $("#CerrarInfoTicket").click();
                } else {
                    NotificacionPopup('error', 'Error de Validación', data.Msn);
                }
            })
           .finally(function () {
               $scope.Cargando = false;
           });
        };
        Servicio = $interval(function () { $scope.ServicioConsulta(); }, 3000);
        $scope.ServicioConsulta();
        $scope.ConsultaTipoSoportes();
    } catch (err) {
        console.log(err.message);
    }
});

HelpDesk.controller('Agenda_Tickets', function ($scope, $http, $timeout, $interval) {
    $scope.CargandoTicket = false;
    try {
        $scope.ConsultarAreas = function () {
            $http({
                method: 'POST',
                url: 'Consultar_MtAreas',
                data: $.param({
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $scope.Datos = JSON.parse(data);
            })
           .finally(function () {
               
           });
        }
        $scope.ConsultarAreas();

        $scope.MostrarTickets = function (CodArea,NomArea) {
            $("#modal-title-add").html(NomArea);
            $("#modal-id").val(CodArea);
            $http.get("Consultar_TicketsxCitar", {
                params: {
                    'Area' : CodArea
                }
            }).success(function (data) {
                data = JSON.parse(data);
                $scope.Tickets = data;
            });
            $("#VerTicket").click();
        }

        $scope.GuardarCita = function () {
            var Cont = 0;
            var Cadena = '';
            var FechaCita = $("#FechaCita").val();
            var HoraCita = $("#HoraCita").val();
            var Area = $("#modal-id").val();

            if (FechaCita.trim() == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar una fecha de cita');
                return;
            }
            if (HoraCita.trim() == '') {
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar una hora de cita');
                return;
            }
            if (Area.trim() == '') {
                NotificacionPopup('error', 'Error de Validación', 'No se ha seleccionado Aréa');
                return;
            }
            $scope.CargandoTicket = true;
            $("#tTickets>tbody>tr>td").each(function () {
                if (Cont == 0) {
                    Cadena = Cadena + $(this).children().children()[0].checked + '~';
                    Cont += 1;
                } else {
                    if (Cont == 1) {
                        Cadena = Cadena + $(this).text() + '~';
                        Cont += 1;
                    } else {
                        if (Cont == 2) {
                            Cadena = Cadena + $(this).text() + '~';
                            Cont += 1;
                        } else {
                            if (Cont == 3) {
                                Cadena = Cadena + $(this).text() + '*';
                                Cont = 0;
                            }
                        }
                    }
                }
            });
            if(Cadena.indexOf("true") == -1){
                NotificacionPopup('error', 'Error de Validación', 'Debe seleccionar tickets a citar');
                $scope.CargandoTicket = false;
                return;
            }
            if (Cadena.split("*").length - 1 > 1) {
                Cadena = Cadena.slice(0, Cadena.length - 1);
            }
            $http({
                method: 'POST',
                url: 'Agendar_Cita',
                data: $.param({
                    "FechaCita": FechaCita,
                    "HoraCita": HoraCita,
                    "Cadena": Cadena,
                    "Area" : Area
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    $scope.ConsultarAreas();
                    $("#xCloseAdd").click();
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Reunión agendada exitosamente');
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
            })
            .finally(function () {
                $scope.CargandoTicket = false;
            });
        }

        $scope.EditarCita = function () {
            var Id = $("#Id_Editar").val();
            var Fecha = $("#FechaCita_Edit").val();
            var Hora = $("#HoraCita_Edit").val();
            $scope.CargandoTicket = true;
            $http({
                method: 'POST',
                url: 'Actualizar_Reunion',
                data: $.param({
                    "Fecha": Fecha,
                    "Hora": Hora,
                    "Id": Id
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    $scope.ConsultarAreas();
                    $("#xCloseEdit").click();
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Reunión actualizada exitosamente');
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
            })
           .finally(function () {
               $scope.CargandoTicket = false;
           });
        }
    } catch (err) {
        console.log(err.message);
    }
});

HelpDesk.controller('TicketsCitados', function ($scope, $http, $timeout, $interval) {
    try {
        $scope.CargandoTicket = false;
        $scope.ConsultarTicketsCitados = function () {
             $http({
                method: 'POST',
                url: 'Consulta_TicketsCitados',
                data: $.param({
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $scope.Tickets_Citados = JSON.parse(data);
            })
            .finally(function () {
                
            });
        }

        $scope.AsignarValTicket = function (CodTicket,Necesidad,Justificacion) {
            $("#mc_CodTicket").val(CodTicket);
            $("#xNecesidad").html(Necesidad);
            $("#xJustificacion").html(Justificacion);
            $("#xNroTicket").html(CodTicket);
        }

        $scope.ValidarAsistencia = function () {
            var Resp = $("#ConfAsistencia").val();
            var NroTicket = $("#mc_CodTicket").val();
            var Observacion = $("#xObservacionT").val();
            if (Resp == '1') {
                $("#xCloseM").click();
                $("#VerTicket").click();
            } else {
                if (Observacion.trim() == '') {
                    NotificacionPopup('error', 'Error de Validación', 'Debe digiar una observación');
                    return;
                }
                $http({
                    method: 'POST',
                    url: 'Guardar_NoAsistenciaTickets',
                    data: $.param({
                        'CodTicket': NroTicket,
                        'Observacion' : Observacion
                    }),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).success(function (data) {
                    data = JSON.parse(data);
                    if (data[0].Result == 'True') {
                        $scope.ConsultarTicketsCitados();
                        $("#xCloseM").click();
                        NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                    } else {
                        NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                    }
                })
           .finally(function () {

           });
            }
        }

        $scope.ResponsableTickets = function () {
            $http({
                method: 'POST',
                url: 'Consulta_ResponsableTickets',
                data: $.param({
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $scope.Responsables = JSON.parse(data);
                console.log(JSON.parse(data));
            })
        }

        $scope.ActualizarEvaluacion = function () {
            $scope.CargandoTicket = true;
            var NroTicket = $("#xNroTicket").text();
            var Viabilidad = $("#Viabilidad").val();
            var Responsable = $("#Recurso").val();
            var Prioridad = $("#Prioridad").val();
            var Duracion = $("#TiempoSolucion").val();
            var Observacion = $("#mObservacion").val();
            var Necesidad = $("#xNecesidad").html();
            $http({
                method: 'POST',
                url: 'Actualizar_EvaluacionTicket',
                data: $.param({
                    'CodTicket': NroTicket,
                    'Responsable': Responsable,
                    'Prioridad': Prioridad,
                    'Viabilidad': Viabilidad,
                    'Duracion': Duracion,
                    'Observacion': Observacion,
                    'Necesidad' :Necesidad
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    $scope.ConsultarTicketsCitados();
                    $("#CerrarInfoTicket").click();
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
            }).finally(function () {
                $scope.CargandoTicket = false;
            });
        }

        //Ejecución de Funciones
        $scope.ConsultarTicketsCitados();
        $scope.ResponsableTickets();
    } catch (err) {
        console.log(err.message);
    }
});

HelpDesk.controller('TicketsAsignados', function ($scope, $http, $timeout, $interval) {
    try {
        $scope.ConsultarTicketsAsignados = function () {
            $http({
                method: 'POST',
                url: 'Consulta_TicketsAsignados',
                data: $.param({
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $scope.Tickets_Asignados = JSON.parse(data);
            })
           .finally(function () {

           });
        }

        $scope.IniciarTicket = function (CodTicket,Tipo) {
            $http({
                method: 'POST',
                url: 'Actualizar_ColumnaTicket',
                data: $.param({
                    "Columna": Tipo,
                    "Valor" : "GETDATE()",
                    "CodTicket" : CodTicket
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
            })
           .finally(function () {
               $scope.ConsultarTicketsAsignados();
           });
        }

        $scope.Actualizar_TicketAsignado = function (CodTicket, Estado) {
            $http({
                method: 'POST',
                url: 'Actualizar_TicketsAsignados',
                data: $.param({
                    "CodTicket": CodTicket,
                    "Estado" : Estado
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                data = JSON.parse(data);
                if (data[0].Result == 'True') {
                    NotificacionPopup('success', 'Guardado Exitosamente', 'Información Guardada Exitosamente');
                } else {
                    NotificacionPopup('error', 'Error de Validación', data[0].Msn);
                }
            })
          .finally(function () {
              $scope.ConsultarTicketsAsignados();
          });
        }
        //Ejecución de Funciones
        $scope.ConsultarTicketsAsignados();
    } catch (err) {
        console.log(err);
    }
});

//Controlador de Dashboard
HelpDesk.controller('Tickets_Dashboard', function ($scope, $http, $timeout, $interval) {
    try {
        $scope.Consultar_TicketsIngresados = function () {
            $http({
                method: 'POST',
                url: 'Consulta_TicketsIngresados',
                data: $.param({
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $scope.Tickets_Ingresados = JSON.parse(data);
            })
           .finally(function () {

           });
        }

        $scope.VerTicket = function (CodTicket, Necesidad, Justificacion, Archivo) {
            $("#xAdjunto").empty();
            $("#xNroTicket").html(CodTicket);
            $("#xNecesidad").html(Necesidad);
            $("#xJustificacion").html(Justificacion);
            $("#xNecesidad").html(Necesidad);
            if (Archivo.trim() != '') {
                $("#xAdjunto").html('<a href="/Home/DescargarAdjunto?Url=' + Archivo + '" target="_blank" class="btn btn-success waves-effect text-left" download>Descargar Adjunto</a>');
            }
            $("#VerTicket").click();
        }

        //Ejecución de Funciones
        $scope.Consultar_TicketsIngresados();
    } catch (err) {
        console.log(err.message);
    }
});