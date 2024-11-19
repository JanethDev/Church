<?php
date_default_timezone_set('America/Tijuana');
?>
<style>
    .hidden {
        display: none;
    }
</style>
<div class="card">
    <div class="card-header card-header-green">
        <h2>Intenciones</h2>
    </div>
    <div class="card-body">
        <div style="text-align: right;">
            <!-- Un solo botón que cambia entre "Agregar" y "Volver" -->
            <button type="button" class="btn btn-info" id="btnToggleIntention" style="margin-left:5px;">Agregar</button>
        </div>

        <!-- Lista de intenciones -->
        <div id="IntentsList" class="visible">
            <div class="table-responsive">
                <form id="PurchaseRequestCreateForm" enctype="multipart/form-data">
                    <table class="table" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td><strong>Motivo</strong></td>
                            <td><strong>Fecha de intención</strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td> 
                                <select class="form-control select2" id="catIntents" name="catIntents">
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                            <td><input type="date" class="form-control control-customer-new" id="dateReq" name="dateReq" /></td>
                        </tr>
                        <tr>
                            <td><strong>Hora</strong></td>
                            <td><strong></strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td>
                                <select class="form-control select2" id="catMisas" name="catMisas">
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                            <td>
                                <button type="button" class="btn btn-danger" id="btnDownload">Descargar</button>
                                <button type="button" class="btn btn-primary" id="btnClear">Limpiar</button>
                                <button type="button" class="btn btn-warning" id="btnSearch">Buscar</button>
                            </td>
                        </tr>
                    </table><br>
                    <table class="table table-striped table-bordered" id="intentsList">
                        <thead>
                            <tr>
                                <th>Persona a mencionar</th>
                                <th>Tipo</th>
                                <th>Fecha solicitud</th>
                                <th>Hora</th>
                                <th>Fecha intención</th>
                                <th></th>
                            </tr>
                        </thead>
                    </table>
                </form>
            </div>
        </div>

        <!-- Formulario de creación de intenciones -->
        <div id="IntentsCreate" class="hidden">
            <div class="table-responsive">
                <h5>Nueva Intención</h5>
                <form id="IntentionCreateForm" enctype="multipart/form-data">
                    <table class="table" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td><strong>Tipo de intención *</strong></td>
                            <td><strong>Fecha de intención *</strong></td>
                            <td><strong>Hora *</strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td> 
                                <select class="form-control select2" id="createCatIntents" name="createCatIntents">
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                            <td><input type="date" class="form-control control-customer-new" id="createDateReq" name="createDateReq" /></td>
                            <td> 
                                <select class="form-control select2" id="createCatMisas" name="createCatMisas"> 
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td><strong>Persona mención *</strong></td>
                            <td><strong>Solicitante *</strong></td>
                            <td><strong>Teléfono *</strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td><input type="text" class="form-control control-customer-new" id="persona" name="persona" /></td>
                            <td><input type="text" class="form-control control-customer-new" id="solicitante" name="solicitante" /></td>
                            <td><input type="text" class="form-control phone control-customer" id="CelPhone" name="CelPhone" /></td>
                        </tr>
                        <tr>
                            <td><strong>Donativo *</strong></td>
                            <td colspan="2"><strong>Descripción </strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td><input type="number" class="form-control control-customer-new" id="donativo" name="donativo" step="0.01" min="0"/></td>
                            <td colspan="2">
                                <textarea class="form-control" id="Description" name="Description" rows="2"></textarea>
                            </td>
                        </tr>
                        <tr >
                            <td colspan="3" style="text-align: end;"><button type="button" class="btn btn-primary" id="btnSave">Guardar</button></td>
                           
                        </tr>
                    </table><br>
                </form>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="editModalLabel">Editar Intención</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form id="editForm">
                    <div class="form-group">
                        <label for="editPersona">Persona a mencionar</label>
                        <input type="text" class="form-control" id="editPersona" name="persona">
                    </div>
                    <div class="form-group">
                        <label for="editIntent">Tipo</label>
                        <input type="text" class="form-control" id="editIntent" name="intent">
                    </div>
                    <div class="form-group">
                        <label for="editDate">Fecha intención</label>
                        <input type="date" class="form-control" id="editDate" name="date">
                    </div>
                    <!-- Agrega más campos según sea necesario -->
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                <button type="button" class="btn btn-primary" id="btnUpdate">Guardar Cambios</button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
$(document).ready(function() {

    // Inicializar DataTables
    if (typeof $.fn.DataTable !== 'undefined') {
        initializeDataTables();
    } else {
        console.error('Error: DataTables no está cargado');
    }

    document.getElementById('donativo').addEventListener('blur', function () {
    // Convierte el valor a número y lo formatea con dos decimales
        const value = parseFloat(this.value).toFixed(2);
        // Asigna el valor formateado al input
        this.value = value;
    });

    // Cambiar entre "Agregar" y "Volver" al hacer clic en el botón
    $('#btnToggleIntention').on('click', function() {
        if ($('#IntentsList').is(':visible')) {
            // Si la lista está visible, mostrar el formulario de creación
            $('#IntentsList').hide();
            $('#IntentsCreate').show();
            $(this).text('Volver');
        } else {
            // Si el formulario de creación está visible, volver a la lista
            $('#IntentsCreate').hide();
            $('#IntentsList').show();
            $(this).text('Agregar');
        }
    });

    // Ejecutar la búsqueda al hacer clic en el botón "Buscar"
    $('#btnSearch').on('click', function() {
        $('#intentsList').DataTable().ajax.reload();
    });

    // Evento para el botón "Limpiar"
    $('#btnClear').on('click', function() {
        // Restablecer la categoría
        $('#catIntents').val('').trigger('change');

        // Restablecer la fecha a la fecha actual
        setTodayDate();

        // Recargar la tabla con los filtros restablecidos
        $('#intentsList').DataTable().ajax.reload();
    });

    // Escuchar cambios en el campo de fecha y actualizar el select de horas
    $('#dateReq').on('change', function() {
        const selectedDate = new Date($('#dateReq').val());
        const dayOfWeek = selectedDate.getDay() + 1; // getDay() devuelve 0 para Domingo, +1 para que sea 1 (Lunes) hasta 7 (Domingo)
        loadMisasForDay(dayOfWeek); // Cargar las horas para el día seleccionado
    });

    // Escuchar cambios en el campo de fecha y actualizar el select de horas
    $('#createDateReq').on('change', function() {
        const selectedDate = new Date($('#createDateReq').val());
        const dayOfWeek = selectedDate.getDay() + 1; // getDay() devuelve 0 para Domingo, +1 para que sea 1 (Lunes) hasta 7 (Domingo)
        loadMisasForDayCreate(dayOfWeek); // Cargar las horas para el día seleccionado
    });

    // Establecer la fecha de hoy y cargar horas al cargar la página
    setTodayDate();

    function applyPhoneMask() {
        $('.phone').inputmask("(999) 999-9999"); // Aplica la máscara de teléfono
    }

    // Llama a la función para aplicar la máscara de teléfono en todos los campos de teléfono al cargar la página
    applyPhoneMask();

    $(document).on('click', '.btnEdit', function(event) {
        event.preventDefault(); // Previene la recarga de la página
        const id = $(this).data('id'); // Obtener el ID del registro

        // Muestra el modal
        $('#editModal').modal('show');

        // Cargar los datos en el modal (puedes hacer una solicitud AJAX si es necesario)
        $.ajax({
            url: `api/intents/getMisaIntent.php?id=${id}`,
            type: 'GET',
            success: function(data) {
                // Suponiendo que data contiene los datos del registro
                $('#editPersona').val(data.mention_person);
                $('#editIntent').val(data.intent);
                $('#editDate').val(data.date);
                // Completa con otros campos según corresponda
            },
            error: function(error) {
                console.error("Error al obtener los datos del registro:", error);
            }
        });
    });

});

