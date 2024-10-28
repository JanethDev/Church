<!-- Primero incluye jQuery -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Luego incluye Select2 -->
<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.4.0/jspdf.umd.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>

<style>
 
</style>
<?php
require_once '../../init.php';
require_once('auth/session.php');
    // Recibe los valores enviados a través de AJAX
    $CustomerId = isset($_POST['customerId']) ? $_POST['customerId'] : '0';
    $fechaUltima = new DateTime();

    // Suma los meses de las mensualidades a la fecha actual
    $fechaUltima->modify("+17months");

    // Fecha actual (inicio)
    $fechaPrimera = new DateTime();
    $fechaPrimera->modify("+1 months");

    // Calcular la diferencia entre las dos fechas
    $diferencia = $fechaPrimera->diff($fechaUltima);

    // Convertir la diferencia en días
    $diasDiferencia = $diferencia->days;

    // Calcular la cantidad de semanas, usando ceil para redondear hacia arriba
    $semanasDiferencia = ceil($diasDiferencia / 7);

    // Formatear las fechas si necesitas mostrar la primera y última fecha
    $diaPrimerPago = $fechaPrimera->format('d');
    $mesPrimerPago = $fechaPrimera->format('m');
    $yPrimerPago = $fechaPrimera->format('Y');

    $diaUltimoPago = $fechaUltima->format('d');
    $mesUltimoPago = $fechaUltima->format('m');
    $yUltimoPago = $fechaUltima->format('Y');

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
                            
                            <td>VENDEDOR</td>
                            
                        </tr>
                        <tr>
                            
                            <td>SERVIDOR</td>
                            
                        </tr>
                        <tr>
                            
                            <td>
                                <?php echo($name); ?>
                            </td>
                            

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
                               
                            </td>
                        </tr>
                       
                        <tr class="tr-new-customer" >
                            <td>APELLIDO PATERNO*</td>
                            <td>APELLIDO MATERNO</td>
                            <td>NOMBRES*</td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td><input type="text" class="form-control control-customer-new" id="PSurname" name="PSurname" /></td>
                            <td><input type="text" class="form-control control-customer-new" id="MSurname" name="MSurname" /></td>
                            <td><input type="text" class="form-control control-customer-new" id="Name" name="Name" /></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                    <tr><td colspan="3" style="text-align:center;"><strong>DOMICILIO PARTICULAR</strong></td></tr>
                        <tr>
                            <td style="width:50%">CALLE, AV., BLDV. CALZ.*</td>
                            <td style="width:25%">NUMERO* </td>
                            <td style="width:25%">INTERIOR </td>
                            
                        </tr>
                        <tr>
                            <td ><input type="text" class="form-control" id="address" name="address" value="" /></td>
                            <td ><input type="text" class="form-control" id="house_number" name="house_number" value="" /></td>
                            <td ><input type="text" class="form-control" id="apt_number" name="apt_number" value="" /></td>
                            
                        </tr>
                        <tr>
                            <td colspan="2">COLONIA*</td>
                            <td >CODIGO POSTAL* </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2"><input type="text" class="form-control" id="neighborhood" name="neighborhood" value="" /></td>
                            <td ><input type="text" class="form-control" id="zip_code" name="zip_code" value="" /></td>
                            
                        </tr>
                        <tr>
                            <td >ESTADO*</td>
                            <td >DELEGACIÓN </td>
                            <td >CIUDAD* </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <select class="form-control select2" id="catStatesId" name="catStatesId">
                                    <option value="">Seleccione un estado</option>
                                </select>
                            <td>
                                <input type="text" class="form-control" id="Deputation" name="Deputation" value="" />
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
                            <td><input type="text" class="form-control" id="social_reason" name="social_reason" /></td>
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
                            <td><input type="text" class="form-control control-reference" id="ReferenceCustomer1" name="ReferenceCustomer1" value="" />
                            <input type="hidden" class="form-control control-beneficiary" id="idReference1" name="idReference1" value="" /></td>
                            <td>TEL.*</td>
                            <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone1" name="ReferenceCustomerPhone1" value="" />
                            <input type="hidden" class="form-control control-beneficiary" id="idReference2" name="idReference2" value="" /></td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td><input type="text" class="form-control control-reference" id="ReferenceCustomer2" name="ReferenceCustomer2" value="" /></td>
                            <td>TEL.</td>
                            <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone2" name="ReferenceCustomerPhone2" value="" /></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" id="tableBeneficiary" style="background-color: white;margin-bottom: 0px;">
                        <tr><td colspan="6" style="text-align:center"><strong>BENEFICIARIOS</strong></td></tr>
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
                            <td><label id="paymentPlanLabel" style="display: none;"></label ><label id="paymentPlanLabelDesc"></label></td>
                            <td><label id="cryptKeyLabel"></label><label></label></td>
                            <td><label id="levelLabel"></label></td>
                            <td><label id="areaLabel"></label></td>
                            <td><label id="zoneLabel"></label></td>
                            <input type="hidden" name="cryptId" id="cryptId" value="" />
                            <input type="hidden" name="cryptSpaces" id="cryptSpaces" value="" />
                            <input type="hidden" name="discountId" id="discountId" value="" />
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
                        <td><label id="totalAmountLabel"></label></td>
                        <td><label id="appliedDiscountLabel"></label></td>
                        <td>
                            <label id="initialPaymentLabel">
                               
                            </label>
                        </td>
                        <td><label id="balanceLabel"></label></td>
                    </tr>
                    <tr>
                        
                            <td colspan="4">
                                EL SALDO SERÁ LIQUIDADO EN &nbsp;
                                <strong></strong> 
                                &nbsp;EN ABONOS: 
                                <!-- Checkbox para abonos mensuales (seleccionado por defecto) -->
                                &nbsp;&nbsp;<input type="checkbox" name="payment_type" value="mensuales" id="mensuales_checkbox" checked>&nbsp;MENSUALES 

                                <!-- Checkbox para abonos semanales -->
                                &nbsp;&nbsp;<input type="checkbox" name="payment_type" value="semanales" id="semanales_checkbox">&nbsp;SEMANALES 

                                <!-- Mostrar el valor calculado como texto -->
                                &nbsp;&nbsp;POR LA CANTIDAD DE: 
                                &nbsp;$ &nbsp;<span id="payment_amount">0.00</span> M.N.
                            </td>
                        
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
                            <td><?php echo $diaPrimerPago; ?></td>
                            <td><?php echo $mesPrimerPago; ?></td>
                            <td><?php echo $yPrimerPago; ?></td>
                            <td colspan="2">Y EL ULTIMO EN</td>
                            <td><?php echo $diaUltimoPago; ?></td>
                            <td><?php echo $mesUltimoPago; ?></td>
                            <td><?php echo $yUltimoPago; ?></td>
                        </tr>
                    </table>
                   
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr><td colspan="3" style="text-align:center"><strong>ADICIONAL</strong></td></tr>
                        <tr>
                            <td colspan="" style="width:30%">Cuota de mantenimiento anual</td>
                            <td style="width:30%">Deposito de cenizas</td>
                            <td>Otro</td>
                        </tr>
                        <tr>
                            <td colspan="">
                                <input type="checkbox" id="ckMaintenance" class="check-box" style="width: 30px; height: 30px" />
                                <label style="position:absolute;margin-top:4px;margin-left:15px"> 
                                    <span id="maintenance">$ </span> <span id="maintenanceIsShared"> Incluido</span> 
                                </label> 
                                <input type="hidden" name="inMaintenance" id="CheckMaintenanceFee" value="False" />
                            </td>
                            <td>
                                <input type="checkbox" id="ckAshDeposit" class="check-box" style="width: 30px; height: 30px;" />
                                <label style="position: absolute; margin-top: 4px; margin-left: 15px">$ <span id="ashDeposit"></span> M.N.</label>
                                <input type="hidden" name="inAshDeposit" id="CheckAshDepositFee" value="False" />
                            </td>
                            <td>
                                <input type="checkbox" id="ckOtherFee" class="check-box" style="width: 30px; height: 30px;" />
                                <label style="margin-left: 15px">
                                    $ <input type="number" id="otherFeeAmount" class="form-control" placeholder="Ingresa la cantidad" style="width:150px; display:none; margin-left: 15px;" step="0.01" min="0" />M.N.</label>
                                <input type="hidden" name="inOtherFee" id="CheckOtherFee" value="False" />
                                
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
                            <!--<td>Comprobante</td>-->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay1" class="typepay" name="TypePay" value="1"></td>
                            <td>CHEQUE</td>
                            <td><input type="number" id="amount_check" class="form-control" name="amount_check" placeholder="Cantidad" disabled></td>
                            <td><input type="text" id="check_number" class="form-control" name="check_number" placeholder="No. de Cheque" disabled></td>
                            <td><input type="text" id="account_number" class="form-control" name="account_number" placeholder="No. de Cuenta" disabled></td>
                            <td><input type="text" id="bank" class="form-control" name="bank" placeholder="Banco" disabled></td>
                            <!--<td></td>  No input file para Cheque -->
                        </tr>
                        <tr style="margin-bottom:5px">
                            <td><input type="checkbox" id="TypePay2" class="typepay" name="TypePay" value="2"></td>
                            <td>T. DE CREDITO/DEBITO</td>
                            <td><input type="number" id="amount_card" class="form-control" name="amount_card" placeholder="Cantidad" disabled></td>
                            <td><input type="text" id="card_number" class="form-control" name="card_number" placeholder="No. de Cheque" disabled></td>
                            <td><input type="text" id="account_card" class="form-control" name="account_card" placeholder="No. de Cuenta" disabled></td>
                            <td><input type="text" id="bank_card" class="form-control" name="bank_card" placeholder="Banco" disabled></td>
                             <!--<td></td> No input file para T. de Crédito/Debito -->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay3" class="typepay" name="TypePay" value="3"></td>
                            <td>TRANSFERENCIA</td>
                            <td><input type="number" id="amount_transfer" class="form-control" name="amount_transfer" placeholder="Cantidad" disabled></td>
                            <td colspan="3"></td>
                            <!--<td><input type="file" class="form-control typepay3" id="TicketTransfer" name="TicketTransfer" disabled></td> Input file para Transferencia -->
                        </tr>
                        <tr>
                            <td><input type="checkbox" id="TypePay5" class="typepay" name="TypePay" value="5"></td>
                            <td>DEPOSITO EN EFECTIVO</td>
                            <td><input type="number" id="amount_cash_deposit" class="form-control" name="amount_cash_deposit" placeholder="Cantidad" disabled></td>
                            <td colspan="3"></td>
                             <!-- <td><input type="file" class="form-control typepay5" id="TicketCashDeposit" name="TicketCashDeposit" disabled></td>Input file para Depósito en Efectivo -->
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

