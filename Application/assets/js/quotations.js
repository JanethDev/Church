$(document).ready(function() {
    // Configuración de DataTable para 'quotationsTable'
    $('#quotationsTable').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "../../api/purchases/purchaseReserved.php",
            "type": "GET",
            "dataSrc": function(json) {
                return json.data.filter(function(record) {
                    return record.customerNumber !== 0;
                });
            }
        },
        "columns": [
            { "data": "purchaseId" },
            { "data": "fullPosition" },
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            { "data": "datePurchase" },
            { "data": "customerNumber" },
            { "data": "customerName" },
            {
                "data": null,
                "render": function(data, type, row) {
                    return `<button class='btn btn-default btn-ver' data-id='${row.purchaseId}'>Ver</button>`;
                }
            }
        ]
    });

    // Configuración de DataTable para 'quotationsProspectsTable'
    $('#quotationsProspectsTable').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "../../api/purchases/purchaseReserved.php",
            "type": "GET",
            "dataSrc": function(json) {
                return json.data.filter(function(record) {
                    return record.customerNumber === 0;
                });
            }
        },
        "columns": [
            { "data": "purchaseId" },
            { "data": "fullPosition" },
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            { "data": "datePurchase" },
            { "data": "customerNumber" },
            { "data": "customerName" },
            {
                "data": null,
                "render": function(data, type, row) {
                    return `<button class='btn btn-default btn-ver' data-id='${row.purchaseId}'>Ver</button>`;
                }
            }
        ]
    });

    // Redirigir al usuario al hacer clic en el botón "Ver"
    $('#quotationsTable, #quotationsProspectsTable').on('click', '.btn-ver', function() {
        const purchaseId = $(this).data('id');
        
        // Redirigir a la URL con el `purchaseId`
        window.location.href = `views/purchases/purchaseRequest.php?purchaseId=${purchaseId}`;
    });
});