function initializeDataTables() {
    $('#intentsList').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "api/intents/misaIntents.php",
            "type": "GET",
            "data": function(d) {
                // Obtener la fecha seleccionada, o usar la fecha de hoy si no hay selección
                d.dateReq = $('#dateReq').val() || new Date().toISOString().split('T')[0];
                
                // Obtener la categoría seleccionada
                d.catIntents = $('#catIntents').val();

                // Verificar si hay una hora seleccionada distinta de "Seleccionar"
                const timeSelectText = $('#catMisas option:selected').text();
                if (timeSelectText && timeSelectText !== "Seleccionar") {
                    d.timeSelect = timeSelectText; // Enviar el texto de la hora seleccionada
                }
            },
            "dataSrc": function(json) {
                if (!json.data || json.data.length === 0) {
                    return []; // Si no hay datos, devuelve un array vacío
                }
                return json.data;
            }
        },
        "columns": [
            { "data": "mention_person" },
            { "data": "intent" },
            { 
                "data": "date",
                "render": function(data, type, row) {
                    if (data) {
                        const dateObj = new Date(data);
                        const day = String(dateObj.getDate()).padStart(2, '0');
                        const month = String(dateObj.getMonth() + 1).padStart(2, '0');
                        const year = dateObj.getFullYear();
                        return `${day}/${month}/${year}`;
                    }
                    return data;
                }
            },
            { "data": "misa_hour" },
            { 
                "data": "date",
                "render": function(data, type, row) {
                    if (data) {
                        const dateObj = new Date(data);
                        const day = String(dateObj.getDate()).padStart(2, '0');
                        const month = String(dateObj.getMonth() + 1).padStart(2, '0');
                        const year = dateObj.getFullYear();
                        return `${day}/${month}/${year}`;
                    }
                    return data;
                }
            },
            { // Nueva columna
                "data": null,
                "render": function(data, type, row) {
                    return `<button class="btn btn-info btnEdit" data-id="${row.id}"><i class="fas fa-edit"></i></button>`;
                },
                "orderable": false
            }
        ],
        "language": {
            "emptyTable": "No hay registros disponibles"
        }
    });
}