<!-- Botones de Acción -->
<div class="row" style="padding-top:15px;">
    <div class="col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-primary" style="width:100%;" id="btnRegresar">Volver</button>
    </div>
    <div class="offset-md-6 col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-default" style="float:right;width:100%;" id="btnPurchase">Confirmar compra</button>
        <button class="btn btn-danger" style="float:right;width:100%;" id="btnCancel">Cancelar compra</button>
    </div>
</div>

<script>
$(document).ready(function() {
    $('.select2').select2();
    const customerID = <?php echo $customerId; ?>;
   
    $.ajax({
        url: 'api/customers/getCustomerData.php', // Cambia esto por la URL de tu API que devuelva los datos del cliente
        type: 'POST',
        data: { customerID: customerID },
        dataType: 'json',
        success: function(data) {
            if (data && data.customer) {
                // Poblamos los campos del formulario con los datos recibidos
                $('#PSurname').val(data.customer.PSurname);
                $('#MSurname').val(data.customer.MSurname);
                $('#Name').val(data.customer.Name);
                $('#address').val(data.customer.address);
                $('#house_number').val(data.customer.house_number);
                $('#apt_number').val(data.customer.apt_number);
                $('#neighborhood').val(data.customer.neighborhood);
                $('#zip_code').val(data.customer.zip_code);
                $('#Deputation').val(data.customer.Deputation);
                $('#CelPhone').val(data.customer.CelPhone);
                $('#Email').val(data.customer.Email);
                $('#social_reason').val(data.customer.social_reason);
                $('#RFCCURP').val(data.customer.RFCCURP);
                $('#DateOfBirth').val(data.customer.DateOfBirth);
                $('#CityOfBirth').val(data.customer.CityOfBirth);
                $('#CivilStatus').val(data.customer.CivilStatus);
                $('#Occupation').val(data.customer.Occupation);
                
                // Llenar selects de Estado y Ciudad si están disponibles en los datos
                if (data.customer.catStatesId) {
                    $('#catStatesId').val(data.customer.catStatesId).trigger('change');
                }
                if (data.customer.catTownsId) {
                    $('#catTownsId').val(data.customer.catTownsId).trigger('change');
                }
                
                // Puedes ajustar más campos si tienes datos adicionales.
            } else {
                console.error('No se encontraron datos para el cliente especificado.');
            }
        },
        error: function(xhr, status, error) {
            console.error('Error al obtener los datos del cliente:', error);
        }
    });
    
    $('#btnRegresar').click(function(e) {
        e.preventDefault(); // Evita que el botón envíe un formulario o recargue la página
        window.location.href = 'cotizaciones'; // Redirige sin parámetros
    });

    function applyPhoneMask() {
        $('.phone').inputmask("(999) 999-9999"); // Aplica la máscara de teléfono
    }

    // Llama a la función para aplicar la máscara de teléfono en todos los campos de teléfono al cargar la página
    applyPhoneMask();

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

    $('#ckOtherFee').change(function() {
        if ($(this).is(':checked')) {
            // Mostrar el input para ingresar el monto
            $('#otherFeeAmount').show().addClass('d-inline-block');
            $('#CheckOtherFee').val($('#otherFeeAmount').val()); // Asignar el valor ingresado en el campo oculto
        } else {
            // Ocultar el input y limpiar el valor
            $('#otherFeeAmount').hide().removeClass('d-inline-block');
            $('#otherFeeAmount').val(''); // Limpiar el valor del input
            $('#CheckOtherFee').val('False'); // Cambiar el valor del campo hidden a False
        }
    });

    // Aplicar la máscara de entrada para números
    $('#otherFeeAmount').inputmask({
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
    

   
    $('#otherFeeAmount').on('input', function() {
        var amount = parseFloat($(this).val().replace(/,/g, ''));
        
        // Validar si el monto es cero, negativo o NaN
        if (amount <= 0 || isNaN(amount)) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'El monto no puede ser cero o negativo.',
            });
            $(this).val(''); // Limpiar el valor si es incorrecto
            $('#CheckOtherFee').val('False'); // Cambiar el valor del campo hidden a False si el monto es inválido
        } else {
            // Solo si el checkbox está marcado y el valor es válido, actualizar el campo hidden
            if ($('#ckOtherFee').is(':checked')) {
                $('#CheckOtherFee').val(amount); // Asignar el valor al campo hidden
            }
        }
    });
    $('#mensuales_checkbox').prop('checked', true);
        calculatePaymentAmount();  // Llama a la función para calcular y mostrar el monto inicial

        // El resto de tu código, incluyendo el listener de cambio en los checkboxes
        $('input[name="payment_type"]').change(function() {
            // Cambia el chequeo de otros checkboxes según el tipo de pago seleccionado
            if ($(this).attr('id') === 'mensuales_checkbox') {
                $('#semanales_checkbox').prop('checked', false);
            } else {
                $('#mensuales_checkbox').prop('checked', false);
            }

            // Recalcula el monto según la opción seleccionada
            calculatePaymentAmount();
        });
    

    function calculatePaymentAmount() {
        var paymentType = $('input[name="payment_type"]:checked').val();
        var totalFinal = parseFloat(<?= round($saldo, 2); ?>); // Redondear saldo a dos decimales en PHP
        var semanasDiferencia = <?= $semanasDiferencia; ?>;
        var mensualidades = parseFloat(<?= round($mensualidades, 2); ?>); // Redondear mensualidad a dos decimales en PHP

        if (paymentType === 'semanales') {
            var totalPorSemana = Math.round(totalFinal / semanasDiferencia);
            $('#payment_amount').text(totalPorSemana.toFixed(2)); // Redondear a entero y mostrar como XXXX.00
        } else {
            $('#payment_amount').text(Math.round(mensualidades).toFixed(2)); // Redondear a entero y mostrar como XXXX.00
        }
    }

    // Seleccionar "mensuales" por defecto y calcular el monto
    $('#mensuales_checkbox').prop('checked', true);
    calculatePaymentAmount();  // Calcular el valor inicial

    // Al cambiar cualquiera de los checkboxes
    $('input[name="payment_type"]').change(function() {
        if ($(this).attr('id') === 'mensuales_checkbox') {
            $('#semanales_checkbox').prop('checked', false);
        } else {
            $('#mensuales_checkbox').prop('checked', false);
        }

        // Calcular el monto de acuerdo a la selección
        calculatePaymentAmount();
    });

    function formatDateToYMD(dateString) {
        var parts = dateString.split('/');
        // Cambia el orden de la fecha a YYYY-MM-DD
        return `${parts[2]}-${parts[1]}-${parts[0]}`;
    }

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
            const idBeneficiary = $(this).find('input[name="idBeneficiary"]').val();
            const name = $(this).find('input[name="BeneficiaryName"]').val();
            const surnames = $(this).find('input[name="BeneficiarySurnames"]').val();
            const birthdate = $(this).find('input[name="BeneficiaryBirthdate"]').val();
            const phone = $(this).find('input[name="BeneficiaryCelPhone"]').val();
            const relationship = $(this).find('input[name="BeneficiaryRelationship"]').val();

            // Solo agrega beneficiarios que tengan un nombre
            if (name) {
                beneficiarios.push({
                    idBeneficiary,
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

    const totalFinal = 1000;
    const enganche = 1000;
    let maintenanceCost = 0; // Ajusta según tus necesidades
    const ashDepositCost = 920; // Usa tu constante ya definida
    let otherFeeAmount = 0;
    const selectedPaymentValue = 1000;

    // Función para actualizar el total del pago inicial
    function updateInitialPayment() {
        let initialPayment = selectedPaymentValue === 1 ? Math.round(totalFinal) : Math.round(enganche);

        if ($('#ckMaintenance').is(':checked')) {
            initialPayment += Math.round(maintenanceCost);
        }
        if ($('#ckAshDeposit').is(':checked')) {
            initialPayment += Math.round(ashDepositCost);
        }
        if ($('#ckOtherFee').is(':checked')) {
            const otherFeeValue = parseFloat($('#otherFeeAmount').val()) || 0;
            initialPayment += Math.round(otherFeeValue);
        }

        $('#initialPaymentLabel').text(`$${initialPayment.toFixed(2)} M.N.`);
    }

    // Listeners para checkboxes y campo de monto "Otro"
    $('#ckMaintenance, #ckAshDeposit, #ckOtherFee').change(updateInitialPayment);
    $('#otherFeeAmount').on('input', updateInitialPayment);


    // Ocultar "Incluido" al inicio y asegurar que el costo esté oculto
    $('#maintenanceIsShared').hide();
    $('#maintenance').hide();

   
// Manejar la lógica según el tipo de cripta seleccionada
    if (isShared === 'Individual') {
        // Si es una cripta individual, bloqueamos el checkbox, lo marcamos y mostramos que está incluido
        $('#ckMaintenance').prop('checked', true);
        $('#ckMaintenance').prop('disabled', true);
        $('#maintenanceIsShared').show();  // Mostrar "Incluido"
        $('#maintenance').hide();  // Ocultar el precio ya que es costo 0
        $('#CheckMaintenanceFee').val(0);  // Asignar valor 0 al campo oculto
    } else {
        // Si no es individual, permitimos marcar/desmarcar el checkbox
        $.ajax({
            url: 'api/purchases/maintenance.php', // Cambia esto a la ruta correcta de tu archivo
            type: 'GET',
            dataType: 'json', // Espera una respuesta en JSON
            success: function(data) {
                // Aquí puedes manejar la respuesta
                if (data.error) {
                    $('#resultado').html("Error: " + data.error);
                } else {
                    // Almacena el costo de mantenimiento según el tipo de cripta
                    if (isShared === 'Familiar') {
                        maintenanceCost = data.cost; 
                    } else {
                        maintenanceCost = 0;  
                    }

                    // Mostrar el costo de mantenimiento formateado
                    $('#maintenance').text(formatPrice(maintenanceCost)).show(); // Mostrar el costo con formato
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                // Manejar errores de la solicitud
                $('#resultado').html("Error en la solicitud: " + textStatus);
            }
        });

        // Mostrar el campo para el mantenimiento
        $('#maintenanceIsShared').hide();  // Ocultar el texto "Incluido"
        $('#ckMaintenance').prop('checked', false);
        $('#ckMaintenance').prop('disabled', false);
        $('#CheckMaintenanceFee').val(maintenanceCost);  // Asignar el valor del costo al campo oculto
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
                    text: "Solo puedes seleccionar un máximo de 2 opción."
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

        applyPhoneMask(); 

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
   
    function validatePaymentAmounts(total, enganche) {
        let sum = 0;
        if ($('#amount_check').is(':enabled')) {
            sum += Math.round(parseFloat($('#amount_check').val()) || 0);
        }
        if ($('#amount_card').is(':enabled')) {
            sum += Math.round(parseFloat($('#amount_card').val()) || 0);
        }
        if ($('#amount_transfer').is(':enabled')) {
            sum += Math.round(parseFloat($('#amount_transfer').val()) || 0);
        }
        if ($('#amount_cash_deposit').is(':enabled')) {
            sum += Math.round(parseFloat($('#amount_cash_deposit').val()) || 0);
        }
        if ($('#amount_cash').is(':enabled')) {
            sum += Math.round(parseFloat($('#amount_cash').val()) || 0);
        }

        sum = parseFloat(sum.toFixed(2));
        total = parseFloat(total.toFixed(2));
        enganche = parseFloat(enganche.toFixed(2));

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

        return true;
    }
    var diaPrimerPago = "<?php echo $diaPrimerPago; ?>";
    var mesPrimerPago = "<?php echo $mesPrimerPago; ?>";
    var yPrimerPago = "<?php echo $yPrimerPago; ?>";

    var diaUltimoPago = "<?php echo $diaUltimoPago; ?>";
    var mesUltimoPago = "<?php echo $mesUltimoPago; ?>";
    var yUltimoPago = "<?php echo $yUltimoPago; ?>";
  

    // IDs de ejemplo: ajusta según tus valores
    const purchaseId = 260;
    const customerId = 6267;

    // Realizar la llamada AJAX para obtener los datos del cliente y de la compra
    $.ajax({
        type: "POST",
        url: "purchaseById.php",
        data: { purchaseId: purchaseId, customerId: customerId },
        success: function(response) {
            const data = JSON.parse(response);
            if (data.length > 0) {
                populateForm(data[0]);  // Llama a la función para asignar valores al formulario
            } else {
                console.error('No se encontraron datos para el cliente y compra especificados.');
            }
        },
        error: function(xhr, status, error) {
            console.error('Error en la solicitud: ' + error);
        }
    });
});

</script>
