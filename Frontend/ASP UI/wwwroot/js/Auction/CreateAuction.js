import * as utils from '/js/site.js';

function configureInputs() {
    const inputFirstPrice = document.getElementById('InputFirstPrice');
    const inputMinBid = document.getElementById('InputBid');

    // Evitar negativos en FirstPrice
    inputFirstPrice.addEventListener('input', function () {
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });

    // Evitar negativos en MinBid
    inputMinBid.addEventListener('input', function () {
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    configureInputs();

    document.getElementById('auctionForm').addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault(); // Evita que el formulario se envíe
        }
    });

    // Configurar fechas mínimas
    const now = new Date();
    const nowString = now.toISOString().slice(0, 16);

    const startDateInput = document.getElementById('InputDateStart');
    const endDateInput = document.getElementById('InputDateEnd');

    startDateInput.min = nowString;
    endDateInput.min = nowString;

    // Validar fechas
    startDateInput.addEventListener('change', function () {
        const startDate = new Date(this.value);
        const minEndDate = new Date(startDate.getTime() + 60 * 60 * 1000); // +1 hora
        endDateInput.min = minEndDate.toISOString().slice(0, 16);

        if (endDateInput.value && new Date(endDateInput.value) <= startDate) {
            endDateInput.value = minEndDate.toISOString().slice(0, 16);
        }
    });

    // Validar precio inicial vs puja mínima
    const firstPriceInput = document.getElementById('InputFirstPrice');
    const minBidInput = document.getElementById('InputMinBid');

    firstPriceInput.addEventListener('input', function () {
        const firstPrice = parseFloat(this.value);
        if (firstPrice && firstPrice > 0) {
            const suggestedMinBid = Math.max(100, Math.floor(firstPrice * 0.05));
            minBidInput.placeholder = suggestedMinBid.toString();
        }
    });

    // Manejar envío del formulario
    document.getElementById('auctionForm').addEventListener('submit', async function (e) {
        e.preventDefault();

        const submitBtn = document.getElementById('submitBtn');
        const originalText = submitBtn.innerHTML;
        submitBtn.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Creando...';
        submitBtn.disabled = true;

        const result = await postAuction();

        // Restaurar botón después de un momento
        setTimeout(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;

            // Ocultar alerta
            setTimeout(() => {
                alert.classList.add('d-none');
            }, 3000);
        }, 1000);
    });

    // Manejar cancelación
    document.getElementById('cancelBtn').addEventListener('click', function () {
        if (confirm('¿Está seguro que desea cancelar la creación de la subasta?')) {
            window.history.back();
        }
    });
});

async function postAuction() {
    

    const name = utils.getValueDOMElementNullOrEmpty('InputName');
    const firstPrice = utils.getValueDOMElementNullOrEmpty('InputFirstPrice');
    const bid = utils.getValueDOMElementNullOrEmpty('InputBid');
    const dateStart = utils.getValueDOMElementNullOrEmpty('InputDateStart');
    const dateEnd = utils.getValueDOMElementNullOrEmpty('InputDateEnd');
    const description = utils.getValueDOMElementNullOrEmpty('InputDescription');
    const statusId = document.getElementById('InputStatus').value;
    const imageBase64 = document.getValueDOMElementNullOrEmpty('imageBase64Input');
    const mimeImage = document.getValueDOMElementNullOrEmpty('mimeInput');


    if (!name || !firstPrice || !bid || !dateStart || !dateEnd || !description || !statusId || !imageBase64 || !mimeImage) return;

    try {
        const response = await fetch(`${window.config.PostAuction}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken(),
            },
            body: JSON.stringify({ name, firstPrice, bid, dateStart, dateEnd, description, statusId, imageBase64, mimeImage})
        });

        if (!response) return;
        const data = await response.json();

        switch (response.status) {
            case 200:
                utils.showToast(data.message, 'success');
                window.location.replace(`/User/Profile/ViewMyProfile`);
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
        console.error('Error creando subasta: ', error);
    }
}

function getAntiForgeryToken() {
    const input = document.querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
}

document.addEventListener('DOMContentLoaded', function () {
    // Referencias a elementos del DOM
    const selectImageBtn = document.getElementById('selectImageBtn');
    const imageInput = document.getElementById('imageInput');
    const previewImage = document.getElementById('previewImage');
    const submitContainer = document.getElementById('submitForm');
    const imageBase64Input = document.getElementById('imageBase64Input');
    const mimeInput = document.getElementById('mimeInput');

    // Evento click del botón personalizado
    selectImageBtn.addEventListener('click', function () {
        imageInput.click();
    });

    // Evento change del input file
    imageInput.addEventListener('change', function (event) {
        const file = event.target.files[0];

        if (file) {
            // Validar que sea una imagen
            if (!file.type.startsWith('image/')) {
                alert('Por favor selecciona un archivo de imagen válido.');
                return;
            }

            // Validar tamaño (opcional - límite de 5MB)
            const maxSize = 5 * 1024 * 1024; // 5MB
            if (file.size > maxSize) {
                alert('El archivo es demasiado grande. Por favor selecciona una imagen menor a 5MB.');
                return;
            }

            // Leer el archivo como base64
            const reader = new FileReader();

            reader.onload = function (e) {
                const base64Result = e.target.result;

                // Extraer solo la parte base64 (sin el prefijo data:image/...;base64,)
                const base64Data = base64Result.split(',')[1];

                // Actualizar los campos ocultos
                imageBase64Input.value = base64Data;
                mimeInput.value = file.type;

                // Mostrar la imagen
                previewImage.src = base64Result;

                // Cambiar el texto del botón
                selectImageBtn.innerHTML = '<i class="fas fa-edit"></i> Cambiar Imagen';
            };

            reader.onerror = function () {
                alert('Error al leer el archivo. Por favor intenta nuevamente.');
            };

            // Leer el archivo
            reader.readAsDataURL(file);
        }
    });

    // Cargar imagen existente al cargar la página (si existe)
    window.addEventListener('load', function () {
        const existingBase64 = imageBase64Input.value;
        const existingMime = mimeInput.value;

        if (existingBase64 && existingMime) {
            previewImage.src = `data:${existingMime};base64,${existingBase64}`;

            // Cambiar el texto del botón
            selectImageBtn.innerHTML = '<i class="fas fa-edit"></i> Cambiar Imagen';
        }
    });
});