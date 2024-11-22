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
    $purchaseId= isset($_POST['purchaseId']) ? $_POST['purchaseId'] : '0';
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
                            <td>
                                <?php echo($name); ?>
                            </td>
                        </tr>
                        <tr>
                            
                            <td>SERVIDOR</td>
                            
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
                        
                        <tr class="tr-new-customer" >
                            <td>APELLIDO PATERNO*</td>
                            <td>APELLIDO MATERNO</td>
                            <td>NOMBRES*</td>
                        </tr>
                        <tr class="tr-new-customer">
                            <td><input type="text" class="form-control control-customer-new" id="PSurname" name="PSurname" disabled /></td>
                            <td><input type="text" class="form-control control-customer-new" id="MSurname" name="MSurname" disabled /></td>
                            <td><input type="text" class="form-control control-customer-new" id="Name" name="Name" disabled/></td>
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
                            <td ><input type="text" class="form-control" id="address" name="address" value="" disabled/></td>
                            <td ><input type="text" class="form-control" id="house_number" name="house_number" value="" disabled/></td>
                            <td ><input type="text" class="form-control" id="apt_number" name="apt_number" value="" disabled/></td>
                            
                        </tr>
                        <tr>
                            <td colspan="2">COLONIA*</td>
                            <td >CODIGO POSTAL* </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2"><input type="text" class="form-control" id="neighborhood" name="neighborhood" value="" disabled/></td>
                            <td ><input type="text" class="form-control" id="zip_code" name="zip_code" value="" disabled /></td>
                            
                        </tr>
                        <tr>
                            <td >ESTADO*</td>
                            <td >DELEGACIÓN </td>
                            <td >CIUDAD* </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <input type="text" class="form-control" id="catStatesId" name="catStatesId" value="" disabled/>
                            <td>
                                <input type="text" class="form-control" id="Deputation" name="Deputation" value="" disabled/>
                            </td>
                            <td>
                                <input type="text" class="form-control" id="catTownsId" name="catTownsId" value="" disabled/>
                            </td>
                            
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td >TELEFONO PARTICULAR*</td>
                            <td>CORREO ELECTRONICO*</td>  
                        </tr>
                        <tr>
                            <td><input type="text" class="form-control phone control-customer" id="CelPhone" name="CelPhone" disabled/></td>
                            <td><input type="text" class="form-control control-customer" id="Email" name="Email" disabled/></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white">
                        <tr>
                            <td style="width:25%">Razón Social</td>
                            <td><input type="text" class="form-control" id="social_reason" name="social_reason" disabled/></td>
                        </tr>
                        <tr>
                            <td style="width:25%">R.F.C o CURP</td>
                            <td><input type="text" class="form-control" id="RFCCURP" name="RFCCURP" disabled></td>
                        </tr>
                        <tr>
                            <td style="width:25%">FECHA DE NACIMIENTO</td>
                            <td><input type="date" class="form-control datepicker" id="DateOfBirth" name="DateOfBirth" value="" disabled/></td>
                        </tr>
                        <tr>
                            <td style="width:25%">LUGAR DE NACIMIENTO</td>
                            <td><input type="text" class="form-control" id="CityOfBirth" name="CityOfBirth" value="" disabled/></td>
                        </tr>
                        <tr>
                            <td style="width:25%">ESTADO CIVIL</td>
                            <td><input type="text" class="form-control" id="CivilStatus" name="CivilStatus" value="" disabled/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:25%">OCUPACION</td>
                            <td><input type="text" class="form-control" id="Occupation" name="Occupation" value="" disabled/></td>
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
                            <td style="width:70%"><input type="text" class="form-control" id="Company" name="Company" value="" disabled/></td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td style="width:70%">DOMICILIO</td>
                            <td>TELEFONO</td>
                            <td><input type="text" class="form-control phone" id="PhoneCompany" name="PhoneCompany" value="" disabled/></td>
                        </tr>
                        <tr>
                            <td style="width:70%"><input type="text" class="form-control" id="AddressCompany" name="AddressCompany" value="" disabled/></td>
                            <td>EXT.</td>
                            <td><input type="text" class="form-control" id="ExtPhoneCompany" name="ExtPhoneCompany" value="" disabled/></td>
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
                                <input type="text" class="form-control" id="StateAddressCompany" name="StateAddressCompany" value="" disabled/>
                            </td>
                            <td>
                                <input type="text" class="form-control" id="MunicipalityAddressCompany" name="MunicipalityAddressCompany" value="" disabled/>
                            </td>
                            <td>
                                <input type="text" class="form-control" id="CityAddressCompany" name="CityAddressCompany" value="" disabled/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">INGRESO PROMEDIO MENSUAL (incluye su conyuge)</td>
                            <td>
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="basic-addon1">$</span>
                                    </div>
                                    <input type="text" class="form-control" id="Income" name="Income" value="" aria-describedby="basic-addon1" disabled/>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="table table-bordered" style="background-color: white;margin-bottom: 0px;">
                        <tr><td colspan="4" style="text-align:center"><strong>REFERENCIAS</strong></td></tr>
                        <tr>
                            <td>1*</td>
                            <td><input type="text" class="form-control control-reference" id="ReferenceCustomer1" name="ReferenceCustomer1" value="" disabled/>
                            <input type="hidden" class="form-control control-beneficiary" id="idReference1" name="idReference1" value="" disabled/></td>
                            <td>TEL.*</td>
                            <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone1" name="ReferenceCustomerPhone1" value="" disabled/>
                            <input type="hidden" class="form-control control-beneficiary" id="idReference2" name="idReference2" value="" disabled/></td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td><input type="text" class="form-control control-reference" id="ReferenceCustomer2" name="ReferenceCustomer2" value="" disabled/></td>
                            <td>TEL.</td>
                            <td><input type="text" class="form-control control-reference phone" id="ReferenceCustomerPhone2" name="ReferenceCustomerPhone2" value="" disabled/></td>
                        </tr>
                    </table>
  
                    <table class="table table-bordered" id="tableBeneficiary" style="background-color: white;margin-bottom: 0px;"> 
                       
                        <thead> 
                            <tr><td colspan="6" style="text-align:center"><strong>BENEFICIARIOS</strong></td></tr>
                            <tr>
                                <td>Nombres</td>
                                <td>Apellidos</td>
                                <td>Fecha nac.</td>
                                <td>Teléfono</td>
                                <td>Parentesco</td>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Las filas se generarán dinámicamente aquí -->
                        </tbody>
                    
                    </table>

                    <table class="table table-bordered" id="economicConditions" style="background-color: white;margin-bottom: 0px;">
                        <tr>
                            <td colspan="5" style="text-align:center"><strong>CONDICIONES ECONOMICAS DE LA OPERACION</strong></td>
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
                                
                                <label style="position:absolute;margin-top:4px;margin-left:15px"> 
                                    <span id="maintenance">$ </span> <span id="maintenanceIsShared"> M.N </span> 
                                </label> 
                                <input type="hidden" name="inMaintenance" id="CheckMaintenanceFee" value="False" />
                            </td>
                            <td>
                               
                                <label style="position: absolute; margin-top: 4px; margin-left: 15px"> <span id="ashDeposit"></span> M.N.</label>
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
                    <table class="table table-bordered" id="tablePayments" style="background-color: white;margin-bottom: 0px;">
                        <thead>
                            <tr>
                                <th style="text-align: center;">A PAGAR</th>
                                <th>FORMA DE PAGO INICIAL</th>
                                <th>CANTIDAD</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Filas de pagos generadas dinámicamente -->
                        </tbody>
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
    <div class="col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-danger" style="float:right;width:100%;" id="btnCancel">Cancelar compra</button>

    </div>
    <div class="offset-md-6 col-md-2 col-sm-2 col-xs-6">
        <button class="btn btn-success" style="float:right;width:100%;" id="btnPurchase">Confirmar compra</button>
       
    
