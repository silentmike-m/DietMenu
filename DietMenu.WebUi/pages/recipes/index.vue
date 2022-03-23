<template>
  <v-card elevation="5">
    <v-card-title>
      <v-card-text>
        <v-select
          label="Rodzaj"
          clearable
          :items="mealTypes"
          item-text="name"
          item-value="id"
          v-model="mealTypeId"
          @change="recipesGridId += 1"
        ></v-select>
        <v-text-field
          label="Składniki"
          append-icon="mdi-magnify"
          clearable
          @click:append="recipesGridId += 1"
          @click:clear="clearIngredientFilter()"
          @keydown.enter="recipesGridId += 1"
          v-model="ingredientName"
        ></v-text-field>
      </v-card-text>
    </v-card-title>
    <v-card-text>
      <Grid
        :canAdd="canAdd"
        :canDelete="canDelete"
        :canEdit="canEdit"
        :canFilter="true"
        :canMultiSelect="false"
        :canPage="true"
        :canShow="canShow"
        :canSort="true"
        :columns="recipesColumns"
        :getData="getData"
        :id="recipesGridId"
        :initialFilter="gridFilter.filter"
        :onRowAdd="addRow"
        :onRowEdit="editRow"
        :onRowDelete="deleteRow"
      />
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import {
  onBeforeMount,
  onMounted,
  Ref,
  ref,
  useContext,
  watch,
} from "@nuxtjs/composition-api";
import Grid from "~/components/grid.vue";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import useMealTypes from "~/capi/useMealTypes";
import useRecipe from "~/capi/useRecipes";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import { RecipeRow, RecipesGridFilter } from "~/types/recipes";
import { MealType } from "~/types/mealTypes";
import { GridDialogOptions, YesNoDialogOptions } from "~/types/core/dialog";
import useDialog from "~/capi/core/useDialog";
import gridState from "~/store/gridStore";
import useIngredients from "~/capi/useIngredients";
import { Ingredient } from "~/types/ingredients";

export default Vue.extend({
  name: "Recipes",
  components: {
    Grid,
  },
  setup() {
    const canAdd = true;
    const canDelete = true;
    const canEdit = true;
    const canShow = true;
    const gridFilter: Ref<RecipesGridFilter> = ref({} as RecipesGridFilter);
    const ingredientName: Ref<string> = ref("");
    const mealTypeId: Ref<string | null> = ref(null);
    const mealTypes: Ref<MealType[]> = ref([]);
    const recipesGridId: Ref<number> = ref(0);

    const ingredientsColumns: GridColumn[] = [
      {
        value: "name",
        text: "Nazwa",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
      {
        value: "type_name",
        text: "Rodzaj",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
      {
        value: "unit_symbol",
        text: "J.m.",
        type: GridColumnType.text,
        sortable: false,
        filterable: false,
      },
    ];

    const recipesColumns: GridColumn[] = [
      {
        value: "name",
        text: "Nazwa",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
      {
        value: "meal_type_name",
        text: "Rodzaj",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
      {
        value: "protein",
        text: "Biało",
        type: GridColumnType.number,
        sortable: true,
        filterable: false,
      },
      {
        value: "energy",
        text: "Energia",
        type: GridColumnType.number,
        sortable: true,
        filterable: false,
      },
      {
        value: "fat",
        text: "Tłuszcze",
        type: GridColumnType.number,
        sortable: true,
        filterable: false,
      },
      {
        value: "carbohydrates",
        text: "Węglowodany",
        type: GridColumnType.number,
        sortable: true,
        filterable: false,
      },
    ];

    const { setBreadcrumbs } = useBreadcrumbs();
    const { redirect }: any = useContext();
    const { closeDialog, showYesNoQuestion } = useDialog();
    const { getMealTypes } = useMealTypes();
    const { deleteRecipe, getRecipesGrid } = useRecipe();

    onBeforeMount(() => {
      const filter = gridState.getFilter("RECIPES");

      if (filter !== null) {
        gridFilter.value = filter;
        ingredientName.value = gridFilter.value.ingedientFilter;
        mealTypeId.value = gridFilter.value.mealTypeId;
      }
    });

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.Recipes, "");

      getMealTypes().then((result) => (mealTypes.value = result));
    });

    const addRow = (): void => {
      redirect("/recipes/new");
    };

    const clearIngredientFilter = (): void => {
      ingredientName.value = "";
      recipesGridId.value += 1;
    };

    const deleteRow = (row: RecipeRow): void => {
      showYesNoQuestion(
        new YesNoDialogOptions(
          "Usunięcie przepisu",
          `Czy na pewno chcesz usunąć przepis ${row.name}?`,
          () => {
            deleteRecipe(row)
              .then((result) => {
                if (result) recipesGridId.value += 1;
              })
              .finally(() => closeDialog());
          },
          () => {
            closeDialog();
          }
        )
      );
    };

    const editRow = (row: RecipeRow): void => {
      redirect(`/recipes/${row.id}`);
    };

    const getData = (request: GridRequest): Promise<GridResponse> => {
      gridFilter.value.filter = request.filter;
      gridFilter.value.ingedientFilter = ingredientName.value;
      gridFilter.value.mealTypeId = mealTypeId.value;
      gridState.setFilter("RECIPES", gridFilter.value);

      return getRecipesGrid(request, ingredientName.value, mealTypeId.value);
    };

    return {
      addRow,
      canAdd,
      canDelete,
      canEdit,
      canShow,
      clearIngredientFilter,
      deleteRow,
      editRow,
      getData,
      gridFilter,
      ingredientName,
      mealTypeId,
      mealTypes,
      recipesColumns,
      recipesGridId,
    };
  },
});
</script>