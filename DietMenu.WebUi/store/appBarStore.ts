import { ref } from "@vue/composition-api";

const state = ref(localStorage.getItem('miniVariant') || 'false');

const get = (): boolean => {
    return state.value == "true";
}

const setMiniVariant = (): void => {
    state.value = state.value == "true" ? "false" : "true";
    localStorage.setItem('miniVariant', state.value)
}

const applicationBarState = {
    get,
    setMiniVariant,
    state
}

export default applicationBarState;