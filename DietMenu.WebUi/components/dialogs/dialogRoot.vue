<template>
  <div>
    <v-row justify="center" v-if="options">
      <v-dialog
        persistent
        v-model="options"
        :max-width="options.maxWidth"
        scrollable
        :fullscreen="!$vuetify.breakpoint.smAndUp"
      >
        <component
          :is="options.component"
          v-bind="options.dialogOptions"
        ></component>
      </v-dialog>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Ref, ref, watch } from "@nuxtjs/composition-api";
import Vue from "vue";
import dialogState from "~/store/dialogStore";
import { DialogOptions } from "~/types/core/dialog";

export default Vue.extend({
  name: "DialogRoot",
  setup() {
    const options: Ref<DialogOptions | null> = ref(null);

    watch(
      () => dialogState.state.value,
      () => showDialog()
    );

    function showDialog() {
      options.value = dialogState.state.value;
    }

    return {
      options,
    };
  },
  created() {
    document.addEventListener("keyup", this.handleKeyup);
  },
  beforeDestroy() {
    document.removeEventListener("keyup", this.handleKeyup);
  },
  methods: {
    handleClose() {
      dialogState.closeDialog();
    },
    handleKeyup(e: any) {
      if (e.keyCode === 27) this.handleClose();
    },
  },
});
</script>
