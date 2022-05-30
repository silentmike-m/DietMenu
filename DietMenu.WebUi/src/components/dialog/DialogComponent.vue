<template>
  <div class="modal fade" id="dialogModal" tabindex="-1">
    <div
      class="
        modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl
      "
    >
      <div class="modal-content">
        <div class="modal-header">
          <h5 v-if="options" class="modal-title">
            {{ options.options.title }}
          </h5>
        </div>
        <component
          v-if="options"
          :is="getComponent()"
          v-bind:options="options.options"
        ></component>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Modal } from "bootstrap";
import { ref, Ref, watch } from "vue";
import { IDialogOptions } from "@/models/Dialog/IDialogOptions";
import dialogState from "@/store/DialogStore";
import GridDialogComponent from "@/components/dialog/GridDialogComponent.vue";
import YesNoDialogComponent from "@/components/dialog/YesNoDialogComponent.vue";
import InputDialogComponent from "@/components/dialog/InputDialogComponent.vue";
import { DialogComponentNames } from "@/models/Dialog/DialogComponentNames";

export default {
  setup() {
    const options: Ref<IDialogOptions | null> = ref(null);

    watch(
      () => dialogState.state.value,
      () => showDialog()
    );

    function getComponent() {
      if (options.value?.component === DialogComponentNames.YesNoDialog) {
        return YesNoDialogComponent;
      }

      if (options.value?.component === DialogComponentNames.InputDialog) {
        return InputDialogComponent;
      }

      if (options.value?.component === DialogComponentNames.GridDialog) {
        return GridDialogComponent;
      }
    }

    function showDialog() {
      var dialogModal = document.getElementById("dialogModal");

      if (dialogModal !== null) {
        options.value = dialogState.state.value;
        const modal = Modal.getOrCreateInstance(dialogModal);
        modal.toggle();
      }
    }

    return {
      options,
      getComponent,
    };
  },
};
</script>