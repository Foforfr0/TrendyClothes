import { getValueDOMElementNullOrEmpty } from '/js/site.js';

document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const Username = getValueDOMElementNullOrEmpty('InputUsername');
    const Password = getValueDOMElementNullOrEmpty('InputPassword');

    if (!Username || !Password) return;

    try {
        const response = await fetch('https://localhost:5001/api/User/Auth/Login/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Encoding': 'UTF-8'
            },
            body: JSON.stringify({
                Username: Username,
                Password: Password
            })
        });
        const result = await response.json();

        if (result.body) {
            window.location.href = "/User/ViewProfile";
        }
    } catch (error) {
        console.error('Error en la solicitud: ', error);
        alert('Ocurrió un error al intentar iniciar sesión.\nPor favor, intente más tarde.');
    }
});