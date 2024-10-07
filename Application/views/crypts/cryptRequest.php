<!-- Primero incluye jQuery -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Luego incluye Inputmask -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7/jquery.inputmask.min.js"></script>

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
                                <label class="" id="sel-interes"></label>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="offset-md-6 col-md-3">
                                <h5>Total: <span id="sel-total"><?php echo '$' . number_format($precioNumerico, 2, '.', ','); ?></span></h5>
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

    function actualizarTotalYCalculos() {
        var selectedDiscountValue = $('#sel-discount').val();
        var descuento = 0;

        if (selectedDiscountValue === '2') {
            descuento = 0.10 * total;
        } else if (selectedDiscountValue === '3') {
            descuento = 0.20 * total;
        } else if (selectedDiscountValue === '4') {
            descuento = 0.05 * total;
        }
        $('#sel-cal-descuento').text('$' + descuento.toFixed(2));
        var nuevoTotal = total - descuento;

        var selectedPaymentValue = $('#sel-pago').val();
        var enganche = convertToNumber($('#sel-enganche').val()); // Obtener el valor actualizado del input
        var mensualidades = 0;
        var mensualidad = 0;

        switch (selectedPaymentValue) {
            case '1':
                enganche = nuevoTotal;
                mensualidades = 0;
                mensualidad = 0;
                break;
            case '2':
                mensualidades = 11;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '5':
                mensualidades = 17;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '3':
                mensualidades = 23;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '6':
                mensualidades = 35;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '7':
                mensualidades = 47;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '4':
                enganche = nuevoTotal / 2;
                mensualidades = 11;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            default:
                enganche = 0;
                mensualidades = 0;
                mensualidad = 0;
        }

        $('#sel-enganche').val(enganche.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('#sel-mensualidades').text(mensualidades + ' mensualidades de $' + mensualidad.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        if (mensualidades > 0 || enganche > 0) {
            $('.divEnganche').show();
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

    // Trigger initial calculation
    actualizarTotalYCalculos();

    $('#btn-aceptar').click(function() {
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

        if (enganche > totalFinal) {
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
            precioSeleccionado: $('#sel-precio').text(),
            descuentoAplicado: $('#sel-cal-descuento').text(),
            totalFinal: $('#sel-total').text(),
            selectedDiscountValue: $('#sel-discount').val(),
            selectedPaymentValue: selectedPaymentValue,
            enganche: $('#sel-enganche').val(),
            positions: <?php echo json_encode($positions); ?>
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
        return parseFloat(value.replace(/[^0-9.-]+/g, ""));
    }

    // Inicialización de máscaras de entrada
    $('.just-decimal').inputmask({
        alias: 'currency',
        prefix: '$',
        groupSeparator: ',',
        autoGroup: true,
        digits: 2,
        digitsOptional: false,
        rightAlign: false
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
