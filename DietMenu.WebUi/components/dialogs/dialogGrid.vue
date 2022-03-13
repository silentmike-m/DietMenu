<template>
  <v-card elevation="5">
    <v-card-title>{{ options.title }}</v-card-title>
    <v-card-text>
      <Grid
        :canAdd="false"
        :canEdit="false"
        :canShow="false"
        :canDelete="false"
        :canFilter="true"
        :canMultiSelect="options.multiSelect"
        :canPage="true"
        :canSelect="true"
        :canSort="true"
        :columns="options.columns"
        :getData="options.getData"
        :initialFilter="options.initialFilter"
        :onFocusRow="save"
        ref="grid"
    /></v-card-text>
    <v-card-actions>
      <v-btn color="primary" elevation="5" small @click="save()">Zapisz </v-btn>
      <v-spacer />
      <v-btn small outlined @click="options.cancelAction()"> Anuluj </v-btn>
    </v-card-actions>
  </v-card>
</template>


<script lang="ts">
import Vue from "vue";
import Grid from "~/components/grid.vue";

export default Vue.extend({
  name: "DialogGrid",
  components: {
    Grid,
  },
  props: ["options"],
  data() {
    return {
      value: this.options.value,
    };
  },
  methods: {
    save() {
      let data = (this.$refs.grid as any).selected;
      if (data.length > 0) this.$props.options.selectAction(data);
    },
  },
});
</script>

