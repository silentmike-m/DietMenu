<template></template>

<script lang="ts">
import Vue from "vue";
import { onMounted } from "@nuxtjs/composition-api";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import { BreadcrumbType } from "~/types/core/breadcrumbs";

export default Vue.extend({
  name: "HomePage",
  setup() {
    const { setBreadcrumbs } = useBreadcrumbs();

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.Desktop, "");
    });

    return {};
  },
});
</script>
