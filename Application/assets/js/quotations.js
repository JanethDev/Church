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
            { "data": "fullPosition"},
            { "data": "zone", "defaultContent": "N/A" },
            { "data": "aisle", "defaultContent": "N/A" },
            { "data": "datePurchase" },
            { "data": "customerNumber" },
            { "data": "customerName" },
            { "data": null, "defaultContent": "<button class='btn btn-default'>Ver</button>" }
        ]
    });

});