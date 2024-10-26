<!-- Incluye CSS de Bootstrap y DataTables -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.0/css/bootstrap.min.css">
<link rel="stylesheet" href="https://cdn.datatables.net/2.1.8/css/dataTables.bootstrap5.min.css">
<link rel="stylesheet" href="https://cdn.datatables.net/responsive/3.0.3/css/responsive.bootstrap5.min.css">

<!-- Incluye jQuery y DataTables JS -->
<!-- jQuery first -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<!-- DataTables -->
<script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap5.min.js"></script>
<!-- Responsive DataTables -->
<script src="https://cdn.datatables.net/responsive/2.2.9/js/dataTables.responsive.min.js"></script>
<script src="https://cdn.datatables.net/responsive/2.2.9/js/responsive.bootstrap5.min.js"></script>


<div class="container">
    <div class="row">
        <div class="col-md-12"><br>
            <h5>Seleccionar sección</h5>
        </div>
    </div>

    <!-- Tabla donde se mostrará la información -->
    <table id="quotationsTable" class="table table-striped table-bordered nowrap" style="width:100%">
        <thead>
            <tr>
                <th>No. de solicitud</th>
                <th>Clave cripta</th>
                <th>Zona</th>
                <th>Area</th>
                <th>Fecha solicitud</th>
                <th>No. de cliente</th>
                <th>Nombre cliente</th>
                <th>Acciones</th>
            </tr>
        </thead>
    </table>
</div>
<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
