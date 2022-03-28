import { ref } from "@vue/composition-api";
import { DialogOptions } from "~/types/core/dialog";

const state = ref({} as DialogOptions | null);

const closeDialog = (): void => {
    state.value = null;
}

const showDialog = (options: any): any => {
    state.value = options;
}

const dialogState = {
    closeDialog,
    showDialog,
    state,
}

export default dialogState;