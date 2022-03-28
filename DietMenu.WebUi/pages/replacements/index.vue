<!--
<template>
  <v-card elevation="5">
    <v-card-title>
      <v-card-text>
        <v-row dense>
          <v-text-field
            label="Składnik"
            append-icon="mdi-magnify"
            @click:append="selectIngredient()"
            @focus="$event.target.select()"
            @keydown.enter="searchIngredient()"
            class="ingredient"
            v-model="ingredientName"
          ></v-text-field>

          <v-text-field
            label="Ilość"
            type="number"
            v-model="quantity"
            hide-details
            style="max-width: 210px"
            class="right-input"
            @keydown.enter="getReplacements()"
            @focus="$event.target.select()"
          >
            <v-icon slot="append-outer" color="green" @click="increment()">
              mdi-plus
            </v-icon>
            <v-icon
              slot="prepend"
              color="red"
              :disabled="quantity === 0"
              @click="decrement()"
            >
              mdi-minus
            </v-icon>
          </v-text-field>
          <v-card-actions>
            <v-btn
              color="primary"
              elevation="5"
              @click="getReplacements()"
              small
              ><v-icon>mdi-calculator</v-icon></v-btn
            >
          </v-card-actions>
        </v-row>
      </v-card-text>
    </v-card-title>
    <v-card-text>
      <Grid
        :canFilter="true"
        :canPage="true"
        :canSort="true"
        :columns="replacementsColumns"
        :getData="getData"
        :id="replacementsGridId"
      />
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref, watch } from "@nuxtjs/composition-api";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import useDialog from "~/capi/core/useDialog";
import { GridDialogOptions } from "~/types/core/dialog";
import { Ingredient } from "~/types/ingredients";
import useIngredient from "~/capi/useIngredients";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";

export default Vue.extend({
  name: "Replacements",
  setup() {
    const ingredientName: Ref<string> = ref("");
    const ingredientRow: Ref<Ingredient | null> = ref(null);
    const quantity: Ref<number> = ref(0);
    const replacementsGridId: Ref<number> = ref(0);

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

    const replacementsColumns: GridColumn[] = [
      {
        value: "name",
        text: "Nazwa",
        type: GridColumnType.text,
        sortable: true,
        filterable: true,
      },
      {
        value: "quantity",
        text: "Ilość",
        type: GridColumnType.number,
        sortable: true,
        filterable: false,
      },
      {
        value: "unit_symbol",
        text: "J.m.",
        type: GridColumnType.text,
        sortable: false,
        filterable: false,
      },
    ];

    const { setBreadcrumbs } = useBreadcrumbs();
    const { showGridDialog, closeDialog } = useDialog();
    const { getIngredientsGrid, getReplacementsGrid } = useIngredient();

    watch(
      () => ingredientName.value,
      () => {
        if (ingredientName.value === "") {
          ingredientRow.value = null;
        }
      }
    );

    watch(quantity, () => {
      if (quantity.value < 0) quantity.value = 0;
    });

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.Replacements, "");
    });

    const decrement = () => {
      if (quantity.value === 0) return;
      quantity.value--;
    };

    const getData = (request: GridRequest): Promise<GridResponse> => {
      if (ingredientRow.value === null) {
        return Promise.reject("");
      }

      return getReplacementsGrid(
        ingredientRow.value?.exchanger ?? 0,
        quantity.value,
        request,
        ingredientRow.value?.type_id ?? ""
      );
    };

    const getReplacements = (): void => {
      if (ingredientRow.value === null) {
        return;
      }

      replacementsGridId.value++;
    };

    const increment = () => {
      quantity.value++;
    };

    const searchIngredient = (): void => {
      let gridRequest: GridRequest = {
        filter: ingredientName.value,
        order_by: "name",
        is_descending: false,
        is_paged: true,
        page_number: 0,
        page_size: 10,
      };

      getIngredientsGrid(gridRequest).then((response) => {
        if (response.count === 1) {
          ingredientName.value = response.elements[0].name;
          ingredientRow.value = response.elements[0];
          getReplacements();
        } else {
          selectIngredient();
        }
      });
    };

    const selectIngredient = () => {
      showGridDialog(
        new GridDialogOptions(
          "Składniki",
          false,
          (request: GridRequest) => getIngredientsGrid(request),
          (data: Ingredient[]) => {
            ingredientName.value = data[0].name;
            ingredientRow.value = data[0];
            closeDialog();
          },
          () => {
            closeDialog();
          },
          ingredientsColumns,
          ingredientName.value
        )
      );
    };

    return {
      decrement,
      getData,
      getReplacements,
      increment,
      ingredientName,
      quantity,
      searchIngredient,
      selectIngredient,
      replacementsColumns,
      replacementsGridId,
    };
  },
});
</script>

<style scoped>
.ingredient {
  max-width: 200px;
  margin-right: 10px;
}
</style>
-->