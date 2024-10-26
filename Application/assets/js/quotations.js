$(document).ready(function() {
    const table = $('#quotationsTable').DataTable({
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "ajax": {
            "url": "../../api/purchases/purchaseReserved.php",
            "type": "GET",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "purchaseId" },
            { 
                "data": "cryptId",
                "render": function(data, type, row) {
                    return data ? `<span class="crypt-data" data-id="${data}">Cargando...</span>` : 'N/A';
                }
            },
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            { "data": "datePurchase" },
            { "data": "customerNumber" },
            { "data": "customerName" },
            { "data": null, "defaultContent": "<button class='btn btn-default'>Ver</button>" }
        ]
    });

    function updateCryptData(cryptId, spanElement) {
        $.ajax({
            url: `../../api/crypts/byid.php?id=${cryptId}`,
            type: "GET",
            dataType: "json",
            success: function(response) {
                if (response && response[0]) {
                    const cryptData = response[0];

                    // Actualizar directamente en el DataTable
                    const rowData = table.row(spanElement.closest('tr')).data();
                    rowData.cryptId = cryptData.full_position || "No encontrado";  // Clave cripta
                    rowData.zone = cryptData.zone || "N/A";                      // Zona
                    rowData.aisle = cryptData.aisle || "N/A";                    // Área
                    table.row(spanElement.closest('tr')).data(rowData).invalidate(); // Forzar actualización de la fila
                } else {
                    spanElement.text("No encontrado");
                }
            },
            error: function() {
                spanElement.text("Error al cargar");
            }
        });
    }

    // Llamar a la función `updateCryptData` para cada `cryptId` una sola vez
    table.on('draw', function() {
        $('#quotationsTable .crypt-data').each(function() {
            const cryptId = $(this).data('id');
            if (cryptId && $(this).text() === "Cargando...") {
                updateCryptData(cryptId, $(this));
            }
        });
    });
});