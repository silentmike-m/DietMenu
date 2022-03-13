<template>
  <v-card elevation="5">
    <v-card-text>
      <Grid
        :canFilter="true"
        :canPage="true"
        :canSort="true"
        :columns="ingredientTypeColumns"
        :getData="getData"
        :id="ingredientTypesGridId"
      />
    </v-card-text>
  </v-card>
</template>


<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref } from "@nuxtjs/composition-api";
import Grid from "~/components/grid.vue";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import useIngredient from "~/capi/useIngredients";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";

export default Vue.extend({
  name: "Ingredients",
  components: {
    Grid,
  },
  setup() {
    const ingredientTypesGridId: Ref<number> = ref(0);

    const ingredientTypeColumns: GridColumn[] = [
      {
        value: "name",
        text: "Nazwa",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
    ];

    const { setBreadcrumbs } = useBreadcrumbs();
    const { getIngredientTypesGrid } = useIngredient();

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.IngredientTypes, "");
    });

    const getData = (request: GridRequest): Promise<GridResponse> => {
      return getIngredientTypesGrid(request);
    };

    return {
      ingredientTypesGridId,
      ingredientTypeColumns,
      getData,
    };
  },
});
</script>