<template>
  <div class="card border-primary shadow">
    <div class="card-header">Rodzaje składników</div>
    <div class="card-body">
      <div>
        <select
          class="form-select"
          aria-label="Rodzaj"
          v-model="ingredientTypeId"
          @change="refreshGrid()"
        >
          <option :value="null" :selected="ingredientTypeId === null">
            wszystkie
          </option>
          <option
            v-for="type in ingredientTypes"
            :value="type.id"
            :key="type.id"
            :selected="ingredientTypeId === type.id"
          >
            {{ type.name }}
          </option>
        </select>
      </div>
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
        ref="grid"
      />
    </div>
  </div>
</template>

<script lang="ts">
import GridComponent from "@/components/grid/GridComponent.vue";
import { GridColumn } from "@/models/Grid/GridColumn";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";
import IngredientService from "@/services/IngredientService";
import IngredientTypeService from "@/services/IngredientTypeService";
import { GridColumnType } from "@/models/Grid/GridColumnType";
import { onMounted, ref, Ref } from "@vue/runtime-core";
import { IngredientType } from "@/models/IngredientType";

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

    const grid: Ref<any> = ref(null);
    const ingredientTypeId: Ref<string | null> = ref(null);
    const ingredientTypes: Ref<IngredientType[]> = ref([]);

    const { getIngredientsGrid } = IngredientService();
    const { getIngredientTypes } = IngredientTypeService();

    onMounted(() => {
      getIngredientTypes().then(
        (response) => (ingredientTypes.value = response)
      );
    });

    const getGridData = (request: GridRequest): Promise<GridResponse> => {
      return getIngredientsGrid(request, ingredientTypeId.value);
    };

    const refreshGrid = () => {
      grid.value?.getData();
    };

    return {
      grid,
      gridColumns,
      ingredientTypeId,
      ingredientTypes,
      getGridData,
      refreshGrid,
    };
  },
};
</script>