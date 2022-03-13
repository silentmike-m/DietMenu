<template>
  <v-app :style="{ background: $vuetify.theme.themes['light'].background }">
    <v-main>
      <Nuxt />
    </v-main>
    <Alert />
    <DialogRoot />
  </v-app>
</template>

<script lang="ts">
import Alert from "~/components/dialogs/alert.vue";
import DialogRoot from "~/components/dialogs/dialogRoot.vue";

export default {
  name: "empty",
  components: {
    Alert,
    DialogRoot,
  },
};
</script>

<style>
.container {
  padding: 5px !important;
}
</style>