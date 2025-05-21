// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    // Aquí puedes asignar métodos o realizar acciones
    const button = document.getElementById('LogoutBtn');

    if (button) {
        button.addEventListener('click', async function () {
            await fetch(`${window.BACKEND_URL}/api/User/Logout`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'content-type': 'application/json'
                },
            });
            localStorage.clear();
            sessionStorage.clear();
            window.location.replace('/User/Auth/Login');
        });
    }
});

export function getValueDOMElementNullOrEmpty(id) {
    const element = document.getElementById(id);
    return element ? element.value || '' : '';
}

export function showToast(message, type) {
    // type: success, danger, warning, info, primary
    const toastEl = document.getElementById('liveToast');
    const toastBody = document.getElementById('toastMessage');

    toastBody.textContent = message;
    toastEl.className = `toast align-items-center text-bg-${type} border-0`;

    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}

export function getTextDangerInput(inputField) {
    const input = document.getElementById(inputField);
    if (input) {
        // Buscar el siguiente hermano que tenga la clase 'text-danger'
        const spanError = input.parentElement.querySelector('.text-danger');

        if (spanError) {
            return spanError;
        }
    }
}