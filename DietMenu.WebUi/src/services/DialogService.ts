import { YesNoDialogOptions } from '@/models/Dialog/YesNoDialogOptions';
import DialogComponents from '@/models/Dialog/DialogComponentNames';
import dialogState from '@/store/DialogStore';
import { InputDialogOptions } from '@/models/Dialog/InputDialogOptions';

export default function DialogService() {
    function closeDialog() {
        dialogState.closeDialog();
    }

    function showInputDialog(options: InputDialogOptions) {
        dialogState.showDialog({ component: DialogComponents.INPUT_DIALOG, options: options });
    }

    function showYesNoDialog(options: YesNoDialogOptions) {
        dialogState.showDialog({ component: DialogComponents.YES_NO_DIALOG, options: options });
    }

    return {
        closeDialog,
        showInputDialog,
        showYesNoDialog,
    }
}