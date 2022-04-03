import { IDialogOptions } from "@/models/Dialog/IDialogOptions";
import { ref } from "vue";

const state = ref({} as IDialogOptions | null);

const closeDialog = () => {
    state.value = null;
}

const showDialog = (options: IDialogOptions) => {
    state.value = options;
}

const dialogState = {
    closeDialog,
    showDialog,
    state,
}

export default dialogState;