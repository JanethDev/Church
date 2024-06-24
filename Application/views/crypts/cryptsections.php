<?php
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['response'])) {
    $response = $_POST['response'];

    // Decodifica el JSON recibido
    $positions = json_decode($response, true);

    if ($positions !== null) { // Verifica si se decodificó correctamente
?>
<head>
    <title>Tabla de Posiciones con Nombres y Selección</title>
    <style>
        /* Estilos específicos para la tabla con id tablecryptsection */
        #tablecryptsection .table-container {
            width: 800px;
            overflow-x: auto;
            margin: 0 auto;
        }

        #tablecryptsection table {
            border-collapse: separate;
            width: 100%;
        }

        #tablecryptsection th, #tablecryptsection td {
            text-align: center;
            cursor: pointer;
            white-space: nowrap;
            border: none;
            padding: 10px; /* Ajusta el padding según sea necesario */
        }

        #tablecryptsection th {
            background-color: none;
        }

        #tablecryptsection .position-name {
            font-size: 12px;
            display: block;
            margin-top: 5px;
        }

        #tablecryptsection .disponible img {
            width: 40px;
            height: 40px;
        }

        #tablecryptsection .no-disponible img {
            width: 40px;
            height: 40px;
        }

        #tablecryptsection .disponible:hover img {
            content: url('../../assets/img/cuadro-selected.png');
        }

        #tablecryptsection .td-inner {
            padding: 5px; /* Ajustar si es necesario */
        }
    </style>
</head>

    <div class="table-container">
        <h2>Tabla de Posiciones con Nombres y Selección</h2>
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
                                <td class="<?= $pos['status'] == 'disponible' ? 'disponible' : 'no-disponible' ?>" data-position="<?= $pos['full_position'] ?>">
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
                alert('Posición completa: ' + this.dataset.position);
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
