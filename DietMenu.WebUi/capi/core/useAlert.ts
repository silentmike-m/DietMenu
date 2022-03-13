import alertState from '~/store/alert';

export default function useAlert() {
    function showError(message: string): void {
        alertState.showMessage({ message: message, type: "error", duration: 5000 });
    }

    function showSuccess(message: string): void {
        alertState.showMessage({ message: message, type: "success", duration: 5000 });
    }
    return {
        showError,
        showSuccess,
    }
}