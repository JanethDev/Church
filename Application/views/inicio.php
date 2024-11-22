<?php
$_title = 'Inicio | Catedral Tijuana';
?>
<style>
    .calendar {
        width: 100%;
        border-collapse: collapse;
    }

    .calendar th, .calendar td {
        border: 1px solid #ddd;
        text-align: center;
        padding: 10px;
        height: 50px;
    }

    .calendar th {
        background-color: #f4f4f4;
    }

    #calendarDiv {
        display: none; /* Ocultamos el calendario por defecto */
    }

    /* Estilo por defecto para las tarjetas */
    .card {
        transition: transform 0.3s, box-shadow 0.3s;
        cursor: pointer;
    }

    /* Efecto hover para todas las tarjetas */
    .card:hover {
        transform: scale(1.05);
        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    }

    /* Estilo para la tarjeta seleccionada */
    .card.selected {
        border: 2px solid #007bff;
        box-shadow: 0 4px 10px rgba(0, 123, 255, 0.5);
    }
</style>

<div id="cardContainer" class="row justify-content-center">
    <!-- Tarjeta 1 -->
    <div class="col-md-3">
        <div class="card text-center select-parish" data-parish="1">
            <img src="assets/img/Arquidiocesis_Color.png" class="card-img-top mx-auto" alt="Logo 1" style="width: 100px; height: 100px; margin-top: 10px;">
            <div class="card-body">
                <h5 class="card-title">Parroquia 1</h5>
                <p class="card-text">Este es un texto de ejemplo para la tarjeta 1.</p>
            </div>
        </div>
    </div>
    <!-- Tarjeta 2 -->
    <div class="col-md-3">
        <div class="card text-center select-parish" data-parish="2">
            <img src="assets/img/LOGO_CATEDRAL_TIJUANA.png" class="card-img-top mx-auto" alt="Logo 2" style="width: 100px; height: 100px; margin-top: 10px;">
            <div class="card-body">
                <h5 class="card-title">Parroquia 2</h5>
                <p class="card-text">Este es un texto de ejemplo para la tarjeta 2.</p>
            </div>
        </div>
    </div>
</div>

<!-- Div del calendario (oculto por defecto) -->
<div class="row justify-content-center calendar" id="calendarDiv">
    <div>
        <button id="prevMonth">Anterior</button>
        <button id="nextMonth">Siguiente</button>
    </div>
    <div id="calendarContainer">
        <?php include 'calendario.php'; ?>
    </div>
</div>

<script>
    $(document).ready(function () {
        let currentMonth = <?= date('m') ?>;
        let currentYear = <?= date('Y') ?>;

        // Función para cargar el calendario
        function loadCalendar(month, year) {
            $.get('calendario.php', { month: month, year: year }, function (data) {
                $('#calendarContainer').html(data);
            });
        }

        // Navegación entre meses
        $('#prevMonth').click(function () {
            currentMonth--;
            if (currentMonth < 1) {
                currentMonth = 12;
                currentYear--;
            }
            loadCalendar(currentMonth, currentYear);
        });

        $('#nextMonth').click(function () {
            currentMonth++;
            if (currentMonth > 12) {
                currentMonth = 1;
                currentYear++;
            }
            loadCalendar(currentMonth, currentYear);
        });

        // Mostrar solo el calendario para "Parroquia 1"
        $('.select-parish').click(function () {
            const selectedParish = $(this).data('parish');

            // Remover la clase 'selected' de todas las tarjetas
            $('.select-parish').removeClass('selected');

            // Agregar la clase 'selected' solo a la tarjeta seleccionada
            $(this).addClass('selected');

            if (selectedParish === 1) {
                $('#cardContainer').hide(); // Oculta las tarjetas
                $('#calendarDiv').fadeIn(); // Muestra el calendario
            } else {
                $('#calendarDiv').fadeOut(); // Oculta el calendario si no es Parroquia 1
                $('#cardContainer').fadeIn(); // Muestra las tarjetas
            }
        });
    });
</script>
