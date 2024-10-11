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
    $nichoSeleccionada = isset($_POST['nichoSeleccionada']) ? $_POST['nichoSeleccionada'] : '';
    $tipoSeleccionado = isset($_POST['tipoSeleccionado']) ? $_POST['tipoSeleccionado'] : '';
    $precioSeleccionado = isset($_POST['precioSeleccionado']) ? $_POST['precioSeleccionado'] : '';
    $descuentoAplicado = isset($_POST['descuentoAplicado']) ? $_POST['descuentoAplicado'] : '';
    $totalFinal = isset($_POST['totalFinal']) ? floatval($_POST['totalFinal']) : 0;
    $enganche = isset($_POST['enganche']) ? floatval($_POST['enganche']) : 0;
    $selectedDiscountValue = isset($_POST['selectedDiscountValue']) ? $_POST['selectedDiscountValue'] : '0';
    $selectedPaymentValue = isset($_POST['selectedPaymentValue']) ? $_POST['selectedPaymentValue'] : '';
    $interes = isset($_POST['interes']) ? $_POST['interes'] : '';
    $mensualidades = isset($_POST['mensualidad']) ? $_POST['mensualidad'] : '';
    $meses = isset($_POST['meses']) ? $_POST['meses'] : '';
    $mesesRest = isset($_POST['mesesres']) ? $_POST['mesesres'] : '';
    $positions = isset($_POST['positions']) ? json_decode($_POST['positions'], true) : [];

    $paymentMethods = [
        '1' => 'Contado',
        '2' => '11 meses',
        '5' => '17 meses',
        '3' => '23 meses',
        '6' => '35 meses',
        '7' => '47 meses',
        '4' => '11 meses'
    ];
    $selectedPaymentDescription = isset($paymentMethods[$selectedPaymentValue]) ? $paymentMethods[$selectedPaymentValue] : 'Opción no válida';
    // Función para convertir el formato de precio a número
    function convertToNumber($value) {
        return floatval(str_replace(['$', 'MXN', ' ', ','], '', $value));
    }

    // Fecha actual
    $fechaUltima = new DateTime();

    // Suma los meses de las mensualidades a la fecha actual
    $fechaUltima->modify("+{$mesesRest} months");

    // Formatear la fecha para mostrar el día, mes y año
    $diaUltimoPago = $fechaUltima->format('d');
    $mesUltimoPago = $fechaUltima->format('m');
    $yUltimoPago = $fechaUltima->format('Y');

    // Fecha actual
    $fechaPrimera = new DateTime();

    // Suma los meses de las mensualidades a la fecha actual
    $fechaPrimera->modify("+1 months");

    // Formatear la fecha para mostrar el día, mes y año
    $diaPrimerPago = $fechaPrimera->format('d');
    $mesPrimerPago = $fechaPrimera->format('m');
    $yPrimerPago = $fechaPrimera->format('Y');

    // Convertir totalFinal y enganche a números
    $totalFinalValue = convertToNumber($totalFinal);
    $engancheValue = convertToNumber($enganche);

    // Calcular el saldo
    if($selectedPaymentValue != 1){
        $saldo = $totalFinalValue - $engancheValue;
    }else{
        $saldo = 0;
    }
    

    // Función para formatear el precio
    function formatPrice($price) {
        return '$ ' . number_format($price, 2, '.', ',') . ' MXN';
    }

    if (!empty($positions)) {
        // Acceder a los valores dentro de positions
        $fullposition = isset($positions[0]['full_position']) ? $positions[0]['full_position'] : '';
        $id = isset($positions[0]['id']) ? $positions[0]['id'] : '';
        $isShared = isset($positions[0]['is_shared']) ? $positions[0]['is_shared'] : '';
        $places = isset($positions[0]['places_shared']) ? $positions[0]['places_shared'] : 0;
    
        // Inicializar la variable para el sufijo
        $suffix = '';
        $spaces = 4;

        // Verificar si el lugar es compartido
        if ($isShared == 1) {
                $spaces = 1;
                switch ($places) {
                    case 0:
                        $suffix = "-1";
                        break;
                    case 1:
                        $suffix = "-2";
                        break;
                    case 2:
                        $suffix = "-3";
                        break;
                    case 3:
                        $suffix = "-4";
                        break;
                    default:
                        // Si hay más de 3 compartidos, puedes manejarlo aquí si es necesario
                        break;
                }
            }
        } else {
            $fullposition = '';
            $places = 0;
            $id = '';
            $isShared = '';
            $suffix = '';
            $spaces = 0;
        }
    echo "<script>var isShared = '" . addslashes($tipoSeleccionado) . "';</script>";
    
    $zone = substr($fullposition, 0, 1);
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
                                <input type="hidden" name="UserID" id="UserID" value="<?php echo($user_id); ?>" />
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
                    <tr><td colspan="3" style="text-align:center;"><strong>DOMICILIO PARTICULAR</strong></td></tr>
                        <tr>
                            <td style="width:50%">CALLE, AV., BLDV. CALZ.*</td>
                            <td style="width:25%">NUMERO </td>
                            <td style="width:25%">INTERIOR </td>
                            
                        </tr>
                        <tr>
                            <td ><input type="text" class="form-control" id="address" name="address" value="" /></td>
                            <td ><input type="text" class="form-control" id="house_number" name="house_number" value="" /></td>
                            <td ><input type="text" class="form-control" id="apt_number" name="apt_number" value="" /></td>
                            
                        </tr>
                        <tr>
                            <td colspan="2">COLONIA*</td>
                            <td >CODIGO POSTAL </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2"><input type="text" class="form-control" id="customer_municipality" name="customer_municipality" value="" /></td>
                            <td ><input type="text" class="form-control" id="zip_code" name="zip_code" value="" /></td>
                            
                        </tr>
                        <tr>
                            <td >ESTADO*</td>
                            <td >DELEGACIÓN </td>
                            <td >CIUDAD </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <select class="form-control select2" id="catStatesId" name="catStatesIdy">
                                    <option value="">Seleccione un estado</option>
                                </select>
                            <td>
                                <input type="text" class="form-control" id="catMunicipalityAddress" name="catMunicipalityAddressCompany" value="" />
                            </td>
                            <td>
                                <select class="form-control select2" id="catTownsId" name="catTownsId">
                                    <option value="">Seleccione una ciudad</option>
                                </select>
                            </td>
                            
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td >TELEFONO PARTICULAR*</td>
                            <td>CORREO ELECTRONICO*</td>  
                        </tr>
                        <tr>
                            <td><input type="text" class="form-control phone control-customer" id="CelPhone" name="CelPhone" /></td>
                            <td><input type="text" class="form-control control-customer" id="Email" name="Email" /></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white">
                        <tr>
                            <td style="width:25%">Razón Social</td>
                            <td><input type="text" class="form-control" id="BusinessName" name="BusinessName" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">R.F.C o CURP</td>
                            <td><input type="text" class="form-control" id="RFCCURP" name="RFCCURP"></td>
                        </tr>
                        <tr>
                            <td style="width:25%">FECHA DE NACIMIENTO</td>
                            <td><input type="date" class="form-control datepicker" id="DateOfBirth" name="DateOfBirth" value="" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">LUGAR DE NACIMIENTO</td>
                            <td><input type="text" class="form-control" id="CityOfBirth" name="CityOfBirth" value="" /></td>
                        </tr>
                        <tr>
                            <td style="width:25%">ESTADO CIVIL</td>
                            <td><select class="form-control select2" id="CivilStatus" name="CivilStatus">
                                    <option value="">Estado civil</option>
                                </select>
                            </td>
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
                            <td>
                                <select class="form-control select2" id="StateAddressCompany" name="StateAddressCompany">
                                    <option value="">Seleccione un estado</option>
                                </select>
                            </td>
                            <td>
                                <input type="text" class="form-control" id="MunicipalityAddressCompany" name="MunicipalityAddressCompany" value="" />
                            </td>
                            <td>
                                <select class="form-control select2" id="CityAddressCompany" name="CityAddressCompany">
                                    <option value="">Seleccione una ciudad</option>
                                </select>
                            </td>
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
                        <tr><td colspan="6" style="text-align:center"><strong>BENEFICIARIOS*</strong></td></tr>
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
                                <td><input type="date" class="form-control datepicker control-beneficiary" name="BeneficiaryBirthdate" value="" /></td>
                                <td><input type="text" class="form-control control-beneficiary phone" name="BeneficiaryCelPhone" value="" /></td>
                                <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryRelationship" value="" /><input type="hidden" name="BeneficiaryCustomerID" value="0" /></td>
                                <td><button class="btn btn-danger btn-remove-beneficiary" type="button">-</button></td>
                            </tr>
                        
                    </table>

                    <table class="table table-bordered" id="economicConditions" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td colspan="4" style="text-align:center"><strong>CONDICIONES ECONOMICAS DE LA OPERACION</strong></td>
                        </tr>
                        <tr>
                            <td>PLAN DE VENTA</td>
                            <td>CLAVE DE LA CRIPTA</td>
                            <td>NIVEL</td>
                            <td>AREA</td>
                            <td>ZONA</td>
                        </tr>
                        <tr>
                            <td><label id="paymentPlanLabel" style="display: none;"><?php echo $mesesRest; ?></label ><label ><?php echo $selectedPaymentDescription; ?></label></td>
                            <td><label id="cryptKeyLabel"><?php echo $fullposition; ?></label><label><?php echo $suffix; ?></label></td>
                            <td><label id="levelLabel"><?php echo $positions[0]['level']; ?></label></td>
                            <td><label id="areaLabel"><?php echo $positions[0]['aisle']; ?></label></td>
                            <td><label id="zoneLabel"><?php echo $zone; ?></label></td>
                            <input type="hidden" name="cryptId" id="cryptId" value="<?php echo($id); ?>" />
                            <input type="hidden" name="cryptSpaces" id="cryptSpaces" value="<?php echo($spaces); ?>" />
                            <input type="hidden" name="discountId" id="discountId" value="<?php echo($selectedDiscountValue); ?>" />
                            <input type="hidden" name="federalTax" id="federalTax" value="0" />
                        </tr>
                    </table>

                    <table class="table table-bordered" id="financialConditions" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td>IMPORTE TOTAL</td>
                            <td>DESC. APLICADO</td>
                            <td>PAGO INICIAL</td>
                            <td>SALDO</td>
                        </tr>
                        <tr>
                            <td><label id="totalAmountLabel"><?php echo '$' . number_format($totalFinal, 2) . ' MXN'; ?></label></td>
                            <td><label id="appliedDiscountLabel"><?php echo '$' . number_format($descuentoAplicado, 2) . ' MXN'; ?></label></td>
                            <td>
                                <label id="initialPaymentLabel"><?php echo ($selectedPaymentValue == 1) ? '$' . number_format($totalFinal, 2) . ' MXN' : '$' . number_format($enganche, 2) . ' MXN'; ?></label>
                            </td>
                            <td><label id="balanceLabel"><?php echo '$' . number_format($saldo, 2) . ' MXN'; ?></label></td>
                        </tr>
                        <tr>
                            <?php if ($paymentMethods != 1): ?>
                                <td colspan="3">EL SALDO SERA LIQUIDADO EN <?= $selectedPaymentDescription; ?> EN ABONOS DE: $ <?= number_format($mensualidades,2); ?> MXN. C/U</td>
                            <?php else: ?>
                                <td colspan="3"></td>
                            <?php endif; ?>
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
                                <input type="checkbox" id="ckMaintenance" class="check-box" style="width: 30px; height: 30px" />
                                <label style="position:absolute;margin-top:4px;margin-left:15px">$ <span id="maintenance"></span> MXN</label> 
                                <input type="hidden" name="inMaintenance" id="CheckMaintenanceFee" value="False" />
                            </td>
                            <td style="">
                                <input type="checkbox" id="ckAshDeposit" class="check-box" style="width: 30px; height: 30px;" />
                                <label style="position: absolute; margin-top: 4px; margin-left: 15px">$ <span id="ashDeposit"></span> MXN</label>
                                <input type="hidden" name="inAshDeposit" id="CheckAshDepositFee" value="False" />
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
                            <td><input type="checkbox" id="TypePay1" class="typepay" name="TypePay" value="1"></td>
                            <td>CHEQUE</td>
                            <td><input type="number" id="amount_check" class="form-control" name="amount_check" placeholder="Cantidad" disabled></td>
                            <td><input type="text" id="check_number" class="form-control" name="check_number" placeholder="No. de Cheque" disabled></td>
                            <td><input type="text" id="account_number" class="form-control" name="account_number" placeholder="No. de Cuenta" disabled></td>
                            <td><input type="text" id="bank" class="form-control" name="bank" placeholder="Banco" disabled></td>
                            <td></td> <!-- No input file para Cheque -->
                        </tr>
                        <tr style="margin-bottom:5px">
                            <td><input type="checkbox" id="TypePay2" class="typepay" name="TypePay" value="2"></td>
                            <td>T. DE CREDITO/DEBITO</td>
                            <td><input type="number" id="amount_card" class="form-control" name="amount_card" placeholder="Cantidad" disabled></td>
                            <td><input type="text" id="card_number" class="form-control" name="card_number" placeholder="No. de Cheque" disabled></td>
                            <td><input type="text" id="account_card" class="form-control" name="account_card" placeholder="No. de Cuenta" disabled></td>
                            <td><input type="text" id="bank_card" class="form-control" name="bank_card" placeholder="Banco" disabled></td>
                            <td></td> <!-- No input file para T. de Crédito/Debito -->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay3" class="typepay" name="TypePay" value="3"></td>
                            <td>TRANSFERENCIA</td>
                            <td><input type="number" id="amount_transfer" class="form-control" name="amount_transfer" placeholder="Cantidad" disabled></td>
                            <td colspan="3"></td>
                            <td><input type="file" class="form-control typepay3" id="TicketTransfer" name="TicketTransfer" disabled></td> <!-- Input file para Transferencia -->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay5" class="typepay" name="TypePay" value="5"></td>
                            <td>DEPOSITO EN EFECTIVO</td>
                            <td><input type="number" id="amount_cash_deposit" class="form-control" name="amount_cash_deposit" placeholder="Cantidad" disabled></td>
                            <td colspan="3"></td>
                            <td><input type="file" class="form-control typepay5" id="TicketCashDeposit" name="TicketCashDeposit" disabled></td> <!-- Input file para Depósito en Efectivo -->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay4" class="typepay" name="TypePay" value="4"></td>
                            <td>EFECTIVO</td>
                            <td><input type="number" id="amount_cash" class="form-control" name="amount_cash" placeholder="Cantidad" disabled></td>
                            <td colspan="4"></td>
                           
                        </tr>
                    </table>
                </div>
            </div>
        </form>
    </div>
