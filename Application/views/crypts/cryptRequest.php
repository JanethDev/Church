<?php
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['positions'])) {
    $response = $_POST['positions'];

    // Decodifica el JSON recibido
    $positions = json_decode($response, true);

    if ($positions !== null) { 

        $idSeleccionado = $positions[0]['id'];
        $zonaSeleccionada = $positions[0]['zone'];
        $criptaSeleccionada = $positions[0]['full_position'];
        $urnaSeleccionada = $positions[0]['position'];
        $tipoSeleccionado = $positions[0]['is_shared'] == 1 ? 'Individual' : 'Familiar';
        $precioSeleccionado = $positions[0]['price'];
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
                                <h5>Urna: <span id="sel-urna"><?php echo $urnaSeleccionada; ?></span></h5>
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
                                <select class="form-control" id="sel-discount" name="sel-discount"><option value="">Seleccionar opción</option>
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

                                <select class="form-control selectPay valid" data-val="true" data-val-required="El campo PaymentMethods es obligatorio." id="sel-pago" name="sel-pago" aria-describedby="enumPaym-error" aria-invalid="false"><option selected="selected" value="0">Seleccionar opción</option>
                                    <option value="1">Contado</option>
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
                                <input type="text" class="form-control just-decimal" id="sel-enganche" placeholder="Enganche">
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
        $('#sel-total').text('$' + nuevoTotal.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        var selectedPaymentValue = $('#sel-pago').val();
        var enganche = 0;
        var mensualidades = 0;
        var mensualidad = 0;

        switch (selectedPaymentValue) {
            case '1':
                enganche = nuevoTotal;
                mensualidades = 0;
                mensualidad = 0;
                break;
            case '2':
                enganche = nuevoTotal / 12;
                mensualidades = 11;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '5':
                enganche = nuevoTotal / 18;
                mensualidades = 17;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '3':
                enganche = nuevoTotal / 24;
                mensualidades = 23;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '6':
                enganche = nuevoTotal / 36;
                mensualidades = 35;
                mensualidad = (nuevoTotal - enganche) / mensualidades;
                break;
            case '7':
                enganche = nuevoTotal / 48;
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

        $('#sel-enganche').val('$' + enganche.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('#sel-mensualidades').text(mensualidades + ' mensualidades de $' + mensualidad.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        // Mostrar los campos de enganche y mensualidades si no se muestran
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

    // Trigger initial calculation
    actualizarTotalYCalculos();
});
$(document).ready(function() {
    $('#btn-aceptar').click(function() {
        var urnaSeleccionada = $('#sel-urna').text();
        var tipoSeleccionado = $('#sel-tipo').text();
        var precioSeleccionado = $('#sel-precio').text();
        var descuentoAplicado = $('#sel-cal-descuento').text();
        var totalFinal = $('#sel-total').text();

        var selectedDiscountValue = $('#sel-discount').val();
        var selectedPaymentValue = $('#sel-pago').val();
        var enganche = $('#sel-enganche').val();
        var positions = <?php echo json_encode($positions); ?>;
        var data = {
            urnaSeleccionada: urnaSeleccionada,
            tipoSeleccionado: tipoSeleccionado,
            precioSeleccionado: precioSeleccionado,
            descuentoAplicado: descuentoAplicado,
            totalFinal: totalFinal,
            selectedDiscountValue: selectedDiscountValue,
            selectedPaymentValue: selectedPaymentValue,
            enganche: enganche,
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
                console.error('Error al enviar los datos:', error);
                // Mostrar mensaje de error o manejar el error de otra manera
            }
        });
    });
});



</script>
<?php
    } else {
        echo '<p>Error: Datos JSON inválidos.</p>';
    }
} else {
    echo '<p>Error: Método no permitido o datos no recibidos.</p>';
}
?>
