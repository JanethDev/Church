<!-- Primero incluye jQuery -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Luego incluye Select2 -->
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<style>
 
</style>
<?php
require_once '../../init.php';
require_once('auth/session.php');
    // Recibe los valores enviados a través de AJAX
    $urnaSeleccionada = isset($_POST['urnaSeleccionada']) ? $_POST['urnaSeleccionada'] : '';
    $tipoSeleccionado = isset($_POST['tipoSeleccionado']) ? $_POST['tipoSeleccionado'] : '';
    $precioSeleccionado = isset($_POST['precioSeleccionado']) ? $_POST['precioSeleccionado'] : '';
    $descuentoAplicado = isset($_POST['descuentoAplicado']) ? $_POST['descuentoAplicado'] : '';
    $totalFinal = isset($_POST['totalFinal']) ? $_POST['totalFinal'] : '';
    $selectedDiscountValue = isset($_POST['selectedDiscountValue']) ? $_POST['selectedDiscountValue'] : '';
    $selectedPaymentValue = isset($_POST['selectedPaymentValue']) ? $_POST['selectedPaymentValue'] : '';
    $enganche = isset($_POST['enganche']) ? $_POST['enganche'] : '';
    $positions = isset($_POST['positions']) ? json_decode($_POST['positions'], true) : [];

    if (!empty($positions)) {
        // Acceder a los valores dentro de positions
        $fullposition = isset($positions[0]['full_position']) ? $positions[0]['full_position'] : '';
        $id = isset($positions[0]['id']) ? $positions[0]['id'] : '';
    } else {
        $fullposition = '';
        $id = '';
    }   

    // Aquí puedes usar las variables para mostrarlas o realizar cualquier otra operación
    echo "<h1>Resumen de Compra</h1>";
    echo "<p>Urna seleccionada: $urnaSeleccionada</p>";
    echo "<p>Tipo seleccionado: $tipoSeleccionado</p>";
    echo "<p>Precio seleccionado: $precioSeleccionado</p>";
    echo "<p>Descuento aplicado: $descuentoAplicado</p>";
    echo "<p>Total final: $totalFinal</p>";
    echo "<p>Descuento: $selectedDiscountValue</p>";
    echo "<p>Forma de pago: $selectedPaymentValue</p>";
    echo "<p>Enganche: $enganche</p>";
    echo "<p>Ubicacion de compra: $fullposition</p>";
    echo "<p>Id: $id</p>";
