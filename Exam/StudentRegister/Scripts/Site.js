$(document).ready(function () {
    $(".DatePicker").datepicker({ dateFormat: 'dd/mm/yy' });

     $(".table tbody tr").each(function () {
            var qty =parseInt(this.cells[3].innerHTML);
            if (qty>25) {
                $(this).attr("bgcolor", "red");
            }
        })
});