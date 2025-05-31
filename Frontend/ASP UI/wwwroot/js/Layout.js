document.getElementById('searchInput').addEventListener('keydown', function (event) {
    if (event.key === 'Enter') {
        const query = event.target.value.trim();
        if (query) {
            window.location.href = `/Product/ConsultProducts?query=${encodeURIComponent(query)}`;
        }
    }
});