<template>
  <div class="card border-primary shadow">
    <div class="card-header">Rodzaje składników</div>
    <div class="card-body">
      <GridComponent
        :canAdd="true"
        :canDelete="true"
        :canEdit="true"
        :canFilter="true"
        :canPage="true"
        :canSort="true"
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
import DialogService from "@/services/DialogService";
import MealTypeService from "@/services/MealTypeService";
import { GridColumnType } from "@/models/Grid/GridColumnType";
import { MealType } from "@/models/MealType";

export default {
  components: {
    GridComponent,
  },
  setup() {
    const gridColumns: GridColumn[] = [
      {
        filterable: false,
        sortable: false,
        title: "Kolejność",
        type: GridColumnType.number,
        value: "order",
      },
      {
        filterable: false,
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
    ];

    const { closeDialog, showInputDialog } = DialogService();
    const { getMealTypesGrid } = MealTypeService();

    const addMealType = () => {
      console.log("ADD MEAL TYPE");
      showInputDialog({
        title: "Dodawanie",
        value: "Test",
        label: "Dodaj wartość",
        confirmText: "Zapisz",
        confirmAction: (value: string) => {
          console.log(value);
          closeDialog();
        },
        cancelText: "Anuluj",
        cancelAction: () => {
          console.log("ANULUJ");
          closeDialog();
        },
      });
    };

    const deleteMealType = (mealType: MealType) => {
      console.log("DELETE MEAL TYPE");
      console.log(mealType);
      console.log(mealType.name);
    };

    const editMealType = (mealType: MealType) => {
      console.log("EDIT MEAL TYPE");
      console.log(mealType);
      console.log(mealType.name);
    };

    const getGridData = (request: GridRequest): Promise<GridResponse> => {
      return getMealTypesGrid(request);
    };

    return {
      addMealType,
      deleteMealType,
      editMealType,
      gridColumns,
      getGridData,
    };
  },
};
</script>

<style scoped>
.card {
  margin: 2px;
}
</style>