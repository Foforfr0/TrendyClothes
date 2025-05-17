// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

export function getValueDOMElementNullOrEmpty(id) {
    const element = document.getElementById(id);
    return element ? element.value || "" : "";
}

export function showToast(message, type) {
    // type: success, danger, warning, info, primary
    const toastEl = document.getElementById("liveToast");
    const toastBody = document.getElementById("toastMessage");

    toastBody.textContent = message;
    toastEl.className = `toast align-items-center text-bg-${type} border-0`;

    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}

/*
export function logoutSession() {
    const response = await fetch(`${window.BACKEND_URL}/api/User/Auth/Logout`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        }
    });
    localStorage.removeItem('jwtToken');
}*/