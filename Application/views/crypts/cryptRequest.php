<!-- Primero incluye jQuery -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Luego incluye Inputmask -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7/jquery.inputmask.min.js"></script>
<!-- Bootstrap CSS -->
<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">

<!-- Bootstrap JavaScript -->
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>

<?php
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['positions'])) {
    $response = $_POST['positions'];

    // Decodifica el JSON recibido
    $positions = json_decode($response, true);

    if ($positions !== null) { 

        $idSeleccionado = $positions[0]['id'];
        $zonaSeleccionada = $positions[0]['zone'];

        $criptaSeleccionada = $positions[0]['full_position'];
        $nichoSeleccionada = $positions[0]['position'];
        $tipoSeleccionado = $positions[0]['is_shared'] == 1 ? 'Individual' : 'Familiar';
        $precioFamiliar = $positions[0]['price'];
        $precioSeleccionado = $positions[0]['total'];
        $precioIndividual = $positions[0]['price_shared'];
        $compartidos = $positions[0]['places_shared'];
        $estatusSeleccionado = $positions[0]['status'];
        $precioFormateado = str_replace(['$', ','], '', $precioSeleccionado);
        $precioNumerico = floatval($precioFormateado);

?>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3>Detalles de la solicitud</h3>
                    <div class="tab-pane fade active show" id="nav-forma-pago" role="tabpanel" aria-labelledby="nav-forma-pago-tab">
                        <div class="row mt-5">
                            <div class="col-md-3">
                                <h5>Nicho: <span id="sel-nicho"><?php echo $nichoSeleccionada; ?></span></h5>
                            </div>
                            <div class="col-md-3">
                                <h5>Tipo: <span id="sel-tipo"><?php echo $tipoSeleccionado; ?></span></h5>
                            </div>
                            <div class="col-md-3">
                                <h5>Precio: <span id="sel-precio"><?php echo $precioSeleccionado; ?></span></h5>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-md-3">
                                <h5 id="divPercentage">Descuento: <span id="spanPercentage"></span></h5>
                                <select class="form-control" id="sel-discount" name="sel-discount">
                                    <option value="">Seleccionar opción</option>
                                    <option value="2">10% Promoción Junio 2023</option>
                                    <option value="3">20% Autorizado Admon</option>
                                    <option value="4">5% Descuento Pronto pago</option>
                                </select>
                            </div>
                            <div class="offset-md-3 col-md-3">
                                <h5>Descuento/Precio: <span id="sel-cal-descuento"></span></h5>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-md-3">
                                <h5>Plan de venta: <span id=""></span></h5>
                                <select class="form-control selectPay valid" id="sel-pago" name="sel-pago" <?php echo ($positions[0]['is_shared'] == 1) ? 'disabled' : ''; ?>>
                                    <option selected="selected" value="0">Seleccionar opción</option>
                                    <option value="1" <?php echo ($positions[0]['is_shared'] == 1) ? 'selected' : ''; ?>>Contado</option>
                                    <option value="2">12 meses</option>
                                    <option value="5">18 meses</option>
                                    <option value="3">24 meses</option>
                                    <option value="6">36 meses</option>
                                    <option value="7">48 meses</option>
                                    <option value="4">Uso inmediato 50%</option>
                                </select>
                            </div>
                                                            
                            <div class="col-md-3 divEnganche" style="display: none;">
                                <h5>Enganche </h5>
                                <input type="text" class="form-control just-decimal" id="sel-enganche" placeholder="$ 0.00 MXN">
                            </div>
                            <div class="col-md-3 divEnganche" style="display: none;">
                                <label class="" id="sel-mensualidades"></label><br>
                                <label class="" id="sel-mensualidad" style="display: none;"></label>
                                <label class="" id="sel-meses" style="display: none;"></label>
                                <label class="" id="sel-mesesres" style="display: none;"></label>
                                <label class="" id="sel-interes"></label>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="offset-md-6 col-md-3">
                                <h5>Total: <span id="sel-total"></span></h5>
                            </div>
                        </div>

                        <div class="row mt-5">
                            <div class="col-md-3">
                                <button type="button" class="btn btn-primary" id="btn-aceptar">Aceptar y continuar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
<script>
    
$(document).ready(function() {
    var total = <?php echo $precioNumerico; ?>; // Variable total inicializada con el precio numérico
    var positions = <?php echo json_encode($positions); ?>;
    $('#sel-pago').change(function() {
        if ($(this).prop('disabled')) {
            $(this).val('1'); // Asegúrate de que se mantenga en "Contado"
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No puedes cambiar la opción de pago.',
            });
        } else {
            actualizarTotalYCalculos();
        }
    });
    $('.divEnganche').hide();

    function actualizarTotalYCalculos() {
        var selectedDiscountValue = $('#sel-discount').val();
        var descuento = 0;
       
        var nuevoTotal = total;


        // Cálculo del descuento
        if (selectedDiscountValue === '2') {
            descuento = 0.10 * total;
        } else if (selectedDiscountValue === '3') {
            descuento = 0.20 * total;
        } else if (selectedDiscountValue === '4') {
            descuento = 0.05 * total;
        }
        
        $('#sel-cal-descuento').text('$ ' + descuento.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        
        var nuevoTotal = total - descuento;

        var selectedPaymentValue = $('#sel-pago').val();
        var mensualidades = 0;
        var mensualidad = 0;
        var interes = 0;

        if (selectedPaymentValue === '1') { // Contado
            // Aplicar un 10% de descuento adicional
            descuento = 0.10 * total;
            nuevoTotal = total - descuento;

            // Ocultar campos que no son necesarios para pago contado
            $('#sel-enganche').closest('.divEnganche').hide(); // Oculta enganche
            $('#sel-mensualidades').hide(); // Oculta mensualidades
            $('#sel-interes').hide(); // Oculta interés

            $('#sel-discount').prop('disabled',true);

            // Establecer el total
            $('#sel-total').text('$' + nuevoTotal.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-cal-descuento').text('$ ' + descuento.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-meses').text('1');
            $('#sel-mesesres').text('1');
            $('#sel-enganche').val(nuevoTotal.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        } else if (selectedPaymentValue !== '0') {

            $('#sel-discount').prop('disabled',false);

            switch (selectedPaymentValue) {
                case '2': // 12 meses
                    mensualidades = 12;
                    mensualidadRes= 11;
                    interes = 0;
                    break;
                case '5': // 18 meses
                    mensualidades = 18;
                    mensualidadRes= 17;
                    interes = 0.065;
                    break;
                case '3': // 24 meses
                    mensualidades = 24;
                    mensualidadRes= 23;
                    interes = 0.10;
                    break;
                case '6': // 36 meses
                    mensualidades = 36;
                    mensualidadRes= 35;
                    interes = 0.145;
                    break;
                case '7': // 48 meses
                    mensualidades = 48;
                    mensualidadRes= 47;
                    interes = 0;
                    break;
                case '4': // Uso inmediato 50%
                    enganche = nuevoTotal * 0.50; // Enganche del 50%
                    mensualidades = 12;
                    mensualidadRes= 11;
                    interes = 0;
                    break;
            }



           
            var interesTotal = nuevoTotal * interes; // Calcular el interés total

            var interesMensual = interesTotal / mensualidades;
            var totalCalculado = nuevoTotal + interesTotal;

            var engancheSugerido = totalCalculado / mensualidades;
            
            var enganche = convertToNumber($('#sel-enganche').val()) || engancheSugerido; // Manejar el valor inicial de enganche


            var montoFinanciar = totalCalculado - enganche;
            
            var mensualidadCalculada = montoFinanciar / mensualidadRes;

      

            // Mostrar resultados
            $('#sel-enganche').val(enganche.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-mensualidades').text(mensualidadRes + ' mensualidades de $' + mensualidadCalculada.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-mensualidad').text(mensualidadCalculada.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-meses').text(mensualidades.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-mesesres').text(mensualidadRes.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-interes').text('Interés mensual: $' + interesMensual.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-total').text('$' + totalCalculado.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('#sel-cal-descuento').text('$ ' + descuento.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('.divEnganche').show();
            $('#sel-enganche').closest('.divEnganche').show(); // Oculta enganche
            $('#sel-mensualidades').show(); // Oculta mensualidades
            $('#sel-interes').show(); // Oculta interés
            
        }else {
            // Si aún no se selecciona un plan de pago, mostrar valores por defecto
            $('#sel-enganche').val('0.00');
            $('#sel-mensualidades').text('0 mensualidades de $0.00');
            $('#sel-interes').text('Interés mensual: $0.00');
            $('#sel-total').text('$' + nuevoTotal.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('.divEnganche').hide();
        }
    }


    $('#sel-discount').change(function() {
        actualizarTotalYCalculos();
    });

    $('#sel-pago').change(function() {
        actualizarTotalYCalculos();
    });
    
    $('#sel-enganche').change(function() {
        actualizarTotalYCalculos();
    });
    $('#sel-enganche').on('input', function() {
        actualizarTotalYCalculos();
    });

    // Trigger initial calculation
    actualizarTotalYCalculos();

    $('#btn-aceptar').click(function() {
        actualizarTotalYCalculos();
        var selectedPaymentValue = $('#sel-pago').val();

        if (selectedPaymentValue === '0') {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Debes seleccionar un plan de venta antes de continuar.',
            });
            return;
        }

        var enganche = convertToNumber($('#sel-enganche').val()); // Asegúrate de obtener el valor actualizado
        var totalFinal = convertToNumber($('#sel-total').text());
        var interes = convertToNumber($('#sel-interes').text());
        var mensualidad = convertToNumber($('#sel-mensualidad').text());
        var meses = convertToNumber($('#sel-meses').text());
        var mesesRest = convertToNumber($('#sel-mesesres').text());
        var precio = convertToNumber($('#sel-precio').text());
        var descuento = convertToNumber($('#sel-discount').val());
        var descuentoAplicado = convertToNumber($('#sel-cal-descuento').text());

        var engancheSugerido = totalFinal / meses; // Cálculo del enganche sugerido

        var tolerancia = 0.01;

        // Validar si el enganche es menor que el enganche sugerido
        if (Math.abs(enganche - engancheSugerido) > tolerancia && enganche < engancheSugerido) {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: 'El enganche no puede ser menor al valor sugerido de $' + engancheSugerido.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }),
            });
            return; // Evitar que continúe la ejecución si la validación falla
        }

        if (enganche > totalFinal) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'El enganche no puede ser mayor valor del nicho.',
            });
            return;
        } 
        if (selectedPaymentValue === '4'  && enganche < total*0.50) {

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'El enganche no puede ser mayor al total final calculado.',
            });
            return;
        }

        var data = {
            nichoSeleccionada: $('#sel-nicho').text(),
            tipoSeleccionado: $('#sel-tipo').text(),
            precioSeleccionado: precio,
            descuentoAplicado: descuentoAplicado,
            totalFinal: totalFinal,
            selectedDiscountValue: descuento,
            selectedPaymentValue: selectedPaymentValue,
            enganche: enganche,
            interes: interes,
            mensualidad: mensualidad,
            meses: meses,
            mesesres: mesesRest,
            positions: JSON.stringify(positions)
        };

        $.ajax({
            type: "POST",
            url: "views/purchases/purchaseRequest.php",
            data: data,
            success: function(response) {
                $('#page-content').html(response);
            },
            error: function(xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al enviar los datos. Verifique la información ingresada o contacte a soporte.',
                });
            }
        });
    });

    // Función para convertir a número
    function convertToNumber(value) {
        return Number(value.replace(/[^0-9.-]+/g,""));
    }

    // Inicialización de máscaras de entrada
    $('#sel-enganche').on('focus', function() {
    // Deshabilitar la máscara de entrada al enfocar
    $(this).inputmask('remove');
        }).on('blur', function() {
            // Aplicar la máscara de nuevo al perder el enfoque
            $(this).inputmask({
                alias: 'currency',
                groupSeparator: ',',
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false
            });
        }).on('click', function() {
            // Seleccionar todo el texto al hacer clic en el campo
            $(this).select();
        });

        // También puedes usar 'input' para actualizar los cálculos sin interferir con la selección de texto
        $('#sel-enganche').on('input', function() {
            // Solo actualiza los cálculos sin cambiar el valor del campo
            actualizarTotalYCalculos();
        });

    $(document).on('keydown', function(event) {
        if (event.key === 'Enter') {
            event.preventDefault(); // Prevenir el envío del formulario
            actualizarTotalYCalculos(); // Llamar la función que actualiza los cálculos
            return false;
        }
    });
});
</script>
<?php
    }
}
?>