function setTodayDate() {
    const today = new Date();
    const day = String(today.getDate()).padStart(2, '0');
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const year = today.getFullYear();
    const todayDate = `${year}-${month}-${day}`; // Formato compatible con <input type="date">
    $('#dateReq').val(todayDate);
    $('#createDateReq').val(todayDate);

    // Obtener el día de la semana y cargar las horas correspondientes
    const dayOfWeek = today.getDay() + 1; // getDay() devuelve 0 para Domingo, +1 para que sea 1 (Lunes) hasta 7 (Domingo)
    loadMisasForDay(dayOfWeek); // Cargar las horas para el día de hoy
    loadMisasForDayCreate(dayOfWeek); // Cargar las horas para el día de hoy
}

function loadMisasForDay(dayOfWeek) {
    $.ajax({
        url: 'api/general/misas.php', // URL que debe recibir el día y devolver las horas correspondientes
        type: 'GET',
        dataType: 'json',
        data: { day_id: dayOfWeek }, // Enviar el día como parámetro
        success: function(data) {
           
            // Limpiar el select de horas antes de llenarlo
            $('#catMisas').empty().append('<option value="">Seleccionar</option>');

            // Llenar el select con las horas correspondientes
            if (Array.isArray(data)) {
                $.each(data, function(index, item) {
                    $('#catMisas').append(new Option(item.hour, item.id));
                });
            } else {
                console.error("Error en la respuesta de la API: ", data.error);
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.error("Error en la solicitud: ", textStatus);
        }
    });
}

function loadMisasForDayCreate(dayOfWeek) {
    $.ajax({
        url: 'api/general/misas.php', // URL que debe recibir el día y devolver las horas correspondientes
        type: 'GET',
        dataType: 'json',
        data: { day_id: dayOfWeek }, // Enviar el día como parámetro
        success: function(data) {
           
            // Limpiar el select de horas antes de llenarlo
            $('#createCatMisas').empty().append('<option value="">Seleccionar</option>');

            // Llenar el select con las horas correspondientes
            if (Array.isArray(data)) {
                $.each(data, function(index, item) {
                    $('#createCatMisas').append(new Option(item.hour, item.id));
                });
            } else {
                console.error("Error en la respuesta de la API: ", data.error);
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.error("Error en la solicitud: ", textStatus);
        }
    });
}


$.ajax({
    url: 'api/general/intents.php', 
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        // Verificar si data es una cadena y convertirla en JSON si es necesario
        if (typeof data === "string") {
            data = JSON.parse(data);
        }

        if (Array.isArray(data)) {
            // Llenar el select de motivos
            $.each(data, function(index, item) {
                $('#catIntents').append(new Option(item.intent, item.id));
            });
        } else {
            console.error("Error en la respuesta de la API: ", data.error);
        }
    },
    error: function(jqXHR, textStatus, errorThrown) {
        console.error("Error en la solicitud: ", textStatus);
    }

});

$.ajax({
    url: 'api/general/intents.php', 
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        // Verificar si data es una cadena y convertirla en JSON si es necesario
        if (typeof data === "string") {
            data = JSON.parse(data);
        }

        if (Array.isArray(data)) {
            // Llenar el select de motivos
            $.each(data, function(index, item) {
                $('#createCatIntents').append(new Option(item.intent, item.id));
            });
        } else {
            console.error("Error en la respuesta de la API: ", data.error);
        }
    },
    error: function(jqXHR, textStatus, errorThrown) {
        console.error("Error en la solicitud: ", textStatus);
    }

});



