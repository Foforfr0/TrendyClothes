/*document.addEventListener('DOMContentLoaded', async function () {
    const query = "@Model.Query";
    const container = document.getElementById("productsContainer");

    try {
        const response = await fetch(`/api/products/search?query=${encodeURIComponent(query)}`);
        const products = await response.json();

        if (products.length === 0) {
            container.innerHTML = `<div class="col-12"><p>No se encontraron productos.</p></div>`;
            return;
        }

        for (const product of products) {
            const col = document.createElement("div");
            col.className = "col-md-4";

            col.innerHTML = `
                <div class="card h-100">
                    <img src="/ImageApi/Product/${product.id}" class="card-img-top" alt="${product.name}" />
                    <div class="card-body">
                        <h5 class="card-title">${product.name}</h5>
                        <p class="card-text">$${product.price}</p>
                        <a href="/Product/ViewProduct?id=${product.id}" class="btn btn-primary">Ver detalles</a>
                    </div>
                </div>
            `;
            container.appendChild(col);
        }
    } catch (err) {
        container.innerHTML = `<div class="col-12 text-danger">Error al cargar productos</div>`;
        console.error(err);
    }
});*/