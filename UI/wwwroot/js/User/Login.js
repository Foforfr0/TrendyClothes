/**TODO 
 * Store username to prevent modification after disabled first fieldset
 */
import * as utils from '/js/site.js';

// Validate Username and Password on server
document.getElementById('loginForm').addEventListener('submit', async function (event) {
    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
    const password = utils.getValueDOMElementNullOrEmpty('InputPassword');

    if (!username || !password) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Login`, {
            method: 'POST',
            headers: {
                'content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        const data = await response.json();

        switch (response.status) {
            case 200:
                EnabledSecondPartLogin();
                utils.showToast('Credenciales correctas.', 'success');
                break;
            case 400:
            case 401:
                utils.showToast(data.message, 'warning');
                break;
            case 500:
                utils.showToast('Error interno del servidor.', 'danger');
                break;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error registrando usuario nuevo: ', error);
    }
});

function EnabledSecondPartLogin() {
    document.getElementById('InputPassword').value = '**********';
    const firstFieldsetLogin = document.getElementById('firstFieldsetLogin');
    firstFieldsetLogin.disabled = !firstFieldsetLogin.disabled;
    const secondFieldsetLogin = document.getElementById('secondFieldsetLogin');
    secondFieldsetLogin.disabled = !secondFieldsetLogin.disabled;
}

document.getElementById('secondFieldsetLogin').addEventListener('submit', async function (event) {
    event.preventDefault();
});

// Validate Email from User on server
document.getElementById('emailForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
    const email = utils.getValueDOMElementNullOrEmpty('InputEmail');

    if (!username || !email) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Login/ValidateEmailUser?username=${encodeURIComponent(username)}&email=${encodeURIComponent(email)}`, {
            method: 'GET',
            headers: {
                'content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:
                utils.showToast(data.message, 'success');
                await CreateTwoFactorCode(username, email);
                break;
            case 400:
            case 404:
                utils.showToast(data.message, 'warning');
                break;
            case 500:
                utils.showToast('Error interno del servidor.', 'danger');
                break;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error validando correo: ', error);
    }
});

// Create TwoFactorCode on server
async function CreateTwoFactorCode(username, email) {
    if (!username || !email) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Login/CreateTwoFactorCode`, {
            method: 'POST',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify({ username, email })
        });

        const data = await response.json();


        switch (response.status) {
            case 201:
                utils.showToast('Código doble factor enviado.', 'success');
                break;
            case 400:
            case 404:
                utils.showToast(data.message, 'warning');
                break;
            case 500:
                utils.showToast('Error interno del servidor.', 'danger');
                break;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error al crear/enviar el código doble factor: ', error);
    }
}

// Delete TwoFactorCode on server
async function DeleteTwoFactorCode(username) {
    if (!username) return;

    try {
        await fetch(`${window.BACKEND_URL}/api/User/Login/DeleteTwoFactorCode?username=${encodeURI(username)}`, {
            method: 'DELETE',
            headers: {
                'content-type': 'application/json'
            }
        });
    } catch (error) {
        console.error('Error al eliminar el código doble factor: ', error);
    }
}

// Validate TwoFactorCode on server
document.getElementById('twoFactorForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
    const twoFactorCode = utils.getValueDOMElementNullOrEmpty('InputTwoFactorCode');

    if (!username || !twoFactorCode) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Login/ValidateTwoFactorCode?username=${encodeURIComponent(username)}&twoFactorCode=${encodeURIComponent(twoFactorCode)}`, {
            method: 'GET',
            headers: {
                'content-type': 'application/json'
            }
        });

        const data = await response.json();

        switch (response.status) {
            case 200:
                utils.showToast(data.message, 'success');
                await DeleteTwoFactorCode(username)
                window.location.replace('/User/Profile/ViewMyProfile');
                break;
            case 400:
            case 401:
            case 404:
                utils.showToast(data.message, 'warning');
                break;
            case 500:
                utils.showToast('Error interno del servidor.', 'danger');
                break;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error al validar el código doble factor: ', error);
    }
});

