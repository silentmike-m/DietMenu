<template>
  <v-container fluid>
    <v-row dense>
      <v-col cols="12" sm="6">
        <ValidationObserver v-slot="{ invalid }">
          <v-card elevation="5">
            <v-card-title>
              <v-card-actions>
                <v-btn
                  color="primary"
                  elevation="5"
                  small
                  @click="save"
                  :disabled="invalid || !canEdit"
                  >Zapisz</v-btn
                >
                <v-menu offset-y>
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn icon v-bind="attrs" v-on="on">
                      <v-icon small>mdi-dots-vertical</v-icon>
                    </v-btn>
                  </template>
                  <v-list>
                    <v-list-item>
                      <v-list-item-action
                        ><v-btn elevation="5" small @click="canEdit = true"
                          >Edytuj</v-btn
                        ></v-list-item-action
                      >
                    </v-list-item>
                    <v-list-item>
                      <v-list-item-action
                        ><v-btn elevation="5" small @click="close"
                          >Zamknij</v-btn
                        ></v-list-item-action
                      >
                    </v-list-item>
                  </v-list>
                </v-menu>
              </v-card-actions>
            </v-card-title>
            <v-card-text>
              <ValidationProvider
                v-slot="{ errors }"
                rules="required"
                vid="recipeName"
              >
                <v-text-field
                  label="Nazwa"
                  v-model="recipe.name"
                  required
                  :error-messages="errors"
                  :readonly="!canEdit"
                  @focus="$event.target.select()"
                  @keydown.enter="selectRecipe()"
                >
                  <v-icon
                    slot="append-outer"
                    color="green"
                    @click="selectRecipe()"
                  >
                    mdi-arrow-right-bold
                  </v-icon>
                </v-text-field>
              </ValidationProvider>

              <v-text-field
                label="Rodzaj"
                v-model="recipe.meal_type_name"
                readonly
              />

              <v-text-field
                label="Białko"
                v-model.number="recipe.protein"
                required
                type="number"
                @focus="$event.target.select()"
                :readonly="!canEdit"
              >
                <v-icon
                  v-if="canEdit"
                  slot="append-outer"
                  color="green"
                  @click="recipe.protein += 1"
                  :disabled="!canEdit"
                >
                  mdi-plus
                </v-icon>
                <v-icon
                  v-if="canEdit"
                  slot="prepend"
                  color="red"
                  :disabled="!canEdit"
                  @click="recipe.protein -= 1"
                >
                  mdi-minus
                </v-icon></v-text-field
              >

              <v-text-field
                label="Energia"
                v-model.number="recipe.energy"
                required
                type="number"
                @focus="$event.target.select()"
                :readonly="!canEdit"
              >
                <v-icon
                  v-if="canEdit"
                  slot="append-outer"
                  color="green"
                  @click="recipe.energy += 1"
                  :disabled="!canEdit"
                >
                  mdi-plus
                </v-icon>
                <v-icon
                  v-if="canEdit"
                  slot="prepend"
                  color="red"
                  :disabled="!canEdit"
                  @click="recipe.energy -= 1"
                >
                  mdi-minus
                </v-icon></v-text-field
              >

              <v-text-field
                label="Tłuszcze"
                v-model.number="recipe.fat"
                required
                type="number"
                @focus="$event.target.select()"
                :readonly="!canEdit"
              >
                <v-icon
                  v-if="canEdit"
                  slot="append-outer"
                  color="green"
                  @click="recipe.fat += 1"
                  :disabled="!canEdit"
                >
                  mdi-plus
                </v-icon>
                <v-icon
                  v-if="canEdit"
                  slot="prepend"
                  color="red"
                  :disabled="!canEdit"
                  @click="recipe.fat -= 1"
                >
                  mdi-minus
                </v-icon></v-text-field
              >

              <v-text-field
                label="Węglowodany"
                v-model.number="recipe.carbohydrates"
                required
                type="number"
                @focus="$event.target.select()"
                :readonly="!canEdit"
              >
                <v-icon
                  v-if="canEdit"
                  slot="append-outer"
                  color="green"
                  @click="recipe.carbohydrates += 1"
                  :disabled="!canEdit"
                >
                  mdi-plus
                </v-icon>
                <v-icon
                  v-if="canEdit"
                  slot="prepend"
                  color="red"
                  :disabled="!canEdit"
                  @click="recipe.carbohydrates -= 1"
                >
                  mdi-minus
                </v-icon></v-text-field
              >
            </v-card-text>
          </v-card>
        </ValidationObserver>
      </v-col>
      <v-col cols="12" sm="6"
        ><v-card elevation="5"
          ><v-card-title
            >Składniki
            <v-btn v-if="canEdit" icon @click="addRow()" color="primary">
              <v-icon>mdi-plus-thick</v-icon>
            </v-btn> </v-card-title
          ><v-card-text>
            <v-simple-table :class="{ mobile: !$vuetify.breakpoint.smAndUp }"
              ><template v-slot:default v-if="$vuetify.breakpoint.smAndUp">
                <thead>
                  <tr>
                    <th class="text-left">Nazwa</th>
                    <th class="text-center">Ilość</th>
                    <th class="text-center">J.m.</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr
                    v-for="ingredient in recipe.ingredients"
                    :key="ingredient.id"
                  >
                    <td>{{ ingredient.ingredient_name }}</td>
                    <td class="text-center">
                      <template v-if="canEdit">
                        <v-text-field
                          v-model.number="ingredient.quantity"
                          required
                          type="number"
                          @focus="$event.target.select()"
                          :readonly="!canEdit"
                        >
                          <v-icon
                            slot="append-outer"
                            color="green"
                            @click="ingredient.quantity += 1"
                            :disabled="!canEdit"
                          >
                            mdi-plus
                          </v-icon>
                          <v-icon
                            slot="prepend"
                            color="red"
                            :disabled="!canEdit"
                            @click="ingredient.quantity -= 1"
                          >
                            mdi-minus
                          </v-icon></v-text-field
                        ></template
                      >
                      <template v-else>
                        {{ ingredient.quantity }}
                      </template>
                    </td>
                    <td class="text-center">{{ ingredient.unit_symbol }}</td>
                    <td>
                      <v-btn icon @click="showReplacements(ingredient)">
                        <v-icon small>mdi-magnify</v-icon>
                      </v-btn>
                      <v-btn icon v-if="canEdit" @click="deleteRow(ingredient)">
                        <v-icon small>mdi-delete</v-icon>
                      </v-btn>
                    </td>
                  </tr>
                </tbody></template
              >
              <template v-else v-slot:default
                ><tbody>
                  <tr
                    v-for="ingredient in recipe.ingredients"
                    :key="ingredient.id"
                  >
                    <v-menu>
                      <template v-slot:activator="{ on }">
                        <td @contextmenu.prevent="on.click">
                          <ul class="flex-content">
                            <li class="flex-item" data-label="Nazwa">
                              {{ ingredient.ingredient_name }}
                            </li>
                            <li class="flex-item" data-label="J.m.">
                              {{ ingredient.unit_symbol }}
                            </li>
                            <li class="flex-item" data-label="Ilość">
                              <template v-if="canEdit">
                                <v-text-field
                                  v-model.number="ingredient.quantity"
                                  required
                                  type="number"
                                  @focus="$event.target.select()"
                                  :readonly="!canEdit"
                                >
                                  <v-icon
                                    slot="append-outer"
                                    color="green"
                                    @click="ingredient.quantity += 1"
                                    :disabled="!canEdit"
                                  >
                                    mdi-plus
                                  </v-icon>
                                  <v-icon
                                    slot="prepend"
                                    color="red"
                                    :disabled="!canEdit"
                                    @click="ingredient.quantity -= 1"
                                  >
                                    mdi-minus
                                  </v-icon></v-text-field
                                ></template
                              >
                              <template v-else>
                                {{ ingredient.quantity }}
                              </template>
                            </li>
                          </ul>
                        </td>
                      </template>
                      <v-list>
                        <v-list-item @click="showReplacements(ingredient)">
                          <v-list-item-title>
                            <v-icon small>mdi-magnify</v-icon>
                            Zamienniki
                          </v-list-item-title>
                        </v-list-item>
                        <v-list-item
                          v-if="canEdit"
                          @click="deleteRow(ingredient)"
                        >
                          <v-list-item-title>
                            <v-icon small>mdi-delete</v-icon>
                            Usuń
                          </v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </tr>
                </tbody></template
              ></v-simple-table
            >
          </v-card-text></v-card
        ></v-col
      >
    </v-row>
    <v-row dense>
      <v-col
        ><v-card elevation="5"
          ><v-card-title>Opis</v-card-title
          ><v-card-text>
            <editor
              v-if="recipe && canEdit"
              :content="recipe.description"
              v-on:update="recipe.description = $event" />
            <p v-else v-html="recipe.description"></p></v-card-text></v-card
      ></v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref, useContext } from "@nuxtjs/composition-api";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import useIngredient from "~/capi/useIngredients";