?>
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
                            <td><?php echo(date('d')); ?></td>
                            <td><?php echo(date(format: 'm')); ?></td>
                            <td><?php echo(date(format: 'Y')); ?></td>
                        </tr>
                    </table>
                </div>
                <div class="offset-2 col-md-6 col-sm-6 col-xs-12">
                    <table class="table table-bordered" style="background-color: white">
                        <tr>
                            
                            <td style="">VENDEDOR</td>
                            
                        </tr>
                        <tr>
                            
                            
                            <td>
                                <?php echo($name); ?>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-6 col-xs-12">
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr><td colspan="3" style="text-align:center;"><strong>DATOS DEL SOLICITANTE</strong></td></tr>
                        <tr>
                            <td colspan="3">
                                <button type="button" class="btn btn-success" id="btnNewCustomer" style="margin-left:5px;">NUEVO CLIENTE</button>
                                <button type="button" class="btn btn-info" id="btnExistingCustomer" style="margin-left:5px;">CLIENTE EXISTENTE</button>
                            </td>
                        </tr>
                        <tr class="tr-search-customer" style="display:none;">
                            <td>BUSCAR CLIENTE</td>
                            <td colspan="2">
                            <select class="form-control" id="CustomerSelect" name="CustomerSelect" style="width: 100%!important;" >
                                <option value="">Buscar cliente...</option>
                            </select>
                                <input type="hidden" name="CustomerID" id="CustomerID" />
                                <input type="hidden" name="UserID" id="UserID" />
                            </td>
                        </tr>
                        <tr class="tr-new-customer" style="display:none;">
                            <td>APELLIDO PATERNO*</td>
                            <td>APELLIDO MATERNO*</td>
                            <td>NOMBRES*</td>
                        </tr>
                        <tr class="tr-new-customer" style="display:none;">
                            <td><input type="text" class="form-control control-customer-new" id="PSurname" name="PSurname" /></td>
                            <td><input type="text" class="form-control control-customer-new" id="MSurname" name="MSurname" /></td>
                            <td><input type="text" class="form-control control-customer-new" id="Name" name="Name" /></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td style="width:25%">TELEFONO PARTICULAR*</td>
                            <td><input type="text" class="form-control phone control-customer" id="CelPhone" name="CelPhone" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">CORREO ELECTRONICO*</td>
                            <td><input type="text" class="form-control control-customer" id="Email" name="Email" /></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white">
                        <tr>
                            <td style="width:25%">Razón Social</td>
                            <td><input type="text" class="form-control" id="BusinessName" name="BusinessName" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">R.F.C</td>
                            <td><input type="text" class="form-control" id="RFCCURP" name="RFCCURP"></td>
                        </tr>
                        <tr>
                            <td style="width:25%">FECHA DE NACIMIENTO</td>
                            <td><input readonly="readonly" type="text" class="form-control datepicker" id="DateOfBirth" name="DateOfBirth" value="" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">LUGAR DE NACIMIENTO</td>
                            <td><input type="text" class="form-control" id="CityOfBirth" name="CityOfBirth" value="" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">ESTADO CIVIL</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width:25%">OCUPACION</td>
                            <td><input type="text" class="form-control" id="Occupation" name="Occupation" value="" /></td>
                        </tr>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 col-sm-6 col-xs-12">
                        <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                            <tr><td colspan="3" style="text-align:center"><strong>DATOS DE LA EMPRESA DONDE PRESTA SUS SERVICIO</strong></td></tr>
                            <tr>
                                <td>NOMBRE DE LA COMPAÑIA</td>
                                <td style="width:70%"><input type="text" class="form-control" id="Company" name="Company" value="" /></td>
                            </tr>
                        </table>
                        <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                            <tr>
                                <td style="width:70%">DOMICILIO</td>
                                <td>TELEFONO</td>
                                <td><input type="text" class="form-control phone" id="PhoneCompany" name="PhoneCompany" value="" /></td>
                            </tr>
                            <tr>
                                <td style="width:70%"><input type="text" class="form-control" id="AddressCompany" name="AddressCompany" value="" /></td>
                                <td>EXT.</td>
                                <td><input type="text" class="form-control" id="ExtPhoneCompany" name="ExtPhoneCompany" value="" /></td>
                            </tr>
                        </table>
                        <table class="table table-bordered" style="background-color: white">
    <tr>
        <td>ESTADO</td>
        <td>DELEGACION</td>
        <td>CIUDAD</td>
    </tr>
    <tr>
        <td></td>
        <td><input type="text" class="form-control" id="DelegationAddressCompany" name="DelegationAddressCompany" value="y" /></td>
        <td></td>
    </tr>
    <tr>
        <td colspan="2">INGRESO PROMEDIO MENSUAL (incluye su conyuge)</td>
        <td>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" id="basic-addon1">$</span>
                </div>
                <input type="text" class="form-control" id="Income" name="Income" value="" aria-describedby="basic-addon1" />
            </div>
        </td>
    </tr>
</table>
<table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
    <tr><td colspan="4" style="text-align:center"><strong>REFERENCIAS</strong></td></tr>
    <tr>
        <td>1*</td>
        <td><input type="text" class="form-control control-reference" id="ReferenceCustomer1" name="ReferenceCustomer1" value="" /></td>
        <td>TEL.*</td>
        <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone1" name="ReferenceCustomerPhone1" value="" /></td>
    </tr>
    <tr>
        <td>2</td>
        <td><input type="text" class="form-control control-reference" id="ReferenceCustomer2" name="ReferenceCustomer2" value="" /></td>
        <td>TEL.</td>
        <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone2" name="ReferenceCustomerPhone2" value="" /></td>
    </tr>
