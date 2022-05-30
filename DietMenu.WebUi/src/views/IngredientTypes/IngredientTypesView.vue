<template>
  <div class="card border-primary shadow">
    <div class="card-header">Rodzaje składników</div>
    <div class="card-body">
      <GridComponent :columns="gridColumns" :getGridData="getGridData" />
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
import { GetIngredientTypesGrid } from "@/models/IngredientType/IngredientTypeRequests";

export default {
  components: {
    GridComponent,
  },
  setup() {
    const gridColumns: GridColumn[] = [
      {
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
    ];

    const { getIngredientTypesGrid } = IngredientTypeService();

    const getGridData = (gridRequest: GridRequest): Promise<GridResponse> => {
      const request = new GetIngredientTypesGrid();
      request.grid_request = gridRequest;

      return getIngredientTypesGrid(request);
    };

    return {
      gridColumns,
      getGridData,
    };
  },
};
</script>