</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7/jquery.inputmask.min.js"></script>
<script>
$(document).ready(function() {

    const purchaseId = <?= $purchaseId; ?>;
   
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
     // Llamada AJAX para obtener los datos del cliente y de la compra
     $.ajax({
        type: "POST",
        url: "api/purchases/purchaseById.php",
        data: { purchaseId: purchaseId},
        success: function(response) {
            try {
                const data = JSON.parse(response);
                if (data.length > 0) {
                    populateForm(data[0]); // Llama a la función para asignar valores al formulario
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Datos no encontrados',
                        text: 'No se encontraron datos para el cliente y compra especificados.',
                        confirmButtonText: 'Aceptar'
                    });
                }
            } catch (error) {
                console.error('Error al procesar los datos:', error);
            }
        },
        error: function(xhr, status, error) {
            console.error('Error en la solicitud:', error);
        }
    });

    // Realizar la llamada AJAX para obtener los datos del cliente y de la compra
    function populateForm(data) {
        $('#PSurname').val(data.customerPsurname);
        $('#MSurname').val(data.customerMsurname);
        $('#Name').val(data.customerName);
        $('#address').val(data.customerAddress);
        $('#house_number').val(data.houseNumber || ''); // Si no existe, deja vacío
        $('#apt_number').val(data.aptNumber || '');
        $('#neighborhood').val(data.customerNeighborhood || '');
        $('#zip_code').val(data.customerZipCode || '');
        $('#catStatesId').val(data.customerStateId || '');
        $('#catTownsId').val(data.customerTownId || '');
        $('#Deputation').val(data.customerMunicipality || '');
        $('#CelPhone').val(data.customerPhone);
        $('#Email').val(data.customerEmail);
        $('#social_reason').val(data.customerSocialReason || '');
        $('#RFCCURP').val(data.customerRFC || '');
        $('#DateOfBirth').val(data.customerBirthdate || '');
        $('#CityOfBirth').val(data.birthPlace || '');
        $('#CivilStatus').val(data.civilStatus || '');
        $('#Occupation').val(data.occupation || '');
        $('#Company').val(data.businessName || '');
        $('#AddressCompany').val(data.businessAddress || '');
        $('#PhoneCompany').val(data.businessPhone || '');
        $('#ExtPhoneCompany').val(data.businessExt || '');
        $('#StateAddressCompany').val(data.businessState || '');
        $('#MunicipalityAddressCompany').val(data.businessMunicipality || '');
        $('#CityAddressCompany').val(data.businessCity || '');
        $('#Income').val(data.averageIncome || '');

        // Referencias
        $('#ReferenceCustomer1').val(data.referencePerson1 || '');
        $('#ReferenceCustomerPhone1').val(data.referencePersonPhone1 || '');
        $('#ReferenceCustomer2').val(data.referencePerson2 || '');
        $('#ReferenceCustomerPhone2').val(data.referencePersonPhone2 || '');

        // Condiciones económicas
        $('#paymentPlanLabelDesc').text(data.paymentPlan || '');
        $('#cryptKeyLabel').text(data.fullPosition || '');
        $('#levelLabel').text(data.level || '');
        $('#areaLabel').text(data.area || '');
        $('#zoneLabel').text(data.zone || '');
        $('#totalAmountLabel').text(formatCurrency(data.cryptPrice || 0));
        $('#appliedDiscountLabel').text(formatCurrency(data.discountAmount || 0));
        $('#initialPaymentLabel').text(formatCurrency(data.initialPayment || 0));
        $('#balanceLabel').text(formatCurrency(data.balance || 0));
        $('#maintenance').text(formatCurrency(data.maintenanceFee || 0));
        $('#ashDeposit').text(formatCurrency(data.ashDeposit || 0));

        // Beneficiarios
        const beneficiariesTable = $('#tableBeneficiary tbody');
        beneficiariesTable.empty(); // Limpia la tabla antes de agregar contenido

        if (data.beneficiaries && data.beneficiaries.length > 0) {
            data.beneficiaries.forEach((beneficiary) => {
                beneficiariesTable.append(`
                    <tr class="tr-beneficiary">
                        <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryName[]" value="${beneficiary.name}" disabled /></td>
                        <td><input type="text" class="form-control control-beneficiary" name="BeneficiarySurnames[]" value="${beneficiary.surnames}" disabled /></td>
                        <td><input type="date" class="form-control datepicker control-beneficiary" name="BeneficiaryBirthdate[]" value="${beneficiary.birthdate}" disabled /></td>
                        <td><input type="text" class="form-control control-beneficiary phone" name="BeneficiaryCelPhone[]" value="${beneficiary.phone}" disabled /></td>
                        <td><input type="text" class="form-control control-beneficiary" name="BeneficiaryRelationship[]" value="${beneficiary.relationship}" disabled /></td>
                    </tr>
                `);
            });
        } else {
            // Caso en que no hay beneficiarios registrados
            beneficiariesTable.append(`
                <tr>
                    <td colspan="5" class="text-center">No se registró ningún beneficiario</td>
                </tr>
            `);
        }

        // Pagos
        const paymentsTable = $('#tablePayments tbody');
        paymentsTable.empty();
        if (data.payments && data.payments.length > 0) {
            data.payments.slice(0, 2).forEach((payment) => {
                paymentsTable.append(`
                    <tr>
                        <td>A PAGAR</td>
                        <td>${payment.typePayment || 'N/A'}</td>
                        <td>${formatCurrency(payment.paymentAmount || 0)} M.N.</td>
                    </tr>
                `);
            });
        } else {
            paymentsTable.append(`
                <tr>
                    <td colspan="3" class="text-center">No se registraron métodos de pago</td>
                </tr>
            `);
        }


    }

    // Función para formatear valores como moneda
    function formatCurrency(value) {
        return `$${parseFloat(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
    }

    // Botón de regresar
    $('#btnRegresar').click(function(e) {
        e.preventDefault(); // Evita recargar la página
        window.location.href = 'cotizaciones'; // Redirige a cotizaciones
    });
    // Botón de regresar
    $('#btnCancel').click(function (e) {
        e.preventDefault(); // Previene el comportamiento por defecto del botón

        // SweetAlert para confirmar la cancelación
        Swal.fire({
            title: '¿Estás seguro?',
            text: "La cotización será cancelada y no podrá ser recuperada.",
            icon: 'warning',
            showCancelButton: true, // Muestra el botón de cancelar
            confirmButtonColor: '#d33',
            cancelButtonColor: '#315D7C',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No, volver'
        }).then((result) => {
            if (result.isConfirmed) {
                // Si el usuario confirma, realiza la solicitud AJAX
                $.ajax({
                    type: "POST",
                    url: "api/purchases/purchaseCancel.php",
                    data: { purchaseId: purchaseId },
                    success: function (response) {
                        try {
                                Swal.fire({
                                icon: 'success',
                                title: 'Cotización cancelada',
                                text: 'La cotización ha sido cancelada con éxito.',
                                confirmButtonText: 'Aceptar',
                                allowOutsideClick: false
                            }).then(() => {
                                // Redirige a otra página o refresca la tabla según sea necesario
                                window.location.href = 'cotizaciones'; // Redirige a la página de cotizaciones
                            });
                        
                        } catch (error) {
                            console.error('Error al procesar los datos:', error);
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: 'Ocurrió un error inesperado. Intenta nuevamente.',
                                confirmButtonText: 'Aceptar'
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error('Error en la solicitud:', error);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error en la solicitud',
                            text: 'No se pudo realizar la operación. Intenta más tarde.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            }
        });
    });
    // Botón de regresar
    $('#btnPurchase').click(function (e) {
        e.preventDefault(); // Previene el comportamiento por defecto del botón
        var formData = new FormData($('#PurchaseRequestCreateForm')[0]);

        // SweetAlert para confirmar la cancelación
        Swal.fire({
            title: '¿Deseas confirmar la compra?',
            text: "La cotización pasará a ser una venta confirmada.",
            icon: 'question',
            showCancelButton: true, // Muestra el botón de cancelar
            confirmButtonColor: '#CCA369 ',
            cancelButtonColor: '#315D7C',
            confirmButtonText: 'Confirmar compra',
            cancelButtonText: 'No, volver'
        }).then((result) => {
            if (result.isConfirmed) {
                // Si el usuario confirma, realiza la solicitud AJAX
                $.ajax({
                    type: "POST",
                    url: "api/purchases/purchaseConfirm.php",
                    data: { purchaseId: purchaseId },
                    success: function(response) {
                        const purchaseId = response.purchaseId; // Obtén `purchaseId` de la respuesta
                        formData.append('purchaseId', purchaseId); // Añade `purchaseId` a formData

                        // Segunda solicitud AJAX para generar y descargar el PDF
                        $.ajax({
                            url: 'views/purchases/purchaseTemplate.php',
                            type: 'POST',
                            data: formData,  
                            contentType: false,
                            processData: false,
                            xhrFields: {
                                responseType: 'blob'  // Recibir el archivo como blob
                            },
                            success: function(blob) {
                                const url = URL.createObjectURL(blob);
                                const link = document.createElement('a');
                                link.href = url;
                                link.download = 'Cotizacion.pdf';
                                link.click();

                                // Liberar el objeto URL después de la descarga
                                URL.revokeObjectURL(url);
                                
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Documento Generado',
                                    text: 'El PDF se ha descargado correctamente.'
                                });
                            },
                            error: function(jqXHR, textStatus, errorThrown) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error',
                                    text: 'Error al generar el PDF: ' + textStatus
                                });
                            }
                        });
                    },
                    error: function (xhr, status, error) {
                        console.error('Error en la solicitud:', error);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error en la solicitud',
                            text: 'No se pudo realizar la operación. Intenta más tarde.',
                            confirmButtonText: 'Aceptar',
                            allowOutsideClick: false
                        });
                    }
                });
            }
        });
    });




});

</script>
