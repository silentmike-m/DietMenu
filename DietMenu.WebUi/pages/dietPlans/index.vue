<!--
<template>
  <v-card elevation="5"
    ><v-card-title
      ><v-card-text
        ><v-container fluid
          ><v-row dense justify="center">
            <v-btn outlined small color="primary" @click="setToday">
              Dzisiaj
            </v-btn>
            <span>
              <v-btn color="primary" small text @click="$refs.calendar.prev()">
                <v-icon> mdi-chevron-left </v-icon>
              </v-btn>
              <span v-if="$refs.calendar">
                {{ $refs.calendar.title }}
              </span>
              <v-btn color="primary" small text @click="$refs.calendar.next()">
                <v-icon> mdi-chevron-right </v-icon>
              </v-btn>
            </span>

            <v-menu bottom right>
              <template v-slot:activator="{ on, attrs }">
                <v-btn outlined color="primary" v-bind="attrs" v-on="on" small>
                  <span>{{ calendarTypes[calendarType] }}</span>
                  <v-icon right> mdi-menu-down </v-icon>
                </v-btn>
              </template>
              <v-list>
                <v-list-item @click="calendarType = 'day'">
                  <v-list-item-title>Dzień</v-list-item-title>
                </v-list-item>
                <v-list-item @click="calendarType = 'week'">
                  <v-list-item-title>Tydzień</v-list-item-title>
                </v-list-item>
                <v-list-item @click="calendarType = 'month'">
                  <v-list-item-title>Miesiąc</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-row></v-container
        ></v-card-text
      ></v-card-title
    ><v-card-text>
      <v-calendar
        ref="calendar"
        v-model="todayFocus"
        color="primary"
        :events="recipes"
        :event-color="getRecipeColor"
        event-start="date"
        interval-count="0"
        interval-width="0"
        locale="pl-PL"
        :type="calendarType"
        :weekdays="weekdays"
        @click:event="showRecipe"
        @change="getRecipes"
      >
        <template v-slot:day-label-header="{ date, day, present }">
          <v-menu offset-y close-on-click>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                fab
                small
                elevation="0"
                :color="present ? 'primary' : 'transparent'"
                v-bind="attrs"
                v-on="on"
              >
                {{ day }}
              </v-btn>
            </template>
            <v-list>
              <v-list-item-group>
                <v-list-item
                  v-for="(type, index) in mealTypes"
                  :key="index"
                  @click="addRecipe(date, type.id)"
                >
                  <v-list-item-content>
                    <v-list-item-title v-text="type.name"></v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list-item-group>
            </v-list>
          </v-menu>
        </template>
        <template v-slot:day-label="{ date, day, present }">
          <v-menu offset-y close-on-click>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                fab
                small
                elevation="0"
                :color="present ? 'primary' : 'transparent'"
                v-bind="attrs"
                v-on="on"
              >
                {{ day }}
              </v-btn>
            </template>
            <v-list>
              <v-list-item-group>
                <v-list-item
                  v-for="(type, index) in mealTypes"
                  :key="index"
                  @click="addRecipe(date, type.id)"
                >
                  <v-list-item-content>
                    <v-list-item-title v-text="type.name"></v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list-item-group>
            </v-list>
          </v-menu>
        </template>
        <template #event="event"> {{ event.event.name }} </template>
      </v-calendar>

      <v-menu
        v-if="selectedRecipe"
        v-model="isSelectedOpen"
        :close-on-content-click="false"
        :activator="selectedEvent"
        offset-x
      >
        <v-card color="grey lighten-4" max-width="350px" flat>
          <v-toolbar :color="getRecipeColor(selectedRecipe)" dark>
            <v-toolbar-title v-html="selectedRecipe.name"></v-toolbar-title>
            <v-spacer />
            <v-btn icon @click="editRecipe(selectedRecipe)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon @click="deleteRecipe(selectedRecipe)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text>
            <span>{{ selectedRecipe.meal_type_name }}</span>
          </v-card-text>
          <v-card-actions>
            <v-btn text color="primary" @click="isSelectedOpen = false">
              Zamknij
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-menu>
    </v-card-text></v-card
  >
