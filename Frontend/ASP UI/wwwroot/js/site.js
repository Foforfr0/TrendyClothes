// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    // Aquí puedes asignar métodos o realizar acciones
    const button = document.getElementById('LogoutBtn');

    if (button) {
        button.addEventListener('click', async function () {
            await fetch(window.config?.logoutUrl, {
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
    if (!toastEl || !toastBody) return;

    toastBody.textContent = message;
    toastEl.className = `toast align-items-center text-bg-${type} border-0`;

    const toast = bootstrap.Toast.getOrCreateInstance(toastEl);
    toast.show();
}

export function showConfirmationToast(message) {
    return new Promise((resolve) => {
        const container = document.getElementById("confirmationToastContainer");
        const toastBodyText = container?.querySelector(".toast-body span");
        const btnConfirm = document.getElementById("btnConfirm");
        const btnCancel = document.getElementById("btnCancel");

        if (!container || !toastBodyText || !btnConfirm || !btnCancel) {
            console.error("No se encontraron elementos del toast de confirmación.");
            return resolve(false);
        }

        toastBodyText.textContent = message;
        container.classList.remove("d-none");

        const handleResponse = (result) => {
            container.classList.add("d-none");
            resolve(result);
        };

        btnConfirm.onclick = () => handleResponse(true);
        btnCancel.onclick = () => handleResponse(false);
    });
}

// Uso de ejemplo:
document.querySelector("#tuBoton").addEventListener("click", async () => {
    const confirmado = await showConfirmationToast("¿Quieres guardar los cambios?");
    if (confirmado) {
        console.log("Usuario confirmó");
        // Lógica de aceptación
    } else {
        console.log("Usuario canceló");
        // Lógica de cancelación
    }
});


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