<?php

$_title = 'Aniversario luctuoso';

?>

<!-- Card header -->
<div class="card">
    <div class="card-header card-header-green">
        <div class="row">
            <div class="col-md-10">
                <h2><?= $_title?></h2>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <p style="text-align:right; padding-top:20px">
                    <a style="width:100%;" href="<?= url('Create', 'Anniversary')?>" class="btn btn-default btn-large">Agregar</a>
                </p>
            </div>
        </div>
    </div>

    <!-- Card body -->
    <div class="card-body">
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label>Nombre</label>
                    <input type="search" name="Name" class="form-control" data-autocomplete="<?php ?>" placeholder="nombre" value="<?= $_GET['Name']?>">
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label>Fecha</label>
                    <input type="search" id="Date" name="Date" class="form-control datepicker" placeholder="Fecha" value="<?= $_GET['Date']?>">
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label>Hora</label>
                    <input type="search" name="Hour" class="form-control" placeholder="Hora" value="<?= $_GET['Hour']?>">
                </div>
            </div>
            <div class="col-md-3">
                <label>&ensp;</label>
                <div class="form-group">
                    <a href="<?= url('Index', null, ['iCleanFilter' => 1])?>" class="btn btn-primary btn-large btn-clean">Limpiar</a>
                    <input id="btnSearch" class="btn btn-success btn-large btn-clean" type="submit" value="Buscar">
                </div>
            </div>
            <div class="col-md-2">
                <label>&ensp;</label>
                <div class="form-group">
                    <button type="button" id="btnExportExcel" class="btn btn-success">Exportar Excel</button>
                </div>
            </div>
            <div class="col-md-2">
                <label>&ensp;</label>
                <div class="form-group">
                    <a target="_blank" class="btn btn-info" id="pdf" href="#">Exportar PDF</a>
                </div>
            </div>
        </div>
</div>
<!-- Scripts -->
<script src="ui.datepicker-es-MX.js"></script>
<script type="text/javascript">
    $(function () {
        $.datepicker.setDefaults($.datepicker.regional['es-MX']);
        $(".datepicker").datepicker();
    });

    $('#btnExportExcel').on('click', function (event) {
        fnExcelReport('tableExport', 'Reporte de aniversario luctuoso ' + $("#Date").val() + ' ');
    });
</script>

<?php
// Helper function to generate URLs
function url($action, $controller, $params = []) {
    // Implement your URL generation logic here
    // For example:
    return '/'. $controller. '/'. $action. '?'. http_build_query($params);
}
?>