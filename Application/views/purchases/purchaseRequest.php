<!-- Incluye jQuery y Select2 -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

<div class="card">
    <div class="card-header card-header-green">
        <h2>Nueva solicitud de compra</h2>
    </div>
    <div class="card-body">
        <form id="PurchaseRequestCreateForm" enctype="multipart/form-data">
            <div class="row">
                <div class="col-md-4 col-sm-6 col-xs-12">
                    <table class="table table-bordered" style="background-color: white">
                        <tr><td colspan="3" style="text-align:center;">Fecha</td></tr>
                        <tr style="text-align:center;">
                            <td>DD</td>
                            <td>MM</td>
                            <td>YYYY</td>
                        </tr>
                    </table>
                </div>
                <div class="offset-2 col-md-6 col-sm-6 col-xs-12">
                    <table class="table table-bordered" style="background-color: white">
                        <tr>
                            <td>VENDEDOR</td>
                        </tr>
                        <tr>
                            <td>Nombre Vendedor</td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <!-- Datos del Solicitante -->
            <div class="row">
                <div class="col-md-12 col-sm-6 col-xs-12">
                    <table class="table table-bordered" style="background-color: white;">
                        <tr><td colspan="3" style="text-align:center;"><strong>DATOS DEL SOLICITANTE</strong></td></tr>
                        <tr class="tr-new-customer" style="display:none;">
                            <td>APELLIDO PATERNO*</td>
                            <td>APELLIDO MATERNO</td>
                            <td>NOMBRES*</td>
                        </tr>
                        <tr class="tr-new-customer" style="display:none;">
                            <td><input type="text" class="form-control" name="PSurname" /></td>
                            <td><input type="text" class="form-control" name="MSurname" /></td>
                            <td><input type="text" class="form-control" name="Name" /></td>
                        </tr>
                    </table>
                    
                    <!-- Domicilio Particular -->
                    <table class="table table-bordered" style="background-color: white;">
                        <tr><td colspan="3" style="text-align:center;"><strong>DOMICILIO PARTICULAR</strong></td></tr>
                        <tr>
                            <td>CALLE*</td>
                            <td>NUMERO*</td>
                            <td>INTERIOR</td>
                        </tr>
                        <tr>
                            <td><input type="text" class="form-control" name="address" /></td>
                            <td><input type="text" class="form-control" name="house_number" /></td>
                            <td><input type="text" class="form-control" name="apt_number" /></td>
                        </tr>
                    </table>

                    <!-- Información de la Empresa -->
                    <table class="table table-bordered" style="background-color: white;">
                        <tr><td colspan="3" style="text-align:center"><strong>DATOS DE LA EMPRESA DONDE PRESTA SUS SERVICIOS</strong></td></tr>
                        <tr>
                            <td>NOMBRE DE LA COMPAÑIA</td>
                            <td><input type="text" class="form-control" name="Company" /></td>
                        </tr>
                    </table>
                    
                    <!-- Beneficiarios -->
                    <table class="table table-bordered" id="tableBeneficiary" style="background-color: white;">
                        <tr><td colspan="6" style="text-align:center"><strong>BENEFICIARIOS</strong></td></tr>
                        <tr>
                            <td>Nombres</td>
                            <td>Apellidos</td>
                            <td>Fecha nac.</td>
                            <td>Teléfono</td>
                            <td>Parentesco</td>
                        </tr>
                        <tr class="tr tr-beneficiary">
                            <td><input type="text" class="form-control" name="BeneficiaryName" /></td>
                            <td><input type="text" class="form-control" name="BeneficiarySurnames" /></td>
                            <td><input type="date" class="form-control" name="BeneficiaryBirthdate" /></td>
                            <td><input type="text" class="form-control" name="BeneficiaryCelPhone" /></td>
                            <td><input type="text" class="form-control" name="BeneficiaryRelationship" /></td>
                        </tr>
                    </table>
                    
                    <!-- Condiciones Económicas -->
                    <table class="table table-bordered" style="background-color: white;">
                        <tr><td colspan="4" style="text-align:center"><strong>CONDICIONES ECONOMICAS DE LA OPERACION</strong></td></tr>
                        <tr>
                            <td>PLAN DE VENTA</td>
                            <td>CLAVE DE LA CRIPTA</td>
                            <td>NIVEL</td>
                            <td>AREA</td>
                            <td>ZONA</td>
                        </tr>
                        <tr>
                            <td><label>Plan Seleccionado</label></td>
                            <td><label>Clave Cripta</label></td>
                            <td><label>Nivel</label></td>
                            <td><label>Área</label></td>
                            <td><label>Zona</label></td>
                        </tr>
                    </table>
                    
                    <!-- Condiciones Financieras -->
                    <table class="table table-bordered" style="background-color: white;">
                        <tr>
                            <td>IMPORTE TOTAL</td>
                            <td>DESC. APLICADO</td>
                            <td>PAGO INICIAL</td>
                            <td>SALDO</td>
                        </tr>
                        <tr>
                            <td><label>Importe Total</label></td>
                            <td><label>Descuento Aplicado</label></td>
                            <td><label>Pago Inicial</label></td>
                            <td><label>Saldo</label></td>
                        </tr>
                    </table>
                </div>
            </div>
        </form>
    </div>
</div>

<!-- Botones de Acción -->
<div class="row" style="padding-top:15px;">
    <div class="col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-primary" style="width:100%;" id="btnSelNicho">Volver</button>
    </div>
    <div class="offset-md-6 col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-default" style="float:right;width:100%;" id="btnQuotation">Confirmar compra</button>
    </div>
</div>

<script>
    // Inicialización de select2
    $(document).ready(function() {
        $('.select2').select2();
    });
</script>
