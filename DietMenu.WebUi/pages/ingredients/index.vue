<template>
  <v-card elevation="5">
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
        :columns="ingredientColumns"
        :getData="getData"
        :id="ingredientsGridId"
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
} from "@nuxtjs/composition-api";
import Grid from "~/components/grid.vue";
import useAlert from "~/capi/core/useAlert";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import useDialog from "~/capi/core/useDialog";
import useIngredient from "~/capi/useIngredients";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import { YesNoDialogOptions } from "~/types/core/dialog";
import { Ingredient, IngredientsGridFilter } from "~/types/ingredients";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";
import editorState from "~/store/editor";
import gridState from "~/store/grid";

export default Vue.extend({
  name: "Ingredients",
  components: {
    Grid,
  },
  setup() {
    const ingredientsGridId: Ref<number> = ref(0);
    const canAdd = true;
    const canDelete = true;
    const canEdit = true;
    const canShow = true;
    const gridFilter: Ref<IngredientsGridFilter> = ref(
      {} as IngredientsGridFilter
    );

    const ingredientColumns: GridColumn[] = [
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

    const { showError } = useAlert();
    const { setBreadcrumbs } = useBreadcrumbs();
    const { redirect } = useContext();
    const { showYesNoQuestion, closeDialog } = useDialog();
    const { deleteIngredient, getIngredientsGrid } = useIngredient();

    onBeforeMount(() => {
      const filter = gridState.getFilter("INGREDIENTS");

      if (filter !== null) {
        gridFilter.value = filter;
      }
    });

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.Ingredient, "");
    });

    const addRow = (): void => {
      redirect("/ingredients/new");
    };

    const deleteRow = (row: Ingredient): void => {
      if (row.is_system) {
        showError("Nie można usunąć systemowego składnika");
      } else {
        showYesNoQuestion(
          new YesNoDialogOptions(
            "Usunięcie składnika",
            `Czy na pewno chcesz usunąć składnik ${row.name}?`,
            () => {
              deleteIngredient(row)
                .then((result) => {
                  if (result) ingredientsGridId.value += 1;
                })
                .finally(() => closeDialog());
            },
            () => {
              closeDialog();
            }
          )
        );
      }
    };

    const editRow = (row: Ingredient): void => {
      editorState.setData(row.id, row);
      redirect(`/ingredients/${row.id}`);
    };

    const getData = (request: GridRequest): Promise<GridResponse> => {
      gridFilter.value.filter = request.filter;
      gridState.setFilter("INGREDIENTS", gridFilter.value);

      return getIngredientsGrid(request);
    };

    return {
      addRow,
      canAdd,
      canDelete,
      canEdit,
      canShow,
      deleteRow,
      editRow,
      getData,
      gridFilter,
      ingredientsGridId,
      ingredientColumns,
    };
  },
});
</script>