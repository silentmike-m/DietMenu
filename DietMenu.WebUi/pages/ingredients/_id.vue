<!--
<template>
  <v-card elevation="5">
    <ValidationObserver v-slot="{ invalid }">
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
      <v-card-text v-if="ingredient">
        <ValidationProvider
          v-slot="{ errors }"
          rules="required"
          vid="ingredientName"
        >
          <v-text-field
            label="Nazwa"
            v-model="ingredient.name"
            required
            :error-messages="errors"
            :readonly="!canEdit"
          ></v-text-field>
        </ValidationProvider>
        <ValidationProvider
          v-slot="{ errors }"
          rules="required"
          vid="ingredientType"
        >
          <v-autocomplete
            auto-select-first
            :items="ingredientTypes"
            item-text="name"
            item-value="id"
            v-model="ingredient.type_id"
            :error-messages="errors"
            :readonly="!canEdit"
          ></v-autocomplete>
        </ValidationProvider>
        <ValidationProvider
          v-slot="{ errors }"
          rules="required"
          vid="ingredientUnitSymbol"
        >
          <v-text-field
            label="J.m."
            v-model="ingredient.unit_symbol"
            required
            :error-messages="errors"
            :readonly="!canEdit"
          ></v-text-field>
        </ValidationProvider>
        <ValidationProvider
          v-slot="{ errors }"
          rules="required"
          vid="ingredientExchanger"
        >
          <v-text-field
            label="Przelicznik"
            v-model.number="ingredient.exchanger"
            required
            type="number"
            @focus="$event.target.select()"
            style="max-width: 210px"
            :error-messages="errors"
            :readonly="!canEdit"
          >
            <v-icon
              v-if="canEdit"
              slot="append-outer"
              color="green"
              @click="increment()"
              :disabled="!canEdit"
            >
              mdi-plus
            </v-icon>
            <v-icon
              v-if="canEdit"
              slot="prepend"
              color="red"
              :disabled="ingredient.exchanger === 0 || !canEdit"
              @click="decrement()"
            >
              mdi-minus
            </v-icon></v-text-field
          ></ValidationProvider
        >
      </v-card-text>
    </ValidationObserver>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref, useContext } from "@nuxtjs/composition-api";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";
import useIngredient from "~/capi/useIngredients";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import { Ingredient, IngredientType } from "~/types/ingredients";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import editorState from "~/store/editorStore";
import { GridRequest } from "~/types/core/grid";
import useAlert from "~/capi/core/useAlert";
import useDialog from "~/capi/core/useDialog";
import { YesNoDialogOptions } from "~/types/core/dialog";

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
  name: "EditIngredient",
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  setup() {
    const canEdit: Ref<Boolean> = ref(false);
    const ingredient: Ref<Ingredient> = ref({} as Ingredient);
    const ingredientTypes: Ref<IngredientType[]> = ref([]);

    const { showError } = useAlert();
    const { setBreadcrumbs } = useBreadcrumbs();
    const { params, redirect }: any = useContext();
    const { showYesNoQuestion, closeDialog } = useDialog();
    const {
      getIngredient,
      getIngredientTypesGrid,
      getNewIngredient,
      saveIngredient,
    } = useIngredient();

    onMounted(() => {
      const id = params.value.id.toString();

      let gridRequest: GridRequest = {
        filter: "",
        order_by: "name",
        is_descending: false,
        is_paged: false,
        page_number: 0,
        page_size: 0,
      };

      getIngredientTypesGrid(gridRequest).then(
        (result) => (ingredientTypes.value = result.elements)
      );

      if (id === "new") {
        canEdit.value = true;
        ingredient.value = getNewIngredient();
        setBreadcrumbs(BreadcrumbType.IngredientDetails, "Nowy");
      } else {
        ingredient.value = editorState.getData(id);

        if (ingredient.value === null) {
          getIngredient(id).then((result) => {
            ingredient.value = result;
            setIngredient();
          });
        } else {
          setIngredient();
        }
      }
    });

    const setIngredient = () => {
      if (ingredient.value === null) {
        showError("Błąd pobierania składnika");
      } else {
        setBreadcrumbs(BreadcrumbType.IngredientDetails, ingredient.value.name);
      }
    };

    const close = () => {
      redirect("/ingredients");
    };

    const decrement = () => {
      if (ingredient.value.exchanger === 0) return;
      ingredient.value.exchanger--;
    };

    const increment = () => {
      ingredient.value.exchanger++;
    };

    const save = () => {
      if (ingredient.value.is_system) {
        showYesNoQuestion(
          new YesNoDialogOptions(
            "Systemowy składnik",
            `Czy na pewno chcesz zaktualizować systemowy składnik?`,
            () => {
              saveIngredient(ingredient.value)
                .then((response) => {
                  if (response) close();
                })
                .finally(() => closeDialog());
            },
            () => {
              closeDialog();
            }
          )
        );
      } else {
        saveIngredient(ingredient.value).then((response) => {
          if (response) close();
        });
      }
    };

    return {
      canEdit,
      close,
      decrement,
      increment,
      ingredient,
      ingredientTypes,
      save,
    };
  },
});
</script>

-->