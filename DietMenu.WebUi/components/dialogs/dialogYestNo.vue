<template>
  <v-card elevation="5">
    <v-card-title>{{ options.title }}</v-card-title>
    <v-card-text>
      {{ options.question }}
    </v-card-text>
    <v-card-actions>
      <v-btn color="primary" elevation="5" @click="options.confirmAction()"
        >TAK</v-btn
      >
      <v-spacer />
      <v-btn color="error" outlined @click="options.cancelAction()">NIE</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "DialogYestNo",
  props: ["options"],
});
</script>