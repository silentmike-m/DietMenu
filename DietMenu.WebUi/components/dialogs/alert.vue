<template>
  <div class="alert-container" v-if="options">
    <v-alert
      v-model="isVisible"
      border="left"
      :type="options.type"
      dismissible
      transition="scale-transition"
    >
      <span v-html="options.message">
        {{ options.message }}
      </span>
    </v-alert>
  </div>
</template>


<script lang="ts">
import { ref, Ref, watch } from "@nuxtjs/composition-api";
import Vue from "vue";
import alertState from "~/store/alert";
import { AlertOptions } from "~/types/core/alert";

export default Vue.extend({
  name: "Alert",
  setup() {
    const options: Ref<AlertOptions | null> = ref(null);
    const timer: Ref<number> = ref(0);
    const isVisible: Ref<boolean> = ref(false);

    watch(
      () => alertState.state.value,
      () => showAlert()
    );

    function showAlert() {
      options.value = alertState.state.value;
      isVisible.value = true;

      if (options.value?.duration && options.value.duration > 0) {
        window.clearTimeout(timer.value);
        timer.value = window.setTimeout(() => {
          options.value = null;
          isVisible.value = false;
        }, options.value.duration);
      }
    }

    return {
      isVisible,
      options,
    };
  },
});
</script>

<style scoped>
.alert-container {
  width: 100%;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 100;
  width: 100%;
}
</style>