$('#btnSave').on('click', function() {
        // Variables para verificar campos
        const missingFields = []; // Array para almacenar campos faltantes
        const createCatIntents = $('#createCatIntents').val();
        const createDateReq = $('#createDateReq').val();
        const createCatMisas = $('#createCatMisas').val();
        const persona = $('#persona').val();
        const solicitante = $('#solicitante').val();
        const CelPhone = $('#CelPhone').val();
        const donativo = $('#donativo').val();
        const Description = $('#Description').val();

        if (!createCatIntents) missingFields.push("Tipo de intención*");
        if (!createDateReq) missingFields.push("Fecha de intención*");
        if (!createCatMisas) missingFields.push("Hora*");
        if (!persona) missingFields.push("Persona mención*");
        if (!solicitante) missingFields.push("Solicitante*");
        if (!CelPhone) missingFields.push("Teléfono *");
        if (!donativo) missingFields.push("Donativo*");
        

        // Si hay campos faltantes, mostrar SweetAlert
        if (missingFields.length > 0) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Faltan los siguientes campos: ' + missingFields.join(', '),
            });
            return; // Detiene la ejecución si hay campos faltantes
        }

        // Serializa los datos del formulario
        var formData = new FormData($('#IntentionCreateForm')[0]);

        $.ajax({
            url: 'api/intents/createMisaIntent.php',
            type: 'POST',
            data: formData,  
            contentType: false,
            processData: false,
            beforeSend: function() {
                Swal.fire({
                    title: 'Procesando solicitud...',
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: function(response) {
                
                Swal.fire({
                    icon: 'success',
                    title: 'Documento Generado',
                    text: 'El PDF se ha descargado correctamente.'
                });
                    
            },
            error: function(jqXHR, textStatus, errorThrown) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al crear la intencion',
                    text: 'Por favor contacte a soporte: ' + textStatus,
                });
            }
        });
        
        
    });
    $('#btnUpdate').on('click', function() {
    const formData = $('#editForm').serialize();
    $.ajax({
        url: 'api/intents/updateMisaIntent.php',
        type: 'POST',
        data: formData,
        success: function(response) {
            $('#editModal').modal('hide');
            $('#intentsList').DataTable().ajax.reload(); // Recargar la tabla
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: 'Los cambios se han guardado correctamente.'
            });
        },
        error: function(error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudieron guardar los cambios. Inténtalo de nuevo.'
            });
        }
    });
});
</script>

