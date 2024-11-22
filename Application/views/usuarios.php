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
                            <td><strong>Nombre</strong></td>
                            <td><strong>Rol</strong></td>
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
                            <td><strong>Estatus</strong></td>
                            <td><strong></strong></td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td>
                                <select class="form-control select2" id="catMisas" name="catMisas">
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                            <td>
                                <!--<button type="button" class="btn btn-danger" id="btnDownload">Descargar</button>-->
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
                        <input type="text" class="form-control" id="editPersona" name="editPersona">
                        <input type="hidden" class="form-control" id="editId" name="editId">
                        <input type="hidden" class="form-control" id="editPhone" name="editPhone">
                        <input type="hidden" class="form-control" id="editDonation" name="editDonation">
                        <input type="hidden" class="form-control" id="editRate" name="editRate">
                        <input type="hidden" class="form-control" id="editStatus" name="editStatus">
                    </div>
                    
                    <div class="form-group">
                        <label for="editDate">Tipo de intención</label>
                        <select class="form-control select2" id="editCatIntents" name="editCatIntents">
                            <option value="">Seleccionar</option>
                        </select>
                    </div>
                    
                    <div class="form-group">
                        <label for="editDate">Fecha intención</label>
                        <input type="date" class="form-control" id="editDate" name="editDate">
                    </div>  
                    <div class="form-group">
                        <label for="editDate">Hora</label>
                        <select class="form-control select2" id="editCatMisas" name="editCatMisas"> 
                            <option value="">Seleccionar</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="editPersona">Solicitante</label>
                        <input type="text" class="form-control" id="editSolicitante" name="editSolicitante">
                    </div>
                    <div class="form-group">
                        <label for="editIntent">Descripción</label>
                        <textarea class="form-control" id="editDescription" name="editDescription" rows="2"></textarea>
                    </div>
                    <!-- Agrega más campos según sea necesario -->
                </form>
            </div>
            <div class="modal-footer">
                
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

    $('#editModal').modal('hide');
    $(document).on('click', '.close', function () {
        $('#editModal').modal('hide');
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
            location.reload();
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
    $('#editDate').on('change', function() {
        const selectedDate = $(this).val();
        if (selectedDate) {
            loadEditIntentsAndMisas(selectedDate);
        }
    });

    $(document).on('click', '.btnEdit', function(event) {
        event.preventDefault();
        const id = $(this).data('id'); // ID del registro

        // Mostrar el modal
        $('#editModal').modal('show');

        // Obtener los datos del registro
        $.ajax({
            url: `api/intents/getMisaIntent.php?idIntent=${id}`,
            type: 'GET',
            dataType: 'json',
            success: function(data) {
                if (data.length > 0) {
                    const intentData = data[0]; // Primer resultado de la respuesta

                    // Asignar valores al formulario
                    $('#editId').val(intentData.id || '');
                    $('#editPhone').val(intentData.phone || '');
                    $('#editDonation').val(intentData.donation || '');
                    $('#editRate').val(intentData.exchange_rate || '');
                    $('#editStatus').val(intentData.status_id || '');
                    $('#editPersona').val(intentData.mention_person || '');
                    $('#editDate').val(intentData.date.split('T')[0] || '');
                    $('#editSolicitante').val(intentData.applicant || '');
                    $('#editDescription').val(intentData.decription || '');

                    // Cargar las opciones de intenciones y misas con misa_id
                    loadEditIntentsAndMisas(intentData.date.split('T')[0], intentData.misa_id);
                    setTimeout(() => {
                        $('#editCatIntents').val(intentData.intent_id || '').trigger('change');
                    }, 300);
                } else {
                    console.error('No se encontraron datos para el ID proporcionado');
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se encontró información para este ID.',
                    });
                }
            },
            error: function(error) {
                console.error("Error al obtener los datos del registro:", error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se pudo cargar la información. Inténtalo más tarde.',
                });
            }
        });
    });
   
    $('#btnDownload').on('click', function () {
        const dateReq = $('#dateReq').val();

        const timeSelectText = $('#catMisas option:selected').text(); // Obtén el texto seleccionado
        const misaReq = (timeSelectText && timeSelectText !== "Seleccionar") ? $('#catMisas option:selected').text(): ""; // Envía vacío si es "Seleccionar"


        if (!dateReq) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Por favor selecciona una fecha para descargar el PDF.',
            });
            return;
        }

        $.ajax({
            url: 'views/intents/intentsTemplate.php',
            type: 'POST',
            data: { dateReq: dateReq, misaReq:misaReq},
            beforeSend: function() {
                Swal.fire({
                    title: 'Procesando solicitud...',
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading(); // Mostrar el loading
                    }
                });
            },
            xhrFields: {
                responseType: 'blob', // Importante para manejar archivos
            },
            success: function (data) {
                Swal.close(); // Cierra el Swal al finalizar la solicitud exitosamente
                const blob = new Blob([data], { type: 'application/pdf' });
                const link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = `intenciones_${dateReq}.pdf`;
                link.click();
                window.URL.revokeObjectURL(link.href);
            },
            error: function () {
                Swal.close(); // Asegúrate de cerrar el Swal también en errores
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Hubo un problema al generar el PDF. Inténtalo de nuevo.',
                });
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
            "url": "api/intents/misaIntentsFilter.php",
            "type": "GET",
            "data": function(d) {
                // Obtener la fecha seleccionada, o usar la fecha de hoy si no hay selección
                d.dateReq = $('#dateReq').val() || (() => {
                    const today = new Date();
                    today.setMinutes(today.getMinutes() - today.getTimezoneOffset()); // Ajuste por zona horaria
                    return today.toISOString().split('T')[0];
                })();
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

    // Ajustar el desfase de zona horaria
    today.setMinutes(today.getMinutes() - today.getTimezoneOffset());

    // Formatear la fecha al formato requerido por <input type="date">
    const todayDate = today.toISOString().split('T')[0];

    // Establecer la fecha en los inputs
    $('#dateReq').val(todayDate);
    $('#createDateReq').val(todayDate);

    // Calcular el día de la semana y cargar las horas
    const dayOfWeek = today.getDay() + 1; // getDay() devuelve 0 para Domingo
    loadMisasForDay(dayOfWeek); 
    loadMisasForDayCreate(dayOfWeek);
   
  
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

function loadEditIntentsAndMisas(selectedDate, misaId) {
    const dayOfWeek = new Date(selectedDate).getDay() + 1; // Día de la semana basado en la fecha

    // Cargar las opciones de misas basadas en el día de la semana
    $.ajax({
        url: 'api/general/misas.php',
        type: 'GET',
        dataType: 'json',
        data: { day_id: dayOfWeek },
        success: function(data) {
            $('#editCatMisas').empty().append('<option value="">Seleccionar</option>');
            if (Array.isArray(data)) {
                // Agregar las opciones al select
                $.each(data, function(index, item) {
                    $('#editCatMisas').append(new Option(item.hour, item.id));
                });

                // Seleccionar el valor del misa_id si está definido
                if (misaId) {
                    $('#editCatMisas').val(misaId).trigger('change');
                }
            }
        },
        error: function(error) {
            console.error("Error cargando horas de misas:", error);
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
                $('#editCatIntents').append(new Option(item.intent, item.id));
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
        // Asume que response es un objeto JSON y contiene un campo "message"
        const intentId = response.message; // Obtén el valor de "message" como el ID

        // Verifica si se obtuvo correctamente el ID
        if (!intentId) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo obtener el ID de la intención. Por favor, inténtalo de nuevo.',
            });
            return;
        }

        // Crear un nuevo FormData para la segunda solicitud
        const pdfData = new FormData();
        pdfData.append('id', intentId); // Enviar el ID como "id"

        // Segunda solicitud AJAX para generar y descargar el PDF
        $.ajax({
            url: 'views/intents/intentTemplate.php',
            type: 'POST',
            data: pdfData,  
            contentType: false,
            processData: false,
            beforeSend: function() {
                Swal.fire({
                    title: 'Generando PDF...',
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            xhrFields: {
                responseType: 'blob', // Importante para manejar archivos
            },
            success: function (data) {
                const blob = new Blob([data], { type: 'application/pdf' });
                const link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = `intencion_${intentId}.pdf`; // Nombre del archivo con el ID
                link.click();
                window.URL.revokeObjectURL(link.href);

                // Muestra el mensaje de éxito después de generar el PDF
                Swal.fire({
                    icon: 'success',
                    title: 'PDF Generado',
                    text: 'El recibo ha sido generado correctamente. Puedes encontrarlo en tu carpeta de descargas.',
                    allowOutsideClick: true // El usuario debe cerrar manualmente
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Redirigir a otra página o realizar otra acción
                        window.location.href = 'intenciones';
                    }
                });
            },
            error: function () {
                Swal.close();
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Hubo un problema al generar el PDF. Inténtalo de nuevo.',
                });
            }
        });

    },
    error: function(jqXHR, textStatus, errorThrown) {
            Swal.fire({
                icon: 'error',
                title: 'Error al crear la intención',
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

