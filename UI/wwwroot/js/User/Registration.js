import * as utils from '/js/site.js';

document.getElementById('signInForm'), addEventListener('submit', async function (event) {
    event.preventDefault();
    if (ValidateExistenceUsername() == false && ValidateExistenceEmail() == false && ValidateExistencePhoneNumber() == false) {
        const firstName = getValueDOMElementNullOrEmpty('InputFirstName');
        const middleName = getValueDOMElementNullOrEmpty('InputMiddleName');
        const lastName = getValueDOMElementNullOrEmpty('InputLastName');
        const username = getValueDOMElementNullOrEmpty('InputUsername');
        const email = getValueDOMElementNullOrEmpty('InputEmail');
        const areaCode = getValueDOMElementNullOrEmpty('InputAreaCode');
        const phoneNumber = getValueDOMElementNullOrEmpty('InputPhoneNumber');
        const password = getValueDOMElementNullOrEmpty('InputPassword');

        if (!firstName || !middleName || !lastName || !username || !email || !areaCode || !phoneNumber || !password) return;

        try {
            const response = await fetch(`${window.BACKEND_URL}/api/User/Registration/AddUser`, {
                method: 'POST',
                headers: {
                    'content-Type': 'application/json'
                },
                body: JSON.stringify({firstName, middleName, lastName, username, email, areaCode, phoneNumber, password})
            });

            const data = await response.json();

            switch (response.status) {
                case 200:
                    utils.showToast(data.message, 'success');
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
        const response = await fetch(`${window.BACKEND_URL}/api/User/ValidateUserData/VerifyExistenceUsername?username=${encodeURI(username)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const data = await response.json();

        const textDanger = utils.getTextDangerInput('InputUsername');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El nombre de usuario ya existe.';
                //utils.showToast('Nombre de usuario utilizado.', 'warning');
                return true;
            case 400:
                textDanger.textContent = 'El nombre de usuario es requerido.';
                //utils.showToast('El nombre de usuario es requerido.', 'warning');
                return true;
            case 404:
                return false;
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
        const response = await fetch(`${window.BACKEND_URL}/api/User/ValidateUserData/VerifyExistenceEmail?email=${encodeURI(email)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const data = await response.json();

        const textDanger = utils.getTextDangerInput('InputEmail');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El email ya existe.';
                //utils.showToast('Nombre de usuario utilizado.', 'warning');
                return true;
            case 400:
                textDanger.textContent = 'El email es requerido.';
                //utils.showToast('El nombre de usuario es requerido.', 'warning');
                return true;
            case 404:
                return false;
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

    if (!areaCode || !phoneNumber) return true
        ;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/ValidateUserData/VerifyExistencePhoneNumber?areaCode=${encodeURI(areaCode)}&phoneNumber=${encodeURI(phoneNumber)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });

        const data = await response.json();

        const textDanger = utils.getTextDangerInput('InputPhoneNumber');
        switch (response.status) {
            case 200:
                textDanger.textContent = 'El número de teléfono ya existe.';
                //utils.showToast('Nombre de usuario utilizado.', 'warning');
                return true;
            case 400:
                textDanger.textContent = 'El número de teléfono es requerido.';
                //utils.showToast('El nombre de usuario es requerido.', 'warning');
                return true;
            case 404:
                return false;
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