</table>
<table class="table table-bordered" id="tableBeneficiary" style="background-color: white;margin-bottom: 0px;">
    <tr><td colspan="4" style="text-align:center"><strong>BENEFICIARIOS*</strong></td></tr>
    <tr>
        <td>Nombres</td>
        <td>Apellidos</td>
        <td>Fecha nac.</td>
        <td>Teléfono</td>
        <td>Parentesco</td>
        <td><button class="btn btn-success" id="btnAddBeneficiary" type="button">+</button></td>
    </tr>
    
        <tr class="tr tr-beneficiary">
            <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryName" value="" /></td>
            <td><input type="text" class="form-control control-beneficiary" name="BeneficiarySurnames" value="" /></td>
            <td><input type="text" class="form-control datepicker control-beneficiary" name="BeneficiaryBirthdate" value="" readonly="readonly" /></td>
            <td><input type="text" class="form-control control-beneficiary phone" name="BeneficiaryCelPhone" value="" /></td>
            <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryRelationship" value="" /><input type="hidden" name="BeneficiaryCustomerID" value="0" /></td>
            <td><button class="btn btn-danger btn-remove-beneficiary" type="button">-</button></td>
        </tr>
    
</table>

<table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
    <tr><td colspan="4" style="text-align:center"><strong>CONDICIONES ECONOMICAS DE LA OPERACION</strong></td></tr>
    <tr>
        <td>PLAN DE VENTA</td>
        <td>CLAVE DE LA CRIPTA</td>
        <td>NIVEL</td>
        <td>AREA</td>
        <td>ZONA</td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>ZONA A</td>
    </tr>
</table>
<table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
    <tr>
        <td>IMPORTE TOTAL</td>
        <td>DESC. APLICADO</td>
        <td>PAGO INICIAL</td>
        <td>SALDO</td>
    </tr>

    <tr>
        <td></td>
        <td>
            
        </td>
        <td></td>
        <td></td>
    </tr>
    
        <tr>
            <td colspan="3">EL SALDO SERA LIQUIDADO EN @iMeses ABONOS DE:  MXN. C/U, </td>
        </tr>
    
</table>

    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
        <tr>
            <td style="border: none;"></td>
            <td style="border: none;"></td>
            <td>DIA</td>
            <td>MES</td>
            <td>AÑO</td>
            <td style="border: none;"></td>
            <td style="border: none;"></td>
            <td>DIA</td>
            <td>MES</td>
            <td>AÑO</td>
        </tr>
        <tr>
            <td colspan="2">SIENDO EL PRIMERO DE ELLOS EN</td>
            <td></td>
            <td></td>
            <td></td>
            <td colspan="2">Y EL ULTIMO EN</td>
            <td></td>
            <td></td>
            <td>
        </td>
    </tr>
</table>

<table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
    <tr><td colspan="2" style="text-align:center"><strong>ADICIONAL</strong></td></tr>
    <tr>
        <td colspan="" style="width:50%">Cuota de mantenimiento anual</td>
        <td style="width:50%">Deposito de cenizas</td>
    </tr>
    <tr>
        <td colspan="" style="">
           
            <input type="checkbox" id="cbCheckMaintenanceFee" class="check-box" data-id="" style="width: 30px; height: 30px"  />
            <label style="position:absolute;margin-top:4px;margin-left:15px"> MXN</label>
            <input type="hidden" name="CheckMaintenanceFee" id="CheckMaintenanceFee" value="False" />
            <input type="hidden" name="MaintenanceFeeID" id="MaintenanceFeeID" value="0" />
        </td>
        <td style="">
            
            <input type="checkbox" id="cbCheckFederalTax" class="check-box" data-id="" style="width: 30px; height: 30px; " @sCheckFederalTax />
            <label style="position: absolute; margin-top: 4px; margin-left: 15px"> MXN</label>
            <input type="hidden" name="CheckFederalTax" id="" value="False" />
            <input type="hidden" name="FederalTaxID" id="FederalTaxID" value="0" />
        </td>
    </tr>
