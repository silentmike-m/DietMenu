import { ref } from "@vue/composition-api";
import { MealType } from "~/types/mealTypes";

const state = ref([] as MealType[]);

const set = (items: MealType[]): void => {
    state.value = items;
}

const get = (): MealType[] => {
    return state.value;
}

const mealTypesState = {
    set,
    get,
}

export default mealTypesState;