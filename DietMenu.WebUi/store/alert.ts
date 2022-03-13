import { ref } from "@vue/composition-api";
import { AlertOptions } from "~/types/core/alert"

const state = ref({} as AlertOptions);

const showMessage = (options: AlertOptions): void => {
    state.value = options;
}

const alertState = {
    showMessage,
    state,
}

export default alertState;