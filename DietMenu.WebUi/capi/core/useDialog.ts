import { GridDialogOptions, InputDialogOptions, YesNoDialogOptions } from '~/types/core/dialog';
import GridDialog from "~/components/dialogs/dialogGrid.vue";
import InputDialog from "~/components/dialogs/dialogInput.vue";
import DialogYestNo from "~/components/dialogs/dialogYestNo.vue";
import dialogState from '~/store/dialogStore';

export default function useDialog() {

    function closeDialog(): void {
        dialogState.closeDialog();
    }

    function showGridDialog(options: GridDialogOptions, maxWidth?: number): void {
        dialogState.showDialog({ component: GridDialog, dialogOptions: [{ options: options }], maxWidth: maxWidth ?? 800 });
    }

    const showInputDialog = (options: InputDialogOptions, maxWidth?: number): void => {
        dialogState.showDialog({ component: InputDialog, dialogOptions: [{ options: options }], maxWidth: maxWidth ?? 290 });
    }

    function showYesNoQuestion(options: YesNoDialogOptions, maxWidth?: number): void {
        dialogState.showDialog({ component: DialogYestNo, dialogOptions: [{ options: options }], maxWidth: maxWidth ?? 290 });
    }

    return {
        closeDialog,
        showGridDialog,
        showInputDialog,
        showYesNoQuestion
    }
}