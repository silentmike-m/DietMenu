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
import MealTypeService from "@/services/MealTypeService";
import { GridColumnType } from "@/models/Grid/GridColumnType";
import { GetMealTypesGrid } from "@/models/MealType/MealTypeRequests";

export default {
  components: {
    GridComponent,
  },
  setup() {
    const gridColumns: GridColumn[] = [
      {
        sortable: false,
        title: "Kolejność",
        type: GridColumnType.number,
        value: "order",
      },
      {
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
    ];

    const { getMealTypesGrid } = MealTypeService();

    const getGridData = (gridRequest: GridRequest): Promise<GridResponse> => {
      const request = new GetMealTypesGrid();
      request.grid_request = gridRequest;

      return getMealTypesGrid(request);
    };

    return {
      gridColumns,
      getGridData,
    };
  },
};
</script>