//Variables
let name;
let email;
let message;
let button;
let spinner;
let ApicallResponseErrors;
let ApicallResponseErrorsText;
const ApiCallErrorDefaultResponseText = 'Hubo un error al enviar el mensaje,porfa vuelve a intentarlo';

//Send Email Button
$('#input-submit').click(function () {
    SendEmail();
});

const SendEmail = () => {
    GetValues();
    if (VerifyValues()) {
        CallAPI();
    }
}

const GetValues = () => {
    name = $('#input-name').val();
    email = $('#input-email').val();
    message = $('#input-message').val();
}

const VerifyValues = () => {

    //Verify Name
    if (isStringNullOrWhiteSpaces(name)) {
        showValidation("Falta el nombre, ¿Cómo te llamas?", false);
        return false;
    }
    else if (name.length > 30) {
        showValidation("El nombre no puede tener mas de 30 letras", false);
        return false;
    }

    //Verify Email
    if (isStringNullOrWhiteSpaces(email)) {
        showValidation("Falta tu correo electrónico para responderte, ¿no seras un bot?", false);
        return false;
    }
    else if (!isEmailValid(email)) {
        showValidation("El correo electrónico que pusiste no es valido", false);
        return false;
    }
    else if (email.length > 300) {
        showValidation("La direccion del correo electrónico es muy larga (máximo 300 caracteres)", false);
        return false;
    }

    //Verify Message
    if (isStringNullOrWhiteSpaces(message)) {
        showValidation("¡Te falto el mensaje campeón!", false);
        return false;
    }
    else if (message.length > 1500) {
        showValidation("¡El mensaje no puede tener mas de 1500 letras!", false);
        return false;
    }

    //All Values Are Provided and Correct, console log for debugging purposes
    //console.log("All Values are provided and correct");
    return true;
}

const CallAPI = () => {

    ShowSpinnerOnButton(true);

    $.ajax({
        url: '/SendEmail',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        datatype: 'JSON',
        data:
            JSON.stringify({
                'name': name,
                'email': email,
                'message': message
            }),
        success: function (textStatus) {
            showValidation(textStatus, true);
            ShowSpinnerOnButton(false);
        },
        error: function (xhr) {

            if (xhr.status === 400) {

                ApicallResponseErrors = JSON.parse(xhr.responseText);

                for (var key in ApicallResponseErrors) {
                    console.log(`Incorrect values for ${key}: ${ApicallResponseErrors[key]}`);
                    ApicallResponseErrorsText = `Valor incorrecto para ${key}: ${ApicallResponseErrors[key]}`;
                }

                showValidation(ApicallResponseErrorsText, false);
            }
            else if (xhr.status === 500) {
                showValidation(xhr.responseText, false);
            }
            else {
                showValidation(ApiCallErrorDefaultResponseText, false);
            }
            ShowSpinnerOnButton(false);
        }
    });
}

const isStringNullOrWhiteSpaces = (stringvalue) => {
    if (stringvalue === null || stringvalue === '' || isWhiteSpaces(stringvalue)) {
        return true;
    }
    else {
        return false;
    }
}

const isWhiteSpaces = (stringvalue) => {
    return (!stringvalue || /^\s*$/.test(stringvalue));
}

//Regex provided by Chronium Team
const isEmailValid = (email) => {
    return email.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
};

const validationMessageWithCustomButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn input-submit',
        popup: 'background-styled-popup',
    },
    buttonsStyling: false,
    scrollbarPadding: false
});

const showValidation = (message, iscorrect) => {
    validationMessageWithCustomButtons.fire({
        icon: iscorrect ? 'success' : 'error',
        title: iscorrect ? '¡Enviado!' : '¡Algo salio mal!',
        text: message,
        color: '#fff',
    });
}

const ShowSpinnerOnButton = (showSpinner) => {
    if (showSpinner) {
        //Get Elements
        button = $('#input-submit');
        spinner = button.find('span');

        //Change Classes / Properties
        button.addClass('input-submit-active');
        button.removeClass('input-submit');
        button.prop('disabled', true);

        spinner.addClass('d-inline-block');
        spinner.removeClass('d-none');

        //Show elements on screen
        button.html('Enviando...');
        button.append(spinner);
    }
    else {
        //Get Elements
        button = $('#input-submit');
        spinner = button.find('span');

        //Change Classes / Properties
        button.removeClass('input-submit-active');
        button.addClass('input-submit'); 
        button.prop('disabled', false);

        spinner.addClass('d-none');
        spinner.removeClass('d-inline-block');

        //Show elements on screen
        button.html('Hablidad en recarga...');
        button.append(spinner);

        //Button cooldown to prevent over posting client-side
        ButtonCooldown();
    }
}


const ButtonCooldownTime = 15000; //15000 miliseconds = 15 seconds

const ButtonCooldown = () => {
    button = $('#input-submit');
    button.prop('disabled', true);
    button.addClass('input-submit-active');
    button.removeClass('input-submit');

    timer = $('.input-submit-cooldown');
    timer.prop('hidden', false);
    timer.addClass('input-submit-cooldown-animation');

    setTimeout(function () {
        $('#input-submit').prop('disabled', false);
        button.removeClass('input-submit-active');
        button.addClass('input-submit');
        button.html('Enviar');
        timer.prop('hidden', true);
    }, ButtonCooldownTime);
}