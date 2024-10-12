<!-- Incluye el CSS de DataTables -->
<link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.min.css">

<!-- Incluye jQuery -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Incluye DataTables JS -->
<script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>

<div class="container">
    <h2>Solicitudes de Compra</h2>

    <!-- Tabla donde se mostrará la información -->
    <table id="purchaseTable" class="display" style="width:100%">
        <thead>
            <tr>
                <th>ID Compra</th>
                <th>Matrícula</th>
                <th>Fecha de Compra</th>
                <th>Estado</th>
                <th>Precio</th>
                <th>Pagos</th>
                <th>Referencia 1</th>
                <th>Referencia 2</th>
            </tr>
        </thead>
    </table>
</div>

<script>
    $(document).ready(function() {
        // Inicializar DataTable con AJAX
        $('#purchaseTable').DataTable({
            "processing": true,
            "serverSide": false,
            "ajax": {
                "url": "../../api/purchases/purchaseReserved.php",
                "type": "GET",
                "dataSrc": "data"  // DataTables espera que los datos estén en la propiedad 'data'
            },
            "columns": [
                { "data": "purchaseId" },
                { "data": "tuition" },
                { "data": "datePurchase" },
                { "data": "status" },
                { "data": "cryptPrice" },
                {
                    "data": "payments",
                    "render": function(data, type, row) {
                        return data.map(payment => `Monto: ${payment.paymentAmount} ${payment.currency}`).join('<br>');
                    }
                },
                { "data": "referencePerson1" },
                { "data": "referencePerson2" }
            ]
        });
    });
</script>
