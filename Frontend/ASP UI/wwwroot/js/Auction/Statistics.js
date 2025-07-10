import * as utils from '/js/site.js';

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('cleanFieldsBtn').addEventListener('click', async function () {
        const dateStart = document.getElementById('InputDateStart');
        const dateEnd = document.getElementById('InputDateEnd');
        dateStart.value = '';
        dateEnd.value = '';
    });

    document.getElementById('retrieveDataBtn').addEventListener('click', async function () {
        const dateStart = new Date(document.getElementById('InputDateStart').value).toISOString();
        const dateEnd = new Date(document.getElementById('InputDateEnd').value).toISOString();
        await retrieveData(encodeURIComponent(dateStart), encodeURIComponent(dateEnd));
    });
    const firstSearch = document.getElementById('retrieveDataBtn');
    firstSearch.click();

    const searchInput = document.getElementById("searchInput");
    const statusFilter = document.getElementById("statusFilter");

    const cards = document.querySelectorAll(".auction-card");

    function applyFilters() {
        const searchTerm = searchInput.value.toLowerCase();
        const selectedStatus = statusFilter.value;

        cards.forEach(card => {
            const name = card.dataset.name.toLowerCase();
            const status = card.dataset.status;

            const matchesName = name.includes(searchTerm);
            const matchesStatus = selectedStatus === "" || status === selectedStatus;

            if (matchesName && matchesStatus) {
                card.style.display = "";
            } else {
                card.style.display = "none";
            }
        });
    }

    searchInput.addEventListener("input", applyFilters);
    statusFilter.addEventListener("change", applyFilters);
});

async function retrieveData(dateStart, dateEnd) {
    await getNumberAuctions(dateStart, dateEnd);
    await getNumberAuctionsByStatus(dateStart, dateEnd);
    await getGeneralReport(dateStart, dateEnd);
}


async function getStatisticsAuction(idAuction) {
    try {
        if (!idAuction) return;

        const response = await fetch(`${window.config.GetStatisticsAuctionUrl}?idAuction=${idAuction}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:
                console.log(`Datos de la subasta: ${data.body}`, 'success');
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

async function getNumberAuctions(dateStart, dateEnd) {
    try {
        const response = await fetch(`${window.config.GetNumberAuctionsUrl}?dateStart=${dateStart}&dateEnd=${dateEnd}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:
                console.log(`Número de subastas: ${data.body}`, 'success');
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

async function getNumberAuctionsByStatus (dateStart, dateEnd) {
    try {
        const response = await fetch(`${window.config.GetNumberAuctionsByStatusUrl}?dateStart=${dateStart}&dateEnd=${dateEnd}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:
                console.log(`Número de subastas: ${data.body}`, 'success');
                createChartStatuses(data.body);
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

function createChartStatuses(data) {
    const labels = data.map(item => item.name);
    const values = data.map(item => item.count);

    const ctx = document.getElementById('chartStatusesAuctions').getContext('2d');

    if (window.auctionChart) {
        window.auctionChart.destroy();
    }

    window.auctionChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: [
                    'rgba(75, 192, 192, 0.6)',
                    'rgb(29, 3, 45, 0.6)',
                    'rgba(255, 206, 86, 0.6)',
                    'rgba(255, 99, 132, 0.6)',
                    'rgb(236, 14, 63, 0.6)'
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)',
                    'rgb(29, 3, 45, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgb(236, 14, 63, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: 'bottom' },
                title: { display: true, text: 'Estados de Subastas' }
            }
        }
    });
}

async function getGeneralReport(dateStart, dateEnd) {
    try {
        const response = await fetch(`${window.config.GetGeneralReportUrl}?dateStart=${dateStart}&dateEnd=${dateEnd}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const contentType = response.headers.get('content-type');

        let data = {};
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        }

        switch (response.status) {
            case 200:

                document.getElementById('total-auctions').textContent = data.body.totalAuctionsCreated ?? 'No disponible';
                document.getElementById('avg-bids-per-auction').textContent = data.body.averageBidsPerAuction?.toFixed(2) ?? 'No disponible';
                document.getElementById('total-bids').textContent = data.body.totalBids ?? 'No disponible';
                document.getElementById('max-bids').textContent = data.body.maxBidsInAuction ?? 'No disponible';
                document.getElementById('min-bids').textContent = data.body.minBidsInAuction ?? 'No disponible';
                document.getElementById('highest-bid').textContent = formatCurrency(data.body.highestBid);
                document.getElementById('lowest-bid').textContent = formatCurrency(data.body.lowestBid);
                document.getElementById('avg-duration').textContent = formatTimeSpan(data.body.averageAuctionDuration);
                document.getElementById('longest-auction').textContent = formatTimeSpan(data.body.longestAuctionDuration);
                document.getElementById('shortest-auction').textContent = formatTimeSpan(data.body.shortestAuctionDuration);
                document.getElementById('most-recent-auction').textContent = formatDate(data.body.mostRecentAuction);
                document.getElementById('oldest-auction').textContent = formatDate(data.body.oldestAuction);
                document.getElementById('highest-gain').textContent = formatCurrency(data.body.highestAuctionGain);
                document.getElementById('lowest-gain').textContent = formatCurrency(data.body.lowestAuctionGain);

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

// Función para formatear cantidades como moneda
function formatCurrency(value) {
    if (value == null) return 'No disponible';
    return new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' }).format(value);
}

// Función para formatear duración (ej. "2h 15m")
function formatTimeSpan(timeSpan) {
    if (!timeSpan) return 'No disponible';
    const duration = typeof timeSpan === 'string' ? parseISODuration(timeSpan) : timeSpan;
    const hours = duration.hours ?? 0;
    const minutes = duration.minutes ?? 0;
    return `${hours}h ${minutes}m`;
}

// Función para parsear ISO 8601 duration string (ej. "00:45:30")
function parseISODuration(duration) {
    const parts = duration.split(':');
    return {
        hours: parseInt(parts[0]),
        minutes: parseInt(parts[1])
    };
}

// Función para formatear fechas (ej. "09/07/2025")
function formatDate(dateStr) {
    if (!dateStr) return 'No disponible';
    const date = new Date(dateStr);
    return date.toLocaleDateString('es-MX');
}