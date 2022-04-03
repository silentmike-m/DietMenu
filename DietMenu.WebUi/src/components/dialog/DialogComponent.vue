<template>
  <div class="modal fade" id="dialogModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
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
import DialogComponentNames from "@/models/Dialog/DialogComponentNames";
import dialogState from "@/store/DialogStore";
import YesNoDialogComponent from "@/components/dialog/YesNoDialogComponent.vue";
import InputDialogComponent from "@/components/dialog/InputDialogComponent.vue";

export default {
  setup() {
    const options: Ref<IDialogOptions | null> = ref(null);

    watch(
      () => dialogState.state.value,
      () => showDialog()
    );

    function getComponent() {
      if (options.value?.component === DialogComponentNames.YES_NO_DIALOG) {
        return YesNoDialogComponent;
      }

      if (options.value?.component === DialogComponentNames.INPUT_DIALOG) {
        return InputDialogComponent;
      }
    }

    function showDialog() {
      var exampleModal = document.getElementById("dialogModal");

      if (exampleModal !== null) {
        options.value = dialogState.state.value;
        const modal = Modal.getOrCreateInstance(exampleModal);
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