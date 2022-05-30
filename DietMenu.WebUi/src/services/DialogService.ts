import { YesNoDialogOptions } from '@/models/Dialog/YesNoDialogOptions';
import dialogState from '@/store/DialogStore';
import { InputDialogOptions } from '@/models/Dialog/InputDialogOptions';
import { GridDialogOptions } from '@/models/Dialog/GridDialogOptions';
import { DialogComponentNames } from '@/models/Dialog/DialogComponentNames';

export default function DialogService() {
    function closeDialog() {
        dialogState.closeDialog();
    }

    function showGridDialog(options: GridDialogOptions): void {
        dialogState.showDialog({ component: DialogComponentNames.GridDialog, options: options });
    }

    function showInputDialog(options: InputDialogOptions) {
        dialogState.showDialog({ component: DialogComponentNames.InputDialog, options: options });
    }

    function showYesNoDialog(options: YesNoDialogOptions) {
        dialogState.showDialog({ component: DialogComponentNames.YesNoDialog, options: options });
    }

    return {
        closeDialog,
        showGridDialog,
        showInputDialog,
        showYesNoDialog,
    }
}