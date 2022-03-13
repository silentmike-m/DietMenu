<template>
  <v-card elevation="5">
    <v-card-title>{{ options.title }}</v-card-title>
    <ValidationObserver v-slot="{ invalid }">
      <v-card-text>
        <v-form @submit.prevent="options.confirmAction(value)" id="input-form">
          <ValidationProvider v-slot="{ errors }" rules="required" vid="value">
            <v-text-field
              :label="options.label"
              v-model="value"
              required
              autofocus
              :error-messages="errors"
              ref="inputValue"
            ></v-text-field>
          </ValidationProvider>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-btn
          color="primary"
          elevation="5"
          :disabled="invalid"
          small
          type="submit"
          form="input-form"
          >{{ options.confirmText }}</v-btn
        >
        <v-spacer />
        <v-btn small outlined @click="options.cancelAction()">{{
          options.cancelText
        }}</v-btn>
      </v-card-actions>
    </ValidationObserver>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";

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
  name: "InputDialog",
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  props: ["options"],
  data() {
    return {
      value: this.options.value,
    };
  },
});
</script>