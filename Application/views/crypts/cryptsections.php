<?php
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['response'])) {
    $response = $_POST['response'];

    // Decodifica el JSON recibido
    $positions = json_decode($response, true);

    if ($positions !== null) { 
?>
<div class="table-container">
    <div class="row">
        <div class="col-md-12"><br>
            <h5>Código: <a id="posicion"></a> </h5>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12"><br>
            <h5>Detalles de la urna </h5>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3"><br>
            <h5>Urna: <a id="urna"></a></h5>
        </div>
        <div class="col-md-3"><br>
            <h5>Tipo: <a id="tipo"></a> </h5>
        </div>
        <div class="col-md-3"><br>
            <h5>Precio: <a id="precio"></a> </h5>
        </div>
        <div class="col-md-3"><br>
            <button type="button" class="btn btn-success" id="btnPaymentMethod" style="width: 100%;">Forma de pago</button>
        </div>
    </div>
    <table id="tablecryptsection">
        <thead>
            <tr>
                <th></th>
                <?php 
                // Obtener el número máximo de columnas a partir de las posiciones disponibles
                $maxColumns = 0;
                foreach ($positions as $pos) {
                    $number = intval(substr($pos['position'], 1));
                    if ($number > $maxColumns) {
                        $maxColumns = $number;
                    }
                }
                

                // Función para obtener el nombre de posición
                function getPositionName($pos) {
                    return $pos['position'];
                }

                // Función para formatear el precio
                function formatPrice($price) {
                    return '$' . number_format($price, 2, '.', ',');
                }
                ?>

                <!-- Crear las columnas en el encabezado -->
                <?php for ($j = 1; $j <= $maxColumns; $j++): ?>
                    <th><?= $j ?></th>
                <?php endfor; ?>
            </tr>
        </thead>
        <tbody>
            <?php 
            // Obtener todas las letras distintas de las posiciones disponibles
            $letters = array_unique(array_map(function($position) {
                return substr($position['position'], 0, 1);
            }, $positions));

            // Ordenar las letras alfabéticamente
            sort($letters);
            ?>

            <?php foreach ($letters as $letter): ?>
                <tr>
                    <td><?= $letter ?></td>
                    <?php for ($k = 1; $k <= $maxColumns; $k++): ?>
                        <?php 
                        $positionFound = false;
                        foreach ($positions as $pos) {
                            if ($pos['position'] == $letter . $k) {
                                $positionFound = true;
                                break;
                            }
                        }
                        ?>
                        <?php if ($positionFound): ?>
                            <td class="<?= $pos['status'] == 'disponible' ? 'disponible' : 'no-disponible' ?>" 
                                data-id="<?= $pos['id'] ?>"
                                data-full-position="<?= htmlspecialchars($pos['full_position']) ?>"
                                data-zone="<?= htmlspecialchars($pos['zone']) ?>"
                                data-position-name="<?= htmlspecialchars(getPositionName($pos)) ?>"
                                data-is-shared="<?= $pos['is_shared']?>"
                                data-places-shared="<?= $pos['places_shared'] ?>"
                                data-price="<?= formatPrice($pos['price']) ?>"
                                data-price-initial="<?= $pos['price'] ?>"
                                data-price-shared="<?= formatPrice($pos['price_shared']) ?>"
                                data-status-id="<?= $pos['status_id'] ?>"
                                data-status="<?= htmlspecialchars($pos['status']) ?>">
                                <div class="td-inner">
                                    <img src="<?= $pos['status'] == 'disponible' ? '../../assets/img/cuadro.png' : '../../assets/img/cuadro-disabled.png' ?>" alt="<?= $pos['status'] ?>">
                                    <span class="position-name"><?= getPositionName($pos) ?></span>
                                </div>
                            </td>
                        <?php else: ?>
                            <td></td>
                        <?php endif; ?>
                    <?php endfor; ?>
                </tr>
            <?php endforeach; ?>
        </tbody>
    </table>
</div>
<script>
    document.querySelectorAll('#tablecryptsection .disponible').forEach(td => {
        td.addEventListener('click', function() {
            if (this.dataset.status === 'no-disponible') {
                Swal.fire({
                    icon: 'warning',
                    title: 'Notificación',
                    text: 'La cripta seleccionada no está disponible.',
                });
            } else {
                document.querySelectorAll('#tablecryptsection .disponible').forEach(el => {
                    el.classList.remove('selected');
                    el.querySelector('.td-inner img').src = '../../assets/img/cuadro.png';
                });
                this.classList.add('selected');
                this.querySelector('.td-inner img').src = '../../assets/img/cuadro-selected.png';
                document.getElementById('posicion').textContent = this.dataset.fullPosition;
                document.getElementById('urna').textContent = this.dataset.positionName;
                document.getElementById('precio').textContent = this.dataset.price;
                const tipo = this.dataset.isShared == 1 ? 'Individual' : 'Familiar';
                document.getElementById('tipo').textContent = tipo;
            }
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
