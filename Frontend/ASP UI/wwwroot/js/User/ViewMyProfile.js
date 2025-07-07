import * as utils from '/js/site.js';

document.addEventListener('DOMContentLoaded', (event) => {
    retrievePersonalData();
    retrieveAddresses();
});

async function retrievePersonalData() {
    try {
        const response = await fetch(window.config?.GetPersonalDataUrl, {
            method: 'GET',
            credentials: 'include',
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
                showPersonalData(data.body);
                break;
            case 401:
                utils.showToast('Error con las cookies.', 'danger');
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
        console.error('Error al obtener datos del usuario: ', error);
    }
}

function showPersonalData(userData) {
    document.getElementById('fullName').textContent = userData.fullName;
    document.getElementById('username').textContent = userData.username;
    document.getElementById('email').textContent = userData.email;
    document.getElementById('phoneNumber').textContent = userData.phoneNumber;
    document.getElementById('role').textContent = userData.role;
}

async function retrieveAddresses() {
    try {
        const response = await fetch(window.config.GetAddressesUrl, {
            method: 'GET',
            credentials: 'include',
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
                showAddresses(data.body.value);
                break;
            case 400:
            case 404:
                utils.showToast("Fallo al proporcionar datos del usuario.", 'warning');
                break;
            case 500:
                utils.showToast('Error interno del servidor.', 'danger');
                break;
        }
    } catch (error) {
        utils.showToast('Error al conectarse con el servidor.', 'danger');
        console.error('Error al obtener datos del usuario: ', error);
    }
}

function showAddresses(addresses) {
    const tableBody = document.getElementById('addressesTableBody');
    tableBody.innerHTML = ''; // Limpiar el contenido previo
    if (!addresses || addresses.length === 0) {
        // Si no hay direcciones, mostrar mensaje
        const row = document.createElement('tr');
        row.innerHTML = '<td></td><td>El usuario no tiene direcciones registradas.</td>';
        tableBody.appendChild(row);
    } else {
        // Iterar sobre las direcciones y crear filas
        addresses.forEach(item => {
            const row = document.createElement('tr');
            row.innerHTML = `
                    <td>${item.isActive ? 'Sí' : 'No'}</td>
                    <td>${item.numberExterior} ${item.street} ${item.neighborhood} ${item.postalCode} ${item.state} ${item.country}</td>
                `;
            tableBody.appendChild(row);
        });
    }
}

// Replace the existing modal initialization code with this:
$('#modalSearchProduct').on('shown.bs.modal', function () {
    // Clear any previous content to avoid duplicates
    document.getElementById('modal-content').innerHTML = '';

    fetch(`/Product/Seller/ConsultProductsToCreateAuction`, {
        method: 'GET',
        credentials: 'include'
    })
        .then(res => res.text())
        .then(html => {
            document.getElementById('modal-content').innerHTML = html;
            // Initialize product selector after content is loaded
            if (typeof initializeProductSelector === 'function') {
                initializeProductSelector();
            } else {
                // Load the script if not already loaded
                const script = document.createElement('script');
                script.src = '/js/Product/ConsultProductsToAuction.js';
                script.onload = () => {
                    initializeProductSelector();
                };
                document.body.appendChild(script);
            }
        })
        .catch(err => {
            console.error('Error loading partial:', err);
            utils.showToast("Error al cargar el formulario", "danger");
        });
});