<?php
$_title = 'Catedral | Catedral Tijuana';
?>
<div class="card">
    <div class="card-header card-header-green">
        <div class="row">
            <div class="col-md-10">
                <h2>@ViewBag.Title</h2>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <p style="text-align:right; padding-top:20px">
                    <a style="width:100%;" href="" class="btn btn-default btn-large">Agregar</a>
                </p>
            </div>
        </div>
    </div>
    <div class="card-body">
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label>Nombre</label>
                    <input type="search" name="Name" class="form-control" data-autocomple="@Url.Action("acName")" placeholder="Nombre" value="@ViewBag.Name" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label>Fecha de creación</label>
                    <input type="search" name="Date" class="form-control datepicker" placeholder="Fecha de creación" value="@ViewBag.Date" />
                </div>
            </div>
            <div class="col-md-3">
                <label>&ensp;</label>
                <div class="form-group">
                    @Html.ActionLink("Limpiar", "Index", new { iCleanFilter = 1 }, htmlAttributes: new { @class = "btn btn-primary btn-large btn-clean" })
                    <input id="btnSearch" class="btn btn-success btn-large btn-clean" type="submit" value="Buscar" />
                </div>
            </div>
        </div>
        <!-- aqui va el datatable -->
    </div>
</div>