import Editor from "~/components/editor.vue";
import Grid from "~/components/grid.vue";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import { GridColumn, GridColumnType, GridRequest } from "~/types/core/grid";
import useDialog from "~/capi/core/useDialog";
import { GridDialogOptions } from "~/types/core/dialog";
import { Ingredient, IngredientReplacementRow } from "~/types/ingredients";
import useDietPlans from "~/capi/useDietPlans";
import { DietPlanRecipe, DietPlanRecipeIngredient } from "~/types/dietPlans";
import useRecipes from "~/capi/useRecipes";
import { RecipeRow } from "~/types/recipes";

extend("required", {
  validate(value) {
    return {
      required: true,
      valid: ["", null, undefined].indexOf(value) === -1,
    };
  },
  computesRequired: true,
  message: "Pole nie może być puste.",
});

export default Vue.extend({
  name: "EditDietPlan",
  components: {
    Editor,
    Grid,
    ValidationObserver,
    ValidationProvider,
  },
  setup() {
    const canEdit: Ref<Boolean> = ref(false);
    const recipe: Ref<DietPlanRecipe> = ref({} as DietPlanRecipe);

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
    const { query, params, redirect }: any = useContext();
    const { showGridDialog, closeDialog } = useDialog();
    const {
      addIngredients,
      fillDietPlanRecipe,
      getDietPlanRecipe,
      getNewDietPlanRecipe,
      saveDietPlanRecipe,
    } = useDietPlans();
    const { getIngredientsGrid, getReplacementsGrid } = useIngredient();
    const { getRecipesGrid } = useRecipes();

    onMounted(() => {
      const id = params.value.id.toString();

      if (id === "new") {
        const date = query.value.date.toString();
        console.log(date);
        const mealTypeId = query.value.mealTypeId.toString();

        canEdit.value = true;
        setBreadcrumbs(BreadcrumbType.DietPlanRecipe, "Nowy");
        getNewDietPlanRecipe(date, mealTypeId).then(
          (response) => (recipe.value = response)
        );
      } else {
        getDietPlanRecipe(id).then((result) => {
          recipe.value = result;
          setBreadcrumbs(BreadcrumbType.DietPlanRecipe, recipe.value.name);
        });
      }
    });

    const addRow = (): void => {
      showGridDialog(
        new GridDialogOptions(
          "Składniki",
          true,
          (request: GridRequest) => getIngredientsGrid(request),
          (data: Ingredient[]) => {
            addIngredients(recipe.value, data);
            closeDialog();
          },
          () => {
            closeDialog();
          },
          ingredientsColumns,
          ""
        )
      );
    };

    const close = () => {
      redirect("/dietPlans");
    };

    const deleteRow = (row: DietPlanRecipeIngredient): void => {
      recipe.value.ingredients = recipe.value.ingredients.filter(
        (i) => i.id != row.id
      );
    };

    const save = () => {
      saveDietPlanRecipe(recipe.value).then((response) => {
        if (response) close();
      });
    };

    const selectRecipe = () => {
      if (!canEdit) {
        return;
      }

      showGridDialog(
        new GridDialogOptions(
          "Przepisy",
          false,
          (request: GridRequest) =>
            getRecipesGrid(request, "", recipe.value.meal_type_id),
          (data: RecipeRow[]) => {
            fillDietPlanRecipe(data[0].id, recipe.value).finally(() =>
              closeDialog()
            );
          },
          () => {
            closeDialog();
          },
          recipesColumns,
          recipe.value.name
        )
      );
    };

    const showReplacements = (row: DietPlanRecipeIngredient) => {
      showGridDialog(
        new GridDialogOptions(
          "Zamienniki",
          false,
          (request: GridRequest) =>
            getReplacementsGrid(
              row.ingredient_exchanger,
              row.quantity,
              request,
              row.ingredient_type_id
            ),
          (ingredient: IngredientReplacementRow[]) => {
            row.ingredient_id = ingredient[0].id;
            row.ingredient_exchanger = ingredient[0].exchanger;
            row.ingredient_name = ingredient[0].name;
            row.quantity = ingredient[0].quantity;
            row.unit_symbol = ingredient[0].unit_symbol;

            closeDialog();
          },
          () => {
            closeDialog();
          },
          replacementsColumns,
          ""
        )
      );
    };

    return {
      addRow,
      canEdit,
      close,
      deleteRow,
      recipe,
      save,
      selectRecipe,
      showReplacements,
    };
  },
});
</script>

<style scoped>
@media screen and (max-width: 768px) {
  .mobile table tbody tr {
    max-width: 100%;
    display: block;
  }

  .mobile table tbody tr:nth-child(odd) {
    border-left: 6px solid #00aa95;
  }

  .mobile table tbody tr td {
    display: flex;
    width: 100%;
    border-bottom: 1px solid #f5f5f5 !important;
    height: auto !important;
    padding: 0 !important;
  }

  .mobile table tbody tr td ul li:before {
    content: attr(data-label);
    padding-right: 0.5em;
    text-align: left;
    display: block;
    color: #999;
  }
  .mobile table tbody tr td ul li {
    margin-top: 0;
  }
}

table tbody tr:nth-child(odd) {
  background: #f8f8f8;
}

.flex-content {
  padding: 0;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  width: 100%;
}

.flex-item {
  padding: 2px;
  width: 50%;
  font-weight: bold;
}
</style>
