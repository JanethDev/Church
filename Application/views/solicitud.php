<?php
$_title = 'Solicitud | Catedral Tijuana';
?>
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

<form id="frmCryptRequest" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">

    <nav>
        <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-zona-tab" data-toggle="tab" href="#nav-zona" role="tab" aria-controls="nav-zona" aria-selected="true">Zonas</a>
            <a class="nav-item nav-link" id="nav-area-tab" data-toggle="tab" href="#nav-area" role="tab" aria-controls="nav-area" aria-selected="false">Areas</a>
            <a class="nav-item nav-link" id="nav-criptas-tab" data-toggle="tab" href="#nav-criptas" role="tab" aria-controls="nav-criptas" aria-selected="false">Criptas</a>
            <a class="nav-item nav-link " id="nav-forma-pago-tab" data-toggle="tab" href="#nav-forma-pago" role="tab" aria-controls="nav-forma-pago" aria-selected="false">Forma de pago</a>
        </div>
    </nav>
    <div class="tab-content">
        <div class="tab-pane fade show active" id="nav-zona" role="tabpanel" aria-labelledby="nav-zona-tab">
            <?php include_once('crypts/cryptmaps.php'); ?>
        </div>
        <div class="tab-pane fade" id="nav-area" role="tabpanel" aria-labelledby="nav-area-tab">
            <div id="areaContent"></div>
        </div>
        <div class="tab-pane fade" id="nav-criptas" role="tabpanel" aria-labelledby="nav-criptas-tab">
            <div id="criptasContent"></div>
            
        </div>
        <div class="tab-pane fade" id="nav-forma-pago" role="tabpanel" aria-labelledby="nav-forma-pago-tab">
            <div id="formaPagoContent"> </div>
        </div>
    </div>
</form>
<script src="../assets/js/solicitud.js"></script>