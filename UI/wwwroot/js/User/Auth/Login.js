/**TODO 
 * Store username to prevent modification after disabled first fieldset
 */
import { getValueDOMElementNullOrEmpty } from '/js/site.js';
import { showToast } from '/js/site.js';

// Validate Username and Password on server
document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const username = getValueDOMElementNullOrEmpty('InputUsername');
    const password = getValueDOMElementNullOrEmpty('InputPassword');

    if (!username || !password) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Auth/Login/Login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username: username, password: password })
        });

        const data = await response.json();

        if (response.status === 200) {
            EnabledSecondPartLogin();
            showToast("Credenciales correctas.", "success");
        } else if (response.status === 400 || response.status === 401)
            showToast(data.message, "warning");
        else if (response.status === 500)
            showToast("Error interno del servidor.", "danger");
    } catch (error) {
        showToast("Error al conectarse con el servidor.", "danger");
        console.error("Error validando usuario y contraseña: ", error);
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
    const username = getValueDOMElementNullOrEmpty('InputUsername');
    const email = getValueDOMElementNullOrEmpty('InputEmail');

    if (!username || !email) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Auth/Login/ValidateEmailUser?username=${encodeURIComponent(username)}&email=${encodeURIComponent(email)}`);
        const contentType = response.headers.get("content-type");

        let data = {};
        if (contentType && contentType.includes("application/json")) {
            data = await response.json();
        }

        if (response.status === 200) {
            showToast(data.message, "success");
            await CreateTwoFactorCode(username);
        } else if (response.status === 400 || response.status === 401 || response.status === 404)
            showToast(data.message, "warning");
        else if (response.status === 500)
            showToast("Error interno del servidor.", "danger");
    } catch (error) {
        showToast("Error al conectarse con el servidor.", "danger");
        console.error("Error validando correo: ", error);
    }
});

// Create TwoFactorCode on server
async function CreateTwoFactorCode(username) {
    if (!username) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Auth/Login/CreateTwoFactorCode`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(username)
        });

        const data = await response.json();

        if (response.status === 200)
            showToast(data.message, "success");
        else if (response.status === 400 || response.status === 404)
            showToast(data.message, "warning");
        else if (response.status === 500)
            showToast("Error interno del servidor.", "danger");
    } catch (error) {
        showToast("Error al conectarse con el servidor.", "danger");
        console.error("Error al crear el código doble factor: ", error);
    }
}

// Create TwoFactorCode on server
async function DeleteTwoFactorCode(username) {
    if (!username) return;

    try {
        await fetch(`${window.BACKEND_URL}/api/User/Auth/Login/DeleteTwoFactorCode?username=${encodeURI(username)}`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json'
            }
        });
    } catch (error) {
        console.error("Error al eliminar el código doble factor: ", error);
    }
}

// Validate TwoFactorCode on server
document.getElementById('twoFactorForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const username = getValueDOMElementNullOrEmpty('InputUsername');
    const twoFactorCode = getValueDOMElementNullOrEmpty('InputTwoFactorCode');

    if (!username || !twoFactorCode) return;

    try {
        const response = await fetch(`${window.BACKEND_URL}/api/User/Auth/Login/ValidateTwoFactorCode?username=${encodeURIComponent(username)}&twoFactorCode=${encodeURIComponent(twoFactorCode)}`, {
            method: 'GET',
            headers: {
                'Content-type': 'application/json'
            }
        });

        const data = await response.json();

        if (response.status === 200) {
            showToast(data.message, "success");
            localStorage.setItem("token", data.jwtToken); // Save JWT token
            DeleteTwoFactorCode(username);
        } else if (response.status === 400 || response.status === 401 || response.status === 404)
            showToast(data.message, "warning");
        else if (response.status === 500)
            showToast("Error interno del servidor.", "danger");
    } catch (error) {
        showToast("Error al conectarse con el servidor.", "danger");
        console.error("Error al validar el código doble factor: ", error);
    }
});