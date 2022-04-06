<template>
  <div class="card border-primary shadow">
    <div class="card-header">
      {{ id == "new" ? "Tworzenie składanika" : "Edycja składnika" }}
    </div>
    <div class="card-body">
      <form id="ingredientForm" novalidate ref="form">
        <div class="mb-3">
          <label for="ingredientName" class="form-label">Nazwa</label>
          <input
            type="text"
            class="form-control"
            id="ingredientName"
            required
            v-model="ingredient.name"
          />
          <div class="invalid-feedback">Należy podać nazwę składnika.</div>
        </div>
        <div class="mb-3">
          <label for="ingredientTypeSelection">Rodzaj</label>
          <select
            id="ingredientTypeSelection"
            class="form-select"
            aria-label="Rodzaj"
            v-model="ingredient.type_id"
            required
          >
            <option
              v-for="type in ingredientTypes"
              :value="type.id"
              :key="type.id"
              :selected="ingredient.type_id === type.id"
            >
              {{ type.name }}
            </option>
          </select>
          <div class="invalid-feedback">Należy wybrać rodzaj.</div>
        </div>
        <div class="mb-3">
          <label for="ingredientUnit" class="form-label">J.m.</label>
          <input
            type="text"
            class="form-control"
            id="ingredientUnit"
            required
            v-model="ingredient.ingredientUnit"
          />
          <div class="invalid-feedback">Należy podać jednostkę.</div>
        </div>
        <div class="mb-3">
          <label for="ingredientExchanger" class="form-label"
            >Przelicznik</label
          >
          <div class="input-group mb-3">
            <button
              class="btn btn-danger"
              type="button"
              :disabled="ingredient.exchanger === 0"
              @click="decrement()"
            >
              <i class="bi-dash" />
            </button>
            <input
              type="number"
              class="form-control"
              id="ingredientExchanger"
              min="0"
              oninput="validity.valid||(value=0);"
              v-model="ingredient.exchanger"
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
      </form>
      <div class="card-actions">
        <button class="btn btn-secondary me-5" type="button" @click="save()">
          Zapisz
        </button>
        <button
          class="btn btn-outline-dark"
          type="button"
          @click="$router.go(-1)"
        >
          Anuluj
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Ref, ref } from "@vue/reactivity";
import { useRoute } from "vue-router";
import { onMounted } from "vue";
import { IngredientType } from "@/models/IngredientType";
import IngredientTypeService from "@/services/IngredientTypeService";
import { Ingredient } from "@/models/Ingredient";
import IngredientService from "@/services/IngredientService";

export default {
  setup() {
    const form: Ref<any> = ref(null);
    const id: Ref<string> = ref("");
    const ingredient: Ref<Ingredient> = ref(new Ingredient());
    const ingredientTypes: Ref<IngredientType[]> = ref([]);

    const { getIngredient, upsertIngredient } = IngredientService();
    const { getIngredientTypes } = IngredientTypeService();
    const route = useRoute();

    onMounted(() => {
      getIngredientTypes().then(
        (response) => (ingredientTypes.value = response)
      );

      id.value = route.params.id.toString();

      if (id.value !== "new") {
        getIngredient(id.value).then(
          (response) => (ingredient.value = response)
        );
      }
    });

    const decrement = () => {
      if (ingredient.value.exchanger === 0) {
        return;
      }

      ingredient.value.exchanger--;
    };

    const increment = () => {
      ingredient.value.exchanger++;
    };

    const save = () => {
      if (form.value.checkValidity()) {
        upsertIngredient(ingredient.value);
      }

      form.value.classList.add("was-validated");
    };

    return {
      form,
      id,
      ingredient,
      ingredientTypes,
      decrement,
      increment,
      save,
    };
  },
};
</script>