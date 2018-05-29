
    $('.slett').click(function() {
        console.log("Blir aktivert.");
        $('#slettDiv').css('visibility', 'visible');
        $('#slettDiv').css('height', '60px');
    });



$(document).ready(function () {
    $('#slettDiv').css('visibility', 'hidden');
    $('#slettDiv').css('height', '0px');
});
