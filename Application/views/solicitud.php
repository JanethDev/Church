<?php
$_title = 'Solicitud | Catedral Tijuana';
?>
<form id="frmCryptRequest" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">

    <nav>
        <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-zona-tab" data-toggle="tab" href="#nav-zona" role="tab" aria-controls="nav-zona" aria-selected="true">Zonas</a>
            <a class="nav-item nav-link" id="nav-area-tab" data-toggle="tab" href="#nav-area" role="tab" aria-controls="nav-area" aria-selected="false">Areas</a>
            <a class="nav-item nav-link" id="nav-criptas-tab" data-toggle="tab" href="#nav-criptas" role="tab" aria-controls="nav-criptas" aria-selected="false">Criptas</a>
            <a class="nav-item nav-link disable" id="nav-forma-pago-tab" data-toggle="tab" href="#nav-forma-pago" role="tab" aria-controls="nav-forma-pago" aria-selected="false">Forma de pago</a>
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