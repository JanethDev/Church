<?php
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['response'])) {
    $response = $_POST['response'];

    // Decodifica el JSON recibido
    $data = json_decode($response, true);

    // Asegúrate de que la decodificación fue exitosa y que tienes 'crypts'
    if ($data !== null && isset($data['crypts'])) { 
        $positions = $data['crypts']; // Accede a 'crypts'

        // Obtener el número máximo de columnas desde la posición completa
        $maxColumns = 0;
        foreach ($positions as $pos) {
            if (isset($pos['position'])) {
                // Extraer el número completo de la posición y actualizar $maxColumns si es mayor
                $number = (int) filter_var($pos['position'], FILTER_SANITIZE_NUMBER_INT);
                if ($number > $maxColumns) {
                    $maxColumns = $number;
                }
            }
        }
?>
<div class="table-container">
    <div class="row">
        <div class="col-md-12"><br>
            <h5>Nicho seleccionado: <a id="aisle"></a>, <a id="posicion"></a></h5>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12"><br>
            <h5>Detalles de la urna</h5>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3"><br>
            <h5>Urna: <a id="urna"></a></h5>
        </div>
        <div class="col-md-3"><br>
            <h5>Tipo: <a id="tipo"></a></h5>
        </div>
        <div class="col-md-3"><br>
            <h5>Precio: <a id="precio"></a></h5>
        </div>
        <div class="col-md-3"><br>
            <button type="button" class="btn btn-success" id="btnPaymentMethod" style="width: 100%;">Forma de pago</button>
        </div>
    </div>
    <table id="tablecryptsection">
        <thead>
            <tr>
                <th>Letra</th>
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
                            $positionNumber = (int) filter_var($pos['position'], FILTER_SANITIZE_NUMBER_INT);
                            if ($pos['position'][0] == $letter && $positionNumber == $k) {
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
                                data-position-name="<?= htmlspecialchars($pos['position']) ?>"
                                data-is-shared="<?= $pos['is_shared'] ?>"
                                data-places-shared="<?= $pos['places_shared'] ?>"
                                data-price="<?= $pos['price'] ?>" 
                                data-price-shared="<?= $pos['price_shared'] ?>"
                                data-status-id="<?= $pos['status_id'] ?>"
                                data-level="<?= $pos['levelNumber'] ?>"
                                data-aisle="<?= htmlspecialchars($pos['aisle']); ?>"
                                data-status="<?= htmlspecialchars($pos['status']) ?>">
                                <div class="td-inner">
                                    <?php
                                    if ($pos['is_shared'] && $pos['status'] == 'disponible') {
                                        $imgSrc = '../../assets/img/cuadro-compartida.png';
                                    } else {
                                        switch ($pos['status']) {
                                            case 'disponible':
                                                $imgSrc = '../../assets/img/cuadro.png';
                                                break;
                                            case 'apartado':
                                                $imgSrc = '../../assets/img/cuadro-temporal.png';
                                                break;
                                            case 'temporal':
                                                $imgSrc = '../../assets/img/cuadro-apartado.png';
                                                break;
                                            case 'vendido':
                                                $imgSrc = '../../assets/img/cuadro-disabled.png';
                                                break;
                                            default:
                                                $imgSrc = '../../assets/img/cuadro.png';
                                                break;
                                        }
                                    }
                                    ?>
                                    <img src="<?= $imgSrc ?>" alt="<?= $pos['status'] ?>"></img>
                                    <span class="position-name"><?= $pos['position'] ?></span>
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
function formatPrice(price) {
    return '$' + price.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
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

                if (el.dataset.isShared == 1 && el.dataset.status === 'disponible') {
                    el.querySelector('.td-inner img').src = '../../assets/img/cuadro-compartida.png';
                } else {
                    switch (el.dataset.status) {
                        case 'disponible':
                            el.querySelector('.td-inner img').src = '../../assets/img/cuadro.png';
                            break;
                        case 'apartado':
                            el.querySelector('.td-inner img').src = '../../assets/img/cuadro-temporal.png';
                            break;
                        case 'temporal':
                            el.querySelector('.td-inner img').src = '../../assets/img/cuadro-apartado.png';
                            break;
                        case 'vendido':
                            el.querySelector('.td-inner img').src = '../../assets/img/cuadro-disabled.png';
                            break;
                        default:
                            el.querySelector('.td-inner img').src = '../../assets/img/cuadro.png';
                            break;
                    }
                }
            });

            this.classList.add('selected');
            this.querySelector('.td-inner img').src = '../../assets/img/cuadro-selected.png';

            document.getElementById('posicion').textContent = this.dataset.fullPosition;
            document.getElementById('aisle').textContent = this.dataset.aisle;
            document.getElementById('urna').textContent = this.dataset.positionName;
            const tipo = this.dataset.isShared == 1 ? 'Individual' : 'Familiar';
            document.getElementById('tipo').textContent = tipo;

            if (this.dataset.isShared == 1) {
                const precioShared = parseFloat(this.dataset.priceShared); 
                document.getElementById('precio').textContent = formatPrice(precioShared); 
            } else {
                const precio = parseFloat(this.dataset.price); 
                document.getElementById('precio').textContent = formatPrice(precio); 
            }
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
