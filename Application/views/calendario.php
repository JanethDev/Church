<?php
function generateCalendar($month = null, $year = null) {
    // Si no se pasan el mes y el año, tomar los actuales
    $month = $month ?? date('m');
    $year = $year ?? date('Y');

    // Obtener el primer día del mes y el último día
    $firstDayOfMonth = strtotime("$year-$month-01");
    $lastDayOfMonth = date('t', $firstDayOfMonth);

    // Día de la semana del primer día del mes
    $firstWeekday = date('N', $firstDayOfMonth);

    // Crear array de días del mes
    $days = [];
    for ($i = 1; $i <= $lastDayOfMonth; $i++) {
        $days[] = $i;
    }

    // Crear una tabla para mostrar el calendario
    $output = "<table class='calendar'>";
    $output .= "<thead>
        <tr>
            <th>Lun</th><th>Mar</th><th>Mié</th><th>Jue</th><th>Vie</th><th>Sáb</th><th>Dom</th>
        </tr>
    </thead>";
    $output .= "<tbody><tr>";

    // Añadir celdas vacías antes del primer día del mes
    for ($i = 1; $i < $firstWeekday; $i++) {
        $output .= "<td></td>";
    }

    $currentDay = 1;
    foreach ($days as $day) {
        $output .= "<td>$day</td>";

        // Salto de fila al terminar la semana
        if (($currentDay + $firstWeekday - 1) % 7 == 0) {
            $output .= "</tr><tr>";
        }

        $currentDay++;
    }

    // Añadir celdas vacías después del último día del mes
    while (($currentDay + $firstWeekday - 1) % 7 != 0) {
        $output .= "<td></td>";
        $currentDay++;
    }

    $output .= "</tr></tbody></table>";

    return $output;
}

// Obtener el mes y el año de la solicitud (si existen)
$month = $_GET['month'] ?? null;
$year = $_GET['year'] ?? null;

// Mostrar el calendario generado
echo generateCalendar($month, $year);
?>
