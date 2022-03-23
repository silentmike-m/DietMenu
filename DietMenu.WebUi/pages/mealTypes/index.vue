<template>
  <v-card elevation="5">
    <v-card-text>
      <Grid
        :canFilter="false"
        :canPage="false"
        :canSort="true"
        :columns="mealTypeColumns"
        :getData="getData"
        :id="mealTypesGridId"
        name="MealTypes"
      />
    </v-card-text>
  </v-card>
</template>


<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref } from "@nuxtjs/composition-api";
import Grid from "~/components/grid.vue";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";
import useMealTypes from "~/capi/useMealTypes";

export default Vue.extend({
  name: "Ingredients",
  components: {
    Grid,
  },
  setup() {
    const mealTypesGridId: Ref<number> = ref(0);

    const mealTypeColumns: GridColumn[] = [
      {
        value: "order",
        text: "Kolejność",
        type: GridColumnType.number,
        sortable: true,
        filterable: true,
      },
      {
        value: "name",
        text: "Nazwa",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
    ];

    const { setBreadcrumbs } = useBreadcrumbs();
    const { getMealTypesGrid } = useMealTypes();

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.MealTypes, "");
    });

    const getData = (request: GridRequest): Promise<GridResponse> => {
      return getMealTypesGrid(request);
    };

    return {
      mealTypesGridId,
      mealTypeColumns,
      getData,
    };
  },
});
</script>