</template>

<script lang="ts">
import Vue from "vue";
import { onBeforeMount, Ref, ref, useContext } from "@nuxtjs/composition-api";
import { DietPlanRecipeRow } from "~/types/dietPlans";
import useDietPlans from "~/capi/useDietPlans";
import { MealType } from "~/types/mealTypes";
import useMealTypes from "~/capi/useMealTypes";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import useDialog from "~/capi/core/useDialog";
import { YesNoDialogOptions } from "~/types/core/dialog";

export default Vue.extend({
  name: "DietPlan",
  setup() {
    const calendarType: Ref<string> = ref("month");
    const calendarTypes: Ref<{}> = ref({
      month: "Miesiąc",
      week: "Tydzień",
      day: "Dzień",
    });
    const isAddDietPlanOpen: Ref<Boolean> = ref(false);
    const isSelectedOpen: Ref<Boolean> = ref(false);
    const mealTypes: Ref<MealType[]> = ref([]);
    const recipes: Ref<DietPlanRecipeRow[]> = ref([]);
    const selectedEvent: Ref<{}> = ref({});
    const selectedRecipe: Ref<DietPlanRecipeRow | null> = ref(null);
    const todayFocus: Ref<string> = ref("");
    const weekdays: Ref<number[]> = ref([1, 2, 3, 4, 5, 6, 0]);

    const { setBreadcrumbs } = useBreadcrumbs();
    const { redirect } = useContext();
    const { closeDialog, showYesNoQuestion } = useDialog();
    const { deleteDietPlanRecipe, getDietPlanRecipes } = useDietPlans();
    const { getMealTypes } = useMealTypes();

    onBeforeMount(() => {
      setBreadcrumbs(BreadcrumbType.DietPlans, "");

      getMealTypes().then((result) => (mealTypes.value = result));
    });

    const addRecipe = (date: string, mealTypeId: string) => {
      redirect(`/dietPlans/new?date=${date}&mealTypeId=${mealTypeId}`);
    };

    const deleteRecipe = (row: DietPlanRecipeRow): void => {
      showYesNoQuestion(
        new YesNoDialogOptions(
          "Usunięcie przepisu",
          `Czy na pewno chcesz usunąć przepis ${row.name}?`,
          () => {
            deleteDietPlanRecipe(row)
              .then((result) => {
                if (result) {
                  const index = recipes.value.indexOf(row, 0);
                  if (index > -1) {
                    recipes.value.splice(index, 1);
                  }
                }
              })
              .finally(() => closeDialog());
          },
          () => {
            closeDialog();
          }
        )
      );
    };

    const editRecipe = (data: DietPlanRecipeRow) => {
      redirect(`/dietPlans/${data.id}`);
    };

    const getRecipeColor = (data: DietPlanRecipeRow): string => {
      const mealType = mealTypes.value.filter(
        (i) => i.id == data.meal_type_id
      )[0];

      return mealType.color_hex;
    };

    const getRecipes = (data: any) => {
      getDietPlanRecipes(data.start.date, data.end.date).then((result) => {
        recipes.value = result;
      });
    };

    const setToday = () => {
      todayFocus.value = "";
    };

    const showRecipe = (data: any) => {
      const open = () => {
        selectedEvent.value = data.nativeEvent.target;
        selectedRecipe.value = data.event;

        requestAnimationFrame(() =>
          requestAnimationFrame(() => (isSelectedOpen.value = true))
        );
      };

      if (isSelectedOpen.value) {
        isSelectedOpen.value = false;
        requestAnimationFrame(() => requestAnimationFrame(() => open()));
      } else {
        open();
      }

      data.nativeEvent.stopPropagation();
    };

    return {
      addRecipe,
      calendarType,
      calendarTypes,
      deleteRecipe,
      editRecipe,
      getRecipeColor,
      getRecipes,
      isAddDietPlanOpen,
      isSelectedOpen,
      mealTypes,
      recipes,
      selectedEvent,
      selectedRecipe,
      setToday,
      showRecipe,
      todayFocus,
      weekdays,
    };
  },
});
</script>

-->