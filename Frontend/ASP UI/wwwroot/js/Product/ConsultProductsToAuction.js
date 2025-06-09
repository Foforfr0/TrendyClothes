// Variables globales
let selectedProductId = null;
let selectedProductData = null;

// Replace the initializeProductSelector function with:
function initializeProductSelector() {
    console.log('Inicializando selector de productos...');

    // Re-initialize search input
    const searchInput = document.querySelector('#searchInput');
    if (searchInput) {
        searchInput.removeEventListener('input', filterProducts);
        searchInput.addEventListener('input', function () {
            const searchTerm = this.value.toLowerCase().trim();
            filterProducts(searchTerm);
        });
    }

    // Re-initialize view toggle
    const listView = document.querySelector('#listView');
    const gridView = document.querySelector('#gridView');
    const container = document.querySelector('#productsContainer');

    if (listView && gridView && container) {
        [listView, gridView].forEach(input => {
            input.removeEventListener('change', handleViewChange);
            input.addEventListener('change', handleViewChange);
        });
    }

    // Re-initialize product selection
    const productItems = document.querySelectorAll('.product-item');
    const confirmButton = document.querySelector('#confirmSelection');

    const cancelButton = document.querySelector('#cancelModal');

    cancelButton.addEventListener('click', () => {
        // Redirigir a la página de perfil
        window.location.replace("/User/Profile/ViewMyProfile");
    });

    productItems.forEach(item => {
        item.removeEventListener('click', handleProductClick);
        item.addEventListener('click', handleProductClick);
    });

    if (confirmButton) {
        confirmButton.removeEventListener('click', handleConfirmClick);
        confirmButton.addEventListener('click', handleConfirmClick);
    }
}

// Add these helper functions:
function handleViewChange(e) {
    const container = document.querySelector('#productsContainer');
    if (container) {
        container.className = e.target.id === 'listView' ? 'products-list' : 'products-grid';
        localStorage.setItem('productViewMode', e.target.id === 'listView' ? 'list' : 'grid');
    }
}

function handleProductClick(e) {
    e.preventDefault();
    selectProduct(this);
}

function handleConfirmClick() {
    if (selectedProductData) {
        confirmProductSelection(selectedProductData);
    }
}

function initializeProductSelection() {
    const productItems = document.querySelectorAll('.product-item');
    const confirmButton = document.getElementById('confirmSelection');

    productItems.forEach(item => {
        // Evento de click para selección
        item.addEventListener('click', function (e) {
            e.preventDefault();
            selectProduct(this);
        });

        // Efectos hover
        item.addEventListener('mouseenter', function () {
            if (!this.classList.contains('selected')) {
                this.style.transform = 'translateY(-2px)';
            }
        });

        item.addEventListener('mouseleave', function () {
            if (!this.classList.contains('selected')) {
                this.style.transform = 'translateY(0)';
            }
        });
    });

    // Botón de confirmación
    if (confirmButton) {
        confirmButton.addEventListener('click', function () {
            if (selectedProductData) {
                confirmProductSelection(selectedProductData);
            }
        });
    }
}

function selectProduct(productElement) {
    console.log('Seleccionando producto...');

    // Animación de selección
    productElement.classList.add('selecting');

    setTimeout(() => {
        // Remover selección anterior
        document.querySelectorAll('.product-item').forEach(item => {
            item.classList.remove('selected');
        });

        // Seleccionar nuevo producto
        productElement.classList.add('selected');
        productElement.classList.remove('selecting');

        // Guardar datos del producto seleccionado
        selectedProductId = productElement.dataset.productId;
        selectedProductData = {
            id: productElement.dataset.productId,
            name: productElement.dataset.productName,
            price: productElement.dataset.productPrice,
            description: productElement.dataset.productDescription,
            status: productElement.dataset.productStatus,
            element: productElement
        };

        // Habilitar botón de confirmación
        const confirmButton = document.getElementById('confirmSelection');
        if (confirmButton) {
            confirmButton.disabled = false;
            confirmButton.innerHTML = `<i class="bi bi-check-circle me-2"></i>Seleccionar "${selectedProductData.name}"`;
        }

        // Scroll al producto seleccionado si es necesario
        productElement.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest'
        });

        // Callback personalizable
        onProductSelected(selectedProductData);

    }, 150);
}

function onProductSelected(productData) {
    console.log('Producto seleccionado:', productData);

    // Aquí puedes agregar lógica adicional
    // Por ejemplo: validar disponibilidad, mostrar detalles, etc.

    // Ejemplo: Validar si el producto está disponible
    if (productData.status && productData.status.toLowerCase().includes('agotado')) {
        showToast('Advertencia: El producto seleccionado está agotado', 'warning');
    }
}

