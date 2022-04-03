import { AlertOptions } from "@/models/Alert/AlertOptions";
import { AlertType } from "@/models/Alert/AlertType";
import { Toast } from "bootstrap";

export default function AlertService() {
    function showAlert(options: AlertOptions) {
        const toastContainer = document.getElementById('toast-container');

        if (toastContainer) {
            const div = document.createElement("div");
            div.innerHTML = getToastHtml(options);

            toastContainer.appendChild(div);

            const toastElement = div.getElementsByClassName("toast")[0];

            toastElement.addEventListener('hidden.bs.toast', function () {
                toastContainer.removeChild(div);
            })

            const toast = new Toast(toastElement);
            toast.show();
        }
    }

    function getToastHtml(options: AlertOptions): string {
        const color = options.type == AlertType.normal
            ? "bg-primary"
            : "bg-danger";

        const html = `
        <div class="toast align-items-center text-white ${color} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">${options.text}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Zamknij"></button>
            </div>
        </div>`;

        return html;
    }

    return {
        showAlert,
    }
}