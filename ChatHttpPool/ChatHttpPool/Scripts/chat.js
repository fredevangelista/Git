$(document).ready(function () {
    // Post /Chat/New
    $('#btnEnviar').bind('click', function () {
        var msgVal = $('#txtMensagem').val();
        $('#txtMensagem').val('');
        $.post("/Chat/Adicionar", { nome: $('#txtNome').val(), msg: msgVal }, function (data, s) {
            if (data.d != 1) {
                alert('Erro no envio da mensagem');
            }
           
        });
    });

    $('#btNome').click(function () {
        var nome = $('#txtMensagem').val();
        if ($("#txtMensagem").val == '') {
            alert("Preencha seu nome");
            return;
        }
        $("#divChat").css("visibility", "visible");
        $("#btNome").css("visibility", "hidden");
        $("#btNome").attr("disabled", "false");
        $("#txtNome").css('background-color', '#efeeef');
        $("#txtNome").css('border-color', '#efeeef');
        $("#txtNome").css('font-weight', 'Bold');
    });

    //Envia a mensagem com enter
    $('#txtMensagem').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnEnviar').click();
        }
    });

    setTimeout(function () {
        getMensagens();
    }, 100)
});



function getMensagens() {
    $.post("/Chat", null, function (data, s) {
        if (data.mensagens) {
            $('#msgTmpl').tmpl(data.mensagens).appendTo('#chatList');
        }
        setTimeout(function () {
            getMensagens();
        }, 500)
    });
}