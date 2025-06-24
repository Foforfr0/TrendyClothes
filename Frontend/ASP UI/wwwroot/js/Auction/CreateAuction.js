function configureInputs() {
    const inputFirstPrice = document.getElementById('InputFirstPrice');
    const inputMinBid = document.getElementById('InputMinBid');
    const inputNumberProducts = document.getElementById('InputNumberProducts');

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

    // Solo números enteros en NumberProducts
    inputNumberProducts.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
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
    document.getElementById('auctionForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const submitBtn = document.getElementById('submitBtn');
        const originalText = submitBtn.innerHTML;

        // Mostrar loading
        submitBtn.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Creando...';
        submitBtn.disabled = true;

        // Simular procesamiento
        setTimeout(() => {
            // Restaurar botón después de un momento
            setTimeout(() => {
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;

                // Ocultar alerta
                setTimeout(() => {
                    alert.classList.add('d-none');
                }, 3000);
            }, 1000);
        }, await postAuction());
    });

    // Manejar cancelación
    document.getElementById('cancelBtn').addEventListener('click', function () {
        if (confirm('¿Está seguro que desea cancelar la creación de la subasta?')) {
            window.history.back();
        }
    });

    // Agregar efectos de hover a los inputs
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function () {
            this.parentElement.classList.remove('focused');
        });
    });

    // Validación en tiempo real
    const requiredInputs = ['InputName', 'InputFirstPrice', 'InputMinBid', 'InputDateStart', 'InputDateEnd', 'InputNumberProducts'];

    requiredInputs.forEach(inputId => {
        const input = document.getElementById(inputId);
        if (input) {
            input.addEventListener('blur', validateInput);
            input.addEventListener('input', clearValidationError);
        }
    });

    function validateInput(e) {
        const input = e.target;
        const errorSpan = input.parentElement.nextElementSibling;

        if (!input.value.trim()) {
            showError(input, errorSpan, 'Este campo es obligatorio');
        } else {
            clearError(input, errorSpan);
        }
    }

    function clearValidationError(e) {
        const input = e.target;
        const errorSpan = input.parentElement.nextElementSibling;
        clearError(input, errorSpan);
    }

    function showError(input, errorSpan, message) {
        input.classList.add('is-invalid');
        input.style.borderColor = 'var(--danger-color)';
        if (errorSpan) {
            errorSpan.textContent = message;
        }
    }

    function clearError(input, errorSpan) {
        input.classList.remove('is-invalid');
        input.style.borderColor = '';
        if (errorSpan) {
            errorSpan.textContent = '';
        }
    }
});

async function postAuction() {
    const name = utils.getValueDOMElementNullOrEmpty('InputName');
    const firstPrice = utils.getValueDOMElementNullOrEmpty('InputFirstPrice');
    const minBid = utils.getValueDOMElementNullOrEmpty('InputMinBid');
    const dateStart = utils.getValueDOMElementNullOrEmpty('InputDateStart');
    const dateEnd = utils.getValueDOMElementNullOrEmpty('InputDateEnd');
    const numberProducts = document.getElementById('InputNumberProducts').value;

    if (!name || !firstPrice || !minBid || !dateStart || !dateEnd|| !numberProducts) return;

    try {
        const response = await fetch(`${window.config.PostAuction}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({idProduct: window.data.idProduct, name, firstPrice, minBid, dateStart, dateEnd, numberProducts })
        });

        if (!response) return;
        const data = await response.json();

        switch (response.status) {
            case 200:
                utils.showToast(data.message, 'success');
                window.location.replace(``);
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