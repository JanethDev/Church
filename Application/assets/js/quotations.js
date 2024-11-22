$(document).ready(function() {
    // Verificar si DataTables está cargado antes de inicializar las tablas
    if (typeof $.fn.DataTable !== 'undefined') {
        initializeDataTables();
    } else {
        console.error('Error: DataTables no está cargado');
    }

    // Evento de clic para el botón "Ver" en la tabla
    $(document).on('click', '.btn-ver', function(event) {
        event.preventDefault(); // Evita que el formulario se envíe o la página se recargue

        const purchaseId = $(this).data('id');
        const customerId = $(this).data('customer-id');
        
        $.ajax({
            type: "POST",
            url: "views/purchases/purchaseRequest.php",
            data: { purchaseId: purchaseId, customerId: customerId },
            success: function(response) {
                $('#page-content').html(response);
            },
            error: function(xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al cargar los datos. Intenta de nuevo o contacta a soporte.',
                });
            }
        });
    });
});

function formatDate(dateString) {
    const date = new Date(dateString);
    if (isNaN(date)) return 'N/A'; // Si la fecha no es válida, mostrar "N/A"
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Los meses son 0-indexados
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
// Función para inicializar DataTables en las tablas correspondientes
function initializeDataTables() {
    $('#quotationsTable').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "api/purchases/purchaseReserved.php",
            "type": "GET",
            "dataSrc": function(json) {
                return json.data.filter(record => record.customerNumber !== 0);
            }
        },
        "columns": [
            { "data": "purchaseId" },
            { "data": "fullPosition" },
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            {
                "data": "datePurchase",
                "render": function(data, type, row) {
                    return formatDate(data); // Formatear la fecha
                }
            },
            { "data": "customerNumber" },
            { "data": "customerName" },
            {
                "data": null,
                "render": function(data, type, row) {
                    return `<button class='btn btn-default btn-ver' data-id='${row.purchaseId}' data-customer-id='${row.customerId}'>Ver</button>`;
                }
            }
        ]
    });

    $('#quotationsProspectsTable').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "api/purchases/purchaseReserved.php",
            "type": "GET",
            "dataSrc": function(json) {
                return json.data.filter(record => record.customerNumber == 0);
            }
        },
        "columns": [
            { "data": "purchaseId" },
            { "data": "fullPosition" },
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            {
                "data": "datePurchase",
                "render": function(data, type, row) {
                    return formatDate(data); // Formatear la fecha
                }
            },
            { "data": "customerNumber" },
            { "data": "customerName" },
            {
                "data": null,
                "render": function(data, type, row) {
                    return `<button class='btn btn-default btn-ver' data-id='${row.purchaseId}' data-customer-id='${row.customerId}'>Ver</button>`;
                }
            }
        ]
    });
}
