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
        <div id="IntentionsList" class="visible">
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
                            </tr>
                        </thead>
                    </table>
                </form>
            </div>
        </div>

        <!-- Formulario de creación de intenciones -->
        <div id="IntentionsCreate" class="hidden">
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
                                <select class="form-control select2" id="createCatIntents" name="catIntents">
                                    <option value="">Seleccionar</option>
                                </select>
                            </td>
                            <td><input type="date" class="form-control control-customer-new" id="createDateReq" name="dateReq" /></td>
                            <td> 
                                <select class="form-control select2" id="createCatMisas" name="catMisas"> 
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
                            <td><input type="text" class="form-control control-customer-new" id="donativo" name="donativo" /></td>
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

<script type="text/javascript">
$(document).ready(function() {
    // Inicializar DataTables
    if (typeof $.fn.DataTable !== 'undefined') {
        initializeDataTables();
    } else {
        console.error('Error: DataTables no está cargado');
    }

    // Cambiar entre "Agregar" y "Volver" al hacer clic en el botón
    $('#btnToggleIntention').on('click', function() {
        if ($('#IntentionsList').is(':visible')) {
            // Si la lista está visible, mostrar el formulario de creación
            $('#IntentionsList').hide();
            $('#IntentionsCreate').show();
            $(this).text('Volver');
        } else {
            // Si el formulario de creación está visible, volver a la lista
            $('#IntentionsCreate').hide();
            $('#IntentionsList').show();
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

    // Establecer la fecha de hoy y cargar horas al cargar la página
    setTodayDate();

    function applyPhoneMask() {
        $('.phone').inputmask("(999) 999-9999"); // Aplica la máscara de teléfono
    }

    // Llama a la función para aplicar la máscara de teléfono en todos los campos de teléfono al cargar la página
    applyPhoneMask();
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

    // Obtener el día de la semana y cargar las horas correspondientes
    const dayOfWeek = today.getDay() + 1; // getDay() devuelve 0 para Domingo, +1 para que sea 1 (Lunes) hasta 7 (Domingo)
    console.log(`Día de la semana de hoy: ${dayOfWeek}`);
    loadMisasForDay(dayOfWeek); // Cargar las horas para el día de hoy
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


$('#btnSave').on('click', function() {
        // Variables para verificar campos
        const missingFields = []; // Array para almacenar campos faltantes
        const apellidoPaterno = $('#PSurname').val();
        const apellidoMaterno = $('#MSurname').val();
        const nombres = $('#Name').val();
        const telefonoParticular = $('#CelPhone').val();
        const correoElectronico = $('#Email').val();
        const customerId = $('#CustomerID').val();
        const address = $('#address').val();
        const house_number = $('#house_number').val();
        const neighborhood = $('#neighborhood').val();
        const catStates = $('#catStatesId').val();
        const catTowns = $('#catTownsId').val();
        const zip_code = $('#zip_code').val();
        const dateBirth = $('#DateOfBirth').val();


        // Si hay campos faltantes, mostrar SweetAlert
        if (missingFields.length > 0) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Faltan los siguientes campos: ' + missingFields.join(', '),
            });
            return; // Detiene la ejecución si hay campos faltantes
        }


        $.ajax({
            url: 'api/purchases/reservePurchase.php',
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
                    title: 'Error al apartar',
                    text: 'Error al realizar la reserva: ' + textStatus,
                });
            }
        });
        
        
    });
</script>

