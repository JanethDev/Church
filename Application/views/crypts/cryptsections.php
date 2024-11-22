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

        // Verificar si la zona es AH01U01
        $isReverseOrder = false;
        foreach ($positions as $pos) {
            if (isset($pos['zone']) && $pos['zone'] === "AH01U01") {
                $isReverseOrder = true;
                break;
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
    <div class="row">
        
        <div class="col-md-3" id="espaciosDisponiblesContainer" style="display: none;"><br>
            <h5>Espacios Disponibles: <a id="espaciosDisponibles">0/4</a></h5>
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

            // Ordenar las letras según la zona
            if ($isReverseOrder) {
                rsort($letters); // Orden inverso (Z a A)
            } else {
                sort($letters); // Orden normal (A a Z)
            }
            ?>

            <?php foreach ($letters as $letter): ?>
                <tr>
                    <td><?= $letter ?></td>
                    <?php for ($k = 1; $k <= $maxColumns; $k++): ?>
                        <?php 
                        $positionFound = false;
                        $currentPosition = null;

                        foreach ($positions as $pos) {
                            $positionNumber = (int) filter_var($pos['position'], FILTER_SANITIZE_NUMBER_INT);
                            if ($pos['position'][0] == $letter && $positionNumber == $k) {
                                $positionFound = true;
                                $currentPosition = $pos;
                                break;
                            }
                        }
                        ?>
                        <?php if ($positionFound): ?>
                            <td class="<?php 
                                    echo ($currentPosition['status'] == 'disponible' || 
                                        ($currentPosition['status'] == 'apartado' && $currentPosition['places_shared'] < 4)) 
                                        ? 'disponible' 
                                        : 'no-disponible'; 
                                ?>"
                                data-id="<?= $currentPosition['id'] ?>"
                                data-full-position="<?= htmlspecialchars($currentPosition['full_position']) ?>"
                                data-zone="<?= htmlspecialchars($currentPosition['zone']) ?>"
                                data-position-name="<?= htmlspecialchars($currentPosition['position']) ?>"
                                data-is-shared="<?= $currentPosition['is_shared'] ?>"
                                data-places-shared="<?= $currentPosition['places_shared'] ?>"
                                data-price="<?= $currentPosition['price'] ?>" 
                                data-price-shared="<?= $currentPosition['price_shared'] ?>"
                                data-status-id="<?= $currentPosition['status_id'] ?>"
                                data-level="<?= $currentPosition['levelNumber'] ?>"
                                data-aisle="<?= htmlspecialchars($currentPosition['aisle']); ?>"
                                data-status="<?= htmlspecialchars($currentPosition['status']) ?>">
                                <div class="td-inner">
                                    <?php
                                    if ($currentPosition['is_shared'] && $currentPosition['status'] == 'disponible') {
                                        $imgSrc = 'assets/img/cuadro-compartida.png';
                                    } else {
                                        switch ($currentPosition['status']) {
                                            case 'disponible':
                                                $imgSrc = 'assets/img/cuadro.png';
                                                break;
                                            case 'apartado':
                                                $imgSrc = 'assets/img/cuadro-temporal.png';
                                                break;
                                            case 'temporal':
                                                $imgSrc = 'assets/img/cuadro-apartado.png';
                                                break;
                                            case 'vendido':
                                                $imgSrc = 'assets/img/cuadro-disabled.png';
                                                break;
                                            default:
                                                $imgSrc = 'assets/img/cuadro.png';
                                                break;
                                        }
                                    }
                                    ?>
                                    <img src="<?= $imgSrc ?>" alt="<?= $currentPosition['status'] ?>"></img>
                                    <span class="position-name"><?= $currentPosition['position'] ?></span>
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
document.querySelectorAll('#tablecryptsection td').forEach(td => {
    td.addEventListener('click', function() {
        const isShared = this.dataset.isShared == '1';
        const status = this.dataset.status;
        const placesShared = parseInt(this.dataset.placesShared);

        // Deshabilitar la selección si no cumple las condiciones
        if (!isShared && status === 'apartado') {
            Swal.fire({
                icon: 'warning',
                title: 'Apartado',
                text: 'Este nicho se encuentra apartado para su selección.',
            });
            return;
        }

        if (isShared && status === 'apartado' && placesShared >= 4) {
            Swal.fire({
                icon: 'warning',
                title: 'No disponible',
                text: 'Este nicho compartido ya está completo.',
            });
            return;
        }

        // Deselecciona todos los cuadros antes de seleccionar uno nuevo
        document.querySelectorAll('#tablecryptsection td').forEach(el => {
        el.classList.remove('selected');
        const imgElement = el.querySelector('.td-inner img');
        const isShared = el.dataset.isShared == '1';
        const status = el.dataset.status;

        // Asegúrate de que imgElement existe antes de cambiar el src
        if (imgElement) {
            if (isShared && status === 'disponible') {
                imgElement.src = 'assets/img/cuadro-compartida.png';
            } else {
                switch (status) {
                    case 'disponible':
                        imgElement.src = 'assets/img/cuadro.png';
                        break;
                    case 'apartado':
                        imgElement.src = 'assets/img/cuadro-temporal.png';
                        break;
                    case 'temporal':
                        imgElement.src = 'assets/img/cuadro-apartado.png';
                        break;
                    case 'vendido':
                        imgElement.src = 'assets/img/cuadro-disabled.png';
                        break;
                    default:
                        imgElement.src = 'assets/img/cuadro.png';
                        break;
                }
            }
        }
    });

        // Selecciona el cuadro actual
        this.classList.add('selected');
        this.querySelector('.td-inner img').src = 'assets/img/cuadro-selected.png';

        // Actualiza los detalles de la selección
        document.getElementById('posicion').textContent = this.dataset.fullPosition;
        document.getElementById('aisle').textContent = this.dataset.aisle;
        document.getElementById('urna').textContent = this.dataset.positionName;
        const tipo = isShared ? 'Individual' : 'Familiar';
        document.getElementById('tipo').textContent = tipo;

        // Mostrar precio según el tipo de nicho
        const precio = isShared ? parseFloat(this.dataset.priceShared) : parseFloat(this.dataset.price);
        document.getElementById('precio').textContent = formatPrice(precio);

        // Mostrar la cantidad de espacios compartidos disponibles solo para nichos individuales
        const espaciosDisponiblesElem = document.getElementById('espaciosDisponibles');
        const espaciosDisponiblesContainer = document.getElementById('espaciosDisponiblesContainer');
        if (isShared) {
            espaciosDisponiblesElem.textContent = `${placesShared}/4`;
            espaciosDisponiblesContainer.style.display = 'block';
        } else {
            espaciosDisponiblesContainer.style.display = 'none';
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
