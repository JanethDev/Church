function AlertConfirm(msj, url) {
    $("#alert").modal("hide");
    $("#alert").remove();
    var vAlert = '<div class="modal fade" id="alert" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">\
                    <div class="modal-dialog" role="document">\
                        <div class="modal-content">\
                            <div class="modal-header">\
                                <h4 class="modal-title" style="color: #FFF;" id="myModalLabel">Notificación</h4>\
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>\
                            </div>\
                            <div class="modal-body">\
                                '+ msj + '\
                            </div>\
                            <div class="modal-footer">\
                                <button type="button" class="btn btn-danger" data-dismiss="modal">No</button>\
                                <a href="' + url + '" class="btn btn-success">Si</a>\
                            </div>\
                        </div>\
                    </div>\
                </div>';

    $("body").append(vAlert);
    $("#alert").modal("show");
}
function AlertMessage(msj) {
    $("#alert").modal("hide");
    $("#alert").remove();

    var vAlert = `<div class="modal fade"  id="alert" tabindex="-1">
                      <div class="modal-dialog">
                        <div class="modal-content">
                          <div class="modal-header">
                            <h5 class="modal-title">Notificación</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                              <span aria-hidden="true">&times;</span>
                            </button>
                          </div>
                          <div class="modal-body">
                            <p>`+msj+`</p>
                          </div>
                          <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                          </div>
                        </div>
                      </div>
                    </div>`;

    $("body").append(vAlert);
    $("#alert").modal("show");
}
$(function () {
    var createAutocomplete = function () {
        var $input = $(this);
        var options = {
            source: $input.attr("data-autocomple")
        };
        $input.autocomplete(options);
    };
    $("input[data-autocomple]").each(createAutocomplete)
    $(".datepicker").removeAttr("data-val");

    $('[data-fancybox]').fancybox();
    $('.just-decimal').mask("##0.00", { reverse: true });

    $(".just-int").keypress(function () {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode != 8 && charCode != 0 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('.phone').mask('(000) 000-0000');
    $('[data-toggle="tooltip"]').tooltip();
});
$(document).ajaxSuccess(function () {
    var createAutocomplete = function () {
        var $input = $(this);
        var options = {
            source: $input.attr("data-autocomple")
        };
        $input.autocomplete(options);
    };

    $("input[data-autocomple]").each(createAutocomplete)
    //Calendar DatePicker
    $.datepicker.setDefaults($.datepicker.regional['es-MX']);
    $(".datepicker").datepicker();
    $(".datepicker").removeAttr("data-val");
    $('[data-toggle="tooltip"]').tooltip();
});
function fnExcelReport(table, filename) {
    var dt = new Date();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    var fullDate = day + "-" + month + "-" + year;
    console.log("tabla:" + table);

    filename = filename + fullDate;
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById(table); //id of table

    for (j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, ""); //remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); //remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); //reomves input params
    tab_text = tab_text.replace(/<th class="ignore"*>[\s\S]*?<\/th>/gi, ""); //removes specific th table
    tab_text = tab_text.replace(/<td class="ignore"*>[\s\S]*?<\/td>/gi, ""); //remove specific td table
    tab_text = tab_text.replace(/<td class="ignore" align="center"*>[\s\S]*?<\/td>/gi, ""); //remove Ver Registro td table

    while (tab_text.indexOf('á') != -1) tab_text = tab_text.replace('á', '&aacute;');
    while (tab_text.indexOf('é') != -1) tab_text = tab_text.replace('é', '&eacute;');
    while (tab_text.indexOf('í') != -1) tab_text = tab_text.replace('í', '&iacute;');
    while (tab_text.indexOf('ó') != -1) tab_text = tab_text.replace('ó', '&oacute;');
    while (tab_text.indexOf('ú') != -1) tab_text = tab_text.replace('ú', '&uacute;');
    while (tab_text.indexOf('Á') != -1) tab_text = tab_text.replace('Á', '&Aacute;');
    while (tab_text.indexOf('É') != -1) tab_text = tab_text.replace('É', '&Eacute;');
    while (tab_text.indexOf('Ì') != -1) tab_text = tab_text.replace('Ì', '&Iacute;');
    while (tab_text.indexOf('Ó') != -1) tab_text = tab_text.replace('Ó', '&Oacute;');
    while (tab_text.indexOf('Ú') != -1) tab_text = tab_text.replace('Ú', '&Uacute;');
    while (tab_text.indexOf('Ñ') != -1) tab_text = tab_text.replace('Ñ', '&Ntilde;');
    while (tab_text.indexOf('ñ') != -1) tab_text = tab_text.replace('ñ', '&ntilde;');

    console.log(tab_text);

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) { // If Internet Explorer
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, filename + ".xls");
    }
    else { //other browser not tested on IE 11      
        var a = document.createElement('a');
        a.href = 'data:application/vnd.ms-excel,' + encodeURIComponent(tab_text);
        a.download = filename + '.xls';
        a.click();
    }
    return (sa);
}



var MyAlert = function (title, message, type) {
    Swal.fire(
        title,
        message,
        type
    )
}

var ResizeTables = function () {
    $('.table-images tbody').css({ 'font-size': '0', 'line-height': '0' });

    $('.table-images').each(function (index, element) {
        var vWidthParentTable = parseFloat($(element).parent().css('width'));
        var vWidthTable = parseFloat($(element).css('width'));

        while (vWidthTable > vWidthParentTable) {
            if (vWidthTable > vWidthParentTable) {
                $(element).find('img').each(function () {
                    var vWidthImg = $(this).prop('width');
                    var vHeightImg = $(this).prop('height');
                    $(this).removeAttr('width');
                    $(this).removeAttr('height');
                    $(this).css({
                        'width': vWidthImg / 1.5 + 'px',
                        'height': vHeightImg / 1.5 + 'px'
                    })
                })
                vWidthTable = parseFloat($(element).css('width'));
            }
        }
    })
}
