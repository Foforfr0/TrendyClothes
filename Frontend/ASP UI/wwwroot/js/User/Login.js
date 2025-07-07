/**TODO 
 * Store username to prevent modification after disabled first fieldset
 */
import * as utils from '/js/site.js';

// Validate Username and Password on server
document.getElementById('firstBtn').addEventListener('submit', async function (event) {
    event.preventDefault();
});
document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
    const password = utils.getValueDOMElementNullOrEmpty('InputPassword');

    if (!username || !password) return;

    try {
        const response = await fetch(window.config.PostLoginUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        const data = await response.json();

        switch (response.status) {
            case 200:
                document.getElementById('InputUsername2').value = document.getElementById('InputUsername').value;
                EnabledSecondPartLogin();
                utils.showToast('Credenciales correctas.', 'success');
                break;
            case 400:
            case 401:
                utils.showToast('Fallo al proporcionar datos del usuario.', 'warning');
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

    const inputEmail = document.getElementById('InputEmail');
    inputEmail.disabled = !inputEmail.disabled;
    const btnEmail = document.getElementById('btnEmail');
    btnEmail.disabled = !btnEmail.disabled;
    const btnForgotEmail = document.getElementById('btnForgotEmail');
    btnForgotEmail.disabled = !btnForgotEmail.disabled;
}

function EnabledThirdPartLogin() {
    const inputEmail = document.getElementById('InputEmail');
    inputEmail.disabled = !inputEmail.disabled;
    inputEmail.value = '*****' + inputEmail.value.slice(inputEmail.value.indexOf('@'));
    const btnEmail = document.getElementById('btnEmail');
    btnEmail.disabled = !btnEmail.disabled;
    const btnForgotEmail = document.getElementById('btnForgotEmail');
    btnForgotEmail.disabled = !btnForgotEmail.disabled;

    const inputTwoFactorCode = document.getElementById('InputTwoFactorCode');
    inputTwoFactorCode.disabled = !inputTwoFactorCode.disabled;
    const btnValidate2fc = document.getElementById('btnValidate2fc');
    btnValidate2fc.disabled = !btnValidate2fc.disabled;
}

// Validate Email from User on server
document.getElementById('btnEmail').addEventListener('submit', async function (event) {
    event.preventDefault();
});
document.getElementById('emailForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const username = utils.getValueDOMElementNullOrEmpty('InputUsername');
    const email = utils.getValueDOMElementNullOrEmpty('InputEmail');

    if (!username || !email) return;

    try {
        const response = await fetch(`${window.config.ValidateEmailUrl}?username=${encodeURIComponent(username)}&email=${encodeURIComponent(email)}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('Content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:
                EnabledThirdPartLogin();
                await CreateTwoFactorCode(username, email);
                break;
            case 400:
            case 404:
                utils.showToast('Fallo al proporcionar datos del usuario.', 'warning');
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
        const response = await fetch(window.config.Post2FAUrl, {
            method: 'PATCH',
            headers: {
                'Content-type': 'application/json'
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
                utils.showToast('Fallo al proporcionar datos del usuario.', 'warning');
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
        await fetch(`${window.config.Delete2FAUrl}?username=${encodeURI(username)}`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json'
            }
        });
    } catch (error) {
        console.error('Error al eliminar el código doble factor: ', error);
    }
}
