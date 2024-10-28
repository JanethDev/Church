<?php
$_title = 'Cotizaciones | Catedral Tijuana';
?>
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

<!-- Cargar quotations.js solo una vez aquí -->
<script src="../assets/js/quotations.js"></script>
