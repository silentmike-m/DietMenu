<template>
  <div class="card border-primary shadow">
    <div class="card-header">Zamienniki składników</div>
    <div class="card-body">
      <div class="mb-5">
        <div class="mb-3">
          <label for="ingredientName">Składnik</label>
          <div class="input-group">
            <input
              type="text"
              class="form-control"
              id="ingredientName"
              required
              v-model="ingredientName"
              @focus="$event.target.select()"
              @keydown.enter="searchIngredient()"
            />
            <button
              class="btn btn-outline-secondary"
              type="button"
              @click="searchIngredient()"
            >
              <i class="bi-search" />
            </button>
          </div>
        </div>
        <div class="mb-3">
          <label for="ingredientQuantity" class="form-label">Przelicznik</label>
          <div class="input-group mb-3">
            <button
              class="btn btn-danger"
              type="button"
              :disabled="quantity === 0"
              @click="decrement()"
            >
              <i class="bi-dash" />
            </button>
            <input
              type="number"
              class="form-control"
              id="ingredientQuantity"
              min="0"
              oninput="validity.valid||(value=0);"
              v-model="quantity"
              @focus="$event.target.select()"
            />
            <button
              class="btn btn-secondary"
              type="button"
              @click="increment()"
            >
              <i class="bi-plus" />
            </button>
          </div>
        </div>
      </div>
      <GridComponent
        :canFilter="true"
        :canPage="true"
        :canSort="true"
        :columns="gridColumns"
        :getGridData="getGridData"
        :stopOnLoad="true"
        ref="grid"
      />
    </div>
  </div>
</template>

<script lang="ts">
import GridComponent from "@/components/grid/GridComponent.vue";
import { GridColumn } from "@/models/Grid/GridColumn";
import { GridColumnType } from "@/models/Grid/GridColumnType";
import { ref, Ref } from "vue";
import IngredientService from "@/services/IngredientService";
import {
  GetIngredientsGrid,
  GetReplacementsGrid,
} from "@/models/Ingredient/IngredientRequests";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";
import { Ingredient } from "@/models/Ingredient/Ingredient";
import DialogService from "@/services/DialogService";

export default {
  components: {
    GridComponent,
  },
  setup() {
    const ingredientsColumns: GridColumn[] = [
      {
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
      {
        sortable: false,
        title: "Rodzaj",
        type: GridColumnType.text,
        value: "type_name",
      },
      {
        sortable: false,
        title: "Przelicznik",
        type: GridColumnType.number,
        value: "exchanger",
      },
      {
        sortable: false,
        title: "J.m.",
        type: GridColumnType.text,
        value: "unit_symbol",
      },
    ];

    const gridColumns: GridColumn[] = [
      {
        sortable: false,
        title: "Nazwa",
        type: GridColumnType.text,
        value: "name",
      },
      {
        sortable: false,
        title: "Ilość",
        type: GridColumnType.number,
        value: "quantity",
      },
      {
        sortable: false,
        title: "J.m.",
        type: GridColumnType.text,
        value: "unit_symbol",
      },
      {
        sortable: false,
        title: "Przelicznik",
        type: GridColumnType.number,
        value: "exchanger",
      },
    ];

    const grid: Ref<any> = ref(null);
    const ingredientName: Ref<string> = ref("");
    const ingredient: Ref<Ingredient | null> = ref(null);
    const quantity: Ref<number> = ref(0);

    const { closeDialog, showGridDialog } = DialogService();
    const { getIngredientsGrid, getReplacementsGrid } = IngredientService();

    const decrement = () => {
      if (quantity.value === 0) {
        return;
      }

      quantity.value--;
    };

    const getGridData = (gridRequest: GridRequest): Promise<GridResponse> => {
      const request = new GetReplacementsGrid();
      request.exchanger = 0;
      request.grid_request = gridRequest;
      request.quantity = 0;
      request.type_id = "";

      return getReplacementsGrid(request);
    };

    const increment = () => {
      quantity.value++;
    };

    const searchIngredient = () => {
      showGridDialog({
        title: "Składniki",
        canMultiSelect: false,
        getGridData: (gridRequest: GridRequest) => {
          const request = new GetIngredientsGrid();
          request.grid_request = gridRequest;

          return getIngredientsGrid(request);
        },
        selectAction: (data: Ingredient[]) => {
          ingredientName.value = data[0].name;
          ingredient.value = data[0];
          closeDialog();
        },
        cancelAction: () => closeDialog(),
        columns: ingredientsColumns,
        initialFilter: ingredientName.value,
      });
    };

    const refreshGrid = () => {
      grid.value?.getData();
    };

    return {
      grid,
      gridColumns,
      ingredientName,
      quantity,
      decrement,
      getGridData,
      increment,
      searchIngredient,
      refreshGrid,
    };
  },
};
</script>