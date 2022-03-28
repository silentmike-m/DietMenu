<template>
  <div>
    <v-breadcrumbs :items="items" divider="/" class="white--text">
      <template v-slot:item="{ item }">
        <v-breadcrumbs-item
          tag="nuxt-link"
          :to="item.href"
          :exact="true"
          :disabled="item.disabled"
        >
          {{ item.text.toUpperCase() }}
        </v-breadcrumbs-item>
      </template>
    </v-breadcrumbs>
  </div>
</template>

<script lang="ts">
import { Ref, ref, watch } from "@nuxtjs/composition-api";
import Vue from "vue";
import breadCrumbsState from "~/store/breadCrumbsStore";
import { BreadcrumbItem } from "~/types/core/breadcrumbs";

export default Vue.extend({
  name: "Breadcrumbs",
  setup() {
    const items: Ref<BreadcrumbItem[]> = ref([]);

    watch(
      () => breadCrumbsState.state.value,
      () => (items.value = breadCrumbsState.state.value)
    );

    return {
      items,
    };
  },
});
</script>

<style>
.v-breadcrumbs__item {
  color: white !important;
}

.v-breadcrumbs__divider {
  color: white !important;
}
</style>