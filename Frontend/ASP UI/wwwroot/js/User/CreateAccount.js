import * as utils from '/js/site.js';

document.getElementById('signInForm'), addEventListener('submit', async function (event) {
    event.preventDefault();
    const vUsername = await ValidateExistenceUsername();
    const vEmail = await ValidateExistenceEmail();
    const vPhoneNumber = await ValidateExistencePhoneNumber();

    if (vUsername == false && vEmail == false && vPhoneNumber == false) {
        const firstName = utils.getValueDOMElementNullOrEmpty('InputFirstName');
        const middleName = utils.getValueDOMElementNullOrEmpty('InputMiddleName');
        const lastName = utils.getValueDOMElementNullOrEmpty('InputLastName');
        const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
        const email = utils.getValueDOMElementNullOrEmpty('InputEmail');
        const areaCode = utils.getValueDOMElementNullOrEmpty('InputAreaCode');
        const phoneNumber = utils.getValueDOMElementNullOrEmpty('InputPhoneNumber');
        const password = utils.getValueDOMElementNullOrEmpty('InputPassword');

        if (!firstName || !middleName || !lastName || !username || !email || !areaCode || !phoneNumber || !password) return;

        try {
            const response = await fetch(window.config.PostUserUrl, {
                method: 'POST',
                headers: {
                    'content-Type': 'application/json'
                },
                body: JSON.stringify({ firstName, middleName, lastName, username, email, areaCode, phoneNumber, password })
            });

            const data = await response.json();

            switch (response.status) {
                case 200:
                    utils.showToast(data.message, 'success');
                    setTimeout(() => {
                        window.location.replace('/User/Auth/Login');
                    }, 2000);
                    return true;
                case 400:
                case 409:
                    utils.showToast(data.message, 'warning');
                    return true;
                case 500:
                    utils.showToast('Error validando existencia nombre de usuario.', 'danger');
                    return true;
            }
        } catch (error) {
            utils.showToast('Error al conectarse con el servidor.', 'danger');
            console.error('Error validando existencia nombre de usuario: ', error);
            return true;
        }
    }
});


// Validate existence of username
async function ValidateExistenceUsername() {
    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');

    if (!username) return true;

    try {
        const response = await fetch(`${window.config.ValidateUsernameUrl}?username=${encodeURI(username)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const textDanger = utils.getTextDangerInput('InputUsername');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El nombre de usuario ya existe.';
                utils.showToast('Nombre de usuario utilizado.', 'warning');
                return true;
            case 204:
                return false;
            case 400:
                textDanger.textContent = 'El nombre de usuario es requerido.';
                utils.showToast('El nombre de usuario es requerido.', 'warning');
                return true;
            case 500:
                utils.showToast('Error validando existencia nombre de usuario.', 'danger');
                return true;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error validando existencia nombre de usuario: ', error);
        return true;
    }
}

// Validate existence of email
async function ValidateExistenceEmail() {
    const email = utils.getValueDOMElementNullOrEmpty('InputEmail');

    if (!email) return true;

    try {
        const response = await fetch(`${window.config.ValidateEmailUrl}?email=${encodeURI(email)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const textDanger = utils.getTextDangerInput('InputEmail');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El email ya existe.';
                utils.showToast('El email ya existe.', 'warning');
                return true;
            case 204:
                return false;
            case 400:
                textDanger.textContent = 'El email es requerido.';
                utils.showToast('El email es requerido.', 'warning');
                return true;
            case 500:
                utils.showToast('Error validando existencia email.', 'danger');
                return true;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error validando existencia email: ', error);
        return true;
    }
}

// Validate existence of phone number
async function ValidateExistencePhoneNumber() {
    const areaCode = utils.getValueDOMElementNullOrEmpty('InputAreaCode');
    const phoneNumber = utils.getValueDOMElementNullOrEmpty('InputPhoneNumber');

    if (!areaCode || !phoneNumber) return true;

    try {
        const response = await fetch(`${window.config.ValidatePhoneNumberUrl}?areaCode=${encodeURIComponent(areaCode)}&phoneNumber=${encodeURI(phoneNumber)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const textDanger = utils.getTextDangerInput('InputPhoneNumber');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El número de teléfono ya existe.';
                utils.showToast('El número de teléfono ya existe.', 'warning');
                return true;
            case 204:
                return false;
            case 400:
                textDanger.textContent = 'El número de teléfono es inválido.';
                utils.showToast('El número de teléfono es inválido.', 'warning');
                return true;
            case 500:
                utils.showToast('Error validando existencia número de teléfono.', 'danger');
                return true;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error validando existencia número de teléfono: ', error);
        return true;
    }
}