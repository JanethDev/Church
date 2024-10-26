<?php
$_title = 'Cotizaciones | Catedral Tijuana';
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

<form id="frmQuotations" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">

    <nav>
        <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-cotizaciones-tab" data-toggle="tab" href="#nav-cotizaciones" role="tab" aria-controls="nav-cotizaciones" aria-selected="true">Cotizaciones</a>
            <a class="nav-item nav-link" id="nav-prospectos-tab" data-toggle="tab" href="#nav-prospectos" role="tab" aria-controls="nav-prospectos" aria-selected="false">Prospectos</a>
        </div>
    </nav>

    <div class="tab-content">
        <div class="tab-pane fade show active" id="nav-cotizaciones" role="tabpanel" aria-labelledby="nav-cotizaciones-tab">
            <?php include_once('purchases/quotations.php'); ?>
        </div>
        <div class="tab-pane fade" id="nav-prospectos" role="tabpanel" aria-labelledby="nav-prospectos-tab">
            <?php include_once('purchases/quotationsProspects.php'); ?>
        </div>
    </div>
</form>
<script src="../assets/js/quotations.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>