function confirmProductSelection(productData) {
    console.log('Confirmando selección del producto:', productData);

    // Mostrar loading
    const confirmButton = document.getElementById('confirmSelection');
    const originalText = confirmButton.innerHTML;
    confirmButton.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Procesando...';
    confirmButton.disabled = true;

    // Simular llamada a servidor (reemplaza con tu lógica)
    setTimeout(() => {
        try {
            // Cerrar modal después de un delay
            setTimeout(() => {
                const modal = bootstrap.Modal.getInstance(document.getElementById('modalSearchProduct'));
                if (modal) {
                    modal.hide();
                }

                window.location.replace(`/Auction/Auctioneer/CreateAuction?idProduct=${productData.id}`);

            }, 1500);

        } catch (error) {
            console.error('Error al crear subasta:', error);
            showToast('Error al crear la subasta', 'danger');

            // Restaurar botón
            confirmButton.innerHTML = originalText;
            confirmButton.disabled = false;
        }
    }, 1000);
}

function createAuctionWithProduct(productData) {
    return fetch(`/Auction/Auctioneer/CreateAuction?idProduct${productData.id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            return data;
        } else {
            throw new Error(data.message || 'Error al crear subasta');
        }
    });
    
}

function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    let searchTimeout;

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase().trim();

        // Debounce search
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            filterProducts(searchTerm);
        }, 300);
    });
}

function filterProducts(searchTerm) {
    const productItems = document.querySelectorAll('.product-item');
    let visibleCount = 0;

    productItems.forEach(item => {
        const title = item.querySelector('.product-title').textContent.toLowerCase();
        const description = item.querySelector('.product-description').textContent.toLowerCase();
        const isVisible = !searchTerm || title.includes(searchTerm) || description.includes(searchTerm);

        item.style.display = isVisible ? 'flex' : 'none';
        if (isVisible) visibleCount++;
    });

    // Mostrar mensaje si no hay resultados
    updateEmptyState(visibleCount === 0 && searchTerm);
}

function initializeViewToggle() {
    const listView = document.getElementById('listView');
    const gridView = document.getElementById('gridView');
    const container = document.getElementById('productsContainer');

    if (!listView || !gridView || !container) return;

    listView.addEventListener('change', function () {
        if (this.checked) {
            container.className = 'products-list';
            localStorage.setItem('productViewMode', 'list');
        }
    });

    gridView.addEventListener('change', function () {
        if (this.checked) {
            container.className = 'products-grid';
            localStorage.setItem('productViewMode', 'grid');
        }
    });

    // Restaurar vista preferida
    const savedView = localStorage.getItem('productViewMode');
    if (savedView === 'grid') {
        gridView.checked = true;
        container.className = 'products-grid';
    }
}

function showEmptyState() {
    const container = document.getElementById('productsContainer');
    if (container && container.children.length === 0) {
        container.innerHTML = `
                <div class="text-center py-5">
                    <i class="bi bi-box-seam display-1 text-muted"></i>
                    <h5 class="mt-3 text-muted">No hay productos disponibles</h5>
                    <p class="text-muted">Agrega productos a tu inventario para crear subastas.</p>
                </div>
            `;
    }
}

function updateEmptyState(show) {
    const container = document.getElementById('productsContainer');
    const emptyState = container.querySelector('.empty-search-state');

    if (show && !emptyState) {
        const emptyDiv = document.createElement('div');
        emptyDiv.className = 'empty-search-state text-center py-4';
        emptyDiv.innerHTML = `
                <i class="bi bi-search display-4 text-muted"></i>
                <h6 class="mt-3 text-muted">No se encontraron productos</h6>
                <p class="text-muted mb-0">Intenta con otros términos de búsqueda.</p>
            `;
        container.appendChild(emptyDiv);
    } else if (!show && emptyState) {
        emptyState.remove();
    }
}

function showToast(message, type = 'info') {
    const toastContainer = document.getElementById('toastContainer') || document.body;

    const toastId = 'toast-' + Date.now();
    const toastHTML = `
            <div class="toast custom-toast show" id="${toastId}" role="alert">
                <div class="toast-header">
                    <i class="bi bi-${getToastIcon(type)} me-2 text-${type}"></i>
                    <strong class="me-auto">Notificación</strong>
                    <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;

    toastContainer.insertAdjacentHTML('beforeend', toastHTML);

    const toastElement = document.getElementById(toastId);

    // Auto-remove after 4 seconds
    setTimeout(() => {
        if (toastElement && toastElement.parentNode) {
            toastElement.remove();
        }
    }, 4000);

    // Handle close button
    const closeBtn = toastElement.querySelector('.btn-close');
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            toastElement.remove();
        });
    }
}

function getToastIcon(type) {
    const icons = {
        'success': 'check-circle-fill',
        'danger': 'exclamation-triangle-fill',
        'warning': 'exclamation-triangle-fill',
        'info': 'info-circle-fill'
    };
    return icons[type] || icons.info;
}

// Cleanup al cerrar modal
document.addEventListener('hidden.bs.modal', function (e) {
    if (e.target.id === 'modalSearchProduct') {
        // Limpiar selección
        selectedProductId = null;
        selectedProductData = null;

        // Remover toasts
        const toasts = document.querySelectorAll('.custom-toast');
        toasts.forEach(toast => toast.remove());
    }
});

window.initializeProductSelector = initializeProductSelector;