</div>
<div class="row" style="padding-top:15px;">
    <div class="col-md-2 col-sm-2 col-xs-6">
    <button class = "btn btn-primary" style="width:100%;" id="btnSelNicho">Volver</button>
    </div>
    <div class="offset-md-6 col-md-2 col-sm-2 col-xs-6">
        <input type="button" value="Cotizar" id="btnQuotation" class="btn btn-default" style="float:right;width:100%;" />
    </div>
    <div class="col-md-2 col-sm-2 col-xs-6">
        <input type="button" value="Guardar" id="btnSave" class="btn btn-default" style="float:right;width:100%;" />
    </div>
</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7/jquery.inputmask.min.js"></script>

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
    $('#btnSelNicho').click(function(e) {
        e.preventDefault(); // Evita que el botón envíe un formulario o recargue la página
        window.location.href = 'solicitud'; // Redirige sin parámetros
    });

    $(document).on('focus', '.phone', function() {
        $(this).inputmask("(999) 999-9999");
    });

    // Aplicar máscara para números con separación de miles y decimales
    $('#Income').inputmask({
        alias: 'numeric',
        groupSeparator: ',',
        autoGroup: true,
        digits: 2,
        radixPoint: '.',
        digitsOptional: false,
        placeholder: "0",
        rightAlign: false,
        removeMaskOnSubmit: true // Esto quita la máscara al enviar el formulario si es necesario.
    });

    $('.select2').select2();

    function formatDateToYMD(dateString) {
        var parts = dateString.split('/');
        // Cambia el orden de la fecha a YYYY-MM-DD
        return `${parts[2]}-${parts[1]}-${parts[0]}`;
    }

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
                            text: customer.name + ' ' + customer.father_last_name + ' - ' + customer.phone,
                            customerNumber: customer.customerNumber,
                            email: customer.email,
                            name: customer.name,
                            father_last_name: customer.father_last_name,
                            mother_last_name: customer.mother_last_name,
                            phone: customer.phone,
                            rfc: customer.rfc,
                            zip_code: customer.zip_code,
                            address: customer.address,
                            state: customer.state,
                            town: customer.town,
                            social_reason: customer.social_reason,
                            birthdate: customer.birthdate,
                            birth_place: customer.birth_place,
                            civil_status: customer.civil_status,
                            occupation: customer.occupation,
                            business_name: customer.business_name,
                            business_address: customer.business_address,
                            business_city: customer.business_city,
                            business_municipality: customer.business_municipality,
                            business_state: customer.business_state,
                            business_phone: customer.business_phone,
                            business_ext: customer.business_ext,
                            deputation: customer.deputation,
                            average_income: customer.average_income
                        };
                    })
                };
            },
            cache: true
        }
    });

    // Manejar el evento de selección
    $('#CustomerSelect').on('select2:select', function(e) {
        var selectedCustomer = e.params.data;

        // Asignar el valor seleccionado a los campos ocultos y visibles
        $('#CustomerID').val(selectedCustomer.id); // ID del cliente
        $('#PSurname').val(selectedCustomer.father_last_name); // Apellido paterno
        $('#MSurname').val(selectedCustomer.mother_last_name); // Apellido materno
        $('#Name').val(selectedCustomer.name);                 // Nombre
        $('#CelPhone').val(selectedCustomer.phone); // Teléfono
        $('#Email').val(selectedCustomer.email); // Correo electrónico
        $('#BusinessName').val(selectedCustomer.business_name); // Nombre de la empresa
        $('#RFCCURP').val(selectedCustomer.rfc); // RFC
        $('#DateOfBirth').val(selectedCustomer.birthdate.split('T')[0]); // Fecha de nacimiento
        $('#CityOfBirth').val(selectedCustomer.birth_place); // Ciudad de nacimiento
        $('#catStatesId').val(selectedCustomer.catStatesId); // Ciudad de nacimiento
        $('#catTownsId').val(selectedCustomer.catTownsId); // Ciudad de nacimiento
        $('#CivilStatus').val(selectedCustomer.civil_status); // Estado civil
        $('#Occupation').val(selectedCustomer.occupation); // Ocupación

        // Asignar datos de la empresa
        $('#Company').val(selectedCustomer.business_name); // Nombre de la empresa
        $('#PhoneCompany').val(selectedCustomer.business_phone); // Teléfono de la empresa
        $('#AddressCompany').val(selectedCustomer.business_address); // Dirección de la empresa
        $('#CityAddressCompany').val(selectedCustomer.business_city); // Ciudad de la empresa
        $('#MunicipalityAddressCompany').val(selectedCustomer.business_municipality); // Municipio de la empresa
        $('#StateAddressCompany').val(selectedCustomer.business_state); // Estado de la empresa
        $('#ExtPhoneCompany').val(selectedCustomer.business_ext); // Extensión de teléfono
        $('#DelegationAddressCompany').val(selectedCustomer.deputation); // Delegación
        $('#Income').val(selectedCustomer.average_income); // Ingreso promedio

        // Disparar los eventos 'change' para que los campos dependientes actualicen su contenido
        $('#CivilStatus').trigger('change');
        $('#StateAddressCompany').trigger('change');
        $('#CityAddressCompany').trigger('change');

        $('#catStatesId').trigger('change');
        $('#catTownsId').trigger('change');

        // Mostrar los campos de nuevo cliente para permitir editar
        $('.tr-new-customer').show();

        // Llamar a la API para obtener beneficiarios
        $.ajax({
            url: "api/customers/consultBeneficiaries.php",
            type: "POST",
            dataType: 'json',
            data: {
                value: selectedCustomer.id // Envía el ID del cliente
            },
            success: function(response) {
                // Verificamos que no haya errores en la respuesta
                if (!response.error) {
                    // Limpiar las filas de beneficiarios antes de agregar nuevos
                    $('#tableBeneficiary tr.tr-beneficiary').remove(); // Elimina todas las filas de beneficiarios

                    // Verificar si hay beneficiarios
                    if (response.length > 0) {
                        // Recorremos los beneficiarios y los agregamos a la tabla
                        response.forEach(function(beneficiary) {
                            const newRow = `
                                <tr class="tr tr-beneficiary">
                                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryName" value="${beneficiary.name || ''}" /></td>
                                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiarySurnames" value="${beneficiary.lastname || ''}" /></td>
                                    <td><input type="date" class="form-control datepicker control-beneficiary" name="BeneficiaryBirthdate" value="${beneficiary.birthdate ? beneficiary.birthdate.split('T')[0] : ''}" /></td>
                                    <td><input type="text" class="form-control control-beneficiary phone" name="BeneficiaryCelPhone" value="${beneficiary.phone || ''}" /></td>
                                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryRelationship" value="${beneficiary.relationship || ''}" /><input type="hidden" name="BeneficiaryCustomerID" value="${beneficiary.customerId || 0}" /></td>
                                    <td><button class="btn btn-danger btn-remove-beneficiary" type="button">-</button></td>
                                </tr>
                            `;
                            $('#tableBeneficiary').append(newRow);
                        });
                    } else {
                        // Si no hay beneficiarios, puedes agregar una fila por defecto o mostrar un mensaje.
                        const defaultRow = `
                            <tr class="tr tr-beneficiary">
                                <td colspan="6" class="text-center">No hay beneficiarios asignados a este cliente.</td>
                            </tr>
                        `;
                        $('#tableBeneficiary').append(defaultRow);
                    }
                } else {
                    alert(response.error); // Mostrar error si existe
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error("Error en la solicitud:", textStatus, errorThrown);
                alert("Hubo un problema al obtener los beneficiarios.");
            }
        });
    });

    $.ajax({
        url: 'api/general/stateTowns.php', 
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            if (Array.isArray(data)) {
                // Llenar el select de estados
                $.each(data, function(index, item) {
                    $('#StateAddressCompany').append(new Option(item.state.state_name, item.state.id));
                    $('#catStatesId').append(new Option(item.state.state_name, item.state.id));
                });
            } else {
                console.error("Error en la respuesta de la API: ", data.error);
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.error("Error en la solicitud: ", textStatus);
        }
    }); 
    

    // Obtener ciudades según el estado seleccionado
    $('#catStatesId').change(function() {
        const stateId = $(this).val();
        $('#catTownsId').empty().append(new Option("Seleccione una ciudad", ""));

        if (stateId) {
            // Buscar las ciudades en el JSON que se obtuvieron previamente
            $.ajax({
                url: 'api/general/stateTowns.php', // Vuelve a llamar a la API para obtener la lista completa
                type: 'GET',
                dataType: 'json',
                success: function(data) {
                    if (Array.isArray(data)) {
                        // Filtrar las ciudades del estado seleccionado
                        const towns = data.find(item => item.state.id == stateId)?.towns_list || [];
                        $.each(towns, function(index, town) {
                            $('#catTownsId').append(new Option(town.town_name, town.id));
                        });
                    } else {
                        console.error("Error en la respuesta de la API: ", data.error);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error("Error en la solicitud: ", textStatus);
                }
            });
        }
    });

    // Obtener ciudades según el estado seleccionado
    $('#StateAddressCompany').change(function() {
        const stateId = $(this).val();
        $('#CityAddressCompany').empty().append(new Option("Seleccione una ciudad", ""));

        if (stateId) {
            // Buscar las ciudades en el JSON que se obtuvieron previamente
            $.ajax({
                url: 'api/general/stateTowns.php', // Vuelve a llamar a la API para obtener la lista completa
                type: 'GET',
                dataType: 'json',
                success: function(data) {
                    if (Array.isArray(data)) {
                        // Filtrar las ciudades del estado seleccionado
                        const towns = data.find(item => item.state.id == stateId)?.towns_list || [];
                        $.each(towns, function(index, town) {
                            $('#CityAddressCompany').append(new Option(town.town_name, town.id));
                        });
                    } else {
                        console.error("Error en la respuesta de la API: ", data.error);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error("Error en la solicitud: ", textStatus);
                }
            });
        }
    });
    // Manejar el evento de eliminación de beneficiarios
    $('#tableBeneficiary').on('click', '.btn-remove-beneficiary', function() {
        // Contar el número de filas en la tabla, excluyendo la cabecera
        var rowCount = $('#tableBeneficiary tr.tr-beneficiary').length;

        // Si hay más de una fila, permitir la eliminación
        if (rowCount > 1) {
            $(this).closest('tr').remove(); // Eliminar la fila correspondiente
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se puede eliminar el único beneficiario.',
            });
        }

        // Después de eliminar, actualizamos el número de filas
        actualizarContadorBeneficiarios();
    });

    // Función para actualizar el contador de beneficiarios
    function actualizarContadorBeneficiarios() {
        var rowCount = $('#tableBeneficiary tr.tr-beneficiary').length;

        // Mostrar el botón de agregar si hay menos de 4 beneficiarios
        if (rowCount < 4) {
            $('#btnAddBeneficiary').prop('disabled', false); // Habilitar el botón de agregar
        } else {
            $('#btnAddBeneficiary').prop('disabled', true); // Deshabilitar el botón si ya hay 4
        }
    }

    function getBeneficiarios() {
        const beneficiarios = [];
        
        $('#tableBeneficiary .tr-beneficiary').each(function() {
            const name = $(this).find('input[name="BeneficiaryName"]').val();
            const surnames = $(this).find('input[name="BeneficiarySurnames"]').val();
            const birthdate = $(this).find('input[name="BeneficiaryBirthdate"]').val();
            const phone = $(this).find('input[name="BeneficiaryCelPhone"]').val();
            const relationship = $(this).find('input[name="BeneficiaryRelationship"]').val();

            // Solo agrega beneficiarios que tengan un nombre
            if (name) {
                beneficiarios.push({
                    name,
                    surnames,
                    birthdate,
                    phone,
                    relationship
                });
            }
        });
        
        return beneficiarios;
    }

    let maintenanceCost = 0;
    const ashDepositCost = 920;

    $.ajax({
        url: 'api/purchases/maintenance.php', // Cambia esto a la ruta correcta de tu archivo
        type: 'GET',
        dataType: 'json', // Espera una respuesta en JSON
        success: function(data) {
            // Aquí puedes manejar la respuesta
            if (data.error) {
                $('#resultado').html("Error: " + data.error);
            } else {
                // Almacena el costo de mantenimiento
                if (isShared == 'Familiar') {
                maintenanceCost = data.cost; 
                }else {
                maintenanceCost = data.shared_cost;  
                }
                $('#maintenance').text(maintenanceCost); // Mostrar costo en la etiqueta
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            // Manejar errores de la solicitud
            $('#resultado').html("Error en la solicitud: " + textStatus);
        }
    });

    if (isShared == 'Individual') {
        // Activar el checkbox de mantenimiento
        var ckMaintenance = document.getElementById('ckMaintenance');
        ckMaintenance.checked = true;
        ckMaintenance.disabled = true; // No permitir quitarlo
    }

    // Actualizar el contenido y el valor del campo oculto cuando el checkbox es seleccionado
    $('#ckMaintenance').change(function() {
        if ($(this).is(':checked')) {
            // Asignar el costo al campo oculto
            $('#CheckMaintenanceFee').val(maintenanceCost); // Asignar costo al campo oculto
        } else {
            // Limpiar el campo oculto si no está seleccionado
            $('#CheckMaintenanceFee').val('False'); // Restablecer campo oculto
        }
    });

    // Manejar el checkbox de depósito de cenizas
    $('#ckAshDeposit').change(function() {
        if ($(this).is(':checked')) {
            // Mostrar costo en la etiqueta y asignar el valor al campo oculto
            $('#ashDeposit').text(ashDepositCost); // Mostrar costo de cenizas
            $('#CheckAshDepositFee').val(ashDepositCost); // Asignar costo al campo oculto
        } else {
            // Limpiar el contenido y el campo oculto si no está seleccionado
            $('#ashDeposit').text(''); // Limpiar costo
            $('#CheckAshDepositFee').val('False'); // Restablecer campo oculto
        }
    });
    const enganche = parseFloat(<?php echo json_encode($enganche); ?>);
    const totalFinal = parseFloat(<?php echo json_encode($totalFinal); ?>);
    const selectedPaymentValue = parseInt(<?php echo json_encode($selectedPaymentValue); ?>); // Agrega esta línea

    $('.typepay').change(function() {
        var row = $(this).closest('tr'); // Obtiene la fila actual

        // Variables de referencia para total final y enganche
        const totalFinal = Math.round(parseFloat($('#totalAmountLabel').text().replace(/[^0-9.-]+/g, "")) * 100) || 0;
        const enganche = Math.round(parseFloat($('#initialPaymentLabel').text().replace(/[^0-9.-]+/g, "")) * 100) || 0;

        // Obtener el valor seleccionado para el plan de pago
        var selectedPaymentValue = parseInt($('#paymentPlanLabel').val());

        // Habilitar o deshabilitar los inputs según el estado del checkbox
        if ($(this).is(':checked')) {
            // Contar los checkboxes seleccionados
            var checkedCount = $('.typepay:checked').length;

            // Si se seleccionan más de 2, deseleccionar el checkbox actual
            if (checkedCount > 2) {
                $(this).prop('checked', false);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: "Solo puedes seleccionar un máximo de 2 opciones."
                });
            } else {
                // Habilitar los inputs de la fila actual
                row.find('input[type="number"], input[type="text"]').prop('disabled', false);

                // Dividir la cantidad si hay 2 seleccionados
                if (checkedCount === 2) {
                    var totalAmount = selectedPaymentValue === 1 ? totalFinal : enganche;
                    var dividedAmount = (totalAmount / 2); // Divide la cantidad sin redondear

                    // Asigna el valor dividido a los inputs de los checkboxes seleccionados
                    $('.typepay:checked').each(function() {
                        $(this).closest('tr').find('input[type="number"]').val((dividedAmount / 100).toFixed(2));
                    });
                } else {
                    // Si hay solo un checkbox seleccionado, asigna el total
                    var totalAmount = selectedPaymentValue === 1 ? totalFinal : enganche;
                    row.find('input[type="number"]').val((totalAmount / 100).toFixed(2));
                }

                // Habilitar el input de archivo si es Transferencia o Depósito en Efectivo
                if ($(this).val() === "3" || $(this).val() === "5") {
                    row.find('input[type="file"]').prop('disabled', false);
                }
            }
        } else {
            // Deshabilitar los inputs de la fila actual
            row.find('input[type="number"], input[type="text"]').prop('disabled', true).val(''); // Limpiar valores

            // Deshabilitar el input de archivo si no es Transferencia o Depósito en Efectivo
            if ($(this).val() === "3" || $(this).val() === "5") {
                row.find('input[type="file"]').prop('disabled', true);
            }
        }

        // Actualizar montos si se edita uno de los campos para mantener la suma correcta
        $('input[type="number"]').on('input', function() {
            var checkedCount = $('.typepay:checked').length;

            // Si hay dos métodos seleccionados, recalcular el monto restante
            if (checkedCount === 2) {
                var totalAmount = selectedPaymentValue === 1 ? totalFinal : enganche;

                // Obtener el otro campo seleccionado para actualizar
                var otherInput = $('.typepay:checked').not($(this).closest('tr').find('input[type="checkbox"]')).closest('tr').find('input[type="number"]');

                // Calcular el monto restante y actualizar el otro input
                var currentAmount = Math.round(parseFloat($(this).val()) * 100) || 0;
                var remainingAmount = (totalAmount - currentAmount) / 100;
                otherInput.val(remainingAmount.toFixed(2));
            }
        });
    });


    
        
    
        let beneficiaryCount = 1; // Contador de beneficiarios (comenzamos con 1 ya que ya hay una fila inicial)

    $('#btnAddBeneficiary').on('click', function() {
        // Contar el número de filas en la tabla, excluyendo la cabecera
        var rowCount = $('#tableBeneficiary tr.tr-beneficiary').length;

        // Verificar si el número de filas es menor a 4
        if (rowCount < 4) {
            // Crear una nueva fila con los campos vacíos
            const newRow = `
                <tr class="tr tr-beneficiary">
                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryName" value="" /></td>
                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiarySurnames" value="" /></td>
                    <td><input type="date" class="form-control datepicker control-beneficiary" name="BeneficiaryBirthdate" value="" /></td>
                    <td><input type="text" class="form-control control-beneficiary phone" name="BeneficiaryCelPhone" value="" /></td>
                    <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryRelationship" value="" /><input type="hidden" name="BeneficiaryCustomerID" value="0" /></td>
                    <td><button class="btn btn-danger btn-remove-beneficiary" type="button">-</button></td>
                </tr>
            `;
            $('#tableBeneficiary').append(newRow); // Agregar la nueva fila

            // Actualizar el contador después de agregar
            actualizarContadorBeneficiarios();
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pueden agregar más de 4 beneficiarios.',
            });
        }
    });

    $.ajax({
        url: 'api/general/civilStatus.php', 
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            if (Array.isArray(data)) {
                // Llenar el select de estado civil
                $.each(data, function(index, item) {
                    $('#CivilStatus').append(new Option(item.civilStatus, item.id));
                });
            } else {
                console.error("Error en la respuesta de la API: ", data.error);
            }
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.error("Error en la solicitud: ", textStatus);
        }
    });

   
    $('#CivilStatus').change(function() {
        const selectedCivilStatusId = $(this).val();
    });
   
    $('#btnQuotation').on('click', function() {
        // Variables para verificar campos
        const missingFields = []; // Array para almacenar campos faltantes
        const apellidoPaterno = $('#PSurname').val();
        const apellidoMaterno = $('#MSurname').val();
        const nombres = $('#Name').val();
        const telefonoParticular = $('#CelPhone').val();
        const correoElectronico = $('#Email').val();
        const customerId = $('#CustomerID').val();
        
        // Referencias
        const referenceCustomer1 = $('#ReferenceCustomer1').val();
        const referenceCustomerPhone1 = $('#ReferenceCustomerPhone1').val();

        // Validar campos requeridos
        if (!apellidoPaterno) missingFields.push("Apellido Paterno*");
        if (!apellidoMaterno) missingFields.push("Apellido Materno*");
        if (!nombres) missingFields.push("Nombres*");
        if (!telefonoParticular) missingFields.push("Teléfono Particular*");
        if (!correoElectronico) missingFields.push("Correo Electrónico*");

        // Validar referencias
        if (!referenceCustomer1) missingFields.push("Referencia Nombre*");
        if (!referenceCustomerPhone1) missingFields.push("Referencia Teléfono*");

        // Validar método de pago inicial
        const paymentMethods = $('.typepay:checked'); // Obtener métodos de pago seleccionados
        if (paymentMethods.length === 0) {
            missingFields.push("Seleccionar al menos un método de pago inicial*");
        }

        // Obtiene el arreglo de beneficiarios
        const beneficiarios = getBeneficiarios();
        
        // Validar que exista al menos un beneficiario
        if (beneficiarios.length === 0) {
            missingFields.push("Debe agregar al menos un beneficiario*");
        }

        // Si hay campos faltantes, mostrar SweetAlert
        if (missingFields.length > 0) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Faltan los siguientes campos: ' + missingFields.join(', '),
            });
            return; // Detiene la ejecución si hay campos faltantes
        }

        // Captura los valores de condiciones económicas
        const paymentPlan = $('#paymentPlanLabel').text();
        const cryptKey = $('#cryptKeyLabel').text();
        const level = $('#levelLabel').text();
        const area = $('#areaLabel').text();
        const zone = $('#zoneLabel').text();
        const totalAmount = parseFloat($('#totalAmountLabel').text().replace(/[^0-9.-]+/g,"")) || 0; 
        const appliedDiscount =  parseFloat($('#appliedDiscountLabel').text().replace(/[^0-9.-]+/g,"")) || 0;
        const initialPayment = parseFloat($('#initialPaymentLabel').text().replace(/[^0-9.-]+/g,"")) || 0;
        const balance = parseFloat($('#balanceLabel').text().replace(/[^0-9.-]+/g,"")) || 0 ;

        const enganche = initialPayment;
        const totalFinal = totalAmount;

        // Validar montos de los métodos de pago seleccionados
        if (!validatePaymentAmounts(totalFinal, enganche)) {
            return; // Detener la ejecución si la validación falla
        }

        // Serializa los datos del formulario
        var formData = new FormData($('#PurchaseRequestCreateForm')[0]);

        // Excluir campos específicos de beneficiario
        formData.delete('BeneficiaryName');
        formData.delete('BeneficiarySurnames');
        formData.delete('BeneficiaryBirthdate');
        formData.delete('BeneficiaryCelPhone');
        formData.delete('BeneficiaryRelationship');
        formData.delete('BeneficiaryCustomerID');

        // Agrega el arreglo de beneficiarios al FormData
        beneficiarios.forEach((beneficiary, index) => {
            formData.append(`beneficiaries[${index}][name]`, beneficiary.name);
            formData.append(`beneficiaries[${index}][surnames]`, beneficiary.surnames);
            formData.append(`beneficiaries[${index}][birthdate]`, beneficiary.birthdate);
            formData.append(`beneficiaries[${index}][phone]`, beneficiary.phone);
            formData.append(`beneficiaries[${index}][relationship]`, beneficiary.relationship);
            formData.append(`beneficiaries[${index}][customerId]`, customerId);
        });

        const payments = [];
        paymentMethods.each(function() {
            const row = $(this).closest('tr');
            const amount = row.find('input[type="number"]').val();
            const paymentType = parseInt($(this).val());

            // Solo agrega si se ha ingresado un monto
            if (amount) {
                payments.push({
                    paymentAmount: parseFloat(amount), // Convierte a número
                    concept: "Pago inicial", // Puedes ajustar esto si es necesario
                    typePaymentId: paymentType,
                    currencyId: 1 // Ajusta según tu necesidad
                });
            }
        });

        // Agrega el arreglo de pagos al FormData
        payments.forEach((payment, index) => {
            formData.append(`payments[${index}][paymentAmount]`, payment.paymentAmount);
            formData.append(`payments[${index}][concept]`, payment.concept);
            formData.append(`payments[${index}][typePaymentId]`, payment.typePaymentId);
            formData.append(`payments[${index}][currencyId]`, payment.currencyId);
        });
        formData.append('paymentPlan', paymentPlan);
        formData.append('cryptKey', cryptKey);
        formData.append('level', level);
        formData.append('area', area);
        formData.append('zone', zone);
        formData.append('totalAmount', totalAmount);
        formData.append('appliedDiscount', appliedDiscount);
        formData.append('initialPayment', initialPayment);
        formData.append('balance', balance);

        // Envía la solicitud AJAX
        $.ajax({
            url: 'api/purchases/sendPurchase.php', // Cambia esto por la ruta a tu archivo PHP
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function(response) {
                // Maneja la respuesta del servidor aquí
                alert('Solicitud enviada con éxito: ' + response);
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert('Error al enviar la solicitud: ' + textStatus);
            }
        });

        // Función para validar los montos de los métodos de pago
        
    });
    function validatePaymentAmounts(total, enganche) {
        let sum = 0;
        
        // Sumar las cantidades de los inputs seleccionados
        if ($('#amount_card').is(':enabled')) {
            sum += parseFloat($('#amount_card').val()) || 0;
        }
        if ($('#amount_transfer').is(':enabled')) {
            sum += parseFloat($('#amount_transfer').val()) || 0;
        }
        if ($('#amount_cash_deposit').is(':enabled')) {
            sum += parseFloat($('#amount_cash_deposit').val()) || 0;
        }
        if ($('#amount_cash').is(':enabled')) {
            sum += parseFloat($('#amount_cash').val()) || 0;
        }

        // Verificar que la suma no exceda el enganche o total final
        if (selectedPaymentValue === 1 && sum > total) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'La suma de los montos no puede exceder el total final.',
            });
            return false;
        } else if (selectedPaymentValue !== 1 && sum > enganche) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'La suma de los montos no puede exceder el enganche.',
            });
            return false;
        }

        if (selectedPaymentValue === 1 && sum < total) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'La suma de los montos no puede ser menor que el total final.',
            });
            return false;
        } else if (selectedPaymentValue !== 1 && sum < enganche) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'La suma de los montos no puede ser menor que el enganche.',
            });
            return false;
        }

        return true;
    }


});
</script>
