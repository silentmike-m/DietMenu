<template>
  <v-list nav flat>
    <template v-for="element in menuElements">
      <template v-if="element.childElements.length">
        <v-menu
          v-if="isMiniVariant && $vuetify.breakpoint.smAndUp"
          :key="element.id"
          open-on-hover
          top
          :offset-x="true"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-list-item v-bind="attrs" v-on="on">
              <v-list-item-icon>
                <v-icon v-text="element.iconClass"></v-icon>
              </v-list-item-icon>
              <v-list-item-title v-text="element.name"></v-list-item-title>
            </v-list-item>
          </template>
          <v-list color="primary">
            <v-list-item
              v-for="child in element.childElements"
              :key="child.id"
              tag="nuxt-link"
              :to="child.path"
              exact-active-class="secondary--text"
              :exact="true"
            >
              <v-list-item-icon>
                <v-icon v-text="child.iconClass"></v-icon>
              </v-list-item-icon>
              <v-list-item-title v-text="child.name"></v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
        <v-list-group
          v-else
          :key="element.id"
          :prepend-icon="element.iconClass"
          no-action
          active-class="secondary--text"
        >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title>{{ element.name }}</v-list-item-title>
            </v-list-item-content>
          </template>
          <v-list-item
            v-for="child in element.childElements"
            :key="child.id"
            tag="nuxt-link"
            :to="child.path"
            exact-active-class="secondary--text"
            :exact="true"
            @click="handleMiniVariant()"
          >
            <v-list-item-icon>
              <v-icon v-text="child.iconClass"></v-icon>
            </v-list-item-icon>
            <v-list-item-title v-text="child.name"></v-list-item-title>
          </v-list-item>
        </v-list-group>
      </template>

      <v-list-item
        v-else
        :key="element.id"
        tag="nuxt-link"
        :to="element.path"
        exact-active-class="secondary--text"
        @click="handleMiniVariant()"
      >
        <v-list-item-icon>
          <v-tooltip right :disabled="!isMiniVariant">
            <template v-slot:activator="{ on, attrs }">
              <v-icon
                v-text="element.iconClass"
                v-bind="attrs"
                v-on="on"
              ></v-icon>
            </template>
            <span>{{ element.name }}</span>
          </v-tooltip>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title v-text="element.name"></v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </template>
  </v-list>
</template>


<script lang="ts">
import Vue from "vue";
import { onMounted, reactive, Ref, ref } from "@nuxtjs/composition-api";
import { MenuElement } from "~/types/core/navigation";
import { Framework } from "vuetify";
import applicationBarState from "~/store/appBar";

export default Vue.extend({
  name: "NavBar",
  methods: {
    handleMiniVariant(): void {
      if (
        !this.$vuetify.breakpoint.smAndUp &&
        applicationBarState.state.value === "false"
      )
        applicationBarState.setMiniVariant();
    },
  },
  setup() {
    const menuElements: MenuElement[] = reactive([]);

    onMounted(() => {
      menuElements.push(new MenuElement("Pulpit", "/", "mdi-home", []));
      menuElements.push(
        new MenuElement("Plan", "/dietPlans", "mdi-silverware", [])
      );
      menuElements.push(
        new MenuElement("Przepisy", "/recipes", "mdi-instrument-triangle", [])
      );
      menuElements.push(
        new MenuElement("Składniki", "/ingredients", "mdi-dresser", [])
      );
      menuElements.push(
        new MenuElement("Zamienniki", "/replacements", "mdi-file-replace", [])
      );
      menuElements.push(
        new MenuElement("Konfiguracja", "", "mdi-cog", [
          new MenuElement(
            "Rodzina",
            "/families",
            "mdi-human-male-female-child",
            []
          ),
          new MenuElement(
            "Rodzaje składników",
            "/ingredientTypes",
            "mdi-dresser",
            []
          ),
        ])
      );
    });

    return {
      menuElements,
    };
  },
  computed: {
    isMiniVariant() {
      return applicationBarState.get();
    },
  },
});
</script>
