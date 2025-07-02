import * as utils from '/js/site.js';

async function confirmCancel() {
    const response = await utils.showConfirmationToast('¿Estás seguro de que deseas cancelar los cambios?\nSe perderán todos los datos no guardados.')
    if (response) {
        window.location.replace(`/Product/Seller/ConsultMyProducts`);
    }
}
window.confirmCancel = confirmCancel;

document.getElementById('registrationForm').addEventListener('submit', async function (event) {
    event.preventDefault();
    const name = utils.getValueDOMElementNullOrEmpty('InputName');
    const price = utils.getValueDOMElementNullOrEmpty('InputPrice');
    const discount = utils.getValueDOMElementNullOrEmpty('InputDiscount');
    const description = utils.getValueDOMElementNullOrEmpty('InputDescription');
    const stockAvailable = utils.getValueDOMElementNullOrEmpty('InputStockAvailable');
    const categoryId = document.getElementById('InputCategory').value;
    const typeId = document.getElementById('InputType').value;
    const statusId = document.getElementById('InputStatus').value;

    if (!name || !price || !discount || !description || !stockAvailable || !categoryId || !typeId || !statusId) return;

    try {
        const response = await fetch(window.config.PostProductData, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({name, price, discount, description, stockAvailable, categoryId, typeId, statusId})
        });

        if (!response) return;
        const data = await response.json();

        switch (response.status) {
            case 200:
                window.data.idProduct = data.productId;
                await sendImageMime();
                utils.showToast(data.message, 'success');
                window.location.replace(`/Product/Seller/ViewDetails?id=${data.productId}`);
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
        console.error('Error modificando producto: ', error);
    }
});

function getAntiForgeryToken() {
    const input = document.querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
}

async function sendImageMime() {
    const imageBase64Input = document.getElementById('imageBase64Input');
    const mimeInput = document.getElementById('mimeInput');
    const submitBtn = document.getElementById('submitForm');

    if (!submitBtn) {
        console.error('submitBtn no encontrado.');
        return;
    }

    if (!imageBase64Input.value || !mimeInput.value) {
        utils.showToast('Por favor selecciona una imagen primero.', 'danger');
        return;
    }

    const originalBtnHTML = submitBtn.innerHTML;
    submitBtn.disabled = true;
    try {
        const response = await fetch('/Product/Seller/RegistrationProduct?handler=SendImage', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken(),
            },
            body: JSON.stringify({
                idProduct: window.data.idProduct,
                imageBase64: imageBase64Input.value,
                mimeImage: mimeInput.value
            })
        });

        const result = await response.json();

        if (response.ok && result.success) {
            utils.showToast(`Imagen guardada exitosamente`, 'success');
        } else {
            console.error('Server error:', result);
            utils.showToast(result.message || 'Error al guardar la imagen', 'danger');
        }

    } catch (error) {
        console.error('Error:', error);
        utils.showToast('Error de conexión al guardar la imagen', 'danger');
    } finally {
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnHTML;
    }
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