</table>
<table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
            
            <tr>
                <td class="text-center align-middle" rowspan="7">FORMA DEL PAGO INICIAL*</td>
            </tr>
            <tr>
                <td colspan="2"></td>
                <td>CANTIDAD</td>
                <td>No. DE CHEQUE</td>
                <td>No. DE CUENTA</td>
                <td>BANCO</td>
                <td>Comprobante</td>
            </tr>
            <tr>
                <td><input type="checkbox" id="TypePay1" class="typepay" name="TypePay" value="1" ></td>
                <td>CHEQUE </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr style="margin-bottom:5px">
                <td><input type="checkbox" id="TypePay2" class="typepay" name="TypePay" value="2" ></td>
                <td>T. DE CREDITO/DEBITO </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><input type="checkbox" id="TypePay3" class="typepay" name="TypePay" value="3" ></td>
                <td>TRANSFERENCIA</td>
                <td></td>
                <td colspan="3"></td>
                <td><input type="file" class="form-control typepay3" id="Ticket" name="Ticket" /></td>
            </tr>
            <tr>
                <td><input type="checkbox" id="TypePay5" class="typepay" name="TypePay" value="5" ></td>
                <td>DEPOSITO EN EFECTIVO</td>
                <td></td>
                <td colspan="4"></td>
                <td><input type="file" class="form-control typepay5" id="TicketCashDeposit" name="TicketCashDeposit" /></td>
            </tr>
            <tr>
                <td><input type="checkbox" id="TypePay4" class="typepay" name="TypePay" value="4" ></td>
                <td>EFECTIVO</td>
                <td></td>
                <td colspan="5"></td>
            </tr>
        </table>
    </div>
</div>
<div class="row" style="padding-top:15px;">
    <div class="col-md-2 col-sm-2 col-xs-6">
    <button class = "btn btn-primary" style="width:100%;" id="btnSelUrna">Volver</button>
    </div>
    <div class="offset-md-6 col-md-2 col-sm-2 col-xs-6">
        <input type="button" value="Cotizar" id="btnQuotation" class="btn btn-default" style="float:right;width:100%;" />
    </div>
    <div class="col-md-2 col-sm-2 col-xs-6">
        <input type="button" value="Guardar" id="btnSave" class="btn btn-default" style="float:right;width:100%;" />
    </div>
</div>


<script>
$(document).ready(function() {
    $('#btnNewCustomer').click(function() {
        $('.tr-search-customer').hide(); // Ocultar la búsqueda de cliente
        $('.tr-new-customer').show();    // Mostrar los campos de nuevo cliente
    });

    // Mostrar campo de "Buscar Cliente"
    $('#btnExistingCustomer').click(function() {
        $('.tr-new-customer').hide();    // Ocultar los campos de nuevo cliente
        $('.tr-search-customer').show(); // Mostrar la búsqueda de cliente
    });
    $('#btnSelUrna').click(function(e) {
        e.preventDefault(); // Evita que el botón envíe un formulario o recargue la página
        window.location.href = 'solicitud'; // Redirige sin parámetros
    });

    $('#CustomerSelect').select2({
        placeholder: "Buscar cliente...",
        minimumInputLength: 2,
        ajax: {
            url: "api/customers/searchCustomers.php",
            type: "POST",
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    value: params.term
                };
            },
            processResults: function(response) {
                return {
                    results: response.map(function(customer) {
                        return {
                            id: customer.id,
                            text: customer.name + ' ' + customer.father_last_name + ' - ' + customer.phone
                        };
                    })
                };
            },
            cache: true
        }
    });

    // Manejar el evento de selección
    $('#CustomerSelect').on('select2:select', function(e) {
        var selectedCustomerId = e.params.data.id;
        var selectedCustomerName = e.params.data.text;

        // Asignar el valor seleccionado a los campos ocultos
        $('#CustomerID').val(selectedCustomerId);
    });


});
</script>
