<template>
  <div class="card border-primary shadow">
    <div class="card-header">Rodzaje składników</div>
    <div class="card-body">
      <GridComponent
        :columns="gridColumns"
        :getGridData="getGridData"
        :onElementAdd="addMealType"
        :onElementEdit="deleteMealType"
        :onElementDelete="editMealType"
      />
    </div>
  </div>
</template>

<script lang="ts">
import GridComponent from "@/components/grid/GridComponent.vue";
import { GridColumn } from "@/models/Grid/GridColumn";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";
import IngredientTypeService from "@/services/IngredientTypeService";
import { GridColumnType } from "@/models/Grid/GridColumnType";

export default {
  components: {
    GridComponent,
  },
  setup() {
    const gridColumns: GridColumn[] = [
      {
        filterable: false,
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
    ];

    const { getIngredientTypesGrid } = IngredientTypeService();

    const getGridData = (request: GridRequest): Promise<GridResponse> => {
      return getIngredientTypesGrid(request);
    };

    return {
      gridColumns,
      getGridData,
    };
  